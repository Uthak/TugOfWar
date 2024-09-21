using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnitEnumManager;

/// <summary>
/// Holds the runtime profile of each unit. This is not manually set, but compiled and updated from the runtime variables.
/// This allows to modify and change the profile in runtime (Battlefield-powers, upgrades, effects) without changing the base profile.
/// </summary>
[System.Serializable]
public class UnitProfile
{
    // scriptable object data: 
    public UnitDataSO unitData;

    // enums:
    public Race race;
    public UnitType unitType;
    public Size size;
    // main item:
    public ItemType mainItemType;
    public ItemGrip mainItemGrip;
    // secondary item:
    public ItemType secondaryItemType;
    public ItemGrip secondaryItemGrip;

    // general information:
    public string unitName = "[no name]";
    public float deploymentCost = 0.0f;
    public float rewardValue = 0.0f;
    public bool canAttackTerrain = false; // used to allow units to attack terrain features
    public float carryCapacity = 0.0f; // currently unused
    public bool canBePlacedManually = false;
    public int footprintWidth = 0;
    public int footprintDepth = 0;

    // health:
    public float healthPoints = 0.0f;
    public float armorValue = 0.0f;

    // movement:
    public float walkingSpeed = 0.0f;
    public float runningSpeed = 0.0f;
    public float chargingSpeed = 0.0f;

    // charge:
    public float chargeImpactDamage = 0.0f; // currently unused
    public float chargeImpactSplashRadius = 0.0f; // currently unused 
    public float chargeImpactForce = 0.0f; // currently unused

    // detection:
    public float detectionRange = 0.0f;
    public float alarmRange = 0.0f; // currently unused

    // combat:
    public float weaponDamage = 0.0f;
    public float attackSpeed = 0.0f;
    public float attackRange = 0.0f;
    public float armorPenetrationValue = 0.0f; // currently unused
    public float toHitChance = 0.0f; // currently unused
    public float criticalHitChance = 0.0f; // currently unused

    public float splashRadius = 0.0f; // currently unused
    public float splashDamage = 0.0f; // currently unused

    public float bashChance = 0.0f; // currently unused
    public float bashForce = 0.0f; // currently unused
}

public class UnitManager : MonoBehaviour
{
    #region notes and resources:
    // tutorial on anim controller, blend treed and animation layers: https://www.youtube.com/watch?v=qc0xU2Ph86Q
    #endregion

    [Header("Unit Setup:")]
    public UnitDataSO unitData; // referenced to compile profile, shouldn't be used after
    public UnitProfile unitProfile; // compiled runtime profile to be used ingame
    

    // cached component references:
    public UnitHealth unitHealth;
    private UnitMovement _unitMovement;
    private UnitCombat _unitCombat;
    private UnitAnimationController _unitAnimationController;
    private UnitDetection _unitDetection;
    public Animator unitAnimator;
    public AnimatorOverrideController unitAnimationOverrideController; // this has to be set manually and is required to speed up certain anim-speeds in runtime:
    
    [Space(10)]
    // variables unrelated to unit profile:
    public int myPlayerID; // set when placed
    public bool isActive = false; // set when launched - tracks whether this unit should interact with it's surroundings
    public bool isDead = false; // set when kileld - this should likely be integrated to isActive!
    public Vector3 immediateUnitDestination; // set when placed; units walk horizontally towards enemy base
    public Vector3 enemyHeadquartersLocation; // set when placed; once in the enemy deployment zone, they divert towards the enemy HQ

    // private variables:
    //private GameObject _myOccupiedSpawnZones;
    List<SpawnZone> _myOccupiedSpawnZones = new List<SpawnZone>();
    private float _secondaryWeaponMultiplier;


    // TEMPORARILY, until army manager implemented:
    private DragNDrop _dragNDrop;
    private CameraController _cameraController;
    private GameManager _gameManager;
    private LevelArchitect _levelBuilder;


