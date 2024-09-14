using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnitEnumManager;


public class UnitCombat : MonoBehaviour
{
    bool _inCoroutine = false;
    bool _isActive = false;
    UnitManager _unitManager;
    //WeaponGrip _myWeaponGrip;
    ItemType _myWeaponType;
    UnitType _myUnitType;
    bool _isBuilding = false;
    int _myPlayerAffiliation;
    //GameObject _targetEnemy;

    // animation stuff:
    UnitAnimationController _myAnimationController;

    // old anim stuff:
    AnimateProjectile _myAnimateProjectile; // in case this is a ranged unit
    Animator _unitAnimator;
    AnimatorOverrideController _unitAnimatorOverrideController;
    float _attackAnimationDuration = 2.0f; // NOT a solution!

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

    public void InitializeUnitCombat()
    {
        // cache components:
        _unitManager = GetComponent<UnitManager>();
        _myPlayerAffiliation = _unitManager.myPlayerID;

        if (GetComponent<UnitAnimationController>())
        {
            _myAnimationController = GetComponent<UnitAnimationController>();
        }

        // setup variables:
        _myUnitType = _unitManager.unitProfile.unitType;
        if (_myUnitType == UnitType.Building)
        {
            _isBuilding = true;
        }
        _penetrationValue = _unitManager.unitProfile.armorPenetrationValue;
        _attackRange = _unitManager.unitProfile.attackRange; // is this actually needed here, or just in movement?
        _attackSpeed = _unitManager.unitProfile.attackSpeed;
        _weaponDamageValue = _unitManager.unitProfile.weaponDamage;

        _isActive = true;
    }


    /// <summary>
    /// Called by EngageTarget from the UnitMovement Script once an enemy is in range.
    /// </summary>
    /// <param name="_targetEnemyUnit"></param>
    public void Attack(GameObject _targetEnemyUnit)
    {
        if (_isActive)
        {
            // if this is a building (no movement), call attack-animation here:
            //_myAnimationController.EnterCombat();

            if (TargetIsAlive(_targetEnemyUnit))
            {
                if(!_isBuilding)
                {
                    transform.LookAt(_targetEnemyUnit.transform);
                }

                if (!_inCoroutine)
                {
                    StartCoroutine(Strike(_targetEnemyUnit));
                }
            }else
            {
                // if this is a building (no movement), exit attack-animations here:
                //_myAnimationController.ExitCombat();
            }
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


    IEnumerator Strike(GameObject _targetEnemyUnit)
    {
        _inCoroutine = true;

        _myAnimationController.AttackAnimation(_targetEnemyUnit, _attackSpeed, _unitManager.unitProfile.mainItemType);

        // administer damage: (there should be a distinction for ranged attacks, causing dmg on impact)
        float _calculatedDamage = CalculateEffectiveDamage(_targetEnemyUnit.GetComponent<UnitManager>().unitProfile.unitData.unitType);
        _targetEnemyUnit.GetComponent<UnitHealth>().TakeDamage(_calculatedDamage, _penetrationValue, _myPlayerAffiliation);

        // wait the length of attack-speed before being able to strike again:
        yield return new WaitForSeconds(_attackSpeed);

        _inCoroutine = false;
    }


    float CalculateEffectiveDamage(UnitType _targetUnitType)
    {
        float _unitTypeInfluence = CombatCalculator.GetUnitTypeInfluence(_myUnitType, _targetUnitType);
        float _weaponTypeInfluence = CombatCalculator.GetWeaponInfluence(_myWeaponType, _targetUnitType);
        
        // base 100% + influences. E.g. 112,5% dmg when slightly advantaged.
        float _efficiencyMultiplier = 1.0f + _unitTypeInfluence + _weaponTypeInfluence;

        float _effectiveDamage = _weaponDamageValue * _efficiencyMultiplier;

        return _effectiveDamage;
    }

    /// <summary>
    /// This gets called by the UnitManager, when this unit is killed.
    /// </summary>
    public void DeactivateCombat()
    {
        _isActive = false;
    }
}
