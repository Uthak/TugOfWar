using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Damage to objects is handled here.
/// </summary>
public class UnitHealth : MonoBehaviour
{
    public float baseHealthPoints = 0.0f; // public to communicate with health bar
    public float currentHealthPoints = 0.0f; // public to communicate with health bar
    public float mySize = 0.0f; // public to update the health bar height to acoount for large creatures/buildings:

    //bool _isActive = false;

    bool _isAlive = true;
    float _myArmorValue = 0.0f;
    float _goldRewardFactor = 0.0f;
    float _myRewardValue = 0.0f;

    ArmorDataSO.ArmorType _armorType;

    UnitManager _unitManager;
    UnitAnimationController _myUnitAnimationController;

    //UnitHealthBar _healthBar;

    // to manage this units health bar:
    HealthBarManager _healthBarManager;
    GoldManager _goldManager;
    ExperienceManager _experienceManager;
    GameObject _myHealthBar;


    /// <summary>
    /// Called by the UnitManager to initialize the units health.
    /// </summary>
    public void InitializeUnitHealth()
    {
        // cache component references:
        _unitManager = GetComponent<UnitManager>();
        _goldManager = FindAnyObjectByType<GoldManager>();
        //_healthBar = GetComponent<UnitHealthBar>();
        _healthBarManager = FindAnyObjectByType<HealthBarManager>();
        _experienceManager = FindAnyObjectByType<ExperienceManager>();

        // asign & populate variables:
        baseHealthPoints = _unitManager.baseHealthPoints;
        currentHealthPoints = baseHealthPoints;
        _myArmorValue = _unitManager.baseArmorValue;
        mySize = _unitManager.baseSize;
        _armorType = _unitManager.armorType; // currently not used...

        // calculate my value when killed:
        _goldRewardFactor = FindAnyObjectByType<GameManager>().goldRewardFactor; // currently not used!
        //_myRewardValue = _unitManager.baseDeploymentCost / _goldRewardFactor;
        _myRewardValue = _unitManager.baseRewardValue;

        // NOTE: the unit health bar is initialized and managed by hte HealthBarManager,
        // a managing component of the Game Manager.

        _myUnitAnimationController = GetComponent<UnitAnimationController>();

        //_isActive = true;

        //initialize health-bar:
        //_healthBar.InitializeHealthBar(_baseHealthPoints);
        //_healthBar.InitializeUnitHealthBar();
    }

    /// <summary>
    /// This is currently not being used for anything. My idea was to optimize the code by updating 
    /// health bar values from each respective UnitHealth script, instead of an update inside the
    /// HealthBarManager (as values don't change that frequently.
    /// </summary>
    /// <param name="_correspondingHealthBar"></param>
    public void GiveHealthBarToUnitHealthScript(GameObject _correspondingHealthBar)
    {
        _myHealthBar = _correspondingHealthBar;
    }

    public void LaunchHealthBar()
    {
        _healthBarManager.RegisterUnit(this);
    }

    /*
    void OnDestroy() // somewhat doubles with the Kill function down at the bottom...
    {
        _healthBarManager.UnregisterUnit(this);
    }*/

    /// <summary>
    /// Target unit takes damage after considering its armor and the attacks penetration value.
    /// </summary>
    /// <param name="_rawIncomingDamage"></param>
    /// <param name="_penetrationValueOfAttack"></param>
    public void TakeDamage(float _rawIncomingDamage, float _penetrationValueOfAttack, int idOfAttacker)
    {
        // animate damage taken:
        _myUnitAnimationController.TakeDamageAnimation();

        // calculate the armor efficiency to get the effective damage of this attack:
        float _armorEfficiency = CombatCalculator.CalculateArmorEfficiency(_myArmorValue, _penetrationValueOfAttack);
        //float _effectiveDamage = _rawIncomingDamage * (1 - CalculateArmorEfficiency(_penetrationValueOfAttack));
        float _effectiveDamage = _rawIncomingDamage * (1 - _armorEfficiency);

        // subtract the effective damage from current health points:
        currentHealthPoints -= _effectiveDamage;

        // update health bar:
        //_healthBar.UpdateUnitHealthBar(currentHealthPoints);

        //Debug.Log("I, " + this.gameObject.name + " took damage and have this much: " + currentHealthPoints + " left");


        if (currentHealthPoints <= 0.0f && _isAlive)
        {
            _isAlive = false;

            StartCoroutine(DeathSequence(idOfAttacker));
        }
    }


