using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MapConfig
{
    // inaccessible map settings:
    [SerializeField] private Transform _spawnZoneParent;
    [SerializeField] private GameObject _spawnZone;

    private const int _gridWidth = 60; // was 60
    private const int _gridDepth = 20; // was 20

    private const int _player1DeploymentZoneWidth = 6; // was 6
    private const int _player1DeploymentZoneDepth = _gridDepth;
    private const int _player1DeploymentZone_X_offset = 0; // was 2 // this sets an offset of the play-grid to the outer perimeter
    private const int _player1DeploymentZone_Z_offset = 0; // was 2 // this sets an offset of the play-grid to the outer perimeter

    private const int _player2DeploymentZoneWidth = 6; // was 6
    private const int _player2DeploymentZoneDepth = _gridDepth;
    private const int _player2DeploymentZone_X_offset = _player1DeploymentZone_X_offset + _player1DeploymentZoneWidth + _neutralZoneWidth; // right-field adjacent to noMansLandZone: 
    private const int _player2DeploymentZone_Z_offset = 0; // was 2

    private const int _neutralZoneWidth = _gridWidth - _player1DeploymentZoneWidth - _player2DeploymentZoneWidth;
    private const int _neutralZoneDepth = _gridDepth;
    private const int _neutralZone_X_offset = _player1DeploymentZone_X_offset + _player1DeploymentZoneWidth; // mid-field adjacent to redTeamDeploymentZone: 
    private const int _neutralZone_Z_offset = 0; // was 2 // this sets an offset of the play-grid to the outer perimeter


    // accessible map data:
    public Transform spawnZoneParent => _spawnZoneParent;
    public GameObject spawnZone => _spawnZone;
    public int gridWidth => _gridWidth;
    public int gridDepth => _gridDepth;

    // player 1:
    public int player1DeploymentZoneWidth => _player1DeploymentZoneWidth;
    public int player1DeploymentZoneDepth => _player1DeploymentZoneDepth;
    public int player1DeploymentZone_X_offset => _player1DeploymentZone_X_offset;
    public int player1DeploymentZone_Z_offset => _player1DeploymentZone_Z_offset;

    public List<GameObject> team1DeploymentZoneTiles = new List<GameObject>();
    public List<Transform> team1UsedDeploymentZoneTiles = new List<Transform>();

    // player 2:
    public int player2DeploymentZoneWidth => _player2DeploymentZoneWidth;
    public int player2DeploymentZoneDepth => _player2DeploymentZoneDepth;
    public int player2DeploymentZone_X_offset => _player2DeploymentZone_X_offset;
    public int player2DeploymentZone_Z_offset => _player2DeploymentZone_Z_offset;

    public List<GameObject> team2DeploymentZoneTiles = new List<GameObject>();
    public List<Transform> usedTeam2DeploymentZoneTiles = new List<Transform>();

    // neurtral:
    public int neutralZoneWidth => _neutralZoneWidth;
    public int neutralZoneDepth => _neutralZoneDepth;
    public int neutralZone_X_offset => _neutralZone_X_offset;
    public int neutralZone_Z_offset => _neutralZone_Z_offset;

    public List<GameObject> neutralDeploymentZoneTiles = new List<GameObject>();
    public List<Transform> usedNeutralDeploymentZoneTiles = new List<Transform>();
}

[System.Serializable]
public class ObstacleConfig
{
    [SerializeField] private Transform _obstacleParent;
    [SerializeField] private int _obstaclesInPlayer1Zone = 0;
    [SerializeField] private int _obstaclesInPlayer2Zone = 0;
    [SerializeField] private int _obstaclesInNeutralZone = 0;
    [SerializeField] private GameObject[] _arrayOfObstacles;

    public Transform obstacleParent => _obstacleParent;
    public int obstaclesInPlayer1Zone => _obstaclesInPlayer1Zone;
    public int obstaclesInPlayer2Zone => _obstaclesInPlayer2Zone;
    public int obstaclesInNeutralZone => _obstaclesInNeutralZone;
    public GameObject[] arrayOfObstacles => _arrayOfObstacles;
}

