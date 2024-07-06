using UnityEngine;
using UnityEngine.AI;

public class UnitManager : MonoBehaviour
{
    // tutorial on anim controller, blend treed and animation layers: https://www.youtube.com/watch?v=qc0xU2Ph86Q

    [Header("Unit Setup:")]
    public string unitName;
    public int myPlayerAffiliation = 0;
    public bool isActive = false; // this gets set true when the unit was launched in a wave:
    public bool isDead = false;

    private GameObject _myLocation;

    // data from scriptable objects:
    public UnitDataSO unitData;
    public ArmorDataSO armorData;
    public WeaponDataSO weaponData;
    //public WeaponDataSO offHandData;

    // cached component references
    private UnitHealth _unitHealth;
    private UnitMovement _unitMovement;
    private UnitCombat _unitCombat;
    private UnitAnimationController _unitAnimationController;
    private UnitDetection _unitDetection;

    private DragNDrop _dragNDrop;
    private CameraController _cameraController;
    private GameManager _gameManager;
    private LevelBuilder _levelBuilder;
    //public Animator unitAnimator;


    // this statline is used by all other component to get relevant profile information
    #region Public Unit Information and Components:
    [SerializeField] bool _reportThisUnitsStatline = false;

    public Animator unitAnimator;
    public AnimatorOverrideController unitAnimationOverrideController; // this is required to speed up certain anim-speeds in runtime:
    public UnitDataSO.Race race;
    public UnitDataSO.UnitType unitType;
    public ArmorDataSO.ArmorType armorType;
    public WeaponDataSO.WeaponType weaponType;
    //WeaponDataSO.WeaponType _offHandWeaponType;

    // general:
    public float baseDeploymentCost = 0.0f;
    public float baseCarryCapacity = 0.0f;

    // health:
    public float baseHealthPoints = 0.0f;
    public float baseArmorValue = 0.0f;

    // movement:
    public Transform unitDestination;
    public float walkingSpeed = 0.0f;
    public float runningSpeed = 0.0f;
    public float chargingSpeed = 0.0f;
    public float baseSize = 0.0f;

    // charge:
    public float baseChargeSpeedMultiplier = 0.0f;
    public float baseChargeImpactDamage = 0.0f;
    public float baseChargeImpactSplashRadius = 0.0f;
    public float baseChargeImpactForce = 0.0f;

    // combat:
    //int _weaponCount = 0;
    public float baseSpottingRange = 0.0f;
    public float baseAlarmRange = 0.0f;
    
    // damage:
    public float baseDamage = 0.0f;
    public float baseAttackSpeed = 0.0f;
    public float baseAttackRange = 0.0f;
    //public float baseWeaponWeight = 0.0f; // currently unused
    public float baseArmorPenetrationValue = 0.0f; // currently unused
    public float baseToHitChance = 0.0f; // currently unused
    public float baseCriticalHitChance = 0.0f; // currently unused
    public float baseSplashRadius = 0.0f; // currently unused
    public float baseSplashDamage = 0.0f; // currently unused
    public float baseBashChance = 0.0f; // currently unused
    public float baseBashForce = 0.0f; // currently unused
    #endregion

    [Header("Temp. Castle Fix:")]
    [SerializeField] int manualPlayerID = 0; 

