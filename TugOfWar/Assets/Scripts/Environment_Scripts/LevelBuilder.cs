using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class LevelBuilder : MonoBehaviour
{
    [Header("Level Builder Setup:")]
    // The prefab for the grid squares
    public GameObject[] spawnZone; // no need to be an array --> only one type of spawn zone

    // Define the size of the grid
    private const int _gridWidth = 60;
    private const int _gridDepth = 20;

    // Define the size and position of the red team (default: player) deployment zone
    private const int _player1DeploymentZoneWidth = 6;
    private const int _player1DeploymentZoneDepth = 20;
    private const int _player1DeploymentZone_X_offset = 0; // was 2
    private const int _player1DeploymentZone_Z_offset = 0; // was 2

    // Define the size and position of the blue team (default: AI) deployment zone
    //private const int _neutralZoneWidth = 40;
    private const int _neutralZoneWidth = _gridWidth - _player1DeploymentZoneWidth - _player2DeploymentZoneWidth; // adjust according to the two deployment zones:
    private const int _neutralZoneDepth = _gridDepth;
    private const int _neutralLandZone_X_offset = _player1DeploymentZone_X_offset + _player1DeploymentZoneWidth; // mid-field adjacent to redTeamDeploymentZone: 
    private const int _neutralZone_Z_offset = 0; // was 2

    // Define the size and position of the blue team (default: AI) deployment zone
    private const int _player2DeploymentZoneWidth = 6;
    private const int _player2DeploymentZoneDepth = 20;
    private const int _player2DeploymentZone_X_offset = _player1DeploymentZone_X_offset + _player1DeploymentZoneWidth + _neutralZoneWidth; // right-field adjacent to noMansLandZone: 
    private const int _player2DeploymentZone_Z_offset = 0; // was 2

    // used for placing obstacles in the various Zones:
    [SerializeField] int _numberOfTerrainInplayer1Zone = 0;
    [SerializeField] int _numberOfTerrainInplayer2Zone = 0;
    [SerializeField] int _numberOfTerrainInNeutralZone = 0;
    [Space(10)]
    [SerializeField] GameObject[] _arrayOfObstacles;

    List<GameObject> _team1DeploymentZoneTiles = new List<GameObject>();
    public List<GameObject> team2DeploymentZoneTiles = new List<GameObject>(); // public so the AI can access this to get its deployment zone
    List<GameObject> _neutralDeploymentZoneTiles = new List<GameObject>();

    List<Transform> _team1UsedDeploymentZoneTiles = new List<Transform>();
    public List<Transform> usedTeam2DeploymentZoneTiles = new List<Transform>(); // public so the AI can access this to get its deployment zone
    List<Transform> _usedNeutralDeploymentZoneTiles = new List<Transform>();

    // Create World:
    void Start()
    {
        // Loop through each row of the grid
        for (int z = 0; z < _gridDepth; z++)
        {
            // Loop through each column of the grid
            for (int x = 0; x < _gridWidth; x++)
            {
                // Instantiate a new grid square prefab at the current position
                Vector3 _position = new Vector3(x + .5f, 0, z + .5f); // adding +.5f to adjust for size of spawnzone offset:
                GameObject _instantiatedSpawnZone = Instantiate(spawnZone[Random.Range(0, spawnZone.Length)], _position, Quaternion.identity, transform);

                // If the current position is within the red teams deployment zone, color it green
                if (InsideNeutralZone(_position))
                {
                    _instantiatedSpawnZone.GetComponent<SpawnZone>().SetupSpawnZone(0);
                    _neutralDeploymentZoneTiles.Add(_instantiatedSpawnZone);
                }
                if (InsidePlayer1Zone(_position))
                {
                    _instantiatedSpawnZone.GetComponent<SpawnZone>().SetupSpawnZone(1);
                    _team1DeploymentZoneTiles.Add(_instantiatedSpawnZone);
                }
                if (InsidePlayer2Zone(_position))
                {
                    _instantiatedSpawnZone.GetComponent<SpawnZone>().SetupSpawnZone(2);
                    team2DeploymentZoneTiles.Add(_instantiatedSpawnZone);
                }
            }
        }

        CreateObstaclesInTeam1DeploymentArea();
        CreateObstaclesInTeam2DeploymentArea();
        CreateObstaclesInNeutralZone();

        // tell the GameManager that the level is setup, this way the AI can place its first wave:
        GetComponent<GameManager>().LevelSetupComplete();
    }


    void CreateObstaclesInTeam1DeploymentArea()
    {
        if(_numberOfTerrainInplayer1Zone > 0)
        {
            for (int i = 0; i < _numberOfTerrainInplayer1Zone; i++)
            {
                int _levelSectorID = 0;

                InstantiateObstacle(_levelSectorID);
            }
        }
    }

    void CreateObstaclesInTeam2DeploymentArea()
    {
        if (_numberOfTerrainInplayer2Zone > 0)
        {
            for (int i = 0; i < _numberOfTerrainInplayer2Zone; i++)
            {
                int _levelSectorID = 1;

                InstantiateObstacle(_levelSectorID);
            }
        }
    }
    void CreateObstaclesInNeutralZone()
    {
        if (_numberOfTerrainInNeutralZone > 0)
        {
            for (int i = 0; i < _numberOfTerrainInNeutralZone; i++)
            {
                int _levelSectorID = 2;

                InstantiateObstacle(_levelSectorID);
            }
        }
    }

    void InstantiateObstacle(int _locationToSpawnThisObstacle)
    {
        GameObject _randomObstacle = null;
        int _randomNr = 0;
        GameObject _randomLocation = null;

        switch (_locationToSpawnThisObstacle)
        { 
            case 0:
                // get random obstacle:
                _randomObstacle = _arrayOfObstacles[Random.Range(0, _arrayOfObstacles.Length)];

                // get random, legal position: 
                _randomNr = Random.Range(0, _team1DeploymentZoneTiles.Count);
                _randomLocation = _team1DeploymentZoneTiles[_randomNr];
                
                if (!_team1UsedDeploymentZoneTiles.Contains(_randomLocation.transform))
                {
                    _team1UsedDeploymentZoneTiles.Add(_randomLocation.transform);
                    GameObject _instantiatedObstacle = Instantiate(_randomObstacle, _randomLocation.transform.position, Quaternion.identity, transform);
                    _team1DeploymentZoneTiles[_randomNr].GetComponent<SpawnZone>().OccupyDeploymentTile();

                    // rotate the obstacles parts for more variation:
                    _instantiatedObstacle.transform.Find("innerBase_Section").Rotate(_instantiatedObstacle.transform.up, Random.Range(0, 360));
                    int _rng = Random.Range(0, 3);
                    float _degreeRot = 0.0f;
                    switch (_rng)
                    {
                        case 0:
                            _degreeRot = 90.0f;
                            break;
                        case 1:
                            _degreeRot = 180.0f;
                            break;
                        case 2:
                            _degreeRot = 270.0f;
                            break;
                        case 3:
                            _degreeRot = 360.0f; // redundant... hence 0-3 random
                            break;
                    }
                    _instantiatedObstacle.transform.Find("outerBase_Section").Rotate(_instantiatedObstacle.transform.up, _degreeRot);
                }else
                {
                    InstantiateObstacle(_locationToSpawnThisObstacle);
                }
                break;

            case 1:
                // get random obstacle:
                _randomObstacle = _arrayOfObstacles[Random.Range(0, _arrayOfObstacles.Length)];

                // get random, legal position: 
                _randomNr = Random.Range(0, team2DeploymentZoneTiles.Count);
                _randomLocation = team2DeploymentZoneTiles[_randomNr];

                if (!usedTeam2DeploymentZoneTiles.Contains(_randomLocation.transform))
                {
                    usedTeam2DeploymentZoneTiles.Add(_randomLocation.transform);
                    GameObject _instantiatedObstacle = Instantiate(_randomObstacle, _randomLocation.transform.position, Quaternion.identity, transform);
                    team2DeploymentZoneTiles[_randomNr].GetComponent<SpawnZone>().OccupyDeploymentTile();

                    // rotate the obstacles parts for more variation:
                    _instantiatedObstacle.transform.Find("innerBase_Section").Rotate(_instantiatedObstacle.transform.up, Random.Range(0, 360));
                    int _rng = Random.Range(0, 3);
                    float _degreeRot = 0.0f;
                    switch (_rng)
                    {
                        case 0:
                            _degreeRot = 90.0f;
                            break;
                        case 1:
                            _degreeRot = 180.0f;
                            break;
                        case 2:
                            _degreeRot = 270.0f;
                            break;
                        case 3:
                            _degreeRot = 360.0f; // redundant...
                            break;
                    }
                    _instantiatedObstacle.transform.Find("outerBase_Section").Rotate(_instantiatedObstacle.transform.up, _degreeRot);
                }else
                {
                    InstantiateObstacle(_locationToSpawnThisObstacle);
                }
                break;

            case 2:
                // get random obstacle:
                _randomObstacle = _arrayOfObstacles[Random.Range(0, _arrayOfObstacles.Length)];

                // get random, legal position: 
                _randomNr = Random.Range(0, _neutralDeploymentZoneTiles.Count);
                _randomLocation = _neutralDeploymentZoneTiles[_randomNr];

                if (!_usedNeutralDeploymentZoneTiles.Contains(_randomLocation.transform))
                {
                    _usedNeutralDeploymentZoneTiles.Add(_randomLocation.transform);
                    GameObject _instantiatedObstacle = Instantiate(_randomObstacle, _randomLocation.transform.position, Quaternion.identity, transform);
                    _neutralDeploymentZoneTiles[_randomNr].GetComponent<SpawnZone>().OccupyDeploymentTile();

                    // rotate the obstacles parts for more variation:
                    _instantiatedObstacle.transform.Find("innerBase_Section").Rotate(_instantiatedObstacle.transform.up, Random.Range(0, 360));
                    int _rng = Random.Range(0, 3);
                    float _degreeRot = 0.0f;
                    switch (_rng)
                    {
                        case 0:
                            _degreeRot = 90.0f;
                            break;
                        case 1:
                            _degreeRot = 180.0f;
                            break;
                        case 2:
                            _degreeRot = 270.0f;
                            break;
                        case 3:
                            _degreeRot = 360.0f; // redundant... ;)
                            break;
                    }
                    _instantiatedObstacle.transform.Find("outerBase_Section").Rotate(_instantiatedObstacle.transform.up, _degreeRot);
                }else
                {
                    InstantiateObstacle(_locationToSpawnThisObstacle);
                }
                break;
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
        }else
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
        }else
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
        }else
        {
            return false;
        }
    }

    public void ResetDeploymentZone()
    {
        foreach(GameObject _deploymentZone in _team1DeploymentZoneTiles)
        {
            _deploymentZone.GetComponent<SpawnZone>().VacateDeploymentTile();
        }
    }

    /* // since the new core loop is to keep deploying and launching waves this has become redundant:
    public void TurnOffAllZoneMarkers()
    {
        foreach(GameObject _spawnZone in _team1DeploymentZoneTiles)
        {
            //_spawnZone.GetComponent<Renderer>().enabled = false;
            //_spawnZone.GetComponent<Collider>().enabled = false;
            _spawnZone.GetComponent<SpawnZone>().OccupyDeploymentTile();
        }
        foreach (GameObject _spawnZone in team2DeploymentZoneTiles)
        {
            //_spawnZone.GetComponent<Renderer>().enabled = false;
            //_spawnZone.GetComponent<Collider>().enabled = false;
            _spawnZone.GetComponent<SpawnZone>().OccupyDeploymentTile();

        }
        foreach (GameObject _spawnZone in _neutralDeploymentZoneTiles)
        {
            //_spawnZone.GetComponent<Renderer>().enabled = false;
            //_spawnZone.GetComponent<Collider>().enabled = false;
            _spawnZone.GetComponent<SpawnZone>().OccupyDeploymentTile();

        }
    }*/
}
