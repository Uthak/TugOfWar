using UnityEngine; // Assuming you are using Unity for Mathf.Clamp

public class CombatCalculator
{
    static float noDamage = 0.0f;
    static float bad = -0.125f;
    static float normal = 0.0f;
    static float good = 0.125f;
    static float optimal = 0.25f;

    /// <summary>
    /// Check how effective the attackin weapon is against the targeted unit Type.
    /// </summary>
    /// <param name="_myWeaponType"></param>
    /// <param name="_targetUnitType"></param>
    /// <returns></returns>
    public static float GetWeaponInfluence(WeaponDataSO.WeaponType _myWeaponType ,UnitDataSO.UnitType _targetUnitType)
    {
        float _weaponEfficiency = 0.0f;

        switch (_targetUnitType)
        {
            case UnitDataSO.UnitType.Peasant: // I'm attacking this unit...
                switch (_myWeaponType)
                {
                    case WeaponDataSO.WeaponType.Unarmed: // ...using this weapon!
                        return CombatCalculator.noDamage;
                        //return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Tool:
                        _weaponEfficiency = CombatCalculator.bad;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Sword:
                        _weaponEfficiency = CombatCalculator.normal;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Axe:
                        _weaponEfficiency = CombatCalculator.normal;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Mace:
                        _weaponEfficiency = CombatCalculator.normal;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Spear:
                        _weaponEfficiency = CombatCalculator.normal;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Lance:
                        _weaponEfficiency = CombatCalculator.optimal;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Bow:
                        _weaponEfficiency = CombatCalculator.normal;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Warmachine:
                        _weaponEfficiency = CombatCalculator.bad;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.MagicalAttack:
                        _weaponEfficiency = CombatCalculator.normal;
                        return _weaponEfficiency;

                    default:
                        return CombatCalculator.noDamage;
                }

            case UnitDataSO.UnitType.Soldier:
                switch (_myWeaponType)
                {
                    case WeaponDataSO.WeaponType.Unarmed:
                        _weaponEfficiency = CombatCalculator.noDamage;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Tool:
                        _weaponEfficiency = CombatCalculator.bad;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Sword:
                        _weaponEfficiency = CombatCalculator.normal;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Axe:
                        _weaponEfficiency = CombatCalculator.normal;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Mace:
                        _weaponEfficiency = CombatCalculator.normal;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Spear:
                        _weaponEfficiency = CombatCalculator.normal;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Lance:
                        _weaponEfficiency = CombatCalculator.optimal;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Bow:
                        _weaponEfficiency = CombatCalculator.normal;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Warmachine:
                        _weaponEfficiency = CombatCalculator.bad;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.MagicalAttack:
                        _weaponEfficiency = CombatCalculator.normal;
                        return _weaponEfficiency;

                    default:
                        return CombatCalculator.noDamage;
                }

            case UnitDataSO.UnitType.LightCavalry:
                switch (_myWeaponType)
                {
                    case WeaponDataSO.WeaponType.Unarmed:
                        _weaponEfficiency = CombatCalculator.noDamage;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Tool:
                        _weaponEfficiency = CombatCalculator.bad;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Sword:
                        _weaponEfficiency = CombatCalculator.bad;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Axe:
                        _weaponEfficiency = CombatCalculator.bad;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Mace:
                        _weaponEfficiency = CombatCalculator.bad;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Spear:
                        _weaponEfficiency = CombatCalculator.optimal;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Lance:
                        _weaponEfficiency = CombatCalculator.normal;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Bow:
                        _weaponEfficiency = CombatCalculator.normal;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Warmachine:
                        _weaponEfficiency = CombatCalculator.bad;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.MagicalAttack:
                        _weaponEfficiency = CombatCalculator.normal;
                        return _weaponEfficiency;

                    default:
                        return CombatCalculator.noDamage;
                }

            case UnitDataSO.UnitType.HeavyCavalry:
                switch (_myWeaponType)
                {
                    case WeaponDataSO.WeaponType.Unarmed:
                        _weaponEfficiency = CombatCalculator.noDamage;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Tool:
                        _weaponEfficiency = CombatCalculator.bad;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Sword:
                        _weaponEfficiency = CombatCalculator.bad;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Axe:
                        _weaponEfficiency = CombatCalculator.bad;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Mace:
                        _weaponEfficiency = CombatCalculator.good; // maces can dent in or puncture armor
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Spear:
                        _weaponEfficiency = CombatCalculator.optimal;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Lance:
                        _weaponEfficiency = CombatCalculator.normal;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Bow:
                        _weaponEfficiency = CombatCalculator.normal;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Warmachine:
                        _weaponEfficiency = CombatCalculator.bad;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.MagicalAttack:
                        _weaponEfficiency = CombatCalculator.normal;
                        return _weaponEfficiency;

                    default:
                        return CombatCalculator.noDamage;
                }

            case UnitDataSO.UnitType.Warmachine:
                switch (_myWeaponType)
                {
                    case WeaponDataSO.WeaponType.Unarmed:
                        _weaponEfficiency = CombatCalculator.noDamage;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Tool:
                        _weaponEfficiency = CombatCalculator.bad;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Sword:
                        _weaponEfficiency = CombatCalculator.normal;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Axe:
                        _weaponEfficiency = CombatCalculator.normal;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Mace:
                        _weaponEfficiency = CombatCalculator.normal;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Spear:
                        _weaponEfficiency = CombatCalculator.normal;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Lance:
                        _weaponEfficiency = CombatCalculator.normal;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Bow:
                        _weaponEfficiency = CombatCalculator.bad; // arrows wont do much to an artillery-piece
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Warmachine:
                        _weaponEfficiency = CombatCalculator.good;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.MagicalAttack:
                        _weaponEfficiency = CombatCalculator.normal;
                        return _weaponEfficiency;

                    default:
                        return CombatCalculator.noDamage;
                }

            case UnitDataSO.UnitType.Building:
                switch (_myWeaponType)
                {
                    case WeaponDataSO.WeaponType.Unarmed:
                        _weaponEfficiency = CombatCalculator.noDamage;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Tool:
                        _weaponEfficiency = CombatCalculator.optimal;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Sword:
                        _weaponEfficiency = CombatCalculator.bad;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Axe:
                        _weaponEfficiency = CombatCalculator.bad;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Mace:
                        _weaponEfficiency = CombatCalculator.bad;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Spear:
                        _weaponEfficiency = CombatCalculator.bad;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Lance:
                        _weaponEfficiency = CombatCalculator.bad;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Bow:
                        _weaponEfficiency = CombatCalculator.bad;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Warmachine:
                        _weaponEfficiency = CombatCalculator.optimal;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.MagicalAttack:
                        _weaponEfficiency = CombatCalculator.bad;
                        return _weaponEfficiency;

                    default:
                        return CombatCalculator.noDamage;
                }

            case UnitDataSO.UnitType.Environment:
                switch (_myWeaponType)
                {
                    case WeaponDataSO.WeaponType.Unarmed:
                        _weaponEfficiency = CombatCalculator.noDamage;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Tool:
                        _weaponEfficiency = CombatCalculator.optimal;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Sword:
                        _weaponEfficiency = CombatCalculator.noDamage;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Axe:
                        _weaponEfficiency = CombatCalculator.noDamage;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Mace:
                        _weaponEfficiency = CombatCalculator.noDamage;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Spear:
                        _weaponEfficiency = CombatCalculator.noDamage;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Lance:
                        _weaponEfficiency = CombatCalculator.noDamage;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Bow:
                        _weaponEfficiency = CombatCalculator.noDamage;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Warmachine:
                        _weaponEfficiency = CombatCalculator.good;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.MagicalAttack:
                        _weaponEfficiency = CombatCalculator.noDamage;
                        return _weaponEfficiency;

                    default:
                        return CombatCalculator.noDamage;
                }

            default:
                return _weaponEfficiency;
        }
    }

