using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] private LayerMask _uiLayerMask;

    // who calls this?
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
            if (_unitType.GetComponent<UnitManager>().unitData.deploymentCost < _minimumUnitCost)
            {
                _minimumUnitCost = _unitType.GetComponent<UnitManager>().unitData.deploymentCost;
            }
        }

        Debug.Log("minimum unit deployment cost of player 2 is " + _minimumUnitCost + " gold.");
    }
    
    
    void GetDeploymentArea()
    {
        _team2DeploymentZoneTiles = GetComponent<LevelArchitect>().mapConfig.team2DeploymentZoneTiles;
        _usedTeam2DeploymentZoneTiles = GetComponent<LevelArchitect>().mapConfig.usedTeam2DeploymentZoneTiles;
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
        List<SpawnZone> invalidAvailableSpawnZones = new List<SpawnZone>();
        int maxAttempts = 10;
        int attempt = 0;

        while (attempt < maxAttempts)
        {
            attempt++;

            // Randomly choose a soldier from available units
            GameObject _randomSoldierToBeBought = _arrayOfAvailableUnits[Random.Range(0, _arrayOfAvailableUnits.Length)];

            // Instantiate the unit (only passing player affiliation for now)
            GameObject _instantiatedUnit = Instantiate(_randomSoldierToBeBought, Vector3.zero, Quaternion.identity, _enemyArmyParentGO.transform);
            UnitManager unitManager = _instantiatedUnit.GetComponent<UnitManager>();
            unitManager.InitializeUnit(2); // Only pass the player affiliation

            // Retrieve the unit's footprint after initialization
            int unitWidth = unitManager.unitProfile.footprintWidth;
            int unitDepth = unitManager.unitProfile.footprintDepth;

            // Choose a random spawn zone
            int _rngNr = Random.Range(0, _team2DeploymentZoneTiles.Count);
            SpawnZone _rngLocation = _team2DeploymentZoneTiles[_rngNr].GetComponent<SpawnZone>();

            // Check if this zone has already been used or is invalid
            if (_usedTeam2DeploymentZoneTiles.Contains(_rngLocation.transform) || invalidAvailableSpawnZones.Contains(_rngLocation))
            {
                continue;
            }

            Debug.Log("Attempting to place unit at zone: " + _rngLocation.name);

            // Check if there is enough room for the unit's footprint
            if (CheckAdjacentZones(_rngLocation, unitWidth, unitDepth))
            {
                // Get the list of zones affected by the footprint
                List<SpawnZone> footprintZones = GetFootprintZones(_rngLocation, unitWidth, unitDepth);

                // Mark all the zones as occupied
                foreach (var zone in footprintZones)
                {
                    _usedTeam2DeploymentZoneTiles.Add(zone.transform);
                    zone.OccupyDeploymentTile();
                }

                // Update the unit with the occupied zones
                unitManager.UpdateZonesDeployedByUnit(footprintZones);

                // Set the unit's position and rotation in the final location
                Quaternion leftFacingRotation = Quaternion.Euler(0, -90, 0);
                _instantiatedUnit.transform.position = _rngLocation.transform.position;
                _instantiatedUnit.transform.rotation = leftFacingRotation;

                // Subtract gold for the unit's cost
                GetComponent<GoldManager>().SubtractGold(2, unitManager.unitProfile.deploymentCost);

                Debug.Log("Successfully deployed unit: " + _randomSoldierToBeBought.name);
                return;
            }
            else
            {
                Debug.LogWarning("Invalid zone found. Adding to invalid zones list.");
                invalidAvailableSpawnZones.Add(_rngLocation);

                // If all zones become invalid for the current unit, clear the list and try a different unit
                if (invalidAvailableSpawnZones.Count >= _team2DeploymentZoneTiles.Count)
                {
                    invalidAvailableSpawnZones.Clear();
                    Debug.LogWarning("All zones invalid for current unit type. Trying different unit.");
                    continue;
                }
            }
        }

        Debug.LogError("Max attempts reached while trying to spawn a unit. Possible issue with available space.");
    }
    /*
    void SpawnSoldier()
    {
        List<SpawnZone> invalidAvailableSpawnZones = new List<SpawnZone>();
        int maxAttempts = 10;
        int attempt = 0;

        while (attempt < maxAttempts)
        {
            attempt++;

            GameObject _randomSoldierToBeBought = _arrayOfAvailableUnits[Random.Range(0, _arrayOfAvailableUnits.Length)];
            UnitManager unitManager = _randomSoldierToBeBought.GetComponent<UnitManager>();
            
            int unitWidth = unitManager.unitProfile.footprintWidth;
            int unitDepth = unitManager.unitProfile.footprintDepth;

            int _rngNr = Random.Range(0, _team2DeploymentZoneTiles.Count);
            SpawnZone _rngLocation = _team2DeploymentZoneTiles[_rngNr].GetComponent<SpawnZone>();

            if (_usedTeam2DeploymentZoneTiles.Contains(_rngLocation.transform) || invalidAvailableSpawnZones.Contains(_rngLocation))
            {
                continue;
            }

            Debug.Log("Attempting to place unit at zone: " + _rngLocation.name);

            if (CheckAdjacentZones(_rngLocation, unitWidth, unitDepth))
            {
                List<SpawnZone> footprintZones = GetFootprintZones(_rngLocation, unitWidth, unitDepth);

                foreach (var zone in footprintZones)
                {
                    _usedTeam2DeploymentZoneTiles.Add(zone.transform);
                    zone.OccupyDeploymentTile();
                }

                Quaternion leftFacingRotation = Quaternion.Euler(0, -90, 0);

                GameObject _instantiatedUnit = Instantiate(_randomSoldierToBeBought, _rngLocation.transform.position, leftFacingRotation, _enemyArmyParentGO.transform);
                _instantiatedUnit.GetComponent<UnitManager>().InitializeUnit(2, footprintZones);

                GetComponent<GoldManager>().SubtractGold(2, unitManager.unitProfile.deploymentCost);

                Debug.Log("Successfully deployed unit: " + _randomSoldierToBeBought.name);
                return;
            }
            else
            {
                Debug.LogWarning("Invalid zone found. Adding to invalid zones list.");
                invalidAvailableSpawnZones.Add(_rngLocation);

                if (invalidAvailableSpawnZones.Count >= _team2DeploymentZoneTiles.Count)
                {
                    invalidAvailableSpawnZones.Clear();
                    Debug.LogWarning("All zones invalid for current unit type. Trying different unit.");
                    continue;
                }
            }
        }

        Debug.LogError("Max attempts reached while trying to spawn a unit. Possible issue with available space.");
    }*/

    private bool CheckAdjacentZones(SpawnZone startZone, int width, int depth)
    {
        Vector3 startPosition = startZone.transform.position;
        float gridSize = 1.0f;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                Vector3 adjacentPosition = startPosition + new Vector3(x * gridSize, 0, z * gridSize);

                if (Physics.Raycast(adjacentPosition + Vector3.up * 10, Vector3.down, out RaycastHit hit, Mathf.Infinity, _uiLayerMask))
                {
                    SpawnZone adjacentZone = hit.collider.GetComponent<SpawnZone>();

                    if (adjacentZone == null)
                    {
                        Debug.LogError("No SpawnZone found at " + adjacentPosition);
                        return false;
                    }

                    if (adjacentZone.occupied)
                    {
                        Debug.LogWarning("Zone " + adjacentZone.name + " is already occupied.");
                        return false;
                    }
                }
                else
                {
                    Debug.LogError("Raycast failed at position " + adjacentPosition);
                    return false;
                }
            }
        }

        return true;
    }

    private List<SpawnZone> GetFootprintZones(SpawnZone startZone, int width, int depth)
    {
        List<SpawnZone> zones = new List<SpawnZone>();

        Vector3 startPosition = startZone.transform.position;
        float gridSize = 1.0f;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                Vector3 adjacentPosition = startPosition + new Vector3(x * gridSize, 0, z * gridSize);

                if (Physics.Raycast(adjacentPosition + Vector3.up * 10, Vector3.down, out RaycastHit hit, Mathf.Infinity, _uiLayerMask))
                {
                    SpawnZone adjacentZone = hit.collider.GetComponent<SpawnZone>();

                    if (adjacentZone != null)
                    {
                        zones.Add(adjacentZone);
                    }
                    else
                    {
                        Debug.LogError("No SpawnZone found at " + adjacentPosition);
                    }
                }
                else
                {
                    Debug.LogError("Raycast failed at position " + adjacentPosition);
                }
            }
        }

        return zones;
    }
}
    /*
    bool CheckIfUnitCanBePlacedInZone(GameObject zone, Footprint footprint)
    {
        // Implement the footprint checking logic here.
        return zone.GetComponent<SpawnZone>().CheckAdjacentZones(footprint);
    }*/
    /*
    bool IsSpaceAvailable(GameObject spawnZone, int width, int depth)
    {
        Vector3 startPosition = spawnZone.transform.position;
        float gridSize = 1.0f; // Assuming each grid tile is 1x1 unit

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                Vector3 checkPosition = startPosition + new Vector3(x * gridSize, 0, z * gridSize);
                if (!IsZoneAvailable(checkPosition))
                {
                    return false; // Space not available
                }
            }
        }
        return true; // All zones are free
    }*/
    /*
    bool IsZoneAvailable(Vector3 position)
    {
        if (Physics.Raycast(position + Vector3.up * 10, Vector3.down, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("SpawnZone")))
        {
            SpawnZone zone = hit.collider.GetComponent<SpawnZone>();
            if (zone != null && !zone.occupied)
            {
                return true; // Zone is available
            }
        }
        return false; // Zone is occupied or not found
    }*/
//}