[System.Serializable]
public class BaseConfig
{
    [SerializeField] private Transform _playerOneUnitsParent;
    [SerializeField] private GameObject _playerOneHeadquarter;
    [SerializeField] private GameObject _playerOneGuardTower;
    [SerializeField] private int _p1NrOfTowers = 2;

    [SerializeField] private Transform _playerTwoUnitsParent;
    [SerializeField] private GameObject _playerTwoHeadquarter;
    [SerializeField] private GameObject _playerTwoGuardTower;
    [SerializeField] private int _p2NrOfTowers = 2;

    public Transform playerOneUnitsParent => _playerOneUnitsParent;
    public GameObject playerOneHeadquarter => _playerOneHeadquarter;
    public GameObject playerOneGuardTower => _playerOneGuardTower;
    public int p1NrOfTowers => _p1NrOfTowers;
    public GameObject player1HQ; // holds instantiated hq

    public Transform playerTwoUnitsParent => _playerTwoUnitsParent;
    public GameObject playerTwoHeadquarter => _playerTwoHeadquarter;
    public GameObject playerTwoGuardTower => _playerTwoGuardTower;
    public int p2NrOfTowers => _p2NrOfTowers;
    public GameObject player2HQ; // holds instantiated hq
}

[System.Serializable]
public class NeutralObjectConfig
{
    [SerializeField] private Transform _neutralUnitsParent;
    [SerializeField] private GameObject _neutralTower;
    [SerializeField] private bool _randomlyPlacedNeutralTowers = false;

    public Transform neutralUnitsParent => _neutralUnitsParent;
    public GameObject neutralTower => _neutralTower;
    public bool randomlyPlacedNeutralTowers => _randomlyPlacedNeutralTowers;
}

/*

[System.Serializable]
public class DeploymentZoneData
{
    public int player1DeploymentZoneWidth = 6; // was 6
    public int player1DeploymentZoneDepth;// = _gridDepth;
    public int player1DeploymentZone_X_offset;// = 0; // was 2 // this sets an offset of the play-grid to the outer perimeter
    public int player1DeploymentZone_Z_offset;// = 0; // was 2 // this sets an offset of the play-grid to the outer perimeter

    public List<GameObject> team1DeploymentZoneTiles;// = new List<GameObject>();
    public List<Transform> team1UsedDeploymentZoneTiles = new List<Transform>();

    public int neutralZoneWidth;// = _gridWidth - _player1DeploymentZoneWidth - _player2DeploymentZoneWidth;
    public int neutralZoneDepth;// = _gridDepth;
    public int neutralLandZone_X_offset;// = _player1DeploymentZone_X_offset + _player1DeploymentZoneWidth; // mid-field adjacent to redTeamDeploymentZone: 
    public int neutralZone_Z_offset;// = 0; // was 2 // this sets an offset of the play-grid to the outer perimeter
    
    public List<GameObject> team2DeploymentZoneTiles; // = new List<GameObject>(); // public so the AI can access this to get its deployment zone
    public List<Transform> usedTeam2DeploymentZoneTiles; // = new List<Transform>();

    public int player2DeploymentZoneWidth = 6; // was 6
    public int player2DeploymentZoneDepth;// = _gridDepth;
    public int player2DeploymentZone_X_offset;// = _player1DeploymentZone_X_offset + _player1DeploymentZoneWidth + _neutralZoneWidth; // right-field adjacent to noMansLandZone: 
    public int player2DeploymentZone_Z_offset;// = 0; // was 2
    
    public List<GameObject> neutralDeploymentZoneTiles; // = new List<GameObject>();
    public List<Transform> usedNeutralDeploymentZoneTiles; // = new List<Transform>();
    /*
    private const int _player1DeploymentZoneWidth = 6; // was 6
    private const int _player1DeploymentZoneDepth = _gridDepth;
    private const int _player1DeploymentZone_X_offset = 0; // was 2 // this sets an offset of the play-grid to the outer perimeter
    private const int _player1DeploymentZone_Z_offset = 0; // was 2 // this sets an offset of the play-grid to the outer perimeter

    private const int _neutralZoneWidth = _gridWidth - _player1DeploymentZoneWidth - _player2DeploymentZoneWidth;
    private const int _neutralZoneDepth = _gridDepth;
    private const int _neutralLandZone_X_offset = _player1DeploymentZone_X_offset + _player1DeploymentZoneWidth; // mid-field adjacent to redTeamDeploymentZone: 
    private const int _neutralZone_Z_offset = 0; // was 2 // this sets an offset of the play-grid to the outer perimeter

    private const int _player2DeploymentZoneWidth = 6; // was 6
    private const int _player2DeploymentZoneDepth = _gridDepth;
    private const int _player2DeploymentZone_X_offset = _player1DeploymentZone_X_offset + _player1DeploymentZoneWidth + _neutralZoneWidth; // right-field adjacent to noMansLandZone: 
    private const int _player2DeploymentZone_Z_offset = 0; // was 2
}*/

