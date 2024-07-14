using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDetection : MonoBehaviour
{
    UnitManager _unitManager;
    UnitMovement _unitMovement;
    LayerMask _unitSpottingLayer;

    // variables set by the UnitManager:
    bool _isWorker = false;
    int _thisUnitsPlayerAffiliation = 0;
    float _spottingRange = 0.0f;
    float _myColliderRadius = 0.0f;
    GameObject _closestEnemy;

    public void InitializeUnitDetection()
    {
        // cache components and references:
        _unitManager = GetComponent<UnitManager>();
        _unitMovement = GetComponent<UnitMovement>(); 
        _unitSpottingLayer = LayerMask.GetMask("Units");

        // set variables:
        _thisUnitsPlayerAffiliation = _unitManager.myPlayerAffiliation;
        _spottingRange = _unitManager.baseSpottingRange;
        _myColliderRadius = GetMyBoundaryRadius(GetComponent<Collider>());

        // if this is a peasant with a tool, it's a worker:
        if(_unitManager.unitType == UnitDataSO.UnitType.Peasant && _unitManager.weaponType == WeaponDataSO.WeaponType.Tool)
        {
            _isWorker = true;
        }
    }
    public void StartScanningForEnemies()
    {
        StartCoroutine(ScanInterval());
    }

    IEnumerator ScanInterval()
    {
        CheckForEnemies();

        yield return new WaitForSeconds(0.1f);

        StartCoroutine(ScanInterval());
    }

    /// <summary>
    /// Check for enemies in the viscinity. Return the closest found enemy. 
    /// </summary>
    /// <param name="_enemyTransform"></param>
    public void CheckForEnemies()
    {
        // use an OverlapSphere to detect all colliders within the detection radius and on the unit layer:
        Collider[] _detectedUnitObjects = Physics.OverlapSphere(transform.position, _spottingRange + _myColliderRadius, _unitSpottingLayer); // spotting Range should be combined with collider Radius!
        List<GameObject> _spottedEnemyUnits = new List<GameObject>();

        // if at least one potential unit was detected check if any are actual units and moreso - enemies:
        if (_detectedUnitObjects.Length > 0)
        {
            foreach (Collider _unitObject in _detectedUnitObjects)
            {
                // is the spotted unit active (has it been launched yet?):
                //if (_unitObject.GetComponent<UnitManager>() && _unitObject.GetComponent<UnitManager>().isActive)
                if (_unitObject.GetComponent<UnitManager>())
                {
                    if (_unitObject.GetComponent<UnitManager>().isActive && IsViableTarget(_unitObject))
                    {
                        _spottedEnemyUnits.Add(_unitObject.gameObject);
                    }
                    /*if (IsOpponent(_unitObject))
                    {
                        _spottedEnemyUnits.Add(_unitObject.gameObject);
                    }*/
                }
                else
                {
                    Debug.LogError("ERROR: This object is on the UNIT-layer, but has no UnitManager attached. Check why!", this.gameObject);
                }
            }
        }

        // of the detected enemies, return the closests:
        if (_spottedEnemyUnits.Count > 0)
        {
            float _closestDistance = Mathf.Infinity;

            foreach (GameObject _enemy in _spottedEnemyUnits)
            {
                float distanceToSpecificTestedUnit = Vector3.Distance(transform.position, _enemy.transform.position);

                if (distanceToSpecificTestedUnit < _closestDistance)
                {
                    // only update the closest enemy unit if it's NOT neutral:
                    if(_closestEnemy == null || !IsNeutral(_enemy))
                    {
                        _closestEnemy = _enemy.gameObject;
                    }

                    //_closestEnemy = _enemy.gameObject;
                    _unitMovement.EnemySpotted(_closestEnemy);

                    _closestDistance = distanceToSpecificTestedUnit;
                }
            }
        }
        else
        {
            _closestEnemy = null;
            _unitMovement.ResetSpottedEnemy();
        }
    }

    private bool IsNeutral(GameObject targetUnit)
    {
        if(targetUnit.GetComponent<UnitManager>().myPlayerAffiliation == 1 || targetUnit.GetComponent<UnitManager>().myPlayerAffiliation == 2)
        {
            return false;
        }else
        {
            return true;
        }
    }

    /// <summary>
    /// Check if the unit actually has a team affiliation. Also check if this affiliation is not the same
    /// as the checking units affiliation.
    /// </summary>
    /// <param name="_spottedUnit"></param>
    /// <returns></returns>
    bool IsViableTarget(Collider _spottedUnit)
    {
        // check if spotted target is a combat-unit:
        if (_spottedUnit.GetComponent<UnitManager>().myPlayerAffiliation != 0)
        {
            // check if it's from the opposing team:
            if (_spottedUnit.GetComponent<UnitManager>().myPlayerAffiliation != _thisUnitsPlayerAffiliation)
            {
                return true;
            }else
            {
                return false;
            }
        }

        // if the spotted target isn't a combat unit, check if this unit is a worker:
        else if (_isWorker)
        {
            return true;
        }

        // any other setup means this is not a viable target:
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Get the radius of any circular colliders or half the longest side of a square collider.
    /// </summary>
    /// <param name="collider"></param>
    /// <returns></returns>
    float GetMyBoundaryRadius(Collider collider)
    {
        if (collider is SphereCollider sphere)
        {
            return sphere.radius;
        }
        else if (collider is CapsuleCollider capsule)
        {
            return capsule.radius;
        }
        else if (collider is BoxCollider box)
        {
            return Mathf.Max(box.size.x, box.size.y, box.size.z) / 2f;
        }
        else if (collider is MeshCollider mesh && mesh.sharedMesh != null)
        {
            return mesh.sharedMesh.bounds.extents.magnitude;
        }

        // default case if none of the above colliders apply:
        return 0f;
    }
}





/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDetection : MonoBehaviour
{
    UnitManager _unitManager;

    LayerMask _unitSpottingLayer;

    // variables set by the UnitManager:
    int _thisUnitsPlayerAffiliation = 0;
    float _spottingRange = 0.0f;
    float _myColliderRadius = 0.0f;

    //Transform _closestEnemyTransform;
    //float _closestDistance = Mathf.Infinity;
    public void InitializeUnitDetection()
    {
        // cache components and references:
        _unitManager = GetComponent<UnitManager>();
        _unitSpottingLayer = LayerMask.GetMask("Units");

        // set variables:
        _thisUnitsPlayerAffiliation = _unitManager.myPlayerAffiliation;
        _spottingRange = _unitManager.baseSpottingRange;
        _myColliderRadius = GetMyBoundaryRadius(GetComponent<Collider>());
    }

    /// <summary>
    /// Check for enemies in the viscinity. Return the closest found enemy. 
    /// </summary>
    /// <param name="_enemyTransform"></param>
    //public void CheckForEnemies(out Transform _enemyTransform)
    public bool CheckForEnemies(out Transform _enemyTransform)
    {
        // use an OverlapSphere to detect all colliders within the detection radius and on the unit layer:
        Collider[] _detectedUnitObjects = Physics.OverlapSphere(transform.position, _spottingRange + _myColliderRadius, _unitSpottingLayer); // spotting Range should be combined with collider Radius!
        List<GameObject> _spottedEnemyUnits = new List<GameObject>();
        _enemyTransform = null;

        // if at least one potential unit was detected check if any are actual units and moreso - enemies:
        if (_detectedUnitObjects.Length > 0)
        {
            foreach (Collider _unitObject in _detectedUnitObjects)
            {
                // check if this object has a UnitManager-script attached (Q: Is it an actual unit?):
                if (_unitObject.GetComponent<UnitManager>())
                {
                    if ((_unitObject.GetComponent<UnitManager>().myPlayerAffiliation == 1 || _unitObject.GetComponent<UnitManager>().myPlayerAffiliation == 2) && _unitObject.GetComponent<UnitManager>().myPlayerAffiliation != _thisUnitsPlayerAffiliation)
                    {
                        _spottedEnemyUnits.Add(_unitObject.gameObject);
                    }
                }
                else
                {
                    Debug.LogError("ERROR: This object is on the UNIT-layer, but has no UnitManager attached. Check why!", _unitObject);
                }

            }
        }

        // of the detected enemies, return the closests:
        if (_spottedEnemyUnits.Count > 0)
        {
            float _closestDistance = Mathf.Infinity;

            foreach (GameObject _enemy in _spottedEnemyUnits)
            {
                float distanceToSpecificTestedUnit = Vector3.Distance(transform.position, _enemy.transform.position);

                if (distanceToSpecificTestedUnit < _closestDistance)
                {
                    _enemyTransform = _enemy.transform;

                    // save last found, closest enemy:
                    //_closestEnemyTransform = _enemyTransform;
                    _closestDistance = distanceToSpecificTestedUnit;
                }
            }

            return true;
        }
        else
        {
            // no more enemies detected, reset all:
            //_closestDistance = Mathf.Infinity;
            //_closestEnemyTransform = null;

            // return nothing:
            _enemyTransform = null;
            return false;
        }
    }


    /// <summary>
    /// Get the radius of any circular colliders or half the longest side of a square collider.
    /// </summary>
    /// <param name="collider"></param>
    /// <returns></returns>
    float GetMyBoundaryRadius(Collider collider)
    {
        if (collider is SphereCollider sphere)
        {
            return sphere.radius;
        }
        else if (collider is CapsuleCollider capsule)
        {
            return capsule.radius;
        }
        else if (collider is BoxCollider box)
        {
            return Mathf.Max(box.size.x, box.size.y, box.size.z) / 2f;
        }
        else if (collider is MeshCollider mesh && mesh.sharedMesh != null)
        {
            return mesh.sharedMesh.bounds.extents.magnitude;
        }

        // default case if none of the above colliders apply:
        return 0f;
    }
}*/