using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeploymentGridGenerator : MonoBehaviour
{
    private MapConfig _mapConfig;
    //private MapConfig _UpdatedMapConfig;
    //private DeploymentZoneData _deploymentZoneData;

    /*
    [SerializeField] Transform _spawnZoneParent;
    [SerializeField] GameObject spawnZone; // may no need to be an array --> only one type of spawn zone currently

    // Define the size of the grid
    private const int _gridWidth = 60; // was 60
    private const int _gridDepth = 20; // was 20

    // Define the size and position of the red team (default: player) deployment zone
    private const int _player1DeploymentZoneWidth = 6;
    private const int _player1DeploymentZoneDepth = _gridDepth;
    private const int _player1DeploymentZone_X_offset = 0;
    private const int _player1DeploymentZone_Z_offset = 0;

    // Define the size and position of the blue team (default: AI) deployment zone
    private const int _neutralZoneWidth = _gridWidth - _player1DeploymentZoneWidth - _player2DeploymentZoneWidth;
    private const int _neutralZoneDepth = _gridDepth;
    private const int _neutralLandZone_X_offset = _player1DeploymentZone_X_offset + _player1DeploymentZoneWidth; // mid-field adjacent to redTeamDeploymentZone: 
    private const int _neutralZone_Z_offset = 0; 

    // Define the size and position of the blue team (default: AI) deployment zone
    private const int _player2DeploymentZoneWidth = 6;
    private const int _player2DeploymentZoneDepth = _gridDepth;
    private const int _player2DeploymentZone_X_offset = _player1DeploymentZone_X_offset + _player1DeploymentZoneWidth + _neutralZoneWidth; // right-field adjacent to noMansLandZone: 
    private const int _player2DeploymentZone_Z_offset = 0; 


    List<GameObject> _team1DeploymentZoneTiles = new List<GameObject>();
    public List<GameObject> team2DeploymentZoneTiles = new List<GameObject>(); // public so the AI can access this to get its deployment zone
    List<GameObject> _neutralDeploymentZoneTiles = new List<GameObject>();

    List<Transform> _team1UsedDeploymentZoneTiles = new List<Transform>();
    public List<Transform> usedTeam2DeploymentZoneTiles = new List<Transform>(); // public so the AI can access this to get its deployment zone
    List<Transform> _usedNeutralDeploymentZoneTiles = new List<Transform>();
    */

    public void InitializeDeploymentGridGenerator(MapConfig mapConfig/*, DeploymentZoneData deploymentZoneData*/)
    {
        _mapConfig = mapConfig; // get and use to set DeploymentZoneData:
        //_UpdatedMapConfig = _mapConfig;
        //_deploymentZoneData = deploymentZoneData; // set and send back:
    }

    public void GenerateDeploymentZoneGrid()
    {
        // loop through each row of the grid:
        for (int z = 0; z < _mapConfig.gridDepth; z++)
        {
            // loop through each column of the grid:
            for (int x = 0; x < _mapConfig.gridWidth; x++)
            {
                // instantiate a new SpawnZone-prefab at the current position:
                Vector3 _position = new Vector3(x + .5f, 0, z + .5f); // adding +.5f to adjust for size of spawnzone offset:
                GameObject _instantiatedSpawnZone = Instantiate(_mapConfig.spawnZone, _position, Quaternion.identity, _mapConfig.spawnZoneParent);

                // sort this zone into the correct list and setup the SpawnZone:
                UpdateSpawnZoneLists(_position, _instantiatedSpawnZone);
                /*
                if (InsideNeutralZone(_position))
                {
                    _instantiatedSpawnZone.GetComponent<SpawnZone>().InitializeSpawnZone(0);
                    _deploymentZoneData.neutralDeploymentZoneTiles.Add(_instantiatedSpawnZone);
                }
                if (InsidePlayer1Zone(_position))
                {
                    _instantiatedSpawnZone.GetComponent<SpawnZone>().InitializeSpawnZone(1);
                    _deploymentZoneData.team1DeploymentZoneTiles.Add(_instantiatedSpawnZone);
                }
                if (InsidePlayer2Zone(_position))
                {
                    _instantiatedSpawnZone.GetComponent<SpawnZone>().InitializeSpawnZone(2);
                    _deploymentZoneData.team2DeploymentZoneTiles.Add(_instantiatedSpawnZone);
                }*/
            }
        }

        //UpdateDeploymentZoneData();

        // report back when DeploymentGrid is setup:
        GetComponent<LevelArchitect>().UpdateMapConfig(_mapConfig);
    }


    void UpdateSpawnZoneLists(Vector3 position, GameObject instantiatedSpawnZone)
    {
        int deploymentZoneID = GetDeploymentZoneID(position);

        instantiatedSpawnZone.GetComponent<SpawnZone>().InitializeSpawnZone(deploymentZoneID);

        switch (deploymentZoneID)
        {
            case 0: // currently impossible!
                Debug.LogError("ERROR: Invalid deploymentZoneID returned!", this);
                break;

            case 1:
                _mapConfig.team1DeploymentZoneTiles.Add(instantiatedSpawnZone);
                break;

            case 2:
                _mapConfig.team2DeploymentZoneTiles.Add(instantiatedSpawnZone);
                break;

            case 3:
                _mapConfig.neutralDeploymentZoneTiles.Add(instantiatedSpawnZone);
                break;

            default:
                Debug.LogError("ERROR: Invalid deploymentZoneID returned!", this);
                return;
        }
    }

    /// <summary>
    /// 0 = environment (here: invalid); 1 = player 1; 2 = player 2; 3 = neutral.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    int GetDeploymentZoneID(Vector3 position)
    {
        float x = position.x;
        float z = position.z;

        // position lies within player1 zone:
        if (x >= _mapConfig.player1DeploymentZone_X_offset && x < _mapConfig.player1DeploymentZone_X_offset + _mapConfig.player1DeploymentZoneWidth
    && z >= _mapConfig.player1DeploymentZone_Z_offset && z < _mapConfig.player1DeploymentZone_Z_offset + _mapConfig.player1DeploymentZoneDepth)
        {
            return 1;
        }

        // position lies within player2 zone:
        else if (x >= _mapConfig.player2DeploymentZone_X_offset && x < _mapConfig.player2DeploymentZone_X_offset + _mapConfig.player2DeploymentZoneWidth
    && z >= _mapConfig.player2DeploymentZone_Z_offset && z < _mapConfig.player2DeploymentZone_Z_offset + _mapConfig.player2DeploymentZoneDepth)
        {
            return 2;
        }

        // position lies within neutral zone:
        else if (x >= _mapConfig.neutralZone_X_offset && x < _mapConfig.neutralZone_X_offset + _mapConfig.neutralZoneWidth
        && z >= _mapConfig.neutralZone_Z_offset && z < _mapConfig.neutralZone_Z_offset + _mapConfig.neutralZoneDepth)
        {
            return 3;
        }

        // invalid or out of bounds:
        else
        {
            return 999;
        }
    }

    /*
    void UpdateDeploymentZoneData()
    {
        _deploymentZoneData.player1DeploymentZoneWidth = 6; // was 6 // redundant as no change
        _deploymentZoneData.player1DeploymentZoneDepth = _mapConfig.gridDepth;
        _deploymentZoneData.player1DeploymentZone_X_offset = 0; // was 2 // this sets an offset of the play-grid to the outer perimeter
        _deploymentZoneData.player1DeploymentZone_Z_offset = 0; // was 2 // this sets an offset of the play-grid to the outer perimeter

        _deploymentZoneData.team1DeploymentZoneTiles = new List<GameObject>();
        _deploymentZoneData.team1UsedDeploymentZoneTiles = new List<Transform>();

        _deploymentZoneData.neutralZoneWidth = _mapConfig.gridWidth - _deploymentZoneData.player1DeploymentZoneWidth - _deploymentZoneData.player2DeploymentZoneWidth;
        _deploymentZoneData.neutralZoneDepth = _mapConfig.gridDepth;
        _deploymentZoneData.neutralLandZone_X_offset = _deploymentZoneData.player1DeploymentZone_X_offset + _deploymentZoneData.player1DeploymentZoneWidth; // mid-field adjacent to redTeamDeploymentZone: 
        _deploymentZoneData.neutralZone_Z_offset = 0; // was 2 // this sets an offset of the play-grid to the outer perimeter // redundant as no change

        _deploymentZoneData.team2DeploymentZoneTiles = new List<GameObject>(); // public so the AI can access this to get its deployment zone
        _deploymentZoneData.usedTeam2DeploymentZoneTiles = new List<Transform>();

        _deploymentZoneData.player2DeploymentZoneWidth = 6; // was 6 // redundant as no change
        _deploymentZoneData.player2DeploymentZoneDepth = _mapConfig.gridDepth;
        _deploymentZoneData.player2DeploymentZone_X_offset = _deploymentZoneData.player1DeploymentZone_X_offset + _deploymentZoneData.player1DeploymentZoneWidth + _deploymentZoneData.neutralZoneWidth; // right-field adjacent to noMansLandZone: 
        _deploymentZoneData.player2DeploymentZone_Z_offset = 0; // was 2 // redundant as no change

        _deploymentZoneData.neutralDeploymentZoneTiles = new List<GameObject>();
        _deploymentZoneData.usedNeutralDeploymentZoneTiles = new List<Transform>();
    }*/

    /*
    bool InsidePlayer1Zone(Vector3 _position)
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
    }
    */
}