    /* // Moved this to a static!
    /// <summary>
    /// Armor can reduce incoming damage by a maximum of 75%. This can be lowered to 0%, however 
    /// overpenetration has no effect. We assume armor and penetration range from 0 - 10 for this to work.
    /// </summary>
    /// <param name="_penetrationValueOfAttack"></param>
    /// <returns></returns>
    float CalculateArmorEfficiency(float _penetrationValueOfAttack)
    {
        // subtract penetration value from armor value:
        float _effectiveArmor = _armorValue - _penetrationValueOfAttack;

        // convert effective armor to a percentage of damage blocked
        // each point of armor corresponds to 10% damage reduction:
        float _blockedDamagePercentage = _effectiveArmor * 10.0f;

        // clamp the blocked damage percentage to a maximum of 75%:
        _blockedDamagePercentage = Mathf.Clamp(_blockedDamagePercentage, 0.0f, 75.0f);

        // convert the percentage to a fraction (for example, 75% becomes 0.75):
        float _blockedDamageFraction = _blockedDamagePercentage / 100.0f;

        return _blockedDamageFraction;
    }*/
    /*
    void Die()
    {
        // the death-coroutine is handled by the Unit AnimationController
        
        //_myUnitAnimationController.DeathAnimation();
    }*/
    /*
    IEnumerator Die()
    {
        // animate death:
        _myUnitAnimationController.DieAnimation();

        yield return new WaitForSeconds(_attackSpeed);

    }*/

    /*
    public void RemoveUnit()
    {
        _healthBarManager.UnregisterUnit(this);

        _unitManager.SignOffThisUnit();
    }*/

    public IEnumerator DeathSequence(int idOfAttacker)
    {
        // currently disabled, could be interesting to use for performance-settings! 
        #region Destroy Unit immediatly after Death Animation:
        // use a callback to handle the animation length:
        bool isLengthRetrieved = false;
        float lengthOfDeathAnimation = 0f;

        _myUnitAnimationController.DeathAnimation(length =>
        {
            lengthOfDeathAnimation = length;
            isLengthRetrieved = true;
        });

        // wait until the length is retrieved:
        yield return new WaitUntil(() => isLengthRetrieved);

        // wait for the length of the death animation:
        yield return new WaitForSeconds(lengthOfDeathAnimation);
        #endregion

        // reward xp to the killer:
        _experienceManager.GainExperience(idOfAttacker, _unitManager.baseDeploymentCost);

        // reward money to the killer:
        _goldManager.AddGold(idOfAttacker, _myRewardValue);

        // turn off health bar:
        _healthBarManager.UnregisterUnit(this);

        // shut down all functions:
        _unitManager.DisableUnfollowAndDisconnectUnit();

        yield return new WaitForSeconds(60.0f); // this time should be managed by performance-settings!

        // delete unit entirely, if desired:
        _unitManager.DestroyUnit();
    }

    /*
    IEnumerator DeathSequence()
    {
        _healthBarManager.UnregisterUnit(this);
        _unitManager.SignOffThisUnit();
        _myUnitAnimationController.DeathAnimation(out float lengthOfDeathAnimation);

        yield return new WaitForSeconds(lengthOfDeathAnimation);

        _unitManager.KillUnit();
    }*/
} 