public class LevelArchitect : MonoBehaviour
{
    [Header("Level Architect Setup:")]
    //[SerializeField] Transform _spawnZoneParent;
    //[SerializeField] GameObject _spawnZone;

    // cache configs:
    public MapConfig mapConfig;
    public ObstacleConfig _obstacleConfig;
    public BaseConfig baseConfig;
    public NeutralObjectConfig _neutralObjectConfig;

    // cache setups (dataset that got setup and is stored here):
    //[SerializeField] private DeploymentZoneData _deploymentZoneData;


    // class references:
    private DeploymentGridGenerator _deploymentGridGenerator;
    private BaseGenerator _baseGenerator;
    private NeutralObjectGenerator _neutralObjectGenerator;
    private ObstacleGenerator _obstacleGenerator;
    /*
    // grid management:
    private const int _gridWidth = 60; // was 60
    private const int _gridDepth = 20; // was 20

    private const int _player1DeploymentZoneWidth = 6; // was 6
    private const int _player1DeploymentZoneDepth = _gridDepth;
    private const int _player1DeploymentZone_X_offset = 0; // was 2 // this sets an offset of the play-grid to the outer perimeter
    private const int _player1DeploymentZone_Z_offset = 0; // was 2 // this sets an offset of the play-grid to the outer perimeter

    private const int _neutralZoneWidth = _gridWidth - _player1DeploymentZoneWidth - _player2DeploymentZoneWidth;
    private const int _neutralZoneDepth = _gridDepth;
    private const int _neutralLandZone_X_offset = _player1DeploymentZone_X_offset + _player1DeploymentZoneWidth; // mid-field adjacent to redTeamDeploymentZone: 
    private const int _neutralZone_Z_offset = 0; // was 2 // this sets an offset of the play-grid to the outer perimeter

    private const int _player2DeploymentZoneWidth = 6; // was 6
    private const int _player2DeploymentZoneDepth = _gridDepth;
    private const int _player2DeploymentZone_X_offset = _player1DeploymentZone_X_offset + _player1DeploymentZoneWidth + _neutralZoneWidth; // right-field adjacent to noMansLandZone: 
    private const int _player2DeploymentZone_Z_offset = 0; // was 2
    */

    /*
    [Header("Obstacle Setup:")]
    [SerializeField] Transform _obstacleParent;
    
    [Space(10)]
    [SerializeField] int _obstaclesInPlayer1Zone = 0;
    [SerializeField] int _obstaclesInPlayer2Zone = 0;
    [SerializeField] int _obstaclesInNeutralZone = 0;

    [Space(10)]
    [SerializeField] GameObject[] _arrayOfObstacles;
    */

