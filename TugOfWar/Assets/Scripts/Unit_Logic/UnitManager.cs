using System;
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
    public float baseRewardValue = 0.0f;
    public float baseCarryCapacity = 0.0f;

    // health:
    public float baseHealthPoints = 0.0f;
    public float baseArmorValue = 0.0f;

    // movement:
    public Vector3 unitDestination; // new
    //public Transform unitDestination; // old
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
    public float baseWeaponDamage = 0.0f;
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

    /// <summary>
    /// Set up the chosen unit on pickup.
    /// </summary>
    public void InitializeUnit(int _playerID)
    {
        if (unitData != null)
        {
            // cache references:
            _gameManager = FindAnyObjectByType<GameManager>();
            //unitDestination = new Vector3(_gameManager.GetEnemyBaseLine(_playerID), transform.position.y, transform.position.z); // new
            //unitDestination = _gameManager.GetEnemyCastleLocation(_playerID); // old // call this later, if need be (when entering enemy deployment)

            // asign playerAffiliation: 
            switch (_playerID)
            {
                case 0: // environment:
                    myPlayerAffiliation = _playerID;
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

                case 3: // neutral units and buildings:
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
                // if this unit is neutral, don't get a destination:
                if (!IsNeutral())
                {
                    unitDestination = new Vector3(_gameManager.GetEnemyBaseLine(_playerID), transform.position.y, transform.position.z); // new
                }

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
                if (weaponType == WeaponDataSO.WeaponType.Bow && GetComponent<AnimateProjectile>())
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

            // launch buildings that were deployed at the start immediatly:
            if (unitType == UnitDataSO.UnitType.Building)
            {
                LaunchUnit();
            }
        }
        else
        {
            Debug.LogError("ERROR: Unit without UnitDataSO detected!", this);
            return;
        }
    }

    /// <summary>
    /// Check if this unit is either environment or neutral.
    /// </summary>
    /// <returns></returns>
    private bool IsNeutral()
    {
        if(myPlayerAffiliation == 0 || myPlayerAffiliation == 3)
        {
            return true;
        }else 
        {
            return false;
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
        baseRewardValue = unitData.monetaryReward + weaponData.deploymentCost + armorData.deploymentCost;
        /*if (IsNeutral())
        {
            // assign new value and ensure the rewarding of the full sum:
            baseDeploymentCost = unitData.valueWhenNeutral * _gameManager.goldRewardFactor;
        }*/
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
        baseWeaponDamage = weaponData.damage;
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
                    case 0: // environment
                        // TBD...
                        break;

                    case 1: // player 1
                        // add friendly units to the cameras follow-list:
                        _cameraController.AddUnitToFollow(this.transform);
                        break;

                    case 2: // player 2
                        // TBD...
                        break;

                    case 3: // neutral
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
                    case 0: // environment
                        // TBD...
                        break;

                    case 1: // player 1
                        // add friendly units to the cameras follow-list:
                        _cameraController.AddUnitToFollow(this.transform);
                        break;

                    case 2: // player 2
                        // TBD...
                        break;

                    case 3: // neutral
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
                    case 0: // environment
                        // TBD...
                        break;

                    case 1: // player 1
                        // add friendly units to the cameras follow-list:
                        _cameraController.AddUnitToFollow(this.transform);
                        break;

                    case 2: // player 2
                        // TBD...
                        break;

                    case 3: // neutral
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
                    case 0: // environment
                        // TBD...
                        break;

                    case 1: // player 1
                        // add friendly units to the cameras follow-list:
                        _cameraController.AddUnitToFollow(this.transform);
                        break;

                    case 2: // player 2
                        // TBD...
                        break;

                    case 3: // neutral
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
                    case 0: // environment
                        // TBD...
                        break;

                    case 1: // player 1
                        // add friendly units to the cameras follow-list:
                        _cameraController.AddUnitToFollow(this.transform);
                        break;

                    case 2: // player 2
                        // TBD...
                        break;

                    case 3: // neutral
                        // TBD...
                        break;

                    default:
                        // TBD...
                        break;
                }
                break;

            case UnitDataSO.UnitType.Building:
                
                _unitMovement.StartMovement(); // why?
                _unitHealth.LaunchHealthBar();
                _unitDetection.StartScanningForEnemies();

                switch (myPlayerAffiliation)
                {
                    case 0: // environment
                        // TBD...
                        break;

                    case 1: // player 1
                        // add friendly units to the cameras follow-list:
                        _cameraController.AddUnitToFollow(this.transform);
                        break;

                    case 2: // player 2
                        // TBD...
                        break;

                    case 3: // neutral
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
    public void DisableUnfollowAndDisconnectUnit() // this needs to take into account WHO killed me!
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
        if(_cameraController != null)
        {
            _cameraController.RemoveUnitToFollow(this.transform);
        }
        /*
        // reward the victor 1/4 of the killed units deployment cost:
        switch (myPlayerAffiliation)
        {
            case 1:
                FindAnyObjectByType<GoldManager>().AddGold(2, GetComponent<UnitManager>().baseDeploymentCost / 4.0f);

                _cameraController.RemoveUnitToFollow(this.transform);
                break;

            case 2:
                FindAnyObjectByType<GoldManager>().AddGold(1, GetComponent<UnitManager>().baseDeploymentCost / 4.0f);
                //Debug.Log("player 1 (you) should have received: " + GetComponent<UnitManager>().baseDeploymentCost / 4.0f + " gold.");
                break;

            case 3:
                FindAnyObjectByType<GoldManager>().AddGold(1, GetComponent<UnitManager>().baseDeploymentCost / 4.0f);
                //Debug.Log("player 1 (you) should have received: " + GetComponent<UnitManager>().baseDeploymentCost / 4.0f + " gold.");
                break;

            default:
                break;
        }*/

        DeactivateAllComponents();

        // end the game, if this was an HQ:
        if (CompareTag("HQ"))
        {
            FindAnyObjectByType<EndOfGame>().RoundFinished(myPlayerAffiliation);
        }
    }

    /// <summary>
    /// Called by <see cref="UnitHealth"/> when a unit gets killed, so the corpse can remain on the 
    /// battlefield until turnt off. This is required to manage the indivvidual behaviors.
    /// </summary>
    public void DeactivateAllComponents() 
    {
        if(GetComponent<Collider>() != null && GetComponent<Collider>().enabled)
        {
            //Destroy(GetComponent<Collider>());
            GetComponent<Collider>().enabled = false;
        }

        if(GetComponent<NavMeshAgent>() != null && GetComponent<NavMeshAgent>().enabled)
        {
            //Destroy(GetComponent<NavMeshAgent>());
            GetComponent<NavMeshAgent>().enabled = false;
        }

        if (GetComponent<UnitMovement>() != null && GetComponent<UnitMovement>().enabled)
        {
            //Destroy(GetComponent<UnitMovement>());
            GetComponent<UnitMovement>().enabled = false;
        }

        if (GetComponent<UnitCombat>() != null && GetComponent<UnitCombat>().enabled)
        {
            //Destroy(GetComponent<UnitCombat>());
            GetComponent<UnitCombat>().enabled = false;
        }
    }

    /// <summary>
    /// Called as last step of the <see cref="IEnumerator DeathSequence()"/> in the <see cref="UnitHealth"/>.
    /// Detstroy the game object at the very end.
    /// </summary>
    public void DestroyUnit()
    {
        Destroy(this.gameObject);
    }
}