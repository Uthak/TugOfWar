using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class UnitPlacement : MonoBehaviour
{
    [Header("Unit Placement Setup:")]
    [SerializeField] Transform _player1UnitParent; // this likely has to be changed in actual multiplayer
    [SerializeField] float _offsetOnMap = 1.0f;

    [Space(10)]
    [SerializeField] LayerMask _UI_layerMask;

    [Header("DO NOT TOUCH:")]
    GameObject _selectedUnitType;
    GameObject _sampleModell;
    public GameObject carriedObject;

    // private variables:
    private GameObject _targetedGameObject;
    bool _unitPlacementEnabled = false;
    Vector3 _mousePosition;

    // call this to enable this script.
    public void InitializeUnitPlacement()
    {
        _unitPlacementEnabled =  true;
    }


    // unit selection button calls this function to update the selected unit:
    public void SelectUnit(GameObject unit)
    {
        if (_unitPlacementEnabled)
        {
            _selectedUnitType = unit;
            //_sampleModell = Instantiatunit.GetChild; // instantiate the modell to be displayed over legal deploymentzones as sample.
        }
    }


    private void Update()
    {
        if (_unitPlacementEnabled)
        {
            // cast a ray from the camera to the mouse position:
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _UI_layerMask))
            {
                _targetedGameObject = hit.collider.gameObject;

                /* // does this actually do anything? Or look better?
                // hover far above normal ground:
                if (_targetedGameObject.CompareTag("Ground"))
                {
                    _mousePosition = new Vector3(hit.point.x, hit.point.y + _offsetOnMap, hit.point.z);
                }*/

                // place unit on center of tiles that are allowed for deployment:
                if (IsLegalDeploymentLocation(_targetedGameObject))
                {
                    if (!_targetedGameObject.GetComponent<SpawnZone>().occupied)
                    {
                        if(_selectedUnitType != null)
                        {
                            _mousePosition = _targetedGameObject.transform.position;
                            _sampleModell.SetActive(true); // enable visibility of sampleModell
                            _sampleModell.transform.position = _mousePosition; // set the sampleModell at mouse cursor

                            if (SufficientRessources())
                            {
                                // TBI: turn transluscent sampleModell green here...

                                if (Input.GetMouseButton(0))
                                {
                                    CreateUnit(_targetedGameObject);
                                    //PlaceUnit(_targetedGameObject);
                                }
                            }
                            else
                            {
                                // TBI: turn transluscent sampleModell red here...
                                // TBI: Play error-sound/effect
                            }
                        }
                    }
                }
            }
        }
    }

    /*
    bool LegalPositionForPlacement(RaycastHit hit)
    {
        // check if a SpawnZone was hit and if whether it's occupied:
        if (hit.collider.gameObject.GetComponent<SpawnZone>() && !hit.collider.gameObject.GetComponent<SpawnZone>().occupied)
        {
            return true;
        }else
        {
            return false; 
        }
    }*/

    // this should probably be moved to the GoldManager! -F
    /// <summary>
    /// Check with the "GoldManager"-script if enough resources are available for this unit.
    /// </summary>
    /// <returns></returns>
    bool SufficientRessources()
    {
        if (GetComponent<GoldManager>().SufficientGold(1, carriedObject.GetComponent<UnitManager>().unitProfile.deploymentCost))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Check with the targeted object if it's a "SpawnZone"-tile and further if its available for occupation.
    /// </summary>
    /// <returns></returns>
    bool IsLegalDeploymentLocation(GameObject _targetedLocation)
    {
        SpawnZone _spawnZone;

        // if the targeted object is not a "SpawnZone" return immediatly:
        if (_targetedLocation.GetComponent<SpawnZone>())
        {
            _spawnZone = _targetedLocation.GetComponent<SpawnZone>();
        }
        else
        {
            return false;
        }

        // if it is a "SpawnZone", of the correct deployment area and unoccupied, return true:
        //if (_spawnZone.player1Zone && !_spawnZone.occupied) // currently based on the notion of player vs AI
        if (_spawnZone.player1Zone) // currently based on the notion of player vs AI
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /*
    /// <summary>
    /// This gets called when a unit is placed in a deployment-area.
    /// </summary>
    void PlaceUnit(GameObject _targetLocation)
    {
        // tell the targeted deployment-tile that it is now occupied:
        _targetLocation.GetComponent<SpawnZone>().OccupyDeploymentTile();

        // place the carried object at the center of the chosen tile:
        carriedObject.transform.position = _mousePosition;

        // setup the placed unit after placing it. You can still revert this by picking it up again:
        carriedObject.GetComponent<UnitManager>().DeployThisUnit(1, _targetLocation);

        // pay for the unit at hand:
        GetComponent<GoldManager>().SubtractGold(1, carriedObject.GetComponent<UnitManager>().unitProfile.deploymentCost);

        #region experiment: Auto Launch
        if (FindAnyObjectByType<GameManager>().autoLaunchEnabled)
        {
            carriedObject.GetComponent<UnitManager>().LaunchUnit();

            Debug.Log("shoulda autolaunched now");
        }
        #endregion

        // empty the carried object (nothing is being dragged anymore):
        carriedObject = null;
    }*/

    void DestroyUnit()
    {
        Destroy(carriedObject);
    }

    void DragItem()
    {
        carriedObject.transform.position = _mousePosition;
    }


    /// <summary>
    /// Button logic to pick up units/items. This is called by TroopSelectorIcons (handbuilt buttons in the UI).
    /// </summary>
    /// <param name="_unit"></param>
    //public void CreateUnit(GameObject _unit)
    public void CreateUnit(GameObject _targetLocation)
    {
        // tell the targeted deployment-tile that it is now occupied:
        _targetLocation.GetComponent<SpawnZone>().OccupyDeploymentTile();

        // define the rotation to face the right side of the screen (positive x direction):
        Quaternion rightFacingRotation = Quaternion.Euler(0, 90, 0);

        // instantiate unit:
        GameObject instantiatedUnit = Instantiate(_selectedUnitType, _mousePosition, rightFacingRotation, _player1UnitParent);

        // place the unit at the center of the chosen tile:
        instantiatedUnit.transform.position = _mousePosition;

        // initialize unit:
        instantiatedUnit.GetComponent<UnitManager>().InitializeUnit(1);

        // setup the placed unit after placing it. You can still revert this by picking it up again:
        instantiatedUnit.GetComponent<UnitManager>().DeployThisUnit(1, _targetLocation); // Should now be part of initialization! -F

        // pay for the unit at hand:
        GetComponent<GoldManager>().SubtractGold(1, instantiatedUnit.GetComponent<UnitManager>().unitProfile.deploymentCost);

        #region experiment: Auto Launch
        if (FindAnyObjectByType<GameManager>().autoLaunchEnabled)
        {
            instantiatedUnit.GetComponent<UnitManager>().LaunchUnit();

            Debug.Log("shoulda autolaunched now");
        }
        #endregion

        // empty the carried object (nothing is being dragged anymore):
        //carriedObject = null;
        // setup the picked up units profile:
        //carriedObject.GetComponent<UnitManager>().InitializeUnit(1);
    }

    /// <summary>
    /// This gets called by the selected units "UnitManager" with a "OnMouseDown"-function.
    /// </summary>
    /// <param name="_unit"></param>
    public void PickUpAgain(GameObject _unit)
    {
        // put the picked-up unit back into the players hand (cursor):
        carriedObject = _unit;

        // get refund for the picked-up unit (playerID is 1, as only player can do this):
        GetComponent<GoldManager>().AddGold(1, carriedObject.GetComponent<UnitManager>().unitProfile.deploymentCost);
    }
}