    /*
    // deployment zone management:
    List<GameObject> _team1DeploymentZoneTiles = new List<GameObject>();
    List<Transform> _team1UsedDeploymentZoneTiles = new List<Transform>();
    
    public List<GameObject> team2DeploymentZoneTiles = new List<GameObject>(); // public so the AI can access this to get its deployment zone
    public List<Transform> usedTeam2DeploymentZoneTiles = new List<Transform>(); // public so the AI can access this to get its deployment zone

    List<GameObject> _neutralDeploymentZoneTiles = new List<GameObject>();
    List<Transform> _usedNeutralDeploymentZoneTiles = new List<Transform>();
    */
    /*
    [Header("Player Base Building Setup:")]
    [SerializeField] Transform _playerOneUnitsParent;
    [SerializeField] GameObject _playerOneHeadquarter;
    [SerializeField] GameObject _playerOneGuardTower;
    [SerializeField] int _p1NrOfTowers = 2;
    private GameObject _player1HQ; // instantiated object in scene

    [Space(10)]
    [SerializeField] Transform _playerTwoUnitsParent;
    [SerializeField] GameObject _playerTwoHeadquarter;
    [SerializeField] GameObject _playerTwoGuardTower;
    [SerializeField] int _p2NrOfTowers = 2;
    private GameObject _player2HQ; // instantiated object in scene


    [Header("Neutral Object Setup:")]
    [SerializeField] Transform _neutralUnitsParent;
    [SerializeField] GameObject _neutralTower;
    [SerializeField] bool _randomlyPlacedNeutralTowers = false;*/

    void InitializeLevelArchitect()
    {
        _deploymentGridGenerator = GetComponent<DeploymentGridGenerator>();
        _baseGenerator = GetComponent<BaseGenerator>();
        _neutralObjectGenerator = GetComponent<NeutralObjectGenerator>();
        _obstacleGenerator = GetComponent<ObstacleGenerator>();
    }

    public void BuildLevel()
    {
        // cache and set references & variables:
        InitializeLevelArchitect();

        _deploymentGridGenerator.InitializeDeploymentGridGenerator(mapConfig);
        _deploymentGridGenerator.GenerateDeploymentZoneGrid();

        _baseGenerator.InitializeBaseGenerator(mapConfig, baseConfig);
        _baseGenerator.PlaceBases();

        //_neutralObjectGenerator.InitializeNeutralObjectGenerator(_mapConfig, _neutralObjectConfig);
        _neutralObjectGenerator.PlaceNeutralObjects();

        //_obstacleGenerator.InitializeObstacleGenerator(_mapConfig, _obstacleConfig);
        _obstacleGenerator.SpawnObstacles();

        // tell the GameManager that the level is setup, this way the AI can place its first wave:
        GetComponent<GameManager>().LevelSetupComplete();
    }

    /// <summary>
    /// Update the DeploymentZoneData stored within the LevelArchitect to be used further. 
    /// The DeploymentZoneData gets modified and updated by the DeploymentGridGenerator.
    /// </summary>
    /// <param name="deploymentZoneData"></param>
    public void GridSetupComplete(MapConfig updatedMapConfig)
    {
        mapConfig = updatedMapConfig;
        //_deploymentZoneData = deploymentZoneData;
    }

    // this function should handle the reset for both players! -F
    public void ResetDeploymentZone()
    {
        foreach (GameObject _deploymentZone in mapConfig.team1DeploymentZoneTiles)
        {
            _deploymentZone.GetComponent<SpawnZone>().VacateDeploymentTile();
        }
    }

    // test: create map + grid, but no towers


    // ***********************************