    private void Start()
    {
        // temporary solution to spawning castles:
        if (unitType == UnitDataSO.UnitType.Building)
        {
            InitializeUnit(manualPlayerID);
            /*
            _gameManager = FindAnyObjectByType<GameManager>();

            switch (manualPlayerID)
            {
                case 0: // this is a piece of environment, either player can farm it:
                    myPlayerAffiliation = 0;
                    break;

                case 1: // placed by player 1:
                    myPlayerAffiliation = manualPlayerID;

                    _cameraController = FindAnyObjectByType<CameraController>();

                    // when all units are dead, fly to the most foward building instead!
                    _cameraController.AddUnitToFollow(this.transform);
                    break;

                case 2: // placed by player 2:
                    myPlayerAffiliation = manualPlayerID;
                    break;

                default: // error!
                    Debug.LogError("ERROR: This object does not have a legal ID assigned!", this);
                    break;
            }
            // set up and update this units profile:
            CompileUnitProfile();

            // assign unit-name if none is given:
            if (unitName == "")
            {
                unitName = unitData.unitName;
            }

            // cache components and initialize them:
            if (GetComponent<Animator>()) // this has to happen before other scripts are initializing!
            {
                unitAnimator = GetComponent<Animator>();
            }
            if (GetComponent<UnitHealth>())
            {
                _unitHealth = GetComponent<UnitHealth>();
                _unitHealth.InitializeUnitHealth();
            }
            if (GetComponent<UnitMovement>())
            {
                _unitMovement = GetComponent<UnitMovement>();
                _unitMovement.InitializeUnitMovement();
            }
            if (GetComponent<UnitCombat>())
            {
                _unitCombat = GetComponent<UnitCombat>();
                _unitCombat.InitializeUnitCombat();
            }
            if (GetComponent<UnitDetection>())
            {
                _unitDetection = GetComponent<UnitDetection>();
                _unitDetection.InitializeUnitDetection();
            }
            
            if (GetComponent<UnitAnimationController>())
            {
                _unitAnimationController = GetComponent<UnitAnimationController>();
                _unitAnimationController.InitializeUnitAnimatonController();
            }

            LaunchUnit();*/
        }
    }

    /// <summary>
    /// Set up the chosen unit on pickup.
    /// </summary>
    public void InitializeUnit(int _playerID)
    {
        if (unitData != null)
        {
            // cache references:
            _gameManager = FindAnyObjectByType<GameManager>();
            unitDestination = _gameManager.GetDestination(_playerID);

            // asign playerAffiliation: 
            switch (_playerID)
            {
                case 0: // this is a piece of environment, either player can farm it:
                    myPlayerAffiliation = 0;
                    break;

                case 1: // placed by player 1:
                    myPlayerAffiliation = _playerID;

                    _dragNDrop = FindAnyObjectByType<DragNDrop>();

                    _cameraController = FindAnyObjectByType<CameraController>();

                    _levelBuilder = FindAnyObjectByType<LevelBuilder>();
                    break;

                case 2: // placed by player 2:
                    myPlayerAffiliation = _playerID;
                    break;

                default: // error!
                    Debug.LogError("ERROR: This object does not have a legal ID assigned!", this);
                    break;
            }

            // set up and update this units profile:
            CompileUnitProfile();

            // assign unit-name if none is given:
            if (unitName == "")
            {
                unitName = unitData.unitName;
            }

            // cache components and initialize them:
            // check first if there is an animator to begin with:
            if (GetComponent<Animator>()) // this has to happen before other scripts are initializing!
            {
                unitAnimator = GetComponent<Animator>();
            }
            if (GetComponent<UnitHealth>())
            {
                _unitHealth = GetComponent<UnitHealth>();
                _unitHealth.InitializeUnitHealth();
            }
            if (GetComponent<UnitMovement>())
            {
                _unitMovement = GetComponent<UnitMovement>();
                _unitMovement.InitializeUnitMovement();
            }
            if (GetComponent<UnitCombat>())
            {
                _unitCombat = GetComponent<UnitCombat>();
                _unitCombat.InitializeUnitCombat();
            }
            if (GetComponent<UnitAnimationController>())
            {
                _unitAnimationController = GetComponent<UnitAnimationController>();
                _unitAnimationController.InitializeUnitAnimatonController();

                // if this is a ranged unit, also initialize projectile-animation:
                if(weaponType == WeaponDataSO.WeaponType.Bow && GetComponent<AnimateProjectile>())
                {
                    // does not need to be referenced, as it's only ever accessed by the Animation Controller:
                    GetComponent<AnimateProjectile>().InitializeProjectileAnimator();
                }
            }
            if (GetComponent<UnitDetection>())
            {
                _unitDetection = GetComponent<UnitDetection>();
                _unitDetection.InitializeUnitDetection();
            }
        }
        else
        {
            Debug.LogError("ERROR: Unit without UnitDataSO detected!", this);
            return;
        }
    }

