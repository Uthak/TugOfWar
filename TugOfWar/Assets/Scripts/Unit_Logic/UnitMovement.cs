using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    UnitManager _unitManager;
    NavMeshAgent _navMeshAgent;

    LayerMask _unitSpottingLayer;
    Vector3 _destinationLocation; 
    bool _wasLaunched = false;

    // variables set by the UnitManager:
    int _thisUnitsPlayerAffiliation = 0;
    float _spottingRange = 0.0f;
    float _attackRange = 0.0f;
    float _myColliderRadius = 0.0f;

    Transform _enemyTransform;

    public void InitializeUnitMovement()
    {
        // cache components and references:
        _unitManager = GetComponent<UnitManager>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        // set variables:
        _unitSpottingLayer = LayerMask.GetMask("Units");
        _thisUnitsPlayerAffiliation = _unitManager.playerAffiliation;
        _navMeshAgent.speed = _unitManager.baseMovementSpeed;
        _spottingRange = _unitManager.baseSpottingRange;
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
        _wasLaunched = true;

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
        _enemyTransform = null; // allows to search for next enemy close by after previous one died.
        _navMeshAgent.SetDestination(_destinationLocation);
    }


    void Update()
    {
        if (_wasLaunched)
        {
            if (EnemySpotted())
            {
                Engage();
            }else
            {
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
                    if ((_unitObject.GetComponent<UnitManager>().playerAffiliation == 1 || _unitObject.GetComponent<UnitManager>().playerAffiliation == 2) && _unitObject.GetComponent<UnitManager>().playerAffiliation  != _thisUnitsPlayerAffiliation)
                    {
                        /*Debug.Log("spotted this enemy: " + _unitObject.gameObject.name, _unitObject.gameObject);
                        Debug.Log("my team ID " + _thisUnitsPlayerAffiliation + " and target team ID " + _unitObject.GetComponent<UnitManager>().playerAffiliation);
                        Debug.Log("my location " + transform.position + " and target location " + _unitObject.transform.position);*/
                        
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
        if (!EnemyInRange()) // enemy is spotted but NOT in range:
        {
            _navMeshAgent.SetDestination(_enemyTransform.position);
        } else if(EnemyInRange()) // enemy is in range and NOT dead:
        {
            _navMeshAgent.SetDestination(transform.position); // when in range, stop moving:
            
            GetComponent<UnitCombat>().Attack(_enemyTransform.gameObject);
        } else // all enemies spotted AND in range are dead:
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
}

//***

/*
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    NavMeshAgent _navMeshAgent;
    NavMeshPath _navMeshPath; // currently unused
    LayerMask _layerOfObjectsToDetect;
    Vector3 _targetLocation; 
    bool _gameStarted = false;

    // variables set by the UnitManager:
    int _teamAffiliation = 0;
    float _spottingRange = 0.0f;
    float _attackRange = 0.0f;
    Transform _enemyTransform;

    //private List<GameObject> _enemiesInRange = new List<GameObject>();
    //bool _isMoving = false;


    public void InitializeUniMovement()
    {
        
    }

    /// <summary>
    /// Called by this units "UnitManager" to update relevant movement-data bfore starting:
    /// </summary>
    /// <param name="_teamID"></param>
    public void UpdateUnitMovement()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        //_navMeshPath = new NavMeshPath();
        _layerOfObjectsToDetect = LayerMask.GetMask("Units");
        _teamAffiliation = GetComponent<UnitManager>().playerAffiliation;
        _navMeshAgent.speed = GetComponent<UnitManager>().baseMovementSpeed;
        _spottingRange = GetComponent<UnitManager>().baseSpottingRange;

        /*
        // compare this units attack-ranges to determine attackRange which is used in making assault-moves:
        if (GetComponent<UnitManager>().rangedAttackRange > GetComponent<UnitManager>().meleeAttackRange)
        {
            _attackRange = GetComponent<UnitManager>().rangedAttackRange;
        }else
        {
            _attackRange = GetComponent<UnitManager>().meleeAttackRange;
        }
_attackRange = GetComponent<UnitManager>().baseAttackRange;

// get enemy-base location based on team affiliation:
switch (_teamAffiliation)
{
    case 1: // player-team:
        _targetLocation = FindObjectOfType<GameManager>().player1Destination.transform.position;
        break;

    case 2: // AI-team:
        _targetLocation = FindObjectOfType<GameManager>().player2Destination.transform.position;
        break;

    default:
        break;
}
    }

    // call this on every unit when play is pressed:
    public void GameStarted()
{
    _gameStarted = true;
    MoveTowardFinalDestination();
}
public void MoveTowardFinalDestination()
{
    _enemyTransform = null; // allows to search for next enemy close by after previous one died.
    _navMeshAgent.SetDestination(_targetLocation);
}

void Update()
{
    if (_gameStarted)
    {
        if (EnemySpotted())
        {
            Engage();
        }
        else
        {
            MoveTowardFinalDestination();
        }

        //StartCoroutine(CheckIfMoving());
    }
}

bool EnemySpotted()
{
    // Use an OverlapSphere to detect all colliders within the detection radius
    Collider[] _detectedUnits = Physics.OverlapSphere(transform.position, _spottingRange, _layerOfObjectsToDetect);
    List<GameObject> _enemiesInRange = new List<GameObject>();

    // If at least one unit was detected check if any are actual enemies:
    if (_detectedUnits.Length > 0)
    {
        foreach (Collider _unit in _detectedUnits)
        {
            if (_unit.GetComponent<UnitManager>().playerAffiliation != _teamAffiliation)
            {
                _enemiesInRange.Add(_unit.gameObject);
            }
        }
    }

    // If at least one enemy was among the detected units:
    if (_enemiesInRange.Count > 0)
    {
        //Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject _enemy in _enemiesInRange)
        {
            float distanceToSpecificTestedUnit = Vector3.Distance(transform.position, _enemy.transform.position);

            // If this enemy is closer than the current closest one, set it as the new closest enemy
            if (distanceToSpecificTestedUnit < closestDistance)
            {
                _enemyTransform = _enemy.transform;
                closestDistance = distanceToSpecificTestedUnit;
            }
        }

        return true; // an enemy was found in range
    }
    else
    {
        _enemyTransform = null;
        return false; // no enemies were found in range
    }
}

IEnumerator CheckIfMoving()
{
    Vector3 startPos = transform.position;
    yield return new WaitForSeconds(.2f);
    Vector3 finalPos = transform.position;

    if (startPos != finalPos)
    {
        //isMoving = true;
        // do nothing (still moving)
    }else
    {
        //isMoving = false;
        GetNewPath();
    }
}
public void GetNewPath()
{
    Vector3 _target;

    if(_enemyTransform == null)
    {
        _target = _targetLocation;
    }else
    {
        _target = _enemyTransform.position;
    }

    //validPath = _navMeshAgent.CalculatePath(_target, _navMeshPath);
    _navMeshAgent.CalculatePath(_target, _navMeshPath);
    _navMeshAgent.SetDestination(_target);
}*/
/*
bool EnemySpotted()
{
    // Use an OverlapSphere to detect all colliders within the detection radius
    Collider[] _detectedUnits = Physics.OverlapSphere(transform.position, _spottingRange, _layerMask);
    List<GameObject> _enemiesInRange = new List<GameObject>();

    // If at least one unit was detected check if any are actual enemies:
    if (_detectedUnits.Length > 0)
    {
        foreach (Collider _unit in _detectedUnits)
        {
            if (_unit.GetComponent<UnitManager>().teamAffiliation != _teamAffiliation)
            {
                _enemiesInRange.Add(_unit.gameObject);
            }
        }
    }

    // If at least one enemy was among the detected units:
    if (_enemiesInRange.Count > 0)
    {
        //Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject _enemy in _enemiesInRange)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _enemy.transform.position);

            // If this enemy is closer than the current closest one, set it as the new closest enemy
            if (distanceToPlayer < closestDistance)
            {
                _enemyTransform = _enemy.transform;
                closestDistance = distanceToPlayer;
            }
        }

        return true; // an enemy was found in range
    }else
    {
        _enemyTransform = null;
        return false; // no enemies were found in range
    }
}
void Engage()
{
     // disabled for testing *************************************
    if (!EnemyInRange()) // enemy is spotted but NOT in range:
    {
        _navMeshAgent.SetDestination(_enemyTransform.position);
    } else if(EnemyInRange()) // enemy is in range and NOT dead:
    {
        _navMeshAgent.SetDestination(transform.position); // stay where you are
        GetComponent<UnitCombat>().Attack(_enemyTransform.gameObject);
    } else // all enemies spotted AND in range are dead:
    {
        MoveTowardFinalDestination();
    }
}

bool EnemyInRange()
{
    if (Vector3.Distance(transform.position, _enemyTransform.position) <= _attackRange)
    {
        return true;
    }
    return false;
}
}
*/