/*using System.Collections;
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

    Transform _closestEnemyTransform;
    float _closestDistance = Mathf.Infinity;
    public float _proximityTolerance = 0.05f;

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
    /// Check for enemies in the vicinity. Return the closest found enemy.
    /// </summary>
    /// <param name="_enemyTransform"></param>
    public bool CheckForEnemies(out Transform _enemyTransform)
    {
        // use an OverlapSphere to detect all colliders within the detection radius and on the unit layer:
        Collider[] _detectedUnitObjects = Physics.OverlapSphere(transform.position, _spottingRange + _myColliderRadius, _unitSpottingLayer);
        List<GameObject> _spottedEnemyUnits = new List<GameObject>();
        _enemyTransform = null;

        // if at least one potential unit was detected, check if any are actual units and more so - enemies:
        if (_detectedUnitObjects.Length > 0)
        {
            foreach (Collider _unitObject in _detectedUnitObjects)
            {
                // check if this object has a UnitManager-script attached:
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

        // of the detected enemies, find the closest:
        if (_spottedEnemyUnits.Count > 0)
        {
            Transform potentialClosestEnemy = null;
            float closestDistanceFound = Mathf.Infinity;

            foreach (GameObject _enemy in _spottedEnemyUnits)
            {
                float distanceToSpecificTestedUnit = Vector3.Distance(transform.position, _enemy.transform.position);

                if (distanceToSpecificTestedUnit < closestDistanceFound)
                {
                    closestDistanceFound = distanceToSpecificTestedUnit;
                    potentialClosestEnemy = _enemy.transform;
                }
            }

            // switch target only if the new closest enemy is significantly closer than the current one
            if (_closestEnemyTransform == null || closestDistanceFound < _closestDistance - _proximityTolerance)
            {
                _closestEnemyTransform = potentialClosestEnemy;
                _closestDistance = closestDistanceFound;
            }

            _enemyTransform = _closestEnemyTransform;
            return true;
        }
        else
        {
            // no more enemies detected, reset all:
            _closestDistance = Mathf.Infinity;
            _closestEnemyTransform = null;

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
}