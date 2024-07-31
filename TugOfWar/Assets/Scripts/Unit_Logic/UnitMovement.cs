using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    UnitManager _unitManager;
    NavMeshAgent _navMeshAgent;
    UnitAnimationController _unitAnimationController;

    //LayerMask _unitSpottingLayer;
    Vector3 _destinationLocation;
    bool _isActive = false;
    bool _isBuilding = false;

    // variables set by the UnitManager:
    int _thisUnitsPlayerAffiliation = 0;
    //float _spottingRange = 0.0f;
    float _attackRange = 0.0f;
    float _myColliderRadius = 0.0f;

    // movement speeds:
    float _walkingSpeed = 0.0f;
    float _runningSpeed = 0.0f;
    float _chargingSpeed = 0.0f;

    Transform _closestEnemyTransform;

    bool _enemySpotted = false;

    public void InitializeUnitMovement()
    {
        // cache components and references:
        _unitManager = GetComponent<UnitManager>();
        _unitAnimationController = GetComponent<UnitAnimationController>();

        if (_unitManager.unitType != UnitDataSO.UnitType.Building)
        {
            // buildings do not have a NavMeshAgent:
            _navMeshAgent = GetComponent<NavMeshAgent>();

            _walkingSpeed = _unitManager.walkingSpeed;
            _runningSpeed = _unitManager.runningSpeed;
            _chargingSpeed = _unitManager.chargingSpeed;
            _destinationLocation = _unitManager.unitDestination;
            //_destinationLocation = _unitManager.unitDestination.position; // old - no longer using a transform
        }
        else
        {
            _isBuilding = true;
        }

        // set variables:
        //_unitSpottingLayer = LayerMask.GetMask("Units"); // detection!
        _thisUnitsPlayerAffiliation = _unitManager.myPlayerAffiliation;  // detection!

        //_spottingRange = _unitManager.baseSpottingRange; // detection!
        _attackRange = _unitManager.baseAttackRange;
        _myColliderRadius = GetRadius(GetComponent<Collider>());

        // TESTING:
        //Debug.Log("my, "+ this.gameObject.name + " collider radius: " + _myColliderRadius, this.gameObject);
        if (_thisUnitsPlayerAffiliation == 1)
        {
            Debug.Log("my destination coord: " + _destinationLocation, this);
        }
    }

    /// <summary>
    /// Called by <see cref="UnitManager"/> to tell deployed units to start moving/engaging the enemy.
    /// </summary>
    public void StartMovement()
    {
        _isActive = true;

        if (!_isBuilding)
        {
            // enable NavMesh (if it's done earlier, units tend to get stuck on interveening terrain):
            _navMeshAgent.enabled = true;

            // launch unit towards enemy base:
            MoveTowardEnemyBase();
        }
    }

    /// <summary>
    /// Called by <see cref="UnitDetection"/> to inform about a viable target in the area. 
    /// </summary>
    /// <param name="_closestEnemy"></param>
    public void EnemySpotted(GameObject _spottedEnemy)
    {
        _closestEnemyTransform = _spottedEnemy.transform;
        _enemySpotted = true;
    }
    public void ResetSpottedEnemy()
    {
        _closestEnemyTransform = null;
        _enemySpotted = false;
    }

    private void Update()
    {
        if (_isActive)
        {
            if (!_isBuilding)
            {
                if (_enemySpotted)
                {
                    if (!_unitAnimationController.inCombat)
                    {
                        _unitAnimationController.EnterCombat();
                    }
                    Engage();
                }else
                {
                    if (_unitAnimationController.inCombat)
                    {
                        _unitAnimationController.ExitCombat();
                    }
                    MoveTowardEnemyBase();
                }
            }

            else
            {
                if (_enemySpotted) // enemy is in range and NOT dead:
                {
                    if (!_unitAnimationController.inCombat)
                    {
                        _unitAnimationController.EnterCombat();
                    }

                    if (EnemyInRange())
                    {
                        GetComponent<UnitCombat>().Attack(_closestEnemyTransform.gameObject);
                    }

                    //_unitAnimationController.EnterCombat();
                    //GetComponent<UnitCombat>().Attack(_closestEnemyTransform.gameObject);

                }else if(_unitAnimationController.inCombat)
                {
                    _unitAnimationController.ExitCombat();
                }
            }
        }
    }
    /* // trying to have detectEnemyies work independently from unit movement!
    private void Update()
    {
        if (_isActive)
        {
            if (GetComponent<UnitDetection>().CheckForEnemies(out Transform _closestEnemy))
            if (_enemySpotted)
            {
                if(_closestEnemy != null)
                {
                    _closestEnemyTransform = _closestEnemy;
                }

                _unitAnimationController.EnterCombat();

                Engage();
            }
            else
            {
                _closestEnemyTransform = null;

                _unitAnimationController.ExitCombat();

                MoveTowardEnemyBase();
            }
        }
    }*/
    /*
    IEnumerator Advance()
    {
        if (GetComponent<UnitDetection>().CheckForEnemies(out Transform _closestEnemy))
        {
            _closestEnemyTransform = _closestEnemy.transform;

            _unitAnimationController.EnterCombat();

            Engage();
        }
        else
        {
            _closestEnemyTransform = null;

            _unitAnimationController.ExitCombat();

            MoveTowardEnemyBase();
        }

        yield return new WaitForSeconds (0.1f);

        if (_isActive)
        {
            StartCoroutine(Advance());
        }
    }*/

    /// <summary>
    /// Calling this function will drop any current objective and move towards the enemy base.
    /// </summary>
    public void MoveTowardEnemyBase()
    {
        // movement animation: CURRENTLY MISSING WALKING VS RUNNING (always walk)
        Vector3 unitVelocity = _navMeshAgent.velocity;
        float unitSpeed = unitVelocity.magnitude;
        float movementAnimationSpeed = Mathf.Clamp01(unitSpeed / _walkingSpeed) * 0.5f;
        _unitAnimationController.MoveUnit(movementAnimationSpeed);

        // this will need to check if all troops are supposed to halt, advance or run down the line!
        _navMeshAgent.speed = _walkingSpeed;
        _closestEnemyTransform = null; // allows to search for next enemy close by after previous one died.
        _navMeshAgent.SetDestination(_destinationLocation);
    }

    void Engage()
    {
        Vector3 unitVelocity = _navMeshAgent.velocity;
        float unitSpeed = unitVelocity.magnitude;

        _navMeshAgent.speed = _chargingSpeed;

        if (!EnemyInRange()) // enemy is spotted but NOT in range:
        {
            _navMeshAgent.SetDestination(_closestEnemyTransform.position);

            float movementAnimationSpeed = Mathf.Clamp01(unitSpeed / _chargingSpeed);
            _unitAnimationController.MoveUnit(movementAnimationSpeed); // charge
            //_unitAnimationController.MoveUnit(1.0f); // charge
        }
        else if (EnemyInRange()) // enemy is in range and NOT dead:
        {
            _navMeshAgent.SetDestination(transform.position); // when in range, stop moving:
            //_navMeshAgent.isStopped = true; // when in range, stop moving:

            GetComponent<UnitCombat>().Attack(_closestEnemyTransform.gameObject);

            float movementAnimationSpeed = Mathf.Clamp01(unitSpeed / _walkingSpeed) * 0.5f;
            _unitAnimationController.MoveUnit(movementAnimationSpeed);
        }
        else // all enemies spotted AND in range are dead:
        {
            MoveTowardEnemyBase();
        }
    }

    /// <summary>
    /// Check distance from this units collider boundary to the target units boundary.
    /// </summary>
    /// <returns></returns>
    bool EnemyInRange()
    {
        float distanceToEnemy = Vector3.Distance(transform.position, _closestEnemyTransform.position);
        float combinedRadii = _myColliderRadius + GetRadius(_closestEnemyTransform.GetComponent<Collider>()); // make sure units have only one collider!

        if (distanceToEnemy - combinedRadii <= _attackRange)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Get the radius of any circular colliders or half the longest side of a square collider.
    /// </summary>
    /// <param name="collider"></param>
    /// <returns></returns>
    float GetRadius(Collider collider)
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

    /// <summary>
    /// This gets called by the UnitManager, when this unit is killed.
    /// </summary>
    public void DeactivateMovement()
    {
        _isActive = false;
    }
}