    /// <summary>
    /// Check how effective the attackin unit type is against the targeted unit Type.
    /// </summary>
    /// <param name="_myWeaponType"></param>
    /// <param name="_targetUnitType"></param>
    /// <returns></returns>
    public static float GetUnitTypeInfluence(UnitDataSO.UnitType _myUnitType, UnitDataSO.UnitType _targetUnitType)
    {
        float _unitTypeEfficiency = 0.0f;

        switch (_targetUnitType)
        {
            case UnitDataSO.UnitType.Peasant: // my target is a peasant...
                switch (_myUnitType)
                {
                    case UnitDataSO.UnitType.Peasant: // ... and I'm a peasant.
                        _unitTypeEfficiency = CombatCalculator.normal;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.Soldier:
                        _unitTypeEfficiency = CombatCalculator.normal;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.LightCavalry:
                        _unitTypeEfficiency = CombatCalculator.good;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.HeavyCavalry:
                        _unitTypeEfficiency = CombatCalculator.optimal;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.Warmachine:
                        _unitTypeEfficiency = CombatCalculator.bad;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.Building:
                        _unitTypeEfficiency = CombatCalculator.normal;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.Environment:
                        _unitTypeEfficiency = CombatCalculator.noDamage;
                        return _unitTypeEfficiency;

                    default:
                        return CombatCalculator.noDamage;
                }

            case UnitDataSO.UnitType.Soldier:
                switch (_myUnitType)
                {
                    case UnitDataSO.UnitType.Peasant:
                        _unitTypeEfficiency = CombatCalculator.normal;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.Soldier:
                        _unitTypeEfficiency = CombatCalculator.normal;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.LightCavalry:
                        _unitTypeEfficiency = CombatCalculator.normal;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.HeavyCavalry:
                        _unitTypeEfficiency = CombatCalculator.good;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.Warmachine:
                        _unitTypeEfficiency = CombatCalculator.bad;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.Building:
                        _unitTypeEfficiency = CombatCalculator.normal;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.Environment:
                        _unitTypeEfficiency = CombatCalculator.noDamage;
                        return _unitTypeEfficiency;

                    default:
                        return CombatCalculator.noDamage;
                }

            case UnitDataSO.UnitType.LightCavalry:
                switch (_myUnitType)
                {
                    case UnitDataSO.UnitType.Peasant:
                        _unitTypeEfficiency = CombatCalculator.bad;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.Soldier:
                        _unitTypeEfficiency = CombatCalculator.bad;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.LightCavalry:
                        _unitTypeEfficiency = CombatCalculator.normal;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.HeavyCavalry:
                        _unitTypeEfficiency = CombatCalculator.good;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.Warmachine:
                        _unitTypeEfficiency = CombatCalculator.bad;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.Building:
                        _unitTypeEfficiency = CombatCalculator.normal;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.Environment:
                        _unitTypeEfficiency = CombatCalculator.noDamage;
                        return _unitTypeEfficiency;

                    default:
                        return CombatCalculator.noDamage;
                }

            case UnitDataSO.UnitType.HeavyCavalry:
                switch (_myUnitType)
                {
                    case UnitDataSO.UnitType.Peasant:
                        _unitTypeEfficiency = CombatCalculator.bad;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.Soldier:
                        _unitTypeEfficiency = CombatCalculator.bad;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.LightCavalry:
                        _unitTypeEfficiency = CombatCalculator.bad;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.HeavyCavalry:
                        _unitTypeEfficiency = CombatCalculator.normal;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.Warmachine:
                        _unitTypeEfficiency = CombatCalculator.bad;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.Building:
                        _unitTypeEfficiency = CombatCalculator.normal;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.Environment:
                        _unitTypeEfficiency = CombatCalculator.noDamage;
                        return _unitTypeEfficiency;

                    default:
                        return CombatCalculator.noDamage;
                }

            case UnitDataSO.UnitType.Warmachine:
                switch (_myUnitType)
                {
                    case UnitDataSO.UnitType.Peasant:
                        _unitTypeEfficiency = CombatCalculator.good;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.Soldier:
                        _unitTypeEfficiency = CombatCalculator.good;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.LightCavalry:
                        _unitTypeEfficiency = CombatCalculator.good;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.HeavyCavalry:
                        _unitTypeEfficiency = CombatCalculator.good;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.Warmachine:
                        _unitTypeEfficiency = CombatCalculator.good;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.Building:
                        _unitTypeEfficiency = CombatCalculator.bad;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.Environment:
                        _unitTypeEfficiency = CombatCalculator.noDamage;
                        return _unitTypeEfficiency;

                    default:
                        return CombatCalculator.noDamage;
                }

            case UnitDataSO.UnitType.Building:
                switch (_myUnitType)
                {
                    case UnitDataSO.UnitType.Peasant:
                        _unitTypeEfficiency = CombatCalculator.bad;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.Soldier:
                        _unitTypeEfficiency = CombatCalculator.bad;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.LightCavalry:
                        _unitTypeEfficiency = CombatCalculator.bad;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.HeavyCavalry:
                        _unitTypeEfficiency = CombatCalculator.bad;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.Warmachine:
                        _unitTypeEfficiency = CombatCalculator.optimal;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.Building:
                        _unitTypeEfficiency = CombatCalculator.bad;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.Environment:
                        _unitTypeEfficiency = CombatCalculator.noDamage;
                        return _unitTypeEfficiency;

                    default:
                        return CombatCalculator.noDamage;
                }

            case UnitDataSO.UnitType.Environment:
                switch (_myUnitType)
                {
                    case UnitDataSO.UnitType.Peasant:
                        _unitTypeEfficiency = CombatCalculator.optimal;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.Soldier:
                        _unitTypeEfficiency = CombatCalculator.noDamage;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.LightCavalry:
                        _unitTypeEfficiency = CombatCalculator.noDamage;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.HeavyCavalry:
                        _unitTypeEfficiency = CombatCalculator.noDamage;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.Warmachine:
                        _unitTypeEfficiency = CombatCalculator.good;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.Building:
                        _unitTypeEfficiency = CombatCalculator.noDamage;
                        return _unitTypeEfficiency;

                    case UnitDataSO.UnitType.Environment:
                        _unitTypeEfficiency = CombatCalculator.noDamage;
                        return _unitTypeEfficiency;

                    default:
                        return CombatCalculator.noDamage;
                }

            default:
                return _unitTypeEfficiency;
        }
    }