/*
// delete this later! *********************

    // temporarily disabled to test SO scripts!*****************
    /
    public void UpdateUnitHealth()
    {
        _myClassID = GetComponent<UnitManager>().myClassID;
        _currentHealthPoints = GetComponent<UnitManager>().healthPoints;
        _myArmorValue = GetComponent<UnitManager>().armorValue;

        GetComponent<UnitHealthBar>().UpdateUnitHealthBar();
    }

    public void TakeDamage(float _rawIncomingDamage, float _penetrationValueOfAttack, int _enemyClassID)
    {
        //Debug.Log("incoming dmg " + _rawIncomingDamage);

        CalculateEffectiveDamage(_rawIncomingDamage, _penetrationValueOfAttack, _enemyClassID, out float _effectiveDamage);

        _currentHealthPoints -= _effectiveDamage;
        //Debug.Log("current healthpoints" + currentHealthPoints + " current dmg taken" + _effectiveDamage);

        if (_currentHealthPoints <= 0.0f)
        {
            Debug.Log("I shouldve died");
            Die();
        }
    }
    void CalculateEffectiveDamage(float _rawIncomingDamage, float _penetrationValueOfAttack, int _enemyClassID, out float _effectiveDamage)
    {
        //Debug.Log("my class: " + _myClassID + " attackers class: " + _enemyClassID);
        float _calculatedDamage = _rawIncomingDamage; // get base damage

        GetDamageEffectiveness(_enemyClassID, out float _damageEffectivenessMultiplier);
        //Debug.Log("the effectiveness of this incoming attack: " + _damageEffectivenessMultiplier);
        _calculatedDamage *= _damageEffectivenessMultiplier;

        // armor is applied AFTER class-effectiveness:
        GetArmorEffectiveness(_penetrationValueOfAttack, out float _armorEffectivenessMultiplier);
        //Debug.Log("the damage reduction of this attack through my armor: " + _myArmorValue + " against the attacks pen value of: "+ _penetrationValueOfAttack + " is: " + _armorEffectivenessMultiplier);
        _calculatedDamage *= _armorEffectivenessMultiplier;

        _effectiveDamage = _calculatedDamage;
        //Debug.Log("I took effective damage: " + _effectiveDamage);
    }
    void GetDamageEffectiveness(int _enemyClassID, out float _effectivenessMultiplier)
    {
        _effectivenessMultiplier = 0.0f; // default value

        switch (_myClassID)
        {
            case 0: // I am a DD:
                switch (_enemyClassID)
                {
                    case 0: // taking damage from a DD!
                        _effectivenessMultiplier = 1.0f; // same class = no modifier
                        break;
                    case 1: // taking damage from a TANK!
                        _effectivenessMultiplier = 0.5f; // the damage is NOT effective
                        break;
                    case 2: // taking damage from a SNIPER!
                        _effectivenessMultiplier = 1.5f; // the damage is Very effective
                        break;
                }
                break;

            case 1: // I am a TANK:
                switch (_enemyClassID)
                {
                    case 0: // taking damage from a DD!
                        _effectivenessMultiplier = 1.5f; // the damage is Very effective
                        break;
                    case 1: // taking damage from a TANK!
                        _effectivenessMultiplier = 1.0f; // same class = no modifier
                        break;
                    case 2: // taking damage from a SNIPER!
                        _effectivenessMultiplier = 0.5f; // the damage is NOT effective
                        break;
                }
                break;

            case 2: // I am a SNIPER:
                switch (_enemyClassID)
                {
                    case 0: // taking damage from a DD!
                        _effectivenessMultiplier = 0.5f; // the damage is NOT effective
                        break;
                    case 1: // taking damage from a TANK!
                        _effectivenessMultiplier = 1.5f; // the damage is Very effective
                        break;
                    case 2: // taking damage from a SNIPER!
                        _effectivenessMultiplier = 1.0f; // same class = no modifier
                        break;
                }
                break;

            default: // for any undefined units
                _effectivenessMultiplier = 1.0f;
                break;
        }
    }

    // currently armor can reduce a maximum of up to 75% damage
    // currently over-penetrating the armorValue of a target is pointless
    // math based on armor- & pen-values between 0 and 10, which get multiplied by 10 for calculation
    void GetArmorEffectiveness(float _penetrationValueOfAttack, out float _armorEffectivenessMultiplier)
    {
        _armorEffectivenessMultiplier = 1.0f; // default = armor at full effect:

        // subtract enemies penetration from my armor:
        float _effectiveArmor = (_myArmorValue - _penetrationValueOfAttack) * 10;
        float _clampedEffectiveArmor = Mathf.Clamp(_effectiveArmor, 0.0f, 75.0f);
        float _effecitveArmorInPercentile = _clampedEffectiveArmor / 100;
        _armorEffectivenessMultiplier -= _effecitveArmorInPercentile;

        // example:
        // armor 55 and inc pen 32 
        // 55-32 = 23 _effectiveArmor
        // clamped to 23
        // percentile of 0.23
        // base effectiveness (1) - modified effectiveness = damage reduced by 23% or: 0.77% damage taken
    }


    float awardedMoney = 0.0f;
    void Die()
    {
        // temporary solution to controll camera:
        if (teamAffiliation == 1) // team 1 is active player:
        {
            FindAnyObjectByType<CameraController>().RemoveUnit(this.transform);
        }

        Debug.Log("i died and my team affil was: " + teamAffiliation);

        // reward the victor 1/4 of the killed units deployment cost:
        switch (teamAffiliation)
        {
            case 1:
                FindAnyObjectByType<GoldManager>().AddGold(2, GetComponent<UnitManager>().deploymentCost / 4.0f);
                Debug.Log("player 2 (AI) should have received: " + GetComponent<UnitManager>().deploymentCost / 4.0f + " gold.");

                break;

            case 2:
                FindAnyObjectByType<GoldManager>().AddGold(1, GetComponent<UnitManager>().deploymentCost / 4.0f);
                Debug.Log("player 1 (you) should have received: " + GetComponent<UnitManager>().deploymentCost / 4.0f + " gold.");

                break;
        }

        awardedMoney = awardedMoney + 2.5f;

        Destroy(this.gameObject);

        Debug.Log("awarded money = " + awardedMoney);

    }
} */