    /// <summary>
    /// When placing unit in the world, it gets initialized. Called by <see cref="GameManager"/> //the REF: UnitPlacement-script.
    /// </summary>
    public void InitializeUnit(int _playerID /*GameObject _spawnZoneOfPlacement*/ /*List<SpawnZone> occupiedZones*/)
    {
        // check if unit is set up correctly:
        if (!CheckCorrectUnitSetup())
        {
            Debug.LogError("ERROR: This unit is not set up correctly!", this.gameObject);
            return;
        }

        // TEMPORARILY REPLACING ARMY MANAGER: // moved to front otherwise its impossible to call the camera controller reference in time...
        _gameManager = FindAnyObjectByType<GameManager>();
        if (_playerID == 1)
        {
            _dragNDrop = FindAnyObjectByType<DragNDrop>(); // unsure why
            _cameraController = FindAnyObjectByType<CameraController>(); // register this unit for the camera to follow
            _levelBuilder = FindAnyObjectByType<LevelArchitect>(); // unsure why
        }

        // populate the unit profile:
        CompileUnitProfile();
        Debug.Log("3. foot w " + unitProfile.footprintWidth + " and footde " + unitProfile.footprintDepth);

        // set player-ID:
        myPlayerID = _playerID;

        // cache & initialize components and references:
        unitAnimator = GetComponent<Animator>();

        unitHealth = GetComponent<UnitHealth>();
        unitHealth.InitializeUnitHealth();

        _unitMovement = GetComponent<UnitMovement>();
        _unitMovement.InitializeUnitMovement(); // the movement shall get its destination from the army manager!!!

        _unitCombat = GetComponent<UnitCombat>();
        _unitCombat.InitializeUnitCombat();

        _unitAnimationController = GetComponent<UnitAnimationController>();
        _unitAnimationController.InitializeUnitAnimatonController();

        _unitDetection = GetComponent<UnitDetection>();
        _unitDetection.InitializeUnitDetection();

        // if this is a ranged unit, also initialize projectile-animation:
        if (unitProfile.mainItemType == ItemType.Bow || unitProfile.secondaryItemType == ItemType.Bow)
        {
            // NOTE: does not need to be referenced, as it's only ever accessed by the Animation Controller:
            GetComponent<AnimateProjectile>().InitializeProjectileAnimator();
        }

        //former DeployThisUnit-method:
        // if this is an actual unit, register it's location: 
        /*if (unitProfile.canBePlacedManually && !isActive)
        {
            _myOccupiedSpawnZones = _spawnZoneOfPlacement;
        }*/
        /*
        if (!isActive)
        {
            //_myOccupiedSpawnZones = _spawnZoneOfPlacement;
            foreach(var zone in occupiedZones)
            {
                _myOccupiedSpawnZones.Add(zone);
            }
        }*/

        if (unitProfile.unitType != UnitType.Building)
        {
            switch (_playerID)
            {
                case 0: // environment
                    // TBD...
                    return;

                case 1: // player 1
                    immediateUnitDestination = new Vector3(_gameManager.player2HQ.transform.position.x, 0, transform.position.z);
                    enemyHeadquartersLocation = _gameManager.player2HQ.transform.position;

                    return;

                case 2: // player 2
                    immediateUnitDestination = new Vector3(_gameManager.player1HQ.transform.position.x, 0, transform.position.z);
                    enemyHeadquartersLocation = _gameManager.player1HQ.transform.position;
                    return;

                case 3: // neutral
                    // TBD...
                    return;
            }
        }


        // launch units that weren't deployed, but existed from the beginning (e.g. HQ's, etc.):
        if (!unitProfile.canBePlacedManually)
        {
            LaunchUnit();
        }
    }


