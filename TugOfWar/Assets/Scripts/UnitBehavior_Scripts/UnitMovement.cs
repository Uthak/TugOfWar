using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    // variables set by itself:
    NavMeshAgent _navMeshAgent;
    NavMeshPath _navMeshPath; // currently unused
    LayerMask _layerMask;
    Vector3 _targetLocation;
    bool _gameStarted = false;

    // variables set by the UnitManager:
    int _teamAffiliation = 0;
    float _spottingRange = 0.0f;
    float _attackRange = 0.0f;
    //bool _rangedUnit = false;
    Transform _enemyTransform;
    //private List<GameObject> _enemiesInRange = new List<GameObject>();

    //bool _isMoving = false;

    /// <summary>
    /// Called by this units "UnitManager" to update relevant movement-data bfore starting:
    /// </summary>
    /// <param name="_teamID"></param>
    public void UpdateUnitMovement()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        //_navMeshPath = new NavMeshPath();
        _layerMask = LayerMask.GetMask("Units");
        _teamAffiliation = GetComponent<UnitManager>().teamAffiliation;
        _navMeshAgent.speed = GetComponent<UnitManager>().movementSpeed;
        _spottingRange = GetComponent<UnitManager>().spottingRange;

        // compare this units attack-ranges to determine attackRange which is used in making assault-moves:
        if (GetComponent<UnitManager>().rangedAttackRange > GetComponent<UnitManager>().meleeAttackRange)
        {
            _attackRange = GetComponent<UnitManager>().rangedAttackRange;
        }else
        {
            _attackRange = GetComponent<UnitManager>().meleeAttackRange;
        }

        // get enemy-base location based on team affiliation:
        switch (_teamAffiliation)
        {
            case 1: // player-team:
                _targetLocation = FindObjectOfType<GameManager>().team1Destination.transform.position;
                break;

            case 2: // AI-team:
                _targetLocation = FindObjectOfType<GameManager>().team2Destination.transform.position;
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
            }else
            {
                MoveTowardFinalDestination();
            }

            //StartCoroutine(CheckIfMoving());
        }
    }

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
    /*
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
    }*/
    void Engage()
    {
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
        if(Vector3.Distance(transform.position, _enemyTransform.position) <= _attackRange)
        {
            return true;
        }
        return false;
    }
}
