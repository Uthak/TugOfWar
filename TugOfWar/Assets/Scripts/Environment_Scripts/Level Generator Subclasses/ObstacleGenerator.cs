using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    public void SpawnObstacles()
    {
        // Logic to spawn random obstacles following the collapsing wave function
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
}
