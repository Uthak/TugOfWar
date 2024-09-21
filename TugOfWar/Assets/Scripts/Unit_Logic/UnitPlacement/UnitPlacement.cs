using System.Collections.Generic;
using UnityEngine;
using static UnitEnumManager;

public class UnitPlacement : MonoBehaviour
{
    public static UnitPlacement unitPlacementSingleton; // Singleton

    [Header("Unit Placement Setup:")]
    [SerializeField] private Transform _player1UnitParent;
    [SerializeField] private float _offsetOnMap = 1.0f;
    [SerializeField] private LayerMask _uiLayerMask;

    private GameObject _selectedUnitPrefab;
    private GameObject _targetedGameObject;
    private bool _unitPlacementEnabled = false;
    private Vector3 _mousePosition;

    // these are set here, as the "SampleUnit"-class gets instantiated and cannot have pre-declared variables:
    [SerializeField] Material _defaultMaterial;
    [SerializeField] Material _validMaterial;
    [SerializeField] Material _invalidMaterial;
    private SampleUnit _sampleUnit;

    // Singleton setup
    private void Awake()
    {
        unitPlacementSingleton = this;
    }

    // Call to enable the script
    public void InitializeUnitPlacement()
    {
        _unitPlacementEnabled = true;
    }

    // Select a unit and spawn its sample model for preview
    public void SelectUnit(GameObject unitPrefab)
    {
        if (_unitPlacementEnabled)
        {
            // Return the previous sample model to its pool
            if (_sampleUnit != null)
            {
                _sampleUnit.DisableSampleModel(); // Disables and returns to pool
            }

            _selectedUnitPrefab = unitPrefab;
            _sampleUnit = new SampleUnit(unitPrefab, _defaultMaterial, _validMaterial, _invalidMaterial);
            _sampleUnit.EnableSampleModel();
        }
    }

    private void Update()
    {
        if (!_unitPlacementEnabled || _selectedUnitPrefab == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _uiLayerMask))
        {
            _targetedGameObject = hit.collider.gameObject;

            if (IsLegalDeploymentLocation(_targetedGameObject))
            {
                if (!_targetedGameObject.GetComponent<SpawnZone>().occupied)
                {
                    _mousePosition = _targetedGameObject.transform.position;
                    _sampleUnit.UpdateSampleModelPosition(_mousePosition);

                    if (SufficientResources())
                    {
                        _sampleUnit.SetSampleMaterial(SampleUnitMaterial.Valid);

                        if (Input.GetMouseButton(0))
                        {
                            PlaceUnit(_targetedGameObject);
                        }
                    }
                    else
                    {
                        Debug.Log("need more money");
                        _sampleUnit.SetSampleMaterial(SampleUnitMaterial.Invalid);
                        // TODO: Play error sound or visual feedback
                    }
                }
            }
        }