// this worked, but unitDetection was part of the movement script here. This is now handled in a seperate component.
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    UnitManager _unitManager;
    NavMeshAgent _navMeshAgent;
    UnitAnimationController _unitAnimationController;

    LayerMask _unitSpottingLayer;
    Vector3 _destinationLocation; 
    bool _isActive = false;

    // variables set by the UnitManager:
    int _thisUnitsPlayerAffiliation = 0;
    float _spottingRange = 0.0f;
    float _attackRange = 0.0f;
    float _myColliderRadius = 0.0f;

    // movement speeds:
    float _walkingSpeed = 0.0f;
    float _runningSpeed = 0.0f;
    float _chargingSpeed = 0.0f;

    Transform _enemyTransform;

    public void InitializeUnitMovement()
    {
        // cache components and references:
        _unitManager = GetComponent<UnitManager>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _unitAnimationController = GetComponent<UnitAnimationController>();

        // set variables:
        _unitSpottingLayer = LayerMask.GetMask("Units"); // detection!
        _thisUnitsPlayerAffiliation = _unitManager.myPlayerAffiliation;  // detection!

        _walkingSpeed = _unitManager.walkingSpeed;
        _runningSpeed = _unitManager.runningSpeed;
        _chargingSpeed = _unitManager.chargingSpeed;
        
        _spottingRange = _unitManager.baseSpottingRange; // detection!
        _attackRange = _unitManager.baseAttackRange;
        _destinationLocation = _unitManager.unitDestination.position;
        _myColliderRadius = GetRadius(GetComponent<Collider>());

        // TESTING:
        //Debug.Log("my, "+ this.gameObject.name + " collider radius: " + _myColliderRadius, this.gameObject);
    }

    /// <summary>
    /// Called by <see cref="UnitManager"/> to tell deployed units to start moving/engaging the enemy.
    /// </summary>
    public void StartMovement()
    {
        _isActive = true;

        // enable NavMesh (if it's done earlier, units tend to get stuck on interveening terrain):
        GetComponent<NavMeshAgent>().enabled = true;

        // launch unit towards enemy base:
        MoveTowardEnemyBase();
    }

    /// <summary>
    /// Calling this function will drop any current objective and move towards the enemy base.
    /// </summary>
    public void MoveTowardEnemyBase()
    {
        // movement animation: CURRENTLY MISSING WALKING VS RUNNING (always walk)
        Vector3 unitVelocity = _navMeshAgent.velocity;
        float unitSpeed = unitVelocity.magnitude;
        float movementAnimationSpeed = Mathf.Clamp01(unitSpeed / _walkingSpeed) * 0.5f;
        _unitAnimationController.MoveUnit(movementAnimationSpeed);

        // this will need to check if all troops are supposed to halt, advance or run down the line!
        _navMeshAgent.speed = _walkingSpeed;
        _enemyTransform = null; // allows to search for next enemy close by after previous one died.
        _navMeshAgent.SetDestination(_destinationLocation);
    }


    void Update()
    {
        if (_isActive)
        {
            if (EnemySpotted())
            {
                _unitAnimationController.EnterCombat();

                Engage();
            }
            else
            {
                _unitAnimationController.ExitCombat();

                MoveTowardEnemyBase();
            }
        }
    }

    bool EnemySpotted()
    {
        // use an OverlapSphere to detect all colliders within the detection radius and on the unit layer:
        Collider[] _detectedUnitObjects = Physics.OverlapSphere(transform.position, _spottingRange + _myColliderRadius, _unitSpottingLayer); // spotting Range should be combined with collider Radius!
        List<GameObject> _spottedEnemyUnits = new List<GameObject>();

        // if at least one potential unit was detected check if any are actual units and moreso - enemies:
        if (_detectedUnitObjects.Length > 0)
        {
            foreach (Collider _unitObject in _detectedUnitObjects)
            {
                // check if this object has a UnitManager-script attached (Q: Is it an actual unit?):
                if (_unitObject.GetComponent<UnitManager>())
                {
                    if ((_unitObject.GetComponent<UnitManager>().myPlayerAffiliation == 1 || _unitObject.GetComponent<UnitManager>().myPlayerAffiliation == 2) && _unitObject.GetComponent<UnitManager>().myPlayerAffiliation  != _thisUnitsPlayerAffiliation)
                    {
                        _spottedEnemyUnits.Add(_unitObject.gameObject);
                    }
                }else
                {
                    Debug.LogError("ERROR: This object is on the UNIT-layer, but has no UnitManager attached. Check why!", _unitObject);
                }

            }
        }

        // if at least one enemy was among the detected units:
        if (_spottedEnemyUnits.Count > 0)
        {
            float closestDistance = Mathf.Infinity;

            foreach (GameObject _enemy in _spottedEnemyUnits)
            {
                float distanceToSpecificTestedUnit = Vector3.Distance(transform.position, _enemy.transform.position);

                // if this enemy is closer than the current closest one, set it as the new closest enemy:
                if (distanceToSpecificTestedUnit < closestDistance)
                {
                    _enemyTransform = _enemy.transform;
                    closestDistance = distanceToSpecificTestedUnit;
                }
            }

            // an enemy was found in spotting-bubble:
            return true; 
        }else
        {
            // no enemies were found in range, reset last known enemy transform:
            _enemyTransform = null; 
            return false; 
        }
    }

    void Engage()
    {
        Vector3 unitVelocity = _navMeshAgent.velocity;
        float unitSpeed = unitVelocity.magnitude;

        _navMeshAgent.speed = _chargingSpeed;

        if (!EnemyInRange()) // enemy is spotted but NOT in range:
        {
            _navMeshAgent.SetDestination(_enemyTransform.position);

            float movementAnimationSpeed = Mathf.Clamp01(unitSpeed / _chargingSpeed);
            _unitAnimationController.MoveUnit(movementAnimationSpeed); // charge
            //_unitAnimationController.MoveUnit(1.0f); // charge
        }
        else if(EnemyInRange()) // enemy is in range and NOT dead:
        {
            _navMeshAgent.SetDestination(transform.position); // when in range, stop moving:
            
            GetComponent<UnitCombat>().Attack(_enemyTransform.gameObject);
            
            float movementAnimationSpeed = Mathf.Clamp01(unitSpeed / _walkingSpeed) * 0.5f;
            _unitAnimationController.MoveUnit(movementAnimationSpeed);
        } 
        else // all enemies spotted AND in range are dead:
        {
            MoveTowardEnemyBase();
        }
    }

    /// <summary>
    /// Check distance from this units collider boundary to the target units boundary.
    /// </summary>
    /// <returns></returns>
    bool EnemyInRange()
    {
        float distanceToEnemy = Vector3.Distance(transform.position, _enemyTransform.position);
        float combinedRadii = _myColliderRadius + GetRadius(_enemyTransform.GetComponent<Collider>()); // make sure units have only one collider!

        if (distanceToEnemy - combinedRadii <= _attackRange)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Get the radius of any circular colliders or half the longest side of a square collider.
    /// </summary>
    /// <param name="collider"></param>
    /// <returns></returns>
    float GetRadius(Collider collider)
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

    /// <summary>
    /// This gets called by the UnitManager, when this unit is killed.
    /// </summary>
    public void DeactivateMovement()
    {
        _isActive = false;
    }
}*/