    /// <summary>
    /// Armor can reduce incoming damage by a maximum of 50%. This can be lowered to 0%, however 
    /// overpenetration has no effect. We assume armor and penetration range from 0 - 8 for this to work.
    /// Each point of armor corresponds with 12,5% damage reduction. Each point of penetration corresponds 
    /// with 12,5% damage reduction being ignored. Thus, they can be subtracted from one another.
    /// </summary>
    /// <param name="_penetrationValueOfAttack"></param>
    /// <returns></returns>
    public static float CalculateArmorEfficiency(float _myArmorValue, float _penetrationValueOfAttack)
    {
        float _blockedDamagePercentage = 0.0f;

        // subtract penetration value from armor value:
        float _effectiveArmor = _myArmorValue - _penetrationValueOfAttack;

        // convert effective armor to a percentage of damage blocked
        if (_effectiveArmor < 0)
        {
            _blockedDamagePercentage = _effectiveArmor * 12.5f;
        }

        // clamp the blocked damage percentage to a maximum of 75%:
        _blockedDamagePercentage = Mathf.Clamp(_blockedDamagePercentage, 0.0f, 50.0f);

        // convert the percentage to a fraction (for example, 12,5% becomes 0.125f):
        float _blockedDamageFraction = _blockedDamagePercentage / 100.0f;

        return _blockedDamageFraction;
    }

