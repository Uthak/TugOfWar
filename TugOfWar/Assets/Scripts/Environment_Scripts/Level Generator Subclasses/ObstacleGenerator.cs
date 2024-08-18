using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Create all obstacles in the scene. This gets managed and called upon by <see cref="LevelArchitect"/>.
/// </summary>
public class ObstacleGenerator : MonoBehaviour
{
    [SerializeField] bool _debug = false;

    MapConfig _mapConfig;
    ObstacleConfig _obstacleConfig;


    /// <summary>
    /// Setup this script. Called by <see cref="LevelArchitect.GenerateLevel"/>.
    /// </summary>
    /// <param name="mapConfig"></param>
    /// <param name="obstacleConfig"></param>
    public void InitializeObstacleGenerator(MapConfig mapConfig, ObstacleConfig obstacleConfig)
    {
        if (_debug)
        {
            Debug.Log($"DEBUG: NeutralObjectGenerator debugging active", this);
        }

        _mapConfig = mapConfig;
        _obstacleConfig = obstacleConfig;
    }


    /// <summary>
    /// This gets called by <see cref="LevelArchitect.GenerateLevel"/>.
    /// This method will create all obstacles in the scene. This happens after both bases and neutral elements have been placed.
    /// </summary>
    public void GenerateObstacles()
    {
        // spawn obstacles in player 1 deployment zone:
        for (int i = 0; i < _obstacleConfig.obstaclesInPlayer1Zone; i++)
        {
            InstantiateObstacle(1);
        }

        // spawn obstacles in player 2 deployment zone:
        for (int i = 0; i < _obstacleConfig.obstaclesInPlayer2Zone; i++)
        {
            InstantiateObstacle(2);
        }

        // spawn obstacles in neutral zone:
        for (int i = 0; i < _obstacleConfig.obstaclesInNeutralZone; i++)
        {
            InstantiateObstacle(3);
        }

        // report back when done:
        GetComponent<LevelArchitect>().UpdateMapConfig(_mapConfig);
    }


    /// <summary>
    /// Instantiates a random obstacle in a random location.
    /// </summary>
    /// <param name="zoneID"></param>
    void InstantiateObstacle(int zoneID)
    {
        // get random obstacle, using ONLY TREES CURRENTLY!
        GameObject randomObstacle = _obstacleConfig.arrayOfTrees[Random.Range(0, _obstacleConfig.arrayOfTrees.Length)];
        GameObject randomLocation;
        List<Transform> usedDeploymentZoneTiles = null;
        List<GameObject> deploymentZoneTiles = null;

        // get correct deployment zones anc cache them.
        switch (zoneID)
        {
            case 1:
                deploymentZoneTiles = _mapConfig.team1DeploymentZoneTiles;
                usedDeploymentZoneTiles = _mapConfig.team1UsedDeploymentZoneTiles;
                break;
            case 2:
                deploymentZoneTiles = _mapConfig.team2DeploymentZoneTiles;
                usedDeploymentZoneTiles = _mapConfig.usedTeam2DeploymentZoneTiles;
                break;
            case 3:
                deploymentZoneTiles = _mapConfig.neutralDeploymentZoneTiles;
                usedDeploymentZoneTiles = _mapConfig.usedNeutralDeploymentZoneTiles;
                break;
        }

        // check if both lists have been assigned, else return:
        if (deploymentZoneTiles == null || usedDeploymentZoneTiles == null) return;

        // get random, legal position
        int randomNr = Random.Range(0, deploymentZoneTiles.Count);
        randomLocation = deploymentZoneTiles[randomNr];

        // if the random location is available, instantiate the obstacle, otherwise start over:
        if (!usedDeploymentZoneTiles.Contains(randomLocation.transform))
        {
            usedDeploymentZoneTiles.Add(randomLocation.transform); // this currently does NOT save to the MapConfig mother-file! Use "_mapConfig.usedNeutralDeploymentZoneTiles" etc. instead
            GameObject instantiatedObstacle = Instantiate(randomObstacle, randomLocation.transform.position, Quaternion.identity, _obstacleConfig.obstacleParent);
            randomLocation.GetComponent<SpawnZone>().OccupyDeploymentTile(); // unsure why?

            // rotate the inner obstacle base randomly for more variation:
            instantiatedObstacle.transform.Find("innerBase_Section").Rotate(instantiatedObstacle.transform.up, Random.Range(0, 360));
            
            // rotate the outer obstacle base (hast to remain perfect square):
            float degreeRot = GetRandomRotation();
            instantiatedObstacle.transform.Find("outerBase_Section").Rotate(instantiatedObstacle.transform.up, degreeRot);
        }else
        {
            InstantiateObstacle(zoneID);
        }
    }


    /// <summary>
    /// Returns a random rotation that is one of 0°, 90°, 180° or 270°. 
    /// Obviously 360° is redundant as it's == 0°.
    /// </summary>
    /// <returns></returns>
    float GetRandomRotation()
    {
        int rngRotation = Random.Range(0, 4);
        switch (rngRotation)
        {
            case 0: return 0.0f;
            case 1: return 90.0f;
            case 2: return 180.0f;
            case 3: return 270.0f;

            default: return 0.0f;
        }
    }
}