    /// <summary>
    /// This function is called when the unit is picked up from a selecor-icon or 
    /// placed in a deploymentzone by an AI. The assigned ID is used to tell friend from foe.
    /// </summary>
    /// <param name="_playerID"></param>
    public void DeployThisUnit(int _playerID, GameObject _spawnZoneOfPlacement)
    {
        // if this is an actual unit, update the current location of this unit: 
        if (unitData.canBeManuallyPlaced && !isActive)
        {
            _myLocation = _spawnZoneOfPlacement;
        }
    }

    /// <summary>
    /// Compile and print the unit's stat line if desired.
    /// </summary>
    void CompileUnitProfile()
    {
        // general:
        race = unitData.race;
        unitType = unitData.unitType;
        armorType = armorData.armorType;
        weaponType =  weaponData.weaponType;

        baseDeploymentCost = unitData.deploymentCost + weaponData.deploymentCost + armorData.deploymentCost;
        baseCarryCapacity = unitData.carryCapacity;

        // health:
        baseHealthPoints = unitData.healthPoints;
        baseArmorValue = armorData.armorValue + weaponData.armorValue;

        // movement:
        walkingSpeed = unitData.walkingSpeed;
        runningSpeed = unitData.runningSpeed;
        chargingSpeed = unitData.chargingSpeed;

        // spotting::
        baseSpottingRange = unitData.spottingRange;
        baseAlarmRange = unitData.alarmRange;

        // charge:
        baseSize = unitData.size;
        //baseChargeSpeedMultiplier = unitData.chargeSpeed; // now part of movement (see above):
        baseChargeImpactDamage = unitData.chargeImpactDamage;
        baseChargeImpactSplashRadius = unitData.chargeImpactSplashRadius; 
        baseChargeImpactForce = unitData.chargeImpactForce; 

        // basic combat:
        baseDamage = weaponData.damage;
        baseAttackSpeed = weaponData.attackSpeed;
        baseAttackRange = weaponData.attackRange;
        baseArmorPenetrationValue = weaponData.armorPenetrationValue;
        baseToHitChance = unitData.toHitSkill * weaponData.toHitFactor;
        baseCriticalHitChance = unitData.toCritSkill * weaponData.criticalHitFactor;

        // special combat:
        baseSplashRadius = weaponData.splashRadius;
        baseSplashDamage = weaponData.splashDamage;
        baseBashChance = weaponData.bashChance;
        baseBashForce = unitData.size * weaponData.bashForceFactor;

        if (_reportThisUnitsStatline)
        {
            Debug.Log("[statline]");
        }
    }

    
    void ResetThisUnit()
    {
        // tell the occupied SpawnZone that the placed unit got picked up again:
        _myLocation.GetComponent<SpawnZone>().VacateDeploymentTile();

        // tell this unit that it has no longer an assigned location/SpawnZone:
        _myLocation = null;
    }

    void OnMouseDown() // doesn't this need to be public?
    {
        // check if this object is meant to be picked up or if it has already been launched:
        if (unitData.canBeManuallyPlaced && !isActive)
        {
            if (_dragNDrop != null && _dragNDrop.carriedObject == null)
            {
                _dragNDrop.PickUpAgain(this.gameObject);
                ResetThisUnit();
            }
        }
    }