    /* // re-enable after testing



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
                GameObject _instantiatedSpawnZone = Instantiate(_spawnZone, _position, Quaternion.identity, _spawnZoneParent);

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
    }

    #region Create Map Buildings:
    private void CreateHeadquarters()
    {
        // place the HQ's:
        float halfSizeOfHQ = GetObjectSize(_playerOneHeadquarter.GetComponent<Collider>());

        // player 1:
        float xPositionPlayerOne = _gridWidth - _gridWidth + halfSizeOfHQ; // e.g. 0 base line
        float zPositionPlayerOne = _gridDepth / 2.0f;
        Vector3 hqPosPlayerOne = new Vector3(xPositionPlayerOne, 0, zPositionPlayerOne);
        _player1HQ = Instantiate(_playerOneHeadquarter, hqPosPlayerOne, Quaternion.Euler(0, 90, 0), _playerOneUnitsParent);

        // player 2:
        float xPositionPlayerTwo = _gridWidth - halfSizeOfHQ; // e.g. 60 base line
        float zPositionPlayerTwo = _gridDepth / 2.0f;
        Vector3 hqPosPlayerTwo = new Vector3(xPositionPlayerTwo, 0, zPositionPlayerTwo);
        _player2HQ = Instantiate(_playerTwoHeadquarter, hqPosPlayerTwo, Quaternion.Euler(0, -90, 0), _playerTwoUnitsParent);

        // initialize them to be game ready:
        _player1HQ.GetComponent<UnitManager>().InitializeUnit(1);
        _player2HQ.GetComponent<UnitManager>().InitializeUnit(2);

        Debug.Log("CONTROL: Both HQ's have been placed and connected!");
    }

    #region Place Towers:
    private void CreateGuardTowers()
    {
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

        Debug.Log("CONTROL: Both players towers have been placed and connected");
    }

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
    }

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
    #endregion

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

// this function should handle the reset for both players! -F
public void ResetDeploymentZone()
{
    foreach(GameObject _deploymentZone in _team1DeploymentZoneTiles)
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
}


/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class LevelArchitect : MonoBehaviour
{
[Header("Level Architect Setup:")]
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
GameObject _player2HQ;

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

    // place the various starting-buildings in the map:
    CreateHeadquarters();
    CreateGuardTowers();
    if (_randomlyPlacedNeutralTowers)
    {
        CreateRandomNeutralTowers();
    }else
    {
        CreateNeutralTowers();
    }

    // create obstacles once mandatory buildings have been placed:
    CreateObstaclesInTeam1DeploymentArea();
    CreateObstaclesInTeam2DeploymentArea();
    CreateObstaclesInNeutralZone();

    // tell the GameManager that the level is setup, this way the AI can place its first wave:
    GetComponent<GameManager>().LevelSetupComplete();
}

#region Create Map Buildings:
private void CreateHeadquarters()
{
    // place the HQ's:
    float halfSizeOfHQ = GetObjectSize(_playerOneHeadquarter.GetComponent<Collider>());

    // player 1:
    float xPositionPlayerOne = _gridWidth - _gridWidth + halfSizeOfHQ; // e.g. 0 base line
    float zPositionPlayerOne = _gridDepth / 2.0f;
    Vector3 hqPosPlayerOne = new Vector3(xPositionPlayerOne, 0, zPositionPlayerOne);
    _player1HQ = Instantiate(_playerOneHeadquarter, hqPosPlayerOne, Quaternion.Euler(0, 90, 0), _playerOneUnitsParent);

    // player 2:
    float xPositionPlayerTwo = _gridWidth - halfSizeOfHQ; // e.g. 60 base line
    float zPositionPlayerTwo = _gridDepth / 2.0f;
    Vector3 hqPosPlayerTwo = new Vector3(xPositionPlayerTwo, 0, zPositionPlayerTwo);
    _player2HQ = Instantiate(_playerTwoHeadquarter, hqPosPlayerTwo, Quaternion.Euler(0, -90, 0), _playerTwoUnitsParent);

    // initialize them to be game ready:
    _player1HQ.GetComponent<UnitManager>().InitializeUnit(1);
    _player2HQ.GetComponent<UnitManager>().InitializeUnit(2);

    Debug.Log("CONTROL: Both HQ's have been placed and connected!");
}

#region Place Towers:
private void CreateGuardTowers()
{
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

    Debug.Log("CONTROL: Both players towers have been placed and connected");
}

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
}

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
#endregion

void CreateObstaclesInTeam1DeploymentArea()
{
    if(_obstaclesInPlayer1Zone > 0)
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
}*/
/*
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
                GameObject _instantiatedObstacle = Instantiate(_randomObstacle, _randomLocation.transform.position, Quaternion.identity, _obstacleParent);
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
                GameObject _instantiatedObstacle = Instantiate(_randomObstacle, _randomLocation.transform.position, Quaternion.identity, _obstacleParent);
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
                GameObject _instantiatedObstacle = Instantiate(_randomObstacle, _randomLocation.transform.position, Quaternion.identity, _obstacleParent);
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
*/
/*
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

// this function should handle the reset for both players! -F
public void ResetDeploymentZone()
{
    foreach(GameObject _deploymentZone in _team1DeploymentZoneTiles)
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
}
}*/
