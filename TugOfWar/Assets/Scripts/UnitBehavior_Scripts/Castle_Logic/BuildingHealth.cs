using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHealth : MonoBehaviour
{
    [Header("Building Health Setup:")]
    public int playerAffiliation = 0;
    public int myClassID = 0;
    public float currentHealthPoints = 1f;
    public float armorValue = 0f;

    /*
    public void UpdateUnitHealth()
    {
        myClassID = GetComponent<UnitManager>().myClassID;
        currentHealthPoints = GetComponent<UnitManager>().healthPoints;
        armorValue = GetComponent<UnitManager>().armorValue;

        GetComponent<UnitHealthBar>().UpdateUnitHealthBar();
    }*/

    /// <summary>
    /// The attackers <see cref="UnitCombat"/> will call this function, delivering it's own statline.
    /// </summary>
    /// <param name="_rawIncomingDamage"></param>
    /// <param name="_penetrationValueOfAttack"></param>
    /// <param name="_enemyClassID"></param>
    public void TakeDamage(float _rawIncomingDamage, float _penetrationValueOfAttack, int _enemyClassID)
    {
        CalculateEffectiveDamage(_rawIncomingDamage, _penetrationValueOfAttack, _enemyClassID, out float _effectiveDamage);

        currentHealthPoints -= _effectiveDamage;

        if (currentHealthPoints <= 0.0f)
        {
            Die();
        }
    }
    void CalculateEffectiveDamage(float _rawIncomingDamage, float _penetrationValueOfAttack, int _enemyClassID, out float _effectiveDamage)
    {
        float _calculatedDamage = _rawIncomingDamage; // get base damage

        GetDamageEffectiveness(_enemyClassID, out float _damageEffectivenessMultiplier);
        _calculatedDamage *= _damageEffectivenessMultiplier;

        // armor is applied AFTER class-effectiveness:
        GetArmorEffectiveness(_penetrationValueOfAttack, out float _armorEffectivenessMultiplier);
        _calculatedDamage *= _armorEffectivenessMultiplier;

        _effectiveDamage = _calculatedDamage;
    }
    void GetDamageEffectiveness(int _enemyClassID, out float _effectivenessMultiplier)
    {
        _effectivenessMultiplier = 0.0f; // default value

        switch (myClassID)
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
        float _effectiveArmor = (armorValue - _penetrationValueOfAttack) * 10;
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

    void Die()
    {
        // disable all input or pause the game here!

        /*
        switch (playerAffiliation)
        {
            case 1:
                // EndGame(victory);
                break;

            case 2:
                // EndGame(defeat);
                break;
        }*/

        Debug.Log("Player " + playerAffiliation + " lost the game!");

        Destroy(this.gameObject);
    }
}