    public void UpdateZonesDeployedByUnit(List<SpawnZone> occupiedZones)
    {
        if (!isActive)
        {
            //_myOccupiedSpawnZones = _spawnZoneOfPlacement;
            foreach (var zone in occupiedZones)
            {
                _myOccupiedSpawnZones.Add(zone);
            }
        }
    }


    /// <summary>
    /// This function is called when the unit is picked up from a selecor-icon or 
    /// placed in a deploymentzone by an AI. The assigned ID is used to tell friend from foe.
    /// </summary>
    /// <param name="_playerID"></param>
    public void DeployThisUnit(int _playerID, GameObject _spawnZoneOfPlacement)
    {
        // if this is an actual unit, register it's location: 
        if (unitProfile.canBePlacedManually && !isActive)
        {
            //_myOccupiedSpawnZones = _spawnZoneOfPlacement; // depreciated; now part of initialization
        }

        if(unitProfile.unitType != UnitType.Building)
        {
            switch (_playerID)
            {
                case 0: // environment
                    // TBD...
                    return;

                case 1: // player 1
                    immediateUnitDestination = new Vector3(_gameManager.player2HQ.transform.position.x, 0, transform.position.z);
                    enemyHeadquartersLocation = _gameManager.player2HQ.transform.position;

                    return;

                case 2: // player 2
                    immediateUnitDestination = new Vector3(_gameManager.player1HQ.transform.position.x, 0, transform.position.z);
                    enemyHeadquartersLocation = _gameManager.player1HQ.transform.position;
                    return;

                case 3: // neutral
                    // TBD...
                    return;
            }
        }
    }

    
    /// <summary>
    /// Compile unit base-profile.
    /// </summary>
    void CompileUnitProfile()
    {
        // scriptable object data:
        unitProfile.unitData = unitData;

        // enums:
        unitProfile.race = unitData.race;
        unitProfile.unitType = unitData.unitType;
        unitProfile.size = unitData.size;
        unitProfile.mainItemType = unitData.mainHandItemData.itemType;
        unitProfile.mainItemGrip = unitData.mainHandItemData.itemGrip;
        unitProfile.secondaryItemType = unitData.secondaryItemData.itemType;
        unitProfile.secondaryItemGrip = unitData.secondaryItemData.itemGrip;

        // general information:
        unitProfile.unitName = unitData.name;
        unitProfile.deploymentCost = unitData.deploymentCost + unitData.armorData.deploymentCost + unitData.mainHandItemData.deploymentCost + unitData.secondaryItemData.deploymentCost;
        unitProfile.rewardValue = unitData.rewardForDestruction /*+  unitData.armorData.reward + unitData.mainWeaponData.reward + unitData.secondaryWeaponData.reward*/;
        unitProfile.carryCapacity = unitData.carryCapacity; // currently unused
        unitProfile.canBePlacedManually = unitData.canBePlacedManually;
        unitProfile.footprintWidth = unitData.footPrint.footprintWidth;
        unitProfile.footprintDepth = unitData.footPrint.footprintDepth;

        Debug.Log("1. foot w " + unitProfile.footprintWidth + " and footde " + unitProfile.footprintDepth);


        // check if this unit is able to attack terrain-features:
        if (unitData.mainHandItemData.canAttackTerrain || unitData.secondaryItemData.canAttackTerrain)
        {
            unitProfile.canAttackTerrain = true;
        }

        // health:
        unitProfile.healthPoints = unitData.healthPoints;
        unitProfile.armorValue = unitData.armorData.armorValue + unitData.mainHandItemData.armorValue + unitData.secondaryItemData.armorValue;

        // movement:
        unitProfile.walkingSpeed = unitData.walkingSpeed;
        unitProfile.runningSpeed = unitData.runningSpeed;
        unitProfile.chargingSpeed = unitData.chargingSpeed;
        unitProfile.size = unitData.size;

        // charge:
        unitProfile.chargeImpactDamage = unitData.chargeImpactDamage; // currently unused
        unitProfile.chargeImpactSplashRadius = unitData.chargeImpactSplashRadius; // currently unused 
        unitProfile.chargeImpactForce = unitData.chargeImpactForce; // currently unused

        // detection:
        unitProfile.detectionRange = unitData.detectionRange;
        unitProfile.alarmRange = unitData.alarmRange; // currently unused

        // combat:
        unitProfile.weaponDamage = unitData.mainHandItemData.damage;
        unitProfile.attackSpeed = unitData.mainHandItemData.attackSpeed;
        unitProfile.attackRange = unitData.mainHandItemData.attackRange;
        unitProfile.armorPenetrationValue = unitData.mainHandItemData.armorPenetrationValue + (_secondaryWeaponMultiplier * unitData.secondaryItemData.armorPenetrationValue); // currently unused
        unitProfile.toHitChance = unitData.toHitSkill + unitData.mainHandItemData.hitChance + (_secondaryWeaponMultiplier * unitData.secondaryItemData.hitChance); // currently unused
        unitProfile.criticalHitChance = unitData.toCritSkill + unitData.mainHandItemData.critChance + (_secondaryWeaponMultiplier * unitData.secondaryItemData.critChance); // currently unused

        unitProfile.splashRadius = unitData.mainHandItemData.splashRadius + (_secondaryWeaponMultiplier * unitData.secondaryItemData.splashRadius); // currently unused
        unitProfile.splashDamage = unitData.mainHandItemData.splashDamage + (_secondaryWeaponMultiplier * unitData.secondaryItemData.splashDamage); // currently unused

        unitProfile.bashChance = unitData.mainHandItemData.bashChance + (_secondaryWeaponMultiplier * unitData.secondaryItemData.bashChance); // currently unused
        unitProfile.bashForce = unitData.mainHandItemData.bashForceFactor + (_secondaryWeaponMultiplier * unitData.secondaryItemData.bashForceFactor); // currently unused
    }


