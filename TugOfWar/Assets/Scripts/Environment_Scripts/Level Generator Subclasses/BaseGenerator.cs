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

    [SerializeField] private LayerMask spawnZoneLayerMask; // LayerMask for deployment zones
    //[SerializeField] private float overlapCheckRadius = 10f; // Adjust the radius based on your setup


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

                //_baseConfig.player1HQ.GetComponent<UnitManager>().InitializeUnit(1);
                //FindOccupyAndReturnDeploymentZones(_baseConfig.player1HQ); // Check and occupy nearby zones

                //_baseConfig.player1HQ.GetComponent<UnitManager>().InitializeUnit(1, FindOccupyAndReturnDeploymentZones(_baseConfig.player1HQ));
                _baseConfig.player1HQ.GetComponent<UnitManager>().InitializeUnit(1);
                _baseConfig.player1HQ.GetComponent<UnitManager>().UpdateZonesDeployedByUnit(FindOccupyAndReturnDeploymentZones(_baseConfig.player1HQ));
                break;

            case 2:
                halfSizeOfHQ = GetObjectSize(_baseConfig.playerTwoHeadquarter.GetComponent<Collider>());
                xPosHQ = _mapConfig.gridWidth - halfSizeOfHQ; // e.g. 0 base line
                zPosHQ = _mapConfig.gridDepth / 2.0f;
                hqPos = new Vector3(xPosHQ, 0, zPosHQ);

                // instantiate and initialize HQ:
                _baseConfig.player2HQ = Instantiate(_baseConfig.playerTwoHeadquarter, hqPos, Quaternion.Euler(0, -90, 0), _baseConfig.playerTwoUnitsParent);

                //_baseConfig.player2HQ.GetComponent<UnitManager>().InitializeUnit(2);
                //FindOccupyAndReturnDeploymentZones(_baseConfig.player2HQ); // Check and occupy nearby zones

                //_baseConfig.player2HQ.GetComponent<UnitManager>().InitializeUnit(2, FindOccupyAndReturnDeploymentZones(_baseConfig.player2HQ));
                _baseConfig.player2HQ.GetComponent<UnitManager>().InitializeUnit(2);
                _baseConfig.player2HQ.GetComponent<UnitManager>().UpdateZonesDeployedByUnit(FindOccupyAndReturnDeploymentZones(_baseConfig.player2HQ));
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
                    
                    //tower.GetComponent<UnitManager>().InitializeUnit(1);
                    //FindOccupyAndReturnDeploymentZones(tower); // Check and occupy nearby zones

                    //tower.GetComponent<UnitManager>().InitializeUnit(1, FindOccupyAndReturnDeploymentZones(tower));
                    tower.GetComponent<UnitManager>().InitializeUnit(1);
                    tower.GetComponent<UnitManager>().UpdateZonesDeployedByUnit(FindOccupyAndReturnDeploymentZones(tower));
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

                    //tower.GetComponent<UnitManager>().InitializeUnit(2); // combined, see below <
                    //OccupyDeploymentZones(tower); // Check and occupy nearby zones // combined, see below <

                    //tower.GetComponent<UnitManager>().InitializeUnit(2, FindOccupyAndReturnDeploymentZones(tower));
                    tower.GetComponent<UnitManager>().InitializeUnit(2);
                    tower.GetComponent<UnitManager>().UpdateZonesDeployedByUnit(FindOccupyAndReturnDeploymentZones(tower));
                }
                break;

            case 3: // neutral/AI: 
                break;
        }

        Debug.Log("CONTROL: Both players towers have been placed and connected");
    }
    #endregion

    // Check which deployment zones overlap with the structure and occupy them
    /*private void OccupyDeploymentZones(GameObject structure)
    {
        Collider[] hitColliders = Physics.OverlapSphere(structure.transform.position, overlapCheckRadius, spawnZoneLayerMask);

        foreach (Collider hitCollider in hitColliders)
        {
            SpawnZone spawnZone = hitCollider.GetComponent<SpawnZone>();
            if (spawnZone != null && !spawnZone.occupied)
            {
                spawnZone.OccupyDeploymentTile(); // Mark the zone as occupied
                Debug.Log($"Zone at {hitCollider.transform.position} occupied by {structure.name}");
            }
        }
    }*/
    /*
    private void OccupyDeploymentZones(GameObject placedUnit)
    {
        // Get the collider of the unit or structure being placed
        Collider unitCollider = placedUnit.GetComponent<Collider>();

        if (unitCollider == null)
        {
            Debug.LogWarning("Placed unit does not have a collider.");
            return;
        }

        // Determine the radius/size to use for checking deployment zones
        float checkRadius = GetObjectSize(unitCollider);

        // Find all deployment zones within the calculated size
        Collider[] hitColliders = Physics.OverlapSphere(placedUnit.transform.position, checkRadius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out SpawnZone spawnZone))
            {
                spawnZone.OccupyDeploymentTile();
            }
        }
    }*/
    private List<SpawnZone> FindOccupyAndReturnDeploymentZones(GameObject unitOrStructure)
    {
        List<SpawnZone> occupiedZones = new List<SpawnZone>();
        Collider[] hitColliders = Physics.OverlapBox(unitOrStructure.transform.position, unitOrStructure.GetComponent<Collider>().bounds.extents);

        foreach (Collider hitCollider in hitColliders)
        {
            SpawnZone spawnZone = hitCollider.GetComponent<SpawnZone>();
            if (spawnZone != null)
            {
                spawnZone.OccupyDeploymentTile(); // Mark the zone as occupied
                occupiedZones.Add(spawnZone);
            }
        }

        return occupiedZones;
    }

    /* // ChatGPT refactoring feedback (not implemented!):
    1. Deployment Zones Overlap: Your FindOccupyAndReturnDeploymentZones method uses Physics.OverlapBox, which should work well for 
    larger structures or irregular shapes. However, ensure that the bounds you’re passing as the extents match the actual footprint 
    of your units or structures.
    2. Performance Considerations: If you have many objects or large structures that will trigger multiple zone checks, make sure 
    the Physics.OverlapBox call isn’t too performance-heavy. If you notice any slowdowns, you might need to optimize how frequently 
    this check occurs or reduce unnecessary zone checks.
    3. Collider Size & Extents: The logic inside GetObjectSize is calculating the bounds of a BoxCollider, but if you use scaling 
    on the objects, you may want to ensure that bounds.extents (used in Physics.OverlapBox) properly takes the scaling into account.
     */


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
            //return box.size.z / 2.0f;
            return Mathf.Max(box.size.x, box.size.z) / 2.0f;

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