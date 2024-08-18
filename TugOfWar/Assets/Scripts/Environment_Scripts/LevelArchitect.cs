using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds all information about the map-setup.
/// </summary>
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

    // neutral:
    public int neutralZoneWidth => _neutralZoneWidth;
    public int neutralZoneDepth => _neutralZoneDepth;
    public int neutralZone_X_offset => _neutralZone_X_offset;
    public int neutralZone_Z_offset => _neutralZone_Z_offset;

    public List<GameObject> neutralDeploymentZoneTiles = new List<GameObject>();
    public List<Transform> usedNeutralDeploymentZoneTiles = new List<Transform>();
}

/// <summary>
/// Holds all information on how to setup/spread obstacles on the map.
/// </summary>
[System.Serializable]
public class ObstacleConfig
{
    [SerializeField] private Transform _obstacleParent;
    [SerializeField] private int _obstaclesInPlayer1Zone = 0;
    [SerializeField] private int _obstaclesInPlayer2Zone = 0;
    [SerializeField] private int _obstaclesInNeutralZone = 0;
    [SerializeField] private GameObject[] _arrayOfTrees;
    [SerializeField] private GameObject[] _arrayOfStones;

    public Transform obstacleParent => _obstacleParent;
    public int obstaclesInPlayer1Zone => _obstaclesInPlayer1Zone;
    public int obstaclesInPlayer2Zone => _obstaclesInPlayer2Zone;
    public int obstaclesInNeutralZone => _obstaclesInNeutralZone;
    public GameObject[] arrayOfTrees => _arrayOfTrees;
    public GameObject[] arrayOfStones => _arrayOfStones;
}

/// <summary>
/// Holds all the information on how to set up each players base.
/// </summary>
[System.Serializable]
public class BaseConfig
{
    [SerializeField] private Transform _playerOneUnitsParent;
    [SerializeField] private GameObject _playerOneHeadquarter; // holds the type of hq to be generated for player 1
    [SerializeField] private GameObject _playerOneGuardTower; // holds the type of tower to be generated for player 1
    [SerializeField] private int _p1NrOfTowers = 2;

    [SerializeField] private Transform _playerTwoUnitsParent;
    [SerializeField] private GameObject _playerTwoHeadquarter; // holds the type of hq to be generated for player 2
    [SerializeField] private GameObject _playerTwoGuardTower; // holds the type of tower to be generated for player 2
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

/// <summary>
/// Holds all the information of how to setup any neutral AI units, if any.
/// </summary>
[System.Serializable]
public class NeutralObjectConfig
{
    [SerializeField] private Transform _neutralUnitsParent;
    [SerializeField] private GameObject _neutralTower;
    [SerializeField] private int _nrOfNeutralTowers = 3;
    [SerializeField] private bool _randomNeutralTowers = false;

    public Transform neutralUnitsParent => _neutralUnitsParent;
    public GameObject neutralTower => _neutralTower;
    public int nrOfNeutralTowers => _nrOfNeutralTowers; 
    public bool randomNeutralTowers => _randomNeutralTowers;
}


/// <summary>
/// Manages map setup. Controlled and called upon by <see cref="GameManager"/>.
/// </summary>
public class LevelArchitect : MonoBehaviour
{
    [Header("Level Architect Setup:")]
    public MapConfig mapConfig;
    public ObstacleConfig obstacleConfig;
    public BaseConfig baseConfig;
    public NeutralObjectConfig neutralObjectConfig;

    // private varibales:
    private DeploymentGridGenerator _deploymentGridGenerator;
    private BaseGenerator _baseGenerator;
    private NeutralObjectGenerator _neutralObjectGenerator;
    private ObstacleGenerator _obstacleGenerator;

    /// <summary>
    /// Setup this script. Called by <see cref="GameManager.Awake"/>.
    /// </summary>
    public void InitializeLevelArchitect()
    {
        // cache script references & dependencies:
        _deploymentGridGenerator = GetComponent<DeploymentGridGenerator>();
        _baseGenerator = GetComponent<BaseGenerator>();
        _neutralObjectGenerator = GetComponent<NeutralObjectGenerator>();
        _obstacleGenerator = GetComponent<ObstacleGenerator>();
    }

    /// <summary>
    /// Generates map and all features therein in succession. Called by <see cref="GameManager.Awake"/> after initialization.
    /// </summary>
    public void GenerateLevel()
    {
        _deploymentGridGenerator.InitializeDeploymentGridGenerator(mapConfig);
        _deploymentGridGenerator.GenerateDeploymentZoneGrid();

        _baseGenerator.InitializeBaseGenerator(mapConfig, baseConfig);
        _baseGenerator.GenerateBases();

        _neutralObjectGenerator.InitializeNeutralObjectGenerator(mapConfig, neutralObjectConfig);
        _neutralObjectGenerator.GenerateNeutralObjects();

        _obstacleGenerator.InitializeObstacleGenerator(mapConfig, obstacleConfig);
        _obstacleGenerator.GenerateObstacles();

        // tell the GameManager that the level is setup, this way the AI can place its first wave:
        GetComponent<GameManager>().LevelSetupComplete();
    }


    /// <summary>
    /// Updates the <see cref="MapConfig"/> stored within the <see cref="LevelArchitect"/>, 
    /// as this holds all relevant information about occupied deployment-zones in each sector.
    /// The <see cref="MapConfig"/>-data gets modified by: <see cref="DeploymentGridGenerator"/>, 
    /// <see cref="NeutralObjectGenerator"/> and <see cref="ObstacleGenerator"/>.
    /// NOTE: Currently not using grid for neutral units or terrain, so not using this function.
    /// </summary>
    /// <param name="deploymentZoneData"></param>
    public void UpdateMapConfig(MapConfig updatedMapConfig)
    {
        mapConfig = updatedMapConfig;
    }


    /// <summary>
    /// Reset all deployment-zones in a given map-sector. They can now be occupied again.
    /// </summary>
    public void ResetDeploymentZone(int zoneID)
    {
        switch (zoneID)
        {
            case 1: // player 1:
                foreach (GameObject _deploymentZone in mapConfig.team1DeploymentZoneTiles)
                {
                    _deploymentZone.GetComponent<SpawnZone>().VacateDeploymentTile();
                }
                return;

            case 2: // player 2:
                foreach (GameObject _deploymentZone in mapConfig.team2DeploymentZoneTiles)
                {
                    _deploymentZone.GetComponent<SpawnZone>().VacateDeploymentTile();
                }
                return;

            case 3: // neutral zone, unsure if or why this should ever be reset:
                foreach (GameObject _deploymentZone in mapConfig.team1DeploymentZoneTiles)
                {
                    _deploymentZone.GetComponent<SpawnZone>().VacateDeploymentTile();
                }
                return;
        }
    }
}