/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth : MonoBehaviour
{ 
    // classID explanation: 0 = DAMAGEDEALER; 1 = TANK; 2 = SNIPER 

    int _myClassID = 0;
    public float currentHealthPoints = 1f;
    float _myArmorValue = 0f;
    //float _enemyArmorPenetrationValue = 0f;

    // temporary solution to controll camera:
    public int teamAffiliation = 0;


    public void UpdateUnitHealth()
    {
        _myClassID = GetComponent<UnitManager>().myClassID;
        currentHealthPoints = GetComponent<UnitManager>().healthPoints;
        _myArmorValue = GetComponent<UnitManager>().armorValue;

        GetComponent<UnitHealthBar>().UpdateUnitHealthBar();
    }

    public void TakeDamage(float _rawIncomingDamage, float _penetrationValueOfAttack, int _enemyClassID)
    {
        //Debug.Log("incoming dmg " + _rawIncomingDamage);

        CalculateEffectiveDamage(_rawIncomingDamage, _penetrationValueOfAttack, _enemyClassID, out float _effectiveDamage);

        currentHealthPoints -= _effectiveDamage;
        //Debug.Log("current healthpoints" + currentHealthPoints + " current dmg taken" + _effectiveDamage);

        if ( currentHealthPoints <= 0.0f)
        {
            Debug.Log("I shouldve died");
            Die();
        }
    }
    void CalculateEffectiveDamage(float _rawIncomingDamage, float _penetrationValueOfAttack, int _enemyClassID, out float _effectiveDamage)
    {
        //Debug.Log("my class: " + _myClassID + " attackers class: " + _enemyClassID);
        float _calculatedDamage = _rawIncomingDamage; // get base damage

        GetDamageEffectiveness(_enemyClassID, out float _damageEffectivenessMultiplier);
        //Debug.Log("the effectiveness of this incoming attack: " + _damageEffectivenessMultiplier);
        _calculatedDamage *= _damageEffectivenessMultiplier;

        // armor is applied AFTER class-effectiveness:
        GetArmorEffectiveness(_penetrationValueOfAttack, out float _armorEffectivenessMultiplier);
        //Debug.Log("the damage reduction of this attack through my armor: " + _myArmorValue + " against the attacks pen value of: "+ _penetrationValueOfAttack + " is: " + _armorEffectivenessMultiplier);
        _calculatedDamage *= _armorEffectivenessMultiplier;

        _effectiveDamage = _calculatedDamage;
        //Debug.Log("I took effective damage: " + _effectiveDamage);
    }
    void GetDamageEffectiveness(int _enemyClassID, out float _effectivenessMultiplier)
    {
        _effectivenessMultiplier = 0.0f; // default value

        switch (_myClassID)
        {
            case 0: // I am a DD:
                switch (_enemyClassID)
                {
                    case 0: // taking damage from a DD!
                        _effectivenessMultiplier = 1.0f; // same class = no modifier
                        break;
                    case 1: // taking damage from a TANK!
                        _effectivenessMultiplier = 0.5f; // the damage is NOT effective
                        break;
                    case 2: // taking damage from a SNIPER!
                        _effectivenessMultiplier = 1.5f; // the damage is Very effective
                        break;
                }
                break;

            case 1: // I am a TANK:
                switch (_enemyClassID)
                {
                    case 0: // taking damage from a DD!
                        _effectivenessMultiplier = 1.5f; // the damage is Very effective
                        break;
                    case 1: // taking damage from a TANK!
                        _effectivenessMultiplier = 1.0f; // same class = no modifier
                        break;
                    case 2: // taking damage from a SNIPER!
                        _effectivenessMultiplier = 0.5f; // the damage is NOT effective
                        break;
                }
                break;

            case 2: // I am a SNIPER:
                switch (_enemyClassID)
                {
                    case 0: // taking damage from a DD!
                        _effectivenessMultiplier = 0.5f; // the damage is NOT effective
                        break;
                    case 1: // taking damage from a TANK!
                        _effectivenessMultiplier = 1.5f; // the damage is Very effective
                        break;
                    case 2: // taking damage from a SNIPER!
                        _effectivenessMultiplier = 1.0f; // same class = no modifier
                        break;
                }
                break;

            default: // for any undefined units
                _effectivenessMultiplier = 1.0f;
                break;
        }
    }

    // currently armor can reduce a maximum of up to 75% damage
    // currently over-penetrating the armorValue of a target is pointless
    // math based on armor- & pen-values between 0 and 10, which get multiplied by 10 for calculation
    void GetArmorEffectiveness(float _penetrationValueOfAttack, out float _armorEffectivenessMultiplier)
    {
        _armorEffectivenessMultiplier = 1.0f; // default = armor at full effect:

        // subtract enemies penetration from my armor:
        float _effectiveArmor = (_myArmorValue - _penetrationValueOfAttack) * 10;
        float _clampedEffectiveArmor = Mathf.Clamp(_effectiveArmor, 0.0f, 75.0f);
        float _effecitveArmorInPercentile = _clampedEffectiveArmor / 100;
        _armorEffectivenessMultiplier -= _effecitveArmorInPercentile;

        // example:
        // armor 55 and inc pen 32 
        // 55-32 = 23 _effectiveArmor
        // clamped to 23
        // percentile of 0.23
        // base effectiveness (1) - modified effectiveness = damage reduced by 23% or: 0.77% damage taken
    }


    float awardedMoney = 0.0f;
    void Die()
    {
        // temporary solution to controll camera:
        if (teamAffiliation == 1) // team 1 is active player:
        {
            FindAnyObjectByType<CameraController>().RemoveUnit(this.transform);
        }

        Debug.Log("i died and my team affil was: " + teamAffiliation);

        // reward the victor 1/4 of the killed units deployment cost:
        switch (teamAffiliation)
        {
            case 1:
                FindAnyObjectByType<GoldManager>().AddGold(2, GetComponent<UnitManager>().deploymentCost / 4.0f);
                Debug.Log("player 2 (AI) should have received: " + GetComponent<UnitManager>().deploymentCost / 4.0f + " gold.");

                break;

            case 2:
                FindAnyObjectByType<GoldManager>().AddGold(1, GetComponent<UnitManager>().deploymentCost / 4.0f);
                Debug.Log("player 1 (you) should have received: " + GetComponent<UnitManager>().deploymentCost / 4.0f + " gold.");

                break;
        }
        
        awardedMoney = awardedMoney + 2.5f;

        Destroy(this.gameObject);

        Debug.Log("awarded money = " + awardedMoney);
        
    }
}*/
