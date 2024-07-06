using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArmyManager : MonoBehaviour
{
    [SerializeField] GameObject[] _arrayOfAvailableUnits;
    [SerializeField] GameObject _enemyArmyParentGO;
    
    List<GameObject> _team2DeploymentZoneTiles = new List<GameObject>(); // public so the AI can access this to get its deployment zone
    List<Transform> _usedTeam2DeploymentZoneTiles = new List<Transform>(); // public so the AI can access this to get its deployment zone

    float _deploymentIntervalMin = 10.0f;
    float _deploymentIntervalMax = 40.0f;
    float _minimumUnitCost;
    bool _gameHasStarted = false; // used to automatically launch waves after the first one was launched by player 1


    public void DeployAIStartingArmy()
    {
        GetDeploymentArea();
        CalculateMinimumDeploymentCost();
        SpawnArmy();
    }

    /// <summary>
    /// NOTE: currently the cost of a unit are derived ONLY from its unitData, not considering pot. gear cost!
    /// </summary>
    void CalculateMinimumDeploymentCost()
    {
        // add up all the unit costs of the units in the array to get a value thats guaranteed higher than the lowest cost therein:
        foreach(GameObject _unitType in _arrayOfAvailableUnits)
        {
            _minimumUnitCost += _unitType.GetComponent<UnitManager>().unitData.deploymentCost;
        }

        // loop through the array again and find the cheapest unit therein:
        foreach (GameObject _unitType in _arrayOfAvailableUnits)
        {
            //if(_unitType.GetComponent<UnitManager>().baseDeploymentCost < _minimumUnitCost)
            if (_unitType.GetComponent<UnitManager>().unitData.deploymentCost < _minimumUnitCost)
            {
                //_minimumUnitCost = _unitType.GetComponent<UnitManager>().baseDeploymentCost;
                _minimumUnitCost = _unitType.GetComponent<UnitManager>().unitData.deploymentCost;
            }
        }

        Debug.Log("minimum unit deployment cost of player 2 is " + _minimumUnitCost + " gold.");
    }
    void GetDeploymentArea()
    {
        _team2DeploymentZoneTiles = GetComponent<LevelBuilder>().team2DeploymentZoneTiles;
        _usedTeam2DeploymentZoneTiles = GetComponent<LevelBuilder>().usedTeam2DeploymentZoneTiles;
    }

    public void StartContineousDeployment()
    {
        StartCoroutine(DeployForce());
        _gameHasStarted = true;
    }
    IEnumerator DeployForce()
    {
        SpawnArmy();

        float _rngIntervalTime = Random.Range(_deploymentIntervalMin, _deploymentIntervalMax);
        Debug.Log("interval time: " + _rngIntervalTime);

        yield return new WaitForSeconds(_rngIntervalTime);

        StartCoroutine(DeployForce());
    }

    /// <summary>
    /// This is currently done randomly and without proper AI. This needs to be overhauled completly 
    /// and is only for testing purposes!!!
    /// </summary>
    void SpawnArmy()
    {
        // deploy as many troops as there is current budget:
        while(GetComponent<GoldManager>().team2Wallet >= _minimumUnitCost && _team2DeploymentZoneTiles.Count > _usedTeam2DeploymentZoneTiles.Count) 
        {
            SpawnSoldier();
        }

        // launch all new AI units:
        if (_gameHasStarted)
        {
            GetComponent<GameManager>().LaunchWave(2);
        }
    }
    void SpawnSoldier()
    {
        // get random soldier:
        GameObject _randomSoldierToBeBought = _arrayOfAvailableUnits[Random.Range(0, _arrayOfAvailableUnits.Length)];
        
        // get random spawn loctation:
        int _rngNr = Random.Range(0, _team2DeploymentZoneTiles.Count);
        GameObject _rngLocation = _team2DeploymentZoneTiles[_rngNr];
        if (!_usedTeam2DeploymentZoneTiles.Contains(_rngLocation.transform))
        {
            // mark the chosen deployment tile as occupied and add it to corresponding list:
            _usedTeam2DeploymentZoneTiles.Add(_rngLocation.transform);
            _team2DeploymentZoneTiles[_rngNr].GetComponent<SpawnZone>().OccupyDeploymentTile();

            // define the rotation to face the right side of the screen (positive x direction):
            Quaternion leftFacingRotation = Quaternion.Euler(0, -90, 0); // Assuming the unit's forward is along the z-axis

            // create and setup the unit:
            GameObject _instantiatedUnit = Instantiate(_randomSoldierToBeBought, _rngLocation.transform.position, leftFacingRotation, _enemyArmyParentGO.transform);
            _instantiatedUnit.GetComponent<UnitManager>().InitializeUnit(2);
            _instantiatedUnit.GetComponent<UnitManager>().DeployThisUnit(2, _rngLocation);

            // pay for the unit:
            GetComponent<GoldManager>().SubtractGold(2, _instantiatedUnit.GetComponent<UnitManager>().baseDeploymentCost);
        }
        else
        {
            // target spawn location was unavailable, so start over:
            SpawnSoldier();
            return;
        }
    }
}