    void OnMouseDown() // doesn't this need to be public?
    {
        // check if this object is meant to be picked up or if it has already been launched:
        if (unitProfile.canBePlacedManually && !isActive)
        {
            if (_dragNDrop != null && _dragNDrop.carriedObject == null)
            {
                _dragNDrop.PickUpAgain(this.gameObject);
                ResetThisUnit();
            }
        }
    }
    void ResetThisUnit()
    {
        // tell the occupied SpawnZone that the placed unit got picked up again:
        //_myOccupiedSpawnZones.GetComponent<SpawnZone>().VacateDeploymentTile();
        foreach(var zone in _myOccupiedSpawnZones)
        {
            zone.GetComponent<SpawnZone>().VacateDeploymentTile();
        }


        // tell this unit that it has no longer an assigned location/SpawnZone:
        _myOccupiedSpawnZones = null; // unsure if this will empty out the list as desired?!
    }



    /// <summary>
    /// This is called when a new wave is launched. Units in that wave can no longer be picked up. This also 
    /// tells the camera-follow-logic to consider this unit for following.
    /// </summary>
    public void LaunchUnit()
    {
        isActive = true;

        _unitMovement.StartMovement();
        unitHealth.LaunchHealthBar();
        _unitDetection.StartScanningForEnemies();

        if(myPlayerID == 1)
        {
            _cameraController.AddUnitToFollow(this.transform);
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
        if (_cameraController != null)
        {
            _cameraController.RemoveUnitToFollow(this.transform);
        }

        //DeactivateAllComponents();
        
        if (GetComponent<Collider>() != null && GetComponent<Collider>().enabled)
        {
            GetComponent<Collider>().enabled = false;
        }
        if (GetComponent<NavMeshAgent>() != null && GetComponent<NavMeshAgent>().enabled)
        {
            GetComponent<NavMeshAgent>().enabled = false;
        }
        /*if (GetComponent<UnitMovement>() != null && GetComponent<UnitMovement>().enabled)
        {
            GetComponent<UnitMovement>().enabled = false; 
        }
        if (GetComponent<UnitCombat>() != null && GetComponent<UnitCombat>().enabled)
        {
            GetComponent<UnitCombat>().enabled = false;
        }*/

        // end the game, if this was an HQ:
        if (CompareTag("HQ"))
        {
            FindAnyObjectByType<EndOfGame>().RoundFinished(myPlayerID);
        }
    }
    
    /// <summary>
    /// Called by <see cref="UnitHealth"/> when a unit gets killed, so the corpse can remain on the 
    /// battlefield until turnt off. This is required to manage the indivvidual behaviors.
    /// </summary>
    public void DeactivateAllComponents()
    {
        if (GetComponent<Collider>() != null && GetComponent<Collider>().enabled)
        {
            GetComponent<Collider>().enabled = false;
        }

        if (GetComponent<NavMeshAgent>() != null && GetComponent<NavMeshAgent>().enabled)
        {
            GetComponent<NavMeshAgent>().enabled = false;
        }

        if (GetComponent<UnitMovement>() != null && GetComponent<UnitMovement>().enabled)
        {
            GetComponent<UnitMovement>().enabled = false;
        }

        if (GetComponent<UnitCombat>() != null && GetComponent<UnitCombat>().enabled)
        {
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


    /// <summary>
    /// Check if this unit was set up correclty and holds all relevant components.
    /// </summary>
    /// <returns></returns>
    bool CheckCorrectUnitSetup()
    {
        int controllNr = 9; // add +1 for each new check
        int nrOfSuccessfullChecks = 0;

        #region SO-Setup check:
        if (unitData != null) 
        {
            nrOfSuccessfullChecks++;
        }else
        {
            Debug.LogError($"ERROR: {nameof(UnitDataSO)} reference missing", this.gameObject);
        }
        
        if (unitData.armorData != null)
        {
            nrOfSuccessfullChecks++;
        }else
        {
            Debug.LogError($"ERROR: {nameof(ArmorDataSO)} reference missing", this.gameObject);
        }
        
        if (unitData.mainHandItemData != null)
        {
            nrOfSuccessfullChecks++;
        }else
        {
            Debug.LogError($"ERROR: {nameof(unitData.mainHandItemData)} reference missing", this.gameObject);
        }
        
        if (unitData.secondaryItemData != null)
        {
            nrOfSuccessfullChecks++;
        }else
        {
            Debug.LogError($"ERROR: {nameof(unitData.secondaryItemData)} reference missing", this.gameObject);
        }
        #endregion
        #region Component-Setup check:
        if (GetComponent<Animator>())
        {
            nrOfSuccessfullChecks++;
        }else
        {
            Debug.LogError($"ERROR: {nameof(Animator)} reference missing", this.gameObject);
        }
        
        if (GetComponent<UnitHealth>())
        {
            nrOfSuccessfullChecks++;
        }else
        {
            Debug.LogError($"ERROR: {nameof(UnitHealth)} reference missing", this.gameObject);
        }

        if (GetComponent<UnitMovement>())
        {
            nrOfSuccessfullChecks++;
        }else
        {
            Debug.LogError($"ERROR: {nameof(UnitMovement)} reference missing", this.gameObject);
        }

        if (GetComponent<UnitDetection>())
        {
            nrOfSuccessfullChecks++;
        }else
        {
            Debug.LogError($"ERROR: {nameof(UnitDetection)} reference missing", this.gameObject);
        }

        if (GetComponent<UnitCombat>())
        {
            nrOfSuccessfullChecks++;
        }else
        {
            Debug.LogError($"ERROR: {nameof(UnitCombat)} reference missing", this.gameObject);
        }
        #endregion

        // return check result:
        if (nrOfSuccessfullChecks == controllNr)
        {
            return true;
        }else
        {
            return false;
        }
    }
}