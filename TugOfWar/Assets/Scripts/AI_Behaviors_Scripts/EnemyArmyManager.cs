using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArmyManager : MonoBehaviour
{
    [SerializeField] GameObject[] _arrayOfSoldiers;
    [SerializeField] GameObject _enemyArmyParentGO;
    
    List<GameObject> _team2DeploymentZoneTiles = new List<GameObject>(); // public so the AI can access this to get its deployment zone
    List<Transform> _usedTeam2DeploymentZoneTiles = new List<Transform>(); // public so the AI can access this to get its deployment zone

    float _deploymentIntervalMin = 10.0f;
    float _deploymentIntervalMax = 40.0f;
    float _minimumUnitCost;


    public void DeployStartingArmy()
    {
        GetDeploymentArea();
        CalculateMinimumDeploymentCost();
        SpawnArmy();
    }
    void CalculateMinimumDeploymentCost()
    {
        // add up all the unit costs of the units in the array to get a value thats guaranteed higher than the lowest cost therein:
        foreach(GameObject _unitType in _arrayOfSoldiers)
        {
            _minimumUnitCost += _unitType.GetComponent<UnitManager>().deploymentCost;
        }

        // loop through the array again and find the cheapest unit therein:
        foreach (GameObject _unitType in _arrayOfSoldiers)
        {
            if(_unitType.GetComponent<UnitManager>().deploymentCost < _minimumUnitCost)
            {
                _minimumUnitCost = _unitType.GetComponent<UnitManager>().deploymentCost;
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
        while(GetComponent<GoldManager>().team2Wallet >= _minimumUnitCost) 
        {
            SpawnSoldier();
        }
    }
    void SpawnSoldier()
    {
        // get random soldier:
        GameObject _randomSoldierToBeBought = _arrayOfSoldiers[Random.Range(0, _arrayOfSoldiers.Length)];
        
        // get random spawn loctation:
        int _rngNr = Random.Range(0, _team2DeploymentZoneTiles.Count);
        GameObject _rngLocation = _team2DeploymentZoneTiles[_rngNr];
        if (!_usedTeam2DeploymentZoneTiles.Contains(_rngLocation.transform))
        {
            // mark the chosen deployment tile as occupied and add it to corresponding list:
            _usedTeam2DeploymentZoneTiles.Add(_rngLocation.transform);
            _team2DeploymentZoneTiles[_rngNr].GetComponent<SpawnZone>().OccupyDeploymentTile();

            // create and setup the unit:
            GameObject _instantiatedUnit = Instantiate(_randomSoldierToBeBought, _rngLocation.transform.position, Quaternion.identity, _enemyArmyParentGO.transform);
            _instantiatedUnit.GetComponent<UnitManager>().SetupThisUnit(false, _rngLocation);

            // pay for the unit:
            GetComponent<GoldManager>().SubtractGold(2, _instantiatedUnit.GetComponent<UnitManager>().deploymentCost);
        }
        else
        {
            // target spawn location was unavailable, so start over:
            SpawnSoldier();
            return;
        }
    }


    /*
    void GetRandomSpawnLocation()
    {
        int _rngNr = Random.Range(0, _team2DeploymentZoneTiles.Count);
        GameObject _rngLocation = _team2DeploymentZoneTiles[_rngNr];
    }*/
    /*
    bool HasEnoughGold(GameObject _soldierToBeBought)
    {
        if (_soldierToBeBought.GetComponent<UnitManager>().deploymentCost <= (GetComponent<GameManager>().goldCoinsMax - GetComponent<GameManager>().usedGoldCoinsAI))
        {
            return true;
        }else
        {
            Debug.Log("Trying to Exit");
            return false;
        }
    }*/


    // safeties:
    /*
    public void SpawnEnemyArmy()
    {
        Debug.Log("1");
        if ((GetComponent<GameManager>().goldCoinsMax - GetComponent<GameManager>().usedGoldCoinsAI) > 0)
        {
            SpawnSoldier();
            Debug.Log("2");
        }
        else
        {
            Debug.Log("finished army!");
        }
    }*/

    /*
    void SpawnSoldier()
    {
        // get random soldier:
        GameObject _randomSoldierToBeBought = _arrayOfSoldiers[Random.Range(0, _arrayOfSoldiers.Length)];
        int _rngNr = Random.Range(0, _team2DeploymentZoneTiles.Count);
        GameObject _rngLocation = _team2DeploymentZoneTiles[_rngNr];

        if (HasEnoughGold(_randomSoldierToBeBought) && !_usedTeam2DeploymentZoneTiles.Contains(_rngLocation.transform))
        {
            _usedTeam2DeploymentZoneTiles.Add(_rngLocation.transform);
            GameObject _instantiatedSoldier = Instantiate(_randomSoldierToBeBought, _rngLocation.transform.position, Quaternion.identity, _enemyArmyParentGO.transform);
            _instantiatedSoldier.GetComponent<UnitManager>().UnitWasPlaced(false, _rngLocation);
            _team2DeploymentZoneTiles[_rngNr].GetComponent<SpawnZone>().OccupyDeploymentTile();
        }
        // this beyond stuff was cancelled out when making this safety
        else
        {
            SpawnSoldier();
            //return;
        }

        // this will make SpawnSoldier loop until all money is spent. For or foreach loops are less practical here, as AI would only be able to spawn 1-gold units or bug out.
        ///SpawnEnemyArmy();
    }*/
    /*void SpawnSoldier()
    {
        // get random soldier:
        GameObject _randomSoldierToBeBought = _arrayOfSoldiers[Random.Range(0, _arrayOfSoldiers.Length)];

        if (HasEnoughGold(_randomSoldierToBeBought))
        {
            //GameObject _rngLocation = _blueTeamDeploymentZoneTiles[Random.Range(0, _blueTeamDeploymentZoneTiles.Count)];
            int _rngNr = Random.Range(0, _blueTeamDeploymentZoneTiles.Count);
            GameObject _rngLocation = _blueTeamDeploymentZoneTiles[_rngNr];

            if (!_usedblueTeamDeploymentZoneTiles.Contains(_rngLocation.transform))
            {
                _usedblueTeamDeploymentZoneTiles.Add(_rngLocation.transform);
                GameObject _instantiatedSoldier = Instantiate(_randomSoldierToBeBought, _rngLocation.transform.position, Quaternion.identity, _enemyArmyParentGO.transform);
                _instantiatedSoldier.GetComponent<UnitManager>().UnitWasPlaced(false, _rngLocation);
                _blueTeamDeploymentZoneTiles[_rngNr].GetComponent<SpawnZone>().ItemWasPlaced();
            }else
            {
                SpawnEnemyArmy();
                return;
            }
        }else
        {
            SpawnEnemyArmy();
            return;
        }

        // this will make SpawnSoldier loop until all money is spent. For or foreach loops are less practical here, as AI would only be able to spawn 1-gold units or bug out.
        SpawnEnemyArmy();
    }*/
    /*
    void PlaceItem()
    {
        _targetedGameObject.GetComponent<SpawnZone>().ItemWasPlaced();
        carriedObject.transform.position = _mousePosition;

        if (carriedObject.GetComponent<UnitManager>())
        {
            carriedObject.GetComponent<UnitManager>().UnitWasPlaced(true, _targetedGameObject);
        }
        carriedObject = null;
    }*/
}