    /// <summary>
    /// This is called when a new wave is launched. Units in that wave can no longer be picked up. This also 
    /// tells the camera-follow-logic to consider this unit for following.
    /// </summary>
    public void LaunchUnit()
    {
        isActive = true;

        // differenciate between unit types as some "units" may not need to move etc:
        switch (unitType)
        {
            case UnitDataSO.UnitType.Peasant: // this is a combat unit, so get moving:
                
                _unitMovement.StartMovement();
                _unitHealth.LaunchHealthBar();
                _unitDetection.StartScanningForEnemies();

                switch (myPlayerAffiliation)
                {
                    case 1:
                        // add friendly units to the cameras follow-list:
                        _cameraController.AddUnitToFollow(this.transform);
                        break;

                    case 2:
                        // TBD...
                        break;

                    default:
                        // TBD...
                        break;
                }
                break;

            case UnitDataSO.UnitType.Soldier: // this is a combat unit, so get moving:
                
                _unitMovement.StartMovement();
                _unitHealth.LaunchHealthBar();
                _unitDetection.StartScanningForEnemies();

                switch (myPlayerAffiliation)
                {
                    case 1:
                        // add friendly units to the cameras follow-list:
                        _cameraController.AddUnitToFollow(this.transform);
                        break;

                    case 2:
                        // TBD...
                        break;

                    default:
                        // TBD...
                        break;
                }
                break;

            case UnitDataSO.UnitType.LightCavalry: // this is a combat unit, so get moving:
                
                _unitMovement.StartMovement();
                _unitHealth.LaunchHealthBar();
                _unitDetection.StartScanningForEnemies();

                switch (myPlayerAffiliation)
                {
                    case 1:
                        // add friendly units to the cameras follow-list:
                        _cameraController.AddUnitToFollow(this.transform);
                        break;

                    case 2:
                        // TBD...
                        break;

                    default:
                        // TBD...
                        break;
                }
                break;

            case UnitDataSO.UnitType.HeavyCavalry: // this is a combat unit, so get moving:
                
                _unitMovement.StartMovement();
                _unitHealth.LaunchHealthBar();
                _unitDetection.StartScanningForEnemies();

                switch (myPlayerAffiliation)
                {
                    case 1:
                        // add friendly units to the cameras follow-list:
                        _cameraController.AddUnitToFollow(this.transform);
                        break;

                    case 2:
                        // TBD...
                        break;

                    default:
                        // TBD...
                        break;
                }
                break;

            case UnitDataSO.UnitType.Warmachine: // this is a combat unit, so get moving:
                
                _unitMovement.StartMovement();
                _unitHealth.LaunchHealthBar();
                _unitDetection.StartScanningForEnemies();

                switch (myPlayerAffiliation)
                {
                    case 1:
                        // add friendly units to the cameras follow-list:
                        _cameraController.AddUnitToFollow(this.transform);
                        break;

                    case 2:
                        // TBD...
                        break;

                    default:
                        // TBD...
                        break;
                }
                break;

            case UnitDataSO.UnitType.Building:
                
                _unitMovement.StartMovement();
                _unitHealth.LaunchHealthBar();
                _unitDetection.StartScanningForEnemies();

                switch (myPlayerAffiliation)
                {
                    case 1:
                        // add friendly units to the cameras follow-list:
                        _cameraController.AddUnitToFollow(this.transform);
                        break;

                    case 2:
                        // TBD...
                        break;

                    default:
                        // TBD...
                        break;
                }
                break;

            case UnitDataSO.UnitType.Environment:
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Called by UnitHealth. This function rewards gold for the kill, updates the camera-follow.
    /// NOTE: This does NOT destroy the unit (as we want the animation to finish playing).
    /// </summary>
    public void SignOffThisUnit()
    {
        // mark this unit as deactivated:
        isActive = false;
        isDead = true;

        if (GetComponent<UnitMovement>())
        {
            _unitMovement.DeactivateMovement();
        }
        if (GetComponent<UnitCombat>())
        {
            _unitCombat.DeactivateCombat();
        }

        // reward the victor 1/4 of the killed units deployment cost:
        switch (myPlayerAffiliation)
        {
            case 1:
                FindAnyObjectByType<GoldManager>().AddGold(2, GetComponent<UnitManager>().baseDeploymentCost / 4.0f);

                // tell camera not to consider this unit for following anymore:
                //if (!CompareTag("HQ")) // we should probably rather have the cam jump here...
                //{
                    _cameraController.RemoveUnitToFollow(this.transform);
                //}
                break;

            case 2:
                FindAnyObjectByType<GoldManager>().AddGold(1, GetComponent<UnitManager>().baseDeploymentCost / 4.0f);
                //Debug.Log("player 1 (you) should have received: " + GetComponent<UnitManager>().baseDeploymentCost / 4.0f + " gold.");
                break;

            default:
                break;
        }

        // end the game, if this was an HQ:
        if (CompareTag("HQ"))
        {
            FindAnyObjectByType<EndOfGame>().RoundFinished(manualPlayerID);
        }

        // destroy this unit:
        //Debug.Log("destroying: " + this.gameObject.name);
        //Destroy(this.gameObject);
    }

    /// <summary>
    /// Called by <see cref="UnitHealth"/> when a unit gets killed, so the corpse can remain on the 
    /// battlefield until turnt off.
    /// </summary>
    public void DeactivateAllComponents()
    {
        if(GetComponent<Collider>() != null && GetComponent<Collider>().enabled)
        {
            Destroy(GetComponent<Collider>());
        }

        if(GetComponent<NavMeshAgent>() != null && GetComponent<NavMeshAgent>().enabled)
        {
            Destroy(GetComponent<NavMeshAgent>());
        }

        if (GetComponent<UnitMovement>() != null && GetComponent<UnitMovement>().enabled)
        {
            Destroy(GetComponent<UnitMovement>());
        }

        if (GetComponent<UnitCombat>() != null && GetComponent<UnitCombat>().enabled)
        {
            Destroy(GetComponent<UnitCombat>());
        }

        /* // this one actually torpedos the death sequence mid execution:
        if (GetComponent<UnitHealth>() != null && GetComponent<UnitHealth>().enabled)
        {
            Destroy(GetComponent<UnitHealth>());
        }*/
    }

    public void DestroyUnit()
    {
        Destroy(this.gameObject);
    }
}


/*using UnityEngine;
using UnityEngine.AI;

public class UnitManager : MonoBehaviour
{
    public enum Race { Human, Orc }
    public enum UnitType { Infantry, Cavalry, Monster, Building }
    public enum ClassType { DamageDealer, Tank, Sniper } // DamageDealer > Tank, Tank > Sniper, Sniper > DamageDealer

    [Header("Unit Setup:")]
    public UnitData unitData;
    public int playerAffiliation = 0;

    [Space(20)]
    [Header("Do Not Touch!")]
    public GameObject myLocation;
    public bool wasLaunched = false;

    // Cached component references
    private UnitHealth unitHealth;
    private UnitMovement unitMovement;
    private UnitCombat unitCombat;

    private void Awake()
    {
        // Cache references to components
        unitHealth = GetComponent<UnitHealth>();
        unitMovement = GetComponent<UnitMovement>();
        unitCombat = GetComponent<UnitCombat>();

        InitializeUnit();
    }

    private void Start()
    {
        if (unitData.unitType == UnitType.Building)
        {
            SetupThisUnit(true, this.gameObject);
            unitHealth.UpdateUnitHealth();
        }
    }

    private void InitializeUnit()
    {
        // Initialize attributes from unitData
        if (unitData != null)
        {
            unitHealth.SetHealthPoints(unitData.healthPoints);
            unitMovement.SetMovementSpeed(unitData.movementSpeed);
            unitCombat.SetCombatStats(unitData.meleeDamage, unitData.rangedDamage);
        }
    }

    public void SetupThisUnit(bool _placedByPlayer, GameObject _spawnZoneOfPlacement)
    {
        if (unitData.unitType != UnitType.Building)
        {
            myLocation = _spawnZoneOfPlacement;
            playerAffiliation = _placedByPlayer ? 1 : 2;

            unitHealth.teamAffiliation = playerAffiliation;

            unitMovement.UpdateUnitMovement();
            unitCombat.UpdateUnitCombat();
            unitHealth.UpdateUnitHealth();
        }
        else
        {
            Debug.Log($"I am a castle, I have {unitData.healthPoints} and belong to player {playerAffiliation}");
        }
    }

    public void ResetThisUnit()
    {
        if (unitData.unitType != UnitType.Building && myLocation != null)
        {
            myLocation.GetComponent<SpawnZone>().VacateDeploymentTile();
            myLocation = null;
        }
    }

    private void OnMouseDown()
    {
        if (unitData.unitType != UnitType.Building)
        {
            DragNDrop dragNDrop = FindObjectOfType<DragNDrop>();
            if (dragNDrop != null && dragNDrop.carriedObject == null)
            {
                dragNDrop.PickUpAgain(this.gameObject);
                ResetThisUnit();
            }
        }
    }
}*/

// this worked:
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This script sits on ever object in the scene with a healthpool.
/// </summary>
public class UnitManager : MonoBehaviour
{
    public enum Race { Human, Orc }
    public enum UnitType { Building, Infantry, Cavalry }
    public enum ClassType { DamageDealer, Tank, Sniper } // DamageDealer > Tank, Tank > Sniper, Sniper > DamageDealer

    [Header("Unit Setup:")]
    public string unitName = "default unit";
    public Race race = Race.Human;
    public UnitType unitType = UnitType.Infantry;
    public ClassType classType = ClassType.DamageDealer; // DD > armored, armored > ranged, ranged > DD
    public int myClassID;
    public int playerAffiliation = 0;
    public int deploymentCost = 1;
    
    [Space(10)]
    public float healthPoints = 1.0f;
    public float armorValue = 0.0f;
    public float movementSpeed = 1.0f;
    public float spottingRange = 5.0f;
    
    [Space(10)]
    public float meleeArmorPenetrationRating = 0.0f;
    public float meleeAttackRange = 0.5f;
    public float meleeAttackSpeed = 1.0f;
    public float meleeDamage = 1.0f;
    
    [Space(10)]
    public float rangedArmorPenetrationRating = 0.0f;
    public float rangedAttackRange = 0.0f;
    public float rangedAttackSpeed = 0.0f;
    public float rangedDamage = 0.0f;

    [Space(20)]
    [Header("Do Not Touch!")]
    public GameObject myLocation;
    public bool wasLaunched = false;

    private void Start()
    {
        // check if unit is a headquaters
        if (unitType == UnitType.Building)
        {
            SetupThisUnit(true, this.gameObject);
            //GetComponent<UnitHealth>().UpdateUnitHealth();
        }
    }

    /// <summary>
    /// Tell the unit it was placed. int = 1 is the players team, int = 2 is the AI's team.
    /// </summary>
    /// <param name="_teamID"></param>
    public void SetupThisUnit(bool _placedByPlayer, GameObject _spawnZoneOfPlacement)
    {
        // check if unit is a headquaters
        if(unitType != UnitType.Building)
        {
            myLocation = _spawnZoneOfPlacement;

            if (_placedByPlayer)
            {
                playerAffiliation = 1;
            }
            else
            {
                playerAffiliation = 2;
            }

            // this is both for kill-rewards and camera control (for players):
            GetComponent<UnitHealth>().teamAffiliation = playerAffiliation;

            // transform my class type into an ID:
            switch (classType)
            {
                case ClassType.DamageDealer:
                    myClassID = 0;
                    break;

                case ClassType.Tank:
                    myClassID = 1;
                    break;

                case ClassType.Sniper:
                    myClassID = 2;
                    break;
            }

            GetComponent<UnitMovement>().UpdateUnitMovement();
            //GetComponent<UnitCombat>().UpdateUnitCombat();
            //GetComponent<UnitHealth>().UpdateUnitHealth();
        }
        else
        {
            Debug.Log("I am a castle, I have " + healthPoints + " and of player " + playerAffiliation);
        }
    }

    
    public void ResetThisUnit()
    {
        // check if unit is a headquaters
        if (unitType != UnitType.Building)
        {
            myLocation.GetComponent<SpawnZone>().VacateDeploymentTile();
            myLocation = null;
        }
    }

    /// <summary>
    /// Allow player to pick this placed unit back up.
    /// </summary>
    private void OnMouseDown()
    {
        // check if unit is a headquaters
        if (unitType != UnitType.Building)
        {
            if (FindObjectOfType<DragNDrop>().carriedObject == null)
            {
                FindObjectOfType<DragNDrop>().PickUpAgain(this.gameObject);

                ResetThisUnit();
            }
        }
    }
}*/