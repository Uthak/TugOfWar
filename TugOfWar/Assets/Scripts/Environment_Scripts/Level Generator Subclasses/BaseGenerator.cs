using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGenerator : MonoBehaviour
{
    LevelArchitect _levelArchitect;
    MapConfig _mapConfig;
    BaseConfig _baseConfig;

    public void InitializeBaseGenerator(MapConfig mapConfig, BaseConfig baseConfig)
    {
        _levelArchitect = FindAnyObjectByType<LevelArchitect>();
        _mapConfig = mapConfig; 
        _baseConfig = baseConfig;
    }

    public void GenerateBases()
    {
        CreateHeadquarter(1);
        CreateHeadquarter(2);

        CreateGuardTowers(1);
        CreateGuardTowers(2);

        _levelArchitect.UpdateMapConfig(_mapConfig);
    }


    #region Create Base HQ's:
    private void CreateHeadquarter(int playerID)
    {
        float halfSizeOfHQ;
        float xPosHQ;
        float zPosHQ;
        Vector3 hqPos;

        switch (playerID)
        {
            case 0: // environment
                break;

            case 1: // player 1:
                halfSizeOfHQ = GetObjectSize(_baseConfig.playerOneHeadquarter.GetComponent<Collider>());
                xPosHQ = _mapConfig.gridWidth - _mapConfig.gridWidth + halfSizeOfHQ; // e.g. 0 base line
                zPosHQ = _mapConfig.gridDepth / 2.0f;
                hqPos = new Vector3(xPosHQ, 0, zPosHQ);

                // instantiate and initialize HQ:
                _baseConfig.player1HQ = Instantiate(_baseConfig.playerOneHeadquarter, hqPos, Quaternion.Euler(0, 90, 0), _baseConfig.playerOneUnitsParent);
                _baseConfig.player1HQ.GetComponent<UnitManager>().InitializeUnit(1);
                break;

            case 2:
                halfSizeOfHQ = GetObjectSize(_baseConfig.playerTwoHeadquarter.GetComponent<Collider>());
                xPosHQ = _mapConfig.gridWidth - halfSizeOfHQ; // e.g. 0 base line
                zPosHQ = _mapConfig.gridDepth / 2.0f;
                hqPos = new Vector3(xPosHQ, 0, zPosHQ);

                // instantiate and initialize HQ:
                _baseConfig.player2HQ = Instantiate(_baseConfig.playerTwoHeadquarter, hqPos, Quaternion.Euler(0, -90, 0), _baseConfig.playerTwoUnitsParent);
                _baseConfig.player2HQ.GetComponent<UnitManager>().InitializeUnit(2);
                break;

            case 3: // neutral
                break;
        }

        Debug.Log("CONTROL: Both HQ's have been placed and connected!");
    }

    #region Create Base Towers:
    private void CreateGuardTowers(int playerID)
    {
        float towerZPosition;
        
        // calculate space between the desired nr of towers along the deployment-zones perimeter:
        //float gridSectionDepth = (float)_gridDepth / ((float)_p1NrOfTowers + 1.0f);
        //float gridSectionDepth = (float)_mapConfig.gridDepth / ((float)_baseConfig.p1NrOfTowers + 1.0f);

        switch (playerID)
        {
            case 0: // environment
                break;

            case 1: // player 1
                float p1HalfSizeOfTower = GetObjectSize(_baseConfig.playerOneGuardTower.GetComponent<Collider>());
                towerZPosition = (float)_mapConfig.gridDepth / ((float)_baseConfig.p1NrOfTowers + 1.0f);

                for (int i = 0; i < _baseConfig.p1NrOfTowers; i++)
                {
                    //float xPos = _mapConfig.gridWidth - _mapConfig.gridWidth + _player1DeploymentZoneWidth + p1HalfSizeOfTower;
                    float xPos = _mapConfig.gridWidth - _mapConfig.gridWidth + _mapConfig.player1DeploymentZoneWidth + p1HalfSizeOfTower;
                    float zPos = towerZPosition * (1 + i);
                    Vector3 towerPosPlayerOne = new Vector3(xPos, 0, zPos);
                    GameObject tower = Instantiate(_baseConfig.playerOneGuardTower, towerPosPlayerOne, Quaternion.Euler(0, 90, 0), _baseConfig.playerOneUnitsParent);
                    tower.GetComponent<UnitManager>().InitializeUnit(1);
                }

                // shouldnt need a reset!
                //towerZPosition = (float)_gridDepth / ((float)_p2NrOfTowers + 1.0f);
                break;

            case 2: // player 2
                float p2HalfSizeOfTower = GetObjectSize(_baseConfig.playerTwoGuardTower.GetComponent<Collider>());
                towerZPosition = (float)_mapConfig.gridDepth / ((float)_baseConfig.p2NrOfTowers + 1.0f);

                for (int i = 0; i < _baseConfig.p2NrOfTowers; i++)
                {
                    float xPos = _mapConfig.gridWidth - _mapConfig.player2DeploymentZoneWidth - p2HalfSizeOfTower;
                    float zPos = towerZPosition * (1 + i);
                    Vector3 towerPosPlayerTwo = new Vector3(xPos, 0, zPos);
                    GameObject tower = Instantiate(_baseConfig.playerTwoGuardTower, towerPosPlayerTwo, Quaternion.Euler(0, -90, 0), _baseConfig.playerTwoUnitsParent);
                    tower.GetComponent<UnitManager>().InitializeUnit(2);
                }
                // shouldnt need a reset!
                //towerZPosition = (float)_gridDepth / ((float)_p2NrOfTowers + 1.0f);
                break;

            case 3: // neutral AI 
                break;
        }

        Debug.Log("CONTROL: Both players towers have been placed and connected");
    }
    #endregion

    /*
    private void CreateRandomNeutralTowers()
    {
        float halfSizeOfNeutralTower = GetObjectSize(_neutralTower.GetComponent<Collider>());

        // place left neutral tower:
        float xPos = (_gridWidth / 4.0f) + (_player1DeploymentZoneWidth / 2.0f);
        float rngZPos = Random.Range(0.0f + halfSizeOfNeutralTower, _gridDepth - halfSizeOfNeutralTower);
        Vector3 neutralTowerPos = new Vector3(xPos, 0, rngZPos);
        float rngRotation = Random.Range(0, 360);
        GameObject neutralTower = Instantiate(_neutralTower, neutralTowerPos, Quaternion.Euler(0, rngRotation, 0), _neutralUnitsParent);
        neutralTower.GetComponent<UnitManager>().InitializeUnit(3);

        // place central neutral tower:
        xPos = _gridWidth / 2.0f;
        rngZPos = Random.Range(0.0f + halfSizeOfNeutralTower, _gridDepth - halfSizeOfNeutralTower);
        neutralTowerPos = new Vector3(xPos, 0, rngZPos);
        rngRotation = Random.Range(0, 360);
        neutralTower = Instantiate(_neutralTower, neutralTowerPos, Quaternion.Euler(0, rngRotation, 0), _neutralUnitsParent);
        neutralTower.GetComponent<UnitManager>().InitializeUnit(3);

        // place right neutral tower:
        xPos = (_gridWidth / 4.0f) * 3.0f - (_player1DeploymentZoneWidth / 2.0f);
        rngZPos = Random.Range(0.0f + halfSizeOfNeutralTower, _gridDepth - halfSizeOfNeutralTower);
        neutralTowerPos = new Vector3(xPos, 0, rngZPos);
        rngRotation = Random.Range(0, 360);
        neutralTower = Instantiate(_neutralTower, neutralTowerPos, Quaternion.Euler(0, rngRotation, 0), _neutralUnitsParent);
        neutralTower.GetComponent<UnitManager>().InitializeUnit(3);

        Debug.Log("CONTROL: Neutral towers have been placed and connected");
    }

    private void CreateNeutralTowers()
    {
        float halfSizeOfNeutralTower = GetObjectSize(_neutralTower.GetComponent<Collider>());

        // place left neutral tower:
        float xPos = (_gridWidth / 4.0f) + (_player1DeploymentZoneWidth / 2.0f);
        float zPos = (_gridDepth / 6.0f) * 5.0f; // top left
        Vector3 neutralTowerPos = new Vector3(xPos, 0, zPos);
        float rngRotation = Random.Range(0, 360);
        GameObject tower = Instantiate(_neutralTower, neutralTowerPos, Quaternion.Euler(0, rngRotation, 0), _neutralUnitsParent);
        tower.GetComponent<UnitManager>().InitializeUnit(3);

        // place central neutral tower:
        xPos = _gridWidth / 2.0f;
        zPos = _gridDepth / 2.0f;
        neutralTowerPos = new Vector3(xPos, 0, zPos);
        rngRotation = Random.Range(0, 360);
        tower = Instantiate(_neutralTower, neutralTowerPos, Quaternion.Euler(0, rngRotation, 0), _neutralUnitsParent);
        tower.GetComponent<UnitManager>().InitializeUnit(3);

        // place right neutral tower:
        xPos = (_gridWidth / 4.0f) * 3.0f - (_player1DeploymentZoneWidth / 2.0f);
        zPos = _gridDepth / 6.0f; // bottom right
        neutralTowerPos = new Vector3(xPos, 0, zPos);
        rngRotation = Random.Range(0, 360);
        tower = Instantiate(_neutralTower, neutralTowerPos, Quaternion.Euler(0, rngRotation, 0), _neutralUnitsParent);
        tower.GetComponent<UnitManager>().InitializeUnit(3);

        Debug.Log("CONTROL: Neutral towers have been placed and connected");
    }
    #endregion
    */

    /*
    /// <summary>
    /// This function allows the <see cref="GameManager"/> to retrieve the correct hq-locations of all players.
    /// </summary>
    /// <param name="playerID"></param>
    /// <returns></returns>
    public GameObject GetHeadquarter(int playerID)
    {
        switch (playerID)
        {
            case 1:
                return _player2HQ;

            case 2:
                return _player1HQ;

            default:
                Debug.LogError("ERROR: There is no HQ for player \"" + playerID + "\"!", this);
                return null;
        }
    }*/

    /// <summary>
    /// Get the radius of any circular colliders or half the lenght of the z-side of a square collider.
    /// </summary>
    /// <param name="collider"></param>
    /// <returns></returns>
    float GetObjectSize(Collider collider)
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
            return box.size.z / 2.0f;
            //return box.bounds.size.z / 2; // unsure why, but for non scaled colliders only this works!
        }
        else if (collider is MeshCollider mesh && mesh.sharedMesh != null)
        {
            return mesh.sharedMesh.bounds.extents.magnitude; // unsure what this would return - UNTESTED!
        }

        // default case if none of the above colliders apply:
        return 0f;
    } 
}
/*
void CreateObstaclesInTeam1DeploymentArea()
{
    if (_obstaclesInPlayer1Zone > 0)
    {
        for (int i = 0; i < _obstaclesInPlayer1Zone; i++)
        {
            int _levelSectorID = 0;

            InstantiateObstacle(_levelSectorID);
        }
    }
}

void CreateObstaclesInTeam2DeploymentArea()
{
    if (_obstaclesInPlayer2Zone > 0)
    {
        for (int i = 0; i < _obstaclesInPlayer2Zone; i++)
        {
            int _levelSectorID = 1;

            InstantiateObstacle(_levelSectorID);
        }
    }
}
void CreateObstaclesInNeutralZone()
{
    if (_obstaclesInNeutralZone > 0)
    {
        for (int i = 0; i < _obstaclesInNeutralZone; i++)
        {
            int _levelSectorID = 2;

            InstantiateObstacle(_levelSectorID);
        }
    }
}
void InstantiateObstacle(int locationToSpawnThisObstacle)
{
    // get random obstacle
    GameObject randomObstacle = _arrayOfObstacles[Random.Range(0, _arrayOfObstacles.Length)]; // can the last obstacle in this array be found?
    GameObject randomLocation = null;
    List<Transform> usedDeploymentZoneTiles = null;
    List<GameObject> deploymentZoneTiles = null;

    switch (locationToSpawnThisObstacle)
    {
        case 0:
            deploymentZoneTiles = _team1DeploymentZoneTiles;
            usedDeploymentZoneTiles = _team1UsedDeploymentZoneTiles;
            break;
        case 1:
            deploymentZoneTiles = team2DeploymentZoneTiles;
            usedDeploymentZoneTiles = usedTeam2DeploymentZoneTiles;
            break;
        case 2:
            deploymentZoneTiles = _neutralDeploymentZoneTiles;
            usedDeploymentZoneTiles = _usedNeutralDeploymentZoneTiles;
            break;
    }

    if (deploymentZoneTiles == null || usedDeploymentZoneTiles == null) return;

    // get random, legal position
    int randomNr = Random.Range(0, deploymentZoneTiles.Count);
    randomLocation = deploymentZoneTiles[randomNr];

    if (!usedDeploymentZoneTiles.Contains(randomLocation.transform))
    {
        usedDeploymentZoneTiles.Add(randomLocation.transform);
        GameObject instantiatedObstacle = Instantiate(randomObstacle, randomLocation.transform.position, Quaternion.identity, _obstacleParent);
        randomLocation.GetComponent<SpawnZone>().OccupyDeploymentTile();

        // rotate the obstacles parts for more variation
        instantiatedObstacle.transform.Find("innerBase_Section").Rotate(instantiatedObstacle.transform.up, Random.Range(0, 360));
        float degreeRot = GetRandomRotation();
        instantiatedObstacle.transform.Find("outerBase_Section").Rotate(instantiatedObstacle.transform.up, degreeRot);
    }
    else
    {
        InstantiateObstacle(locationToSpawnThisObstacle);
    }
}

float GetRandomRotation()
{
    int rng = Random.Range(0, 4);
    switch (rng)
    {
        case 0: return 90.0f;
        case 1: return 180.0f;
        case 2: return 270.0f;
        default: return 360.0f; // redundant but included for completeness
    }
}

bool InsidePlayer1Zone(Vector3 _position)
{
    float x = _position.x;
    float z = _position.z;

    if (x >= _player1DeploymentZone_X_offset && x < _player1DeploymentZone_X_offset + _player1DeploymentZoneWidth
    && z >= _player1DeploymentZone_Z_offset && z < _player1DeploymentZone_Z_offset + _player1DeploymentZoneDepth)
    {
        return true;
    }
    else
    {
        return false;
    }
}

bool InsidePlayer2Zone(Vector3 _position)
{
    float x = _position.x;
    float z = _position.z;

    if (x >= _player2DeploymentZone_X_offset && x < _player2DeploymentZone_X_offset + _player2DeploymentZoneWidth
    && z >= _player2DeploymentZone_Z_offset && z < _player2DeploymentZone_Z_offset + _player2DeploymentZoneDepth)
    {
        return true;
    }
    else
    {
        return false;
    }
}

bool InsideNeutralZone(Vector3 _position)
{
    float x = _position.x;
    float z = _position.z;

    if (x >= _neutralLandZone_X_offset && x < _neutralLandZone_X_offset + _neutralZoneWidth
    && z >= _neutralZone_Z_offset && z < _neutralZone_Z_offset + _neutralZoneDepth)
    {
        return true;
    }
    else
    {
        return false;
    }
}*/
/*
// this function should handle the reset for both players! -F
public void ResetDeploymentZone()
{
    foreach (GameObject _deploymentZone in _team1DeploymentZoneTiles)
    {
        _deploymentZone.GetComponent<SpawnZone>().VacateDeploymentTile();
    }
}

/// <summary>
/// Gets called by <see cref="GameManager"/> to deliver units with the correct, respective deployment zones. 
/// This method assumes that the map-grid always starts at 0,0,0 and extends in width towards x+.
/// </summary>
public float GetDeploymentBacklineX(int _playerID)
{
    switch (_playerID)
    {
        case 1: // Player 1 targets the far edge of Player 2's deployment zone
            return _gridWidth - _gridWidth; // here: 60 - 60 = 0

        case 2: // Player 2 targets the far edge of Player 1's deployment zone
            return _gridWidth; // here: 60

        default:
            Debug.LogError("ERROR: A unit-destination was requested, but invalid player-ID given!", this);
            return 0.0f; // Return the original position in case of error
    }
}*/
#endregion