using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates the map grid for the two players and neutral deployment-zones.
/// </summary>
public class DeploymentGridGenerator : MonoBehaviour
{
    private MapConfig _mapConfig;


    /// <summary>
    /// Setup this script. Called by <see cref="LevelArchitect.GenerateLevel"/>.
    /// </summary>
    /// <param name="mapConfig"></param>
    public void InitializeDeploymentGridGenerator(MapConfig mapConfig)
    {
        _mapConfig = mapConfig;
    }


    /// <summary>
    /// Generates the underlying map-grid for all other features to be placed upon. Called by <see cref="GameManager.Awake"/>.
    /// </summary>
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
            }
        }

        // report back when DeploymentGrid is setup:
        GetComponent<LevelArchitect>().UpdateMapConfig(_mapConfig);
    }

    /// <summary>
    /// Sort the created SpawnZone into the correct list of spawn zones.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="instantiatedSpawnZone"></param>
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
    /// Automatically assignes the correct deployment zone based on the tiles location.
    /// 0 = environment (here: same as neutral); 1 = player 1; 2 = player 2; 3 = neutral.
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
}
