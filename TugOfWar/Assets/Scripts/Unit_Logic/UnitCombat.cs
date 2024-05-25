using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCombat : MonoBehaviour
{
    UnitManager _unitManager;

    bool _inCoroutine = false;

    // basic:
    WeaponDataSO.WeaponGrip _myWeaponGrip;
    WeaponDataSO.WeaponType _myWeaponType;

    float _penetrationValue = 0f;
    float _attackRange = 0.0f;
    float _attackSpeed = 0.0f;
    float _weaponDamageValue = 0.0f;
    
    // advanced:
    float _hitChance = 0.0f;
    float _critChance = 0.0f;
    float _bashChance = 0.0f;
    float _bashForce = 0.0f;
    float _splashRadius = 0.0f;
    float _splashDamage = 0.0f;

    UnitDataSO.UnitType _myUnitType;
    GameObject _targetEnemy;

    public void InitializeUnitCombat()
    {
        // cache components:
        _unitManager = GetComponent<UnitManager>();

        // setup variables:
        _myUnitType = _unitManager.unitType;

        _penetrationValue = _unitManager.baseArmorPenetrationValue;
        _attackRange = _unitManager.baseAttackRange; // is this actually needed here, or just in movement?

        _attackSpeed = _unitManager.baseAttackSpeed;
        _weaponDamageValue = _unitManager.baseDamage;
    }

    /*
    /// <summary>
    /// Call this function when a unit gets placed on the battlefield. This function will set the unit up with all required information.
    /// </summary>
    public void UpdateUnitCombat()
    {
        _myUnitType = GetComponent<UnitManager>().myClassID;

        _penetrationValue = GetComponent<UnitManager>().meleeArmorPenetrationRating;
        _attackRange = GetComponent<UnitManager>().meleeAttackRange;
        _attackSpeed = GetComponent<UnitManager>().meleeAttackSpeed;
        _damageAmount = GetComponent<UnitManager>().meleeDamage;

        _rangedPenetrationValue = GetComponent<UnitManager>().rangedArmorPenetrationRating;
        _rangedAttackRange = GetComponent<UnitManager>().rangedAttackRange;
        _rangedAttackSpeed = GetComponent<UnitManager>().rangedAttackSpeed;
        _rangedDamageAmount = GetComponent<UnitManager>().rangedDamage;
    }*/

    /// <summary>
    /// Called by EngageTarget from the UnitMovement Script once an enemy is in range.
    /// </summary>
    /// <param name="_targetEnemyUnit"></param>
    public void Attack(GameObject _targetEnemyUnit)
    {
        Debug.Log("I am attacking: " + this.gameObject);

        //_targetEnemy = _enemyUnit;

        if (TargetIsAlive(_targetEnemyUnit))
        {
            if (!_inCoroutine)
            {
                StartCoroutine(Strike(_targetEnemyUnit));
            }
            //StartCoroutine(Strike(_targetEnemyUnit));

            /*
            if (!_inCoroutine)
            {
                if (RangedAttackPossible())
                {
                    StartCoroutine(Shoot());
                }
                else
                {
                    StartCoroutine(Strike());
                }
            }*/
        }
        else
        {
            GetComponent<UnitMovement>().MoveTowardEnemyBase(); // redundant?
        }
    }
    bool TargetIsAlive(GameObject _enemyUnit)
    {
        if (_enemyUnit.GetComponent<UnitManager>().isActive)
        {
            return true;
        }else
        {
            return false;
        }
    }

    /*
    bool RangedAttackPossible()
    {
        if (Vector3.Distance(transform.position, _targetEnemy.transform.position) > _attackRange)
        {
            return true;
        }
        return false;
    }*/

    IEnumerator Strike(GameObject _targetEnemyUnit)
    {
        Debug.Log("I, " + this.gameObject.name + " strike at " + _targetEnemyUnit.name);

        _inCoroutine = true;

        float _calculatedDamage = CalculateEffectiveDamage(_targetEnemyUnit);

        _targetEnemyUnit.GetComponent<UnitHealth>().TakeDamage(_calculatedDamage, _penetrationValue);

        yield return new WaitForSeconds(_attackSpeed);

        _inCoroutine = false;
    }

    float CalculateEffectiveDamage(GameObject _targetEnemyUnit)
    {
        float _effectiveDamage = 0.0f;

        UnitDataSO.UnitType _targetUnitType = _targetEnemyUnit.GetComponent<UnitManager>().unitType;

        // calculate weapon efficiency:
        //float _weaponTypeInfluence = GetWeaponInfluence(_targetUnitType);
        float _weaponTypeInfluence = CombatCalculator.GetWeaponInfluence(_myWeaponType, _targetUnitType);

        // calculate unit type influence:
        //float _unitTypeInfluence = GetUnitTypeInfluence(_targetUnitType);
        float _unitTypeInfluence = CombatCalculator.GetUnitTypeInfluence(_myUnitType, _targetUnitType);

        _effectiveDamage = _weaponDamageValue + _weaponTypeInfluence + _unitTypeInfluence;

        return _effectiveDamage;
    }

    /*
    // this should be a static function somewhere!
    float GetWeaponInfluence(UnitDataSO.UnitType _targetUnitType)
    {
        float _weaponEfficiency = 0.0f;
            
        switch(_targetUnitType)
        {
            case UnitDataSO.UnitType.Peasant:
                switch (_myWeaponType)
                {
                    case WeaponDataSO.WeaponType.Unarmed:
                        _weaponEfficiency = 0.0f;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Tool:
                        _weaponEfficiency = 0.8f;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Sword:
                        _weaponEfficiency = 1.0f;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Axe:
                        _weaponEfficiency = 1.0f;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Mace:
                        _weaponEfficiency = 1.0f;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Spear:
                        _weaponEfficiency = 1.0f;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Lance:
                        _weaponEfficiency = 2.0f;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Bow:
                        _weaponEfficiency = 1.0f;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.Warmachine:
                        _weaponEfficiency = 0.25f;
                        return _weaponEfficiency;

                    case WeaponDataSO.WeaponType.MagicalAttack:
                        _weaponEfficiency = 1.0f;
                        return _weaponEfficiency;

                    default:
                        return _weaponEfficiency;
                }
            case UnitDataSO.UnitType.Soldier:
            case UnitDataSO.UnitType.LightCavalry:
            case UnitDataSO.UnitType.Warmachine:
            case UnitDataSO.UnitType.Building:
            case UnitDataSO.UnitType.Environment:

            default:
                return _weaponEfficiency;
        }
    }*/

    /*
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
    }*/





    /* // this is from a time i was considering having ranged and close combat attacks for some units
    IEnumerator Shoot()
    {
        _inCoroutine = true;

        _targetEnemy.GetComponent<UnitHealth>().TakeDamage(_rangedDamageAmount, _rangedPenetrationValue, _myUnitType);

        yield return new WaitForSeconds(_rangedAttackSpeed);

        _inCoroutine = false;
    }*/







    /*    public void UpdateUnitCombat()
        {
            _myClassID = GetComponent<UnitManager>().myClassID;

            _meleePenetrationValue = GetComponent<UnitManager>().meleeArmorPenetrationRating;
            _meleeAttackRange = GetComponent<UnitManager>().meleeAttackRange;
            _meleeAttackSpeed = GetComponent<UnitManager>().meleeAttackSpeed;
            _meleeDamageAmount = GetComponent<UnitManager>().meleeDamage;

            _rangedPenetrationValue = GetComponent<UnitManager>().rangedArmorPenetrationRating;
            _rangedAttackRange = GetComponent<UnitManager>().rangedAttackRange;
            _rangedAttackSpeed = GetComponent<UnitManager>().rangedAttackSpeed;
            _rangedDamageAmount = GetComponent<UnitManager>().rangedDamage;
        }

        /// <summary>
        /// Called by EngageTarget from the UnitMovement Script once an enemy is in range.
        /// </summary>
        /// <param name="_enemyUnit"></param>
        public void Attack(GameObject _enemyUnit)
        {
            Debug.Log("1");

            _targetEnemy = _enemyUnit;

            if (TargetIsAlive())
            {
                if (!_inCoroutine)
                {
                    if (RangedAttackPossible())
                    {
                        StartCoroutine(Shoot());
                    }
                    else
                    {
                        StartCoroutine(Strike());
                    }
                }
            }else
            {
                GetComponent<UnitMovement>().MoveTowardFinalDestination();
            }
        }
        bool TargetIsAlive()
        {
            if(_targetEnemy.GetComponent<UnitHealth>()._currentHealthPoints > 0.0f)
            {
                return true;
            }else
            {
                return false;
            }
        }
        bool RangedAttackPossible()
        {
                if (Vector3.Distance(transform.position, _targetEnemy.transform.position) > _meleeAttackRange)
                {
                    return true;
                }
                return false;
        }

        IEnumerator Strike()
        {
            _inCoroutine = true;

            _targetEnemy.GetComponent<UnitHealth>().TakeDamage(_meleeDamageAmount, _meleePenetrationValue, _myClassID);

            yield return new WaitForSeconds(_meleeAttackSpeed);

            _inCoroutine = false;
        }

        IEnumerator Shoot()
        {
            _inCoroutine = true;

            _targetEnemy.GetComponent<UnitHealth>().TakeDamage(_rangedDamageAmount, _rangedPenetrationValue, _myClassID);

            yield return new WaitForSeconds(_rangedAttackSpeed);

            _inCoroutine = false;
        }*/
}
