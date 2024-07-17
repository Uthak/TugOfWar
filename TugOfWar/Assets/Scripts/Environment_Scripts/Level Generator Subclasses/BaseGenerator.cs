using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGenerator : MonoBehaviour
{
    MapConfig _mapConfig;
    BaseConfig _baseConfig;

    public void InitializeBaseGenerator(MapConfig mapConfig, BaseConfig baseConfig)
    {
        _mapConfig = mapConfig; 
        _baseConfig = baseConfig;
    }

    public void PlaceBases()
    {
        CreateHeadquarter(1);
        CreateHeadquarter(2);

        CreateGuardTowers(1);
        CreateGuardTowers(2);
    }



    /*
    [SerializeField] Transform _spawnZoneParent;
    [SerializeField] Transform _obstacleParent;
    public GameObject[] spawnZone; // may no need to be an array --> only one type of spawn zone currently

    // Define the size of the grid
    private const int _gridWidth = 60; // was 60
    private const int _gridDepth = 20; // was 20

    // Define the size and position of the red team (default: player) deployment zone
    private const int _player1DeploymentZoneWidth = 6;
    private const int _player1DeploymentZoneDepth = _gridDepth;
    private const int _player1DeploymentZone_X_offset = 0; // was 2 // this sets an offset of the play-grid to the outer perimeter
    private const int _player1DeploymentZone_Z_offset = 0; // was 2 // this sets an offset of the play-grid to the outer perimeter

    // Define the size and position of the blue team (default: AI) deployment zone
    private const int _neutralZoneWidth = _gridWidth - _player1DeploymentZoneWidth - _player2DeploymentZoneWidth;
    private const int _neutralZoneDepth = _gridDepth;
    private const int _neutralLandZone_X_offset = _player1DeploymentZone_X_offset + _player1DeploymentZoneWidth; // mid-field adjacent to redTeamDeploymentZone: 
    private const int _neutralZone_Z_offset = 0; // was 2 // this sets an offset of the play-grid to the outer perimeter

    // Define the size and position of the blue team (default: AI) deployment zone
    private const int _player2DeploymentZoneWidth = 6;
    private const int _player2DeploymentZoneDepth = _gridDepth;
    private const int _player2DeploymentZone_X_offset = _player1DeploymentZone_X_offset + _player1DeploymentZoneWidth + _neutralZoneWidth; // right-field adjacent to noMansLandZone: 
    private const int _player2DeploymentZone_Z_offset = 0; // was 2

    // used for placing obstacles in the various Zones:
    [SerializeField] int _obstaclesInPlayer1Zone = 0;
    [SerializeField] int _obstaclesInPlayer2Zone = 0;
    [SerializeField] int _obstaclesInNeutralZone = 0;
    [Space(10)]
    [SerializeField] GameObject[] _arrayOfObstacles;


    // all of this should be handled over a getter method! -F
    List<GameObject> _team1DeploymentZoneTiles = new List<GameObject>();
    public List<GameObject> team2DeploymentZoneTiles = new List<GameObject>(); // public so the AI can access this to get its deployment zone
    List<GameObject> _neutralDeploymentZoneTiles = new List<GameObject>();

    List<Transform> _team1UsedDeploymentZoneTiles = new List<Transform>();
    public List<Transform> usedTeam2DeploymentZoneTiles = new List<Transform>(); // public so the AI can access this to get its deployment zone
    List<Transform> _usedNeutralDeploymentZoneTiles = new List<Transform>();

    [Header("Default Constructs and Buildings:")]
    [SerializeField] GameObject _playerOneHeadquarter;
    [SerializeField] GameObject _playerTwoHeadquarter;
    [SerializeField] int _p1NrOfTowers = 2;

    [Space(5)]
    [SerializeField] GameObject _playerOneGuardTower;
    [SerializeField] GameObject _playerTwoGuardTower;
    [SerializeField] int _p2NrOfTowers = 2;

    [Space(5)]
    [SerializeField] bool _randomlyPlacedNeutralTowers = true;
    [SerializeField] GameObject _neutralTower;

    [Space(5)]
    [SerializeField] Transform _playerOneUnitsParent;
    [SerializeField] Transform _playerTwoUnitsParent;
    [SerializeField] Transform _neutralUnitsParent;

    // private variables:
    GameObject _player1HQ;
    GameObject _player2HQ;*/
    /*
    // create World:
    public void BuildLevel()
    {
        // Loop through each row of the grid
        for (int z = 0; z < _gridDepth; z++)
        {
            // Loop through each column of the grid
            for (int x = 0; x < _gridWidth; x++)
            {
                // Instantiate a new grid square prefab at the current position
                Vector3 _position = new Vector3(x + .5f, 0, z + .5f); // adding +.5f to adjust for size of spawnzone offset:
                GameObject _instantiatedSpawnZone = Instantiate(spawnZone[Random.Range(0, spawnZone.Length)], _position, Quaternion.identity, _spawnZoneParent);

                // If the current position is within the red teams deployment zone, color it green
                if (InsideNeutralZone(_position))
                {
                    _instantiatedSpawnZone.GetComponent<SpawnZone>().InitializeSpawnZone(0);
                    _neutralDeploymentZoneTiles.Add(_instantiatedSpawnZone);
                }
                if (InsidePlayer1Zone(_position))
                {
                    _instantiatedSpawnZone.GetComponent<SpawnZone>().InitializeSpawnZone(1);
                    _team1DeploymentZoneTiles.Add(_instantiatedSpawnZone);
                }
                if (InsidePlayer2Zone(_position))
                {
                    _instantiatedSpawnZone.GetComponent<SpawnZone>().InitializeSpawnZone(2);
                    team2DeploymentZoneTiles.Add(_instantiatedSpawnZone);
                }
            }
        }

        // place the various starting-buildings in the map:
        CreateHeadquarters();
        CreateGuardTowers();
        if (_randomlyPlacedNeutralTowers)
        {
            CreateRandomNeutralTowers();
        }
        else
        {
            CreateNeutralTowers();
        }

        // create obstacles once mandatory buildings have been placed:
        CreateObstaclesInTeam1DeploymentArea();
        CreateObstaclesInTeam2DeploymentArea();
        CreateObstaclesInNeutralZone();

        // tell the GameManager that the level is setup, this way the AI can place its first wave:
        GetComponent<GameManager>().LevelSetupComplete();
    }*/

    #region Create Map Buildings:
    private void CreateHeadquarter(int playerID)
    {
        float halfSizeOfHQ;
        float xPosHQ;
        float zPosHQ;
        Vector3 hqPos;

        switch (playerID)
        {
            case 0:
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

            case 3:
                break;
        }
            
        /*
        // place the HQ's:
        float halfSizeOfHQ = GetObjectSize(_playerOneHeadquarter.GetComponent<Collider>());

        // player 1:
        float xPositionPlayerOne = _gridWidth - _gridWidth + halfSizeOfHQ; // e.g. 0 base line
        float zPositionPlayerOne = _gridDepth / 2.0f;
        Vector3 hqPosPlayerOne = new Vector3(xPosHQ, 0, zPosHQ);
        _player1HQ = Instantiate(_playerOneHeadquarter, hqPos, Quaternion.Euler(0, 90, 0), _playerOneUnitsParent);
        */
        // player 2:
        /*
        float xPositionPlayerTwo = _gridWidth - halfSizeOfHQ; // e.g. 60 base line
        float zPositionPlayerTwo = _gridDepth / 2.0f;
        Vector3 hqPosPlayerTwo = new Vector3(xPositionPlayerTwo, 0, zPositionPlayerTwo);
        _player2HQ = Instantiate(_playerTwoHeadquarter, hqPosPlayerTwo, Quaternion.Euler(0, -90, 0), _playerTwoUnitsParent);

        // initialize them to be game ready:
        _player1HQ.GetComponent<UnitManager>().InitializeUnit(1);
        _player2HQ.GetComponent<UnitManager>().InitializeUnit(2);

        Debug.Log("CONTROL: Both HQ's have been placed and connected!");*/
    }

    private void CreateGuardTowers(int playerID)
    {
        switch (playerID)
        {
            case 0:
                break;

            case 1:
                break;

            case 2:
                break;

            case 3:
                break;
        }
        /*
        // place the bases guard towers:
        float p1HalfSizeOfTower = GetObjectSize(_playerOneGuardTower.GetComponent<Collider>());
        float p2HalfSizeOfTower = GetObjectSize(_playerTwoGuardTower.GetComponent<Collider>());

        // calculate space between the desired nr of towers along the deployment-zones perimeter:
        float gridSectionDepth = (float)_gridDepth / ((float)_p1NrOfTowers + 1.0f);

        // create player 1 towers:
        for (int i = 0; i < _p1NrOfTowers; i++)
        {
            float xPos = _gridWidth - _gridWidth + _player1DeploymentZoneWidth + p1HalfSizeOfTower;
            float zPos = gridSectionDepth * (1 + i);
            Vector3 towerPosPlayerOne = new Vector3(xPos, 0, zPos);
            GameObject tower = Instantiate(_playerOneGuardTower, towerPosPlayerOne, Quaternion.Euler(0, 90, 0), _playerOneUnitsParent);
            tower.GetComponent<UnitManager>().InitializeUnit(1);
        }

        gridSectionDepth = (float)_gridDepth / ((float)_p2NrOfTowers + 1.0f);

        // create player 2 towers:
        for (int i = 0; i < _p2NrOfTowers; i++)
        {
            float xPos = _gridWidth - _player2DeploymentZoneWidth - p2HalfSizeOfTower;
            float zPos = gridSectionDepth * (1 + i);
            Vector3 towerPosPlayerTwo = new Vector3(xPos, 0, zPos);
            GameObject tower = Instantiate(_playerTwoGuardTower, towerPosPlayerTwo, Quaternion.Euler(0, -90, 0), _playerTwoUnitsParent);
            tower.GetComponent<UnitManager>().InitializeUnit(2);
        }

        Debug.Log("CONTROL: Both players towers have been placed and connected");*/
    }

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