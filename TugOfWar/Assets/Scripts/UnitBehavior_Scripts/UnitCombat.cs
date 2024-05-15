using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCombat : MonoBehaviour
{
    bool _inCoroutine = false;

    float _meleePenetrationValue = 0f;
    float _meleeAttackRange = 0.0f;
    float _meleeAttackSpeed = 0.0f;
    float _meleeDamageAmount = 0.0f;

    float _rangedPenetrationValue = 0f;
    float _rangedAttackRange = 0.0f;
    float _rangedAttackSpeed = 0.0f;
    float _rangedDamageAmount = 0.0f;

    int _myClassID;
    GameObject _targetEnemy;

    /// <summary>
    /// Call this function when a unit gets placed on the battlefield. This function will set the unit up with all required information.
    /// </summary>
    public void UpdateUnitCombat()
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
        if(_targetEnemy.GetComponent<UnitHealth>().currentHealthPoints > 0.0f)
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
    }
}