        // Handle logic for sample model removal if the player clicks outside the deployment area
        if (Input.GetMouseButton(1)) // For instance, right-click cancels unit placement
        {
            _sampleUnit.DisableSampleModel(); // Disable only on specific action
        }
    }

    private void PlaceUnit(GameObject spawnZone)
    {
        // Get unit footprint dimensions
        //UnitProfile profile = _selectedUnitPrefab.GetComponent<UnitManager>().unitProfile;
        /*int footprintWidth = profile.footprintWidth;
        int footprintDepth = profile.footprintDepth;


        Debug.Log("2. foot w " + footprintWidth + " and footde " + footprintDepth);

        // Find and mark all deployment zones
        List<SpawnZone> zonesToOccupy = GetFootprintZones(spawnZone.GetComponent<SpawnZone>(), footprintWidth, footprintDepth);

        foreach (SpawnZone zone in zonesToOccupy)
        {
            zone.OccupyDeploymentTile();
            Debug.Log("zone occupied." +  zone.transform.position);
        }
        Debug.Log("tst");*/

        // Get the unit instance and place it
        GameObject unitInstance = UnitPoolManager.Instance.GetUnitFromPool(_selectedUnitPrefab);
        unitInstance.transform.position = _mousePosition;

        // Initialize the unit with all occupied zones
        unitInstance.GetComponent<UnitManager>().InitializeUnit(1);
        UnitProfile profile = unitInstance.GetComponent<UnitManager>().unitProfile;

        int footprintWidth = profile.footprintWidth;
        int footprintDepth = profile.footprintDepth;


        Debug.Log("2. foot w " + footprintWidth + " and footde " + footprintDepth);

        // Find and mark all deployment zones
        List<SpawnZone> zonesToOccupy = GetFootprintZones(spawnZone.GetComponent<SpawnZone>(), footprintWidth, footprintDepth);

        foreach (SpawnZone zone in zonesToOccupy)
        {
            zone.OccupyDeploymentTile();
            Debug.Log("zone occupied." + zone.transform.position);
        }
        unitInstance.GetComponent<UnitManager>().UpdateZonesDeployedByUnit(zonesToOccupy);



        // Subtract resources
        GetComponent<GoldManager>().SubtractGold(1, unitInstance.GetComponent<UnitManager>().unitProfile.deploymentCost);
    }
    /*
    // Place the actual unit
    private void PlaceUnit(GameObject spawnZone)
    {
        // tell the targeted deployment-tile that it is now occupied:
        spawnZone.GetComponent<SpawnZone>().OccupyDeploymentTile();

        // get a unit instance and place it in desired deployment tile:
        GameObject unitInstance = UnitPoolManager.Instance.GetUnitFromPool(_selectedUnitPrefab);
        unitInstance.transform.position = _mousePosition;

        // as only the player 1 can access this currently, its set default to "1":
        unitInstance.GetComponent<UnitManager>().InitializeUnit(1, spawnZone);


        GetComponent<GoldManager>().SubtractGold(1, unitInstance.GetComponent<UnitManager>().unitProfile.deploymentCost);
    }*/

    // Check if enough resources are available
    private bool SufficientResources()
    {
        return GetComponent<GoldManager>().SufficientGold(1, _selectedUnitPrefab.GetComponent<UnitManager>().unitProfile.deploymentCost);
    }

    /*
    // Validate if the target location is legal for placement
    private bool IsLegalDeploymentLocation(GameObject targetedLocation)
    {
        SpawnZone spawnZone = targetedLocation.GetComponent<SpawnZone>();

        if (spawnZone != null && spawnZone.player1Zone)
        {
            return true;
        }
        return false;
    }*/
    /*
    private bool IsLegalDeploymentLocation(GameObject targetedLocation)
    {
        SpawnZone spawnZone = targetedLocation.GetComponent<SpawnZone>();

        if (spawnZone != null && spawnZone.player1Zone)
        {
            // Get unit footprint dimensions from UnitProfile
            UnitProfile profile = _selectedUnitPrefab.GetComponent<UnitManager>().unitProfile;
            int footprintWidth = profile.footprintWidth;
            int footprintDepth = profile.footprintDepth;

            // Now check if there are enough free zones for the entire footprint
            List<SpawnZone> zonesToCheck = GetFootprintZones(spawnZone, footprintWidth, footprintDepth);

            foreach (var zone in zonesToCheck)
            {
                if (zone.occupied) return false; // At least one zone is occupied
            }

            return true; // All required zones are free
        }
        return false;
    }*/

    private bool IsLegalDeploymentLocation(GameObject targetedLocation)
    {
        SpawnZone spawnZone = targetedLocation.GetComponent<SpawnZone>();

        if (spawnZone != null && spawnZone.player1Zone)
        {
            // Get unit footprint dimensions from UnitProfile
            UnitProfile profile = _selectedUnitPrefab.GetComponent<UnitManager>().unitProfile;
            int footprintWidth = profile.footprintWidth;
            int footprintDepth = profile.footprintDepth;

            // Check the current zone and all adjacent zones based on footprint
            if (!CheckAdjacentZones(spawnZone, footprintWidth, footprintDepth))
            {
                return false; // Some zones are occupied
            }

            return true; // All zones are free
        }
        return false;
    }

    private bool CheckAdjacentZones(SpawnZone startZone, int width, int depth)
    {
        Vector3 startPosition = startZone.transform.position;
        float gridSize = 1.0f; // Assuming each grid tile is 1x1 unit

        // Iterate through footprint dimensions
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                // Calculate the position of the adjacent zone
                Vector3 adjacentPosition = startPosition + new Vector3(x * gridSize, 0, z * gridSize);

                // Raycast to find the adjacent grid tile
                if (Physics.Raycast(adjacentPosition + Vector3.up * 10, Vector3.down, out RaycastHit hit, Mathf.Infinity, _uiLayerMask))
                {
                    SpawnZone adjacentZone = hit.collider.GetComponent<SpawnZone>();
                    if (adjacentZone == null || adjacentZone.occupied)
                    {
                        return false; // Zone is occupied or invalid
                    }
                }
                else
                {
                    return false; // No zone found in that direction
                }
            }
        }

        return true; // All zones are available
    }

    // Get all deployment zones affected by the unit's footprint
    /*private List<SpawnZone> GetFootprintZones(SpawnZone startZone, int width, int depth)
    {
        List<SpawnZone> zones = new List<SpawnZone>();

        // Assume the SpawnZone system has a grid, and that startZone knows its coordinates
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                // You need logic here to get adjacent zones based on the start zone position
                SpawnZone zone = GetAdjacentZone(startZone, x, z);
                if (zone != null)
                {
                    zones.Add(zone);
                }
            }
        }

        return zones;
    }*/
    private List<SpawnZone> GetFootprintZones(SpawnZone startZone, int width, int depth)
    {
        Debug.Log("1");
        Debug.Log("initial spawn pos " + startZone.transform.position+ ", w " + width + ", d " + depth);


        List<SpawnZone> zones = new List<SpawnZone>();
        Debug.Log("2");

        Vector3 startPosition = startZone.transform.position;
        float gridSize = 1.0f; // Assuming each grid tile is 1x1 unit
        Debug.Log("3");

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                Debug.Log("4");

                Vector3 adjacentPosition = startPosition + new Vector3(x * gridSize, 0, z * gridSize);

                if (Physics.Raycast(adjacentPosition + Vector3.up * 10, Vector3.down, out RaycastHit hit, Mathf.Infinity, _uiLayerMask))
                {
                    SpawnZone adjacentZone = hit.collider.GetComponent<SpawnZone>();
                    if (adjacentZone != null)
                    {
                        zones.Add(adjacentZone);
                    }
                }
            }
        }

        return zones; // Return all valid zones
    }
}
