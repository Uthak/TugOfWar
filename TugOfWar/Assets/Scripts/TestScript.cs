using UnityEngine;
using UnityEngine.AI;

public class TestScript : MonoBehaviour
{/*
    [Header("Unit Setup:")]
    public string unitName;
    public int playerAffiliation = 0;
    public GameObject myLocation;
    public bool wasLaunched = false;
    float _offHandModifier = 0.5f; // half the impact of the offhand weapon

    // data from scriptable objects:
    public UnitDataSO unitData;
    public ArmorDataSO armorData;
    public WeaponDataSO mainWeaponData;
    public WeaponDataSO offWeaponData;

    // cached component references
    private UnitHealth unitHealth;
    private UnitMovement unitMovement;
    private UnitCombat unitCombat;

    #region Statline:
    [SerializeField] bool _reportThisUnitsStatline = false;

    UnitDataSO.Race _race;
    UnitDataSO.UnitType _unitType;
    ArmorDataSO.ArmorType _armorType;
    WeaponDataSO.WeaponType _mainHandWeaponType;
    WeaponDataSO.WeaponType _offHandWeaponType;

    // general:
    float _deploymentCost = 0.0f;
    float _carryCapacity = 0.0f;

    // health:
    float _healthPoints = 0.0f;

    // movement:
    float _movementSpeed = 0.0f;
    float _weigth = 0.0f;
    float _chargeSpeedMultiplier = 0.0f;

    // combat:
    int _weaponCount = 0;
    float _spottingRange = 0.0f;
    float _alarmRange = 0.0f;
    // --> charge:
    float _chargeImpactDamage = 0.0f;
    float _chargeImpactSplashRadius = 0.0f;
    float _chargeImpactForce = 0.0f;
    // --> damage:
    float _damage = 0.0f;
    float _attackSpeed = 0.0f;
    float _attackRange = 0.0f;
    float _weaponWeight = 0.0f;
    float _armorPenetrationValue = 0.0f;
    float _criticalChance = 0.0f;
    float _splashRadius = 0.0f;
    float _splashDamage = 0.0f;
    #endregion

    private void Awake()
    {
        // cache references to components
        unitHealth = GetComponent<UnitHealth>();
        unitMovement = GetComponent<UnitMovement>();
        unitCombat = GetComponent<UnitCombat>();

        CompileUnitStatline();
        // setup this unit:
        InitializeUnit(); // not ideal, this would happen before the unit is placed!
    }

    /// <summary>
    /// Compile and print the unit's stat line if desired.
    /// </summary>
    void CompileUnitStatline()
    {
        // general:
        _race = unitData.race;
        _unitType = unitData.unitType;
        _armorType = armorData.armorType;
        _mainHandWeaponType = mainWeaponData.weaponType;
        _offHandWeaponType = offWeaponData.weaponType;
        _deploymentCost = unitData.deploymentCost + armorData.additionalDeploymentCost 
            + mainWeaponData.additionalDeploymentCost + offWeaponData.additionalDeploymentCost;
        _carryCapacity = unitData.carryCapacity;

        // health:
        _healthPoints = unitData.healthPoints;

        // movement:
        _movementSpeed = unitData.movementSpeed; // impairment by weight needs to be added here!
        _weigth = armorData.armorWeight + mainWeaponData.weaponWeight + offWeaponData.weaponWeight;
        _chargeSpeedMultiplier = unitData.chargeSpeedMultiplier; // impairment by weight needs to be added here!

        // combat:
        if(mainWeaponData != null)
        {
            _weaponCount++;
            if(offWeaponData != null)
            {
                _weaponCount++;
            }
        }else if(offWeaponData != null)
        {
            _weaponCount++;
            if (mainWeaponData != null)
            {
                _weaponCount++;
            }
        }
        _spottingRange = unitData.spottingRange;
        _alarmRange = unitData.alarmRange;
        // --> charge:
        _chargeImpactDamage = unitData.chargeImpactDamage; // weight * speed could be an interesting modifier here!
        _chargeImpactSplashRadius = unitData.chargeImpactSplashRadius; // weight * speed could be an interesting modifier here!
        _chargeImpactForce = unitData.chargeImpactForce; // weight * speed could be an interesting modifier here!
        // --> damage:
        _damage = mainWeaponData.damage + (offWeaponData.damage * _offHandModifier);
        _attackSpeed = mainWeaponData.attackSpeed + (offWeaponData.attackSpeed * _offHandModifier); // this should increase, not decrease!
        _attackRange = Mathf.Max(mainWeaponData.attackRange, offWeaponData.attackRange);
        _armorPenetrationValue = mainWeaponData.armorPenetrationValue + (offWeaponData.armorPenetrationValue * _offHandModifier);
        _criticalChance = mainWeaponData.criticalHitFactor + (offWeaponData.criticalHitFactor * _offHandModifier);
        _splashRadius = Mathf.Max(mainWeaponData.splashRadius + offWeaponData.splashRadius * _offHandModifier);
        _splashDamage = mainWeaponData.splashDamage + (offWeaponData.splashDamage * _offHandModifier);

        if (_reportThisUnitsStatline)
        {
            Debug.Log(
            $"Unit Stat Card:\n" +
            $"Race: {_race}\n" +
            $"Unit Type: {_unitType}\n" +
            $"Armor Type: {_armorType}\n" +
            $"Main Hand Weapon Type: {_mainHandWeaponType}\n" +
            $"Off Hand Weapon Type: {_offHandWeaponType}\n" +
            $"Deployment Cost: {_deploymentCost}\n" +
            $"Carry Capacity: {_carryCapacity}\n\n" +

            $"Health Points: {_healthPoints}\n\n" +

            $"Movement Speed: {_movementSpeed}\n" +
            $"Weight: {_weigth}\n" +
            $"Charge Speed Multiplier: {_chargeSpeedMultiplier}\n\n" +

            $"Weapon Count: {_weaponCount}\n" +
            $"Spotting Range: {_spottingRange}\n" +
            $"Alarm Range: {_alarmRange}\n\n" +

            $"Charge Impact Damage: {_chargeImpactDamage}\n" +
            $"Charge Impact Splash Radius: {_chargeImpactSplashRadius}\n" +
            $"Charge Impact Force: {_chargeImpactForce}\n\n" +

            $"Damage: {_damage}\n" +
            $"Attack Speed: {_attackSpeed}\n" +
            $"Attack Range: {_attackRange}\n" +
            $"Armor Penetration Value: {_armorPenetrationValue}\n" +
            $"Critical Chance: {_criticalChance}\n" +
            $"Splash Radius: {_splashRadius}\n" +
            $"Splash Damage: {_splashDamage}\n");
        }
    }

    /// <summary>
    /// This function is called when the unit is picked up from a selecor-icon or 
    /// placed in a deploymentzone by an AI. The assigned ID is used to tell friend from foe.
    /// </summary>
    /// <param name="_playerID"></param>
    public void AssignPlayerID(int _playerID)
    {
        switch (_playerID)
        {
            case 1: // player 1:
                playerAffiliation = _playerID;
                break;

            case 2: // player 2:
                playerAffiliation = _playerID;
                break;

            default: // no player affiliation:
                Debug.LogError("ERROR: Unit without player affiliation detected!", this);
                break;
        }
    }*/
    /*
    private void Start()
    {
        if (unitData.unitType == UnitType.Building)
        {
            SetupThisUnit(true, this.gameObject);
            unitHealth.UpdateUnitHealth();
        }
    }*/
    /*
    /// <summary>
    /// Initialize attributes from the attached scriptable objects to the cached components.
    /// </summary>
    private void InitializeUnit()
    {
        // Initialize attributes from unitData
        if (unitData != null)
        {
            // assign unit-name if none is given:
            if(unitName == "")
            {
                unitName = unitData.unitName;
            }





            /*
            unitHealth.InitializeUnitHealth(unitData, armorData);
            unitMovement.InitializeUnitMovement(unitData, armorData, mainWeaponData, offWeaponData);
            unitCombat.InitializeCombatStats(mainWeaponData, offWeaponData);
        }
        else
        {
            Debug.LogError("ERROR: Unit without UnitDataSO detected!", this);
        }
    }

    public void SetupThisUnit(bool _placedByPlayer, GameObject _spawnZoneOfPlacement)
    {
        //if (unitData.unitType != UnitType.Building)
        //{
            myLocation = _spawnZoneOfPlacement;
            playerAffiliation = _placedByPlayer ? 1 : 2;

            unitHealth.teamAffiliation = playerAffiliation;
        */
            //unitMovement.UpdateUnitMovement();
            //unitCombat.UpdateUnitCombat();
            //unitHealth.UpdateUnitHealth();
        /*}
        else
        {
            Debug.Log($"I am a castle, I have {unitData.healthPoints} and belong to player {playerAffiliation}");
        }*/
    }










    /*
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
