using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth : MonoBehaviour
{ 
    // classID explanation: 0 = DAMAGEDEALER; 1 = TANK; 2 = SNIPER 

    int _myClassID = 0;
    //int _enemyClassID = 0;

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
}
