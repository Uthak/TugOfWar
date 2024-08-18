using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates the bases for the two players.
/// </summary>
public class BaseGenerator : MonoBehaviour
{
    LevelArchitect _levelArchitect;
    MapConfig _mapConfig;
    BaseConfig _baseConfig;

    /// <summary>
    /// Setup this script. Called by <see cref="LevelArchitect.GenerateLevel"/>.
    /// </summary>
    /// <param name="mapConfig"></param>
    /// <param name="baseConfig"></param>
    public void InitializeBaseGenerator(MapConfig mapConfig, BaseConfig baseConfig)
    {
        _levelArchitect = FindAnyObjectByType<LevelArchitect>();
        _mapConfig = mapConfig; 
        _baseConfig = baseConfig;
    }


    /// <summary>
    /// This gets called by <see cref="LevelArchitect.GenerateLevel"/>. This gets called before obstacles and neutrals are placed.
    /// This method will create both player bases in the scene.
    /// </summary>
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
    #endregion

    #region Create Base Towers:
    private void CreateGuardTowers(int playerID)
    {
        float towerZPosition;

        switch (playerID)
        {
            case 0: // environment:
                break;

            case 1: // player 1:
                float p1HalfSizeOfTower = GetObjectSize(_baseConfig.playerOneGuardTower.GetComponent<Collider>());
                towerZPosition = (float)_mapConfig.gridDepth / ((float)_baseConfig.p1NrOfTowers + 1.0f);

                for (int i = 0; i < _baseConfig.p1NrOfTowers; i++)
                {
                    float xPos = _mapConfig.gridWidth - _mapConfig.gridWidth + _mapConfig.player1DeploymentZoneWidth + p1HalfSizeOfTower;
                    float zPos = towerZPosition * (1 + i);
                    Vector3 towerPosPlayerOne = new Vector3(xPos, 0, zPos);
                    GameObject tower = Instantiate(_baseConfig.playerOneGuardTower, towerPosPlayerOne, Quaternion.Euler(0, 90, 0), _baseConfig.playerOneUnitsParent);
                    tower.GetComponent<UnitManager>().InitializeUnit(1);
                }
                break;

            case 2: // player 2:
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
                break;

            case 3: // neutral/AI: 
                break;
        }

        Debug.Log("CONTROL: Both players towers have been placed and connected");
    }
    #endregion

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