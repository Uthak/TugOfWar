using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Create all neutral buildings and objects in the scene. This gets managed and called upon by <see cref="LevelArchitect"/>.
/// </summary>
public class NeutralObjectGenerator : MonoBehaviour
{
    [SerializeField] bool _debug = false;

    MapConfig _mapConfig;
    NeutralObjectConfig _neutralObjectConfig;

    [SerializeField] private LayerMask spawnZoneLayerMask; // LayerMask for deployment zones

    /// <summary>
    /// Setup this script. Called by <see cref="LevelArchitect.GenerateLevel"/>.
    /// </summary>
    /// <param name="mapConfig"></param>
    /// <param name="neutralObjectConfig"></param>
    public void InitializeNeutralObjectGenerator(MapConfig mapConfig, NeutralObjectConfig neutralObjectConfig)
    {
        if (_debug)
        {
            Debug.Log($"DEBUG: NeutralObjectGenerator debugging active", this);
        }

        _mapConfig = mapConfig;
        _neutralObjectConfig = neutralObjectConfig; 
    }


    /// <summary>
    /// This gets called by <see cref="LevelArchitect.GenerateLevel"/>. This gets called before obstacles are placed.
    /// This method will create all neutral objects in the scene: buildings, units and other objects if applicable.
    /// </summary>
    public void GenerateNeutralObjects()
    {
        // call methods to generate various neutral buildings, units and objects as desired:
        CreateNeutralTowers(_neutralObjectConfig.randomNeutralTowers);

        // report back when done:
        GetComponent<LevelArchitect>().UpdateMapConfig(_mapConfig);
    }


    /// <summary>
    /// Instantiate a given number of neutral towers either following a pattern or randomly in the maps neutral zone.
    /// </summary> 
    /// <param name="randomPlacement"></param>
    void CreateNeutralTowers(bool randomPlacement)
    {
        #region Instructions:
        // 1. Get the width of the field and devide it into 6 parts.
        //    The 4 center parts make up the midfield. All neutral objects belong in this section.
        // 2. Devide the midfield into an sections to place neutral towers in.
        //    If nr of neutral towers is uneven, one tower is always smack in the middle, the others at equal distance spread to left & right.
        #endregion
        GameObject neutralTower = _neutralObjectConfig.neutralTower;
        int nrOfNeutralTowers = _neutralObjectConfig.nrOfNeutralTowers;

        float halfSizeOfNeutralTower = GetObjectSize(neutralTower.GetComponent<Collider>());

        // variables to spread towers horizontally:
        float singleHorizontalFieldSegment = (float)_mapConfig.gridWidth / 6.0f; // eg: 60/6=10
        float centerFieldWidth = (float)_mapConfig.gridWidth - (2 * singleHorizontalFieldSegment); // eg: 60-2*10=40
        float horizontalDistanceBetweenTowers = centerFieldWidth / (nrOfNeutralTowers + 1);
        float xPos = singleHorizontalFieldSegment; // start point of centerField, here eg: 10

        // variables to spread towers vertically:
        float verticalDistanceBetweenTowers = _mapConfig.gridDepth / (nrOfNeutralTowers + 1);
        float zPos;

        // FOR TESTING:
        if (_debug)
        {
            Debug.Log($"DEBUG: singleHorizontalFieldSegment {singleHorizontalFieldSegment}, centerFieldWidth {centerFieldWidth}, " +
            $"horizontalDistanceBetweenTowers {horizontalDistanceBetweenTowers}, starting xPos {xPos}, " +
            $"nrOfNeutralTowers {nrOfNeutralTowers}", this);
        }
        
        switch (randomPlacement)
        {
            case true: // place randomly:
                for (int i = 0; i < nrOfNeutralTowers; i++)
                {
                    xPos += horizontalDistanceBetweenTowers;
                    zPos = Random.Range(0.0f + halfSizeOfNeutralTower, _mapConfig.gridDepth - halfSizeOfNeutralTower);
                    Vector3 neutralTowerPos = new Vector3(xPos, 0, zPos);
                    float rngRotation = Random.Range(0, 360);
                    GameObject instantiatedNeutralTower = Instantiate(neutralTower, neutralTowerPos, Quaternion.Euler(0, rngRotation, 0), _neutralObjectConfig.neutralUnitsParent);


                    //instantiatedNeutralTower.GetComponent<UnitManager>().InitializeUnit(3);
                    //instantiatedNeutralTower.GetComponent<UnitManager>().InitializeUnit(3, FindOccupyAndReturnDeploymentZones(instantiatedNeutralTower));
                    instantiatedNeutralTower.GetComponent<UnitManager>().InitializeUnit(3);
                    instantiatedNeutralTower.GetComponent<UnitManager>().UpdateZonesDeployedByUnit(FindOccupyAndReturnDeploymentZones(instantiatedNeutralTower));

                    // add occupied area in neutral zone to occupied list in MapConfig to be sent back to LevelArchitect later.
                    // curently disabled as grid is not being used.
                }
                break;

            case false: // place in specific pattern:

                if (nrOfNeutralTowers == 2) // special case for exactly 2 towers:
                {
                    for (int i = 0; i < nrOfNeutralTowers; i++)
                    {
                        xPos += horizontalDistanceBetweenTowers;
                        zPos = (i == 0) ? _mapConfig.gridDepth / 6.0f :
                               5 * _mapConfig.gridDepth / 6.0f; // 1/6, 5/6
                        Vector3 neutralTowerPos = new Vector3(xPos, 0, zPos);
                        float rngRotation = Random.Range(0, 360);
                        GameObject neutralTowerInstance = Instantiate(neutralTower, neutralTowerPos, Quaternion.Euler(0, rngRotation, 0), _neutralObjectConfig.neutralUnitsParent);

                        //neutralTowerInstance.GetComponent<UnitManager>().InitializeUnit(3);
                        //neutralTowerInstance.GetComponent<UnitManager>().InitializeUnit(3, FindOccupyAndReturnDeploymentZones(neutralTowerInstance));
                        neutralTowerInstance.GetComponent<UnitManager>().InitializeUnit(3);
                        neutralTowerInstance.GetComponent<UnitManager>().UpdateZonesDeployedByUnit(FindOccupyAndReturnDeploymentZones(neutralTowerInstance));

                        // add occupied area in neutral zone to occupied list in MapConfig to be sent back to LevelArchitect later.
                        // curently disabled as grid is not being used.
                    }
                }
                else if (nrOfNeutralTowers == 3) // special case for exactly 3 towers:
                {
                    for (int i = 0; i < nrOfNeutralTowers; i++)
                    {
                        xPos += horizontalDistanceBetweenTowers;
                        zPos = (i == 0) ? _mapConfig.gridDepth / 6.0f :
                               (i == 1) ? _mapConfig.gridDepth / 2.0f :
                               5 * _mapConfig.gridDepth / 6.0f; // 1/6, 3/6, 5/6
                        Vector3 neutralTowerPos = new Vector3(xPos, 0, zPos);
                        float rngRotation = Random.Range(0, 360);
                        GameObject neutralTowerInstance = Instantiate(neutralTower, neutralTowerPos, Quaternion.Euler(0, rngRotation, 0), _neutralObjectConfig.neutralUnitsParent);

                        //neutralTowerInstance.GetComponent<UnitManager>().InitializeUnit(3);
                        //neutralTowerInstance.GetComponent<UnitManager>().InitializeUnit(3, FindOccupyAndReturnDeploymentZones(neutralTowerInstance));
                        neutralTowerInstance.GetComponent<UnitManager>().InitializeUnit(3);
                        neutralTowerInstance.GetComponent<UnitManager>().UpdateZonesDeployedByUnit(FindOccupyAndReturnDeploymentZones(neutralTowerInstance));

                        // add occupied area in neutral zone to occupied list in MapConfig to be sent back to LevelArchitect later.
                        // curently disabled as grid is not being used.
                    }
                }
                else // any other number of towers:
                {
                    for (int i = 1; i <= nrOfNeutralTowers; i++)
                    {
                        xPos += horizontalDistanceBetweenTowers;
                        zPos = verticalDistanceBetweenTowers * i;
                        Vector3 neutralTowerPos = new Vector3(xPos, 0, zPos);
                        float rngRotation = Random.Range(0, 360);
                        GameObject neutralTowerInstance = Instantiate(neutralTower, neutralTowerPos, Quaternion.Euler(0, rngRotation, 0), _neutralObjectConfig.neutralUnitsParent);

                        //neutralTowerInstance.GetComponent<UnitManager>().InitializeUnit(3);
                        //neutralTowerInstance.GetComponent<UnitManager>().InitializeUnit(3, FindOccupyAndReturnDeploymentZones(neutralTowerInstance));
                        neutralTowerInstance.GetComponent<UnitManager>().InitializeUnit(3);
                        neutralTowerInstance.GetComponent<UnitManager>().UpdateZonesDeployedByUnit(FindOccupyAndReturnDeploymentZones(neutralTowerInstance));

                        // add occupied area in neutral zone to occupied list in MapConfig to be sent back to LevelArchitect later.
                        // curently disabled as grid is not being used.
                    }
                }
                break;
        }
    }

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
