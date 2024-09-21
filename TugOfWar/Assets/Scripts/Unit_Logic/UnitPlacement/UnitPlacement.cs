using System.Collections.Generic;
using UnityEngine;
using static UnitEnumManager;

public class UnitPlacement : MonoBehaviour
{
    //debugging:
    [SerializeField] bool _debugThisClass = false; // this is serializable so that cou can debug distinct classes instead of everything!

    public static UnitPlacement unitPlacementSingleton; // Singleton

    [Header("UnitPlacement Setup:")]
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
    public void InitializeUnitPlacement(bool debug)
    {
        if (!_debugThisClass)
        {
            _debugThisClass = debug;
        }

        _unitPlacementEnabled = true;

        // debug:
        if (_debugThisClass)
        {
            ApplicationDebugger.LogClassInitialization();
        }
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

    // Check if enough resources are available
    private bool SufficientResources()
    {
        return GetComponent<GoldManager>().SufficientGold(1, _selectedUnitPrefab.GetComponent<UnitManager>().unitProfile.deploymentCost);
    }

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


    private List<SpawnZone> GetFootprintZones(SpawnZone startZone, int width, int depth)
    {
        List<SpawnZone> zones = new List<SpawnZone>();

        Vector3 startPosition = startZone.transform.position;
        float gridSize = 1.0f; // Assuming each grid tile is 1x1 unit

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
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