    /*
    /// <summary>
    /// Armor can reduce incoming damage by a maximum of 75%. This can be lowered to 0%, however 
    /// overpenetration has no effect. We assume armor and penetration range from 0 - 10 for this to work.
    /// </summary>
    /// <param name="_penetrationValueOfAttack"></param>
    /// <returns></returns>
    public static float CalculateArmorEfficiency(float _myArmorValue,float _penetrationValueOfAttack)
    {
        // subtract penetration value from armor value:
        float _effectiveArmor = _myArmorValue - _penetrationValueOfAttack;

        // convert effective armor to a percentage of damage blocked
        // each point of armor corresponds to 10% damage reduction:
        if(_effectiveArmor < 0)
        {
            float _blockedDamagePercentage = _effectiveArmor * 10.0f;
        }

        // clamp the blocked damage percentage to a maximum of 75%:
        _blockedDamagePercentage = Mathf.Clamp(_blockedDamagePercentage, 0.0f, 75.0f);

        // convert the percentage to a fraction (for example, 75% becomes 0.75):
        float _blockedDamageFraction = _blockedDamagePercentage / 100.0f;

        return _blockedDamageFraction;
    }*/

    /*
    public static float CalculateBlockedDamage(float _armorValue, float _penetrationValueOfAttack)
    {
        // Subtract penetration value from armor value
        float _effectiveArmor = _armorValue - _penetrationValueOfAttack;

        // Convert effective armor to a percentage of damage blocked
        // Each point of armor corresponds to 10% damage reduction
        float _blockedDamagePercentage = _effectiveArmor * 10.0f;

        // Clamp the blocked damage percentage to a maximum of 75%
        _blockedDamagePercentage = Mathf.Clamp(_blockedDamagePercentage, 0.0f, 75.0f);

        // Convert the percentage to a fraction (for example, 75% becomes 0.75)
        float _blockedDamageFraction = _blockedDamagePercentage / 100.0f;

        return _blockedDamageFraction;
    }*/
}
