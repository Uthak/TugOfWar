using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNDrop : MonoBehaviour
{
    [Header("Drag'n'drop Setup:")]
    [SerializeField] Transform _player1UnitParent;
    [SerializeField] float _offsetOnMap = 1.0f;
    
    [Space(10)]
    [SerializeField] LayerMask _UI_layerMask;

    [Header("DO NOT TOUCH:")]
    public GameObject carriedObject;

    // private variables:
    private GameObject _targetedGameObject;
    Vector3 _mousePosition;

    private void Update()
    {
        // cast a ray from the camera to the mouse position:
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _UI_layerMask))
        {
            _targetedGameObject = hit.collider.gameObject;
            
            // hover far above normal ground:
            if (_targetedGameObject.CompareTag("Ground")) 
            {
                _mousePosition = new Vector3(hit.point.x, hit.point.y + _offsetOnMap, hit.point.z);
            }
            
            // place unit on center of tiles that are allowed for deployment:
            if (hit.collider.gameObject.GetComponent<SpawnZone>() && !hit.collider.gameObject.GetComponent<SpawnZone>().occupied)
            {
                _mousePosition = hit.collider.transform.position;
            }
        }

        if (Input.GetMouseButton(0) && carriedObject != null)
        {
            DragItem();
        }

        else if (Input.GetMouseButtonUp(0) && carriedObject != null)
        {
            if(LegalDeploymentLocation(_targetedGameObject) && SufficientRessources())
            {
                PlaceUnit(_targetedGameObject);
            }else
            {
                DestroyUnit();
            }
        }
    }

    // this should probably be moved to the GoldManager! -F
    /// <summary>
    /// Check with the "GoldManager"-script if enough resources are available for this unit.
    /// </summary>
    /// <returns></returns>
    bool SufficientRessources()
    {
        if (GetComponent<GoldManager>().SufficientGold(1, carriedObject.GetComponent<UnitManager>().baseDeploymentCost))
        {
            return true;
        }else 
        {
            return false;
        }    
    }

    /// <summary>
    /// Check with the targeted object if it's a "SpawnZone"-tile and further if its available for occupation.
    /// </summary>
    /// <returns></returns>
    bool LegalDeploymentLocation(GameObject _targetedLocation)
    {
        SpawnZone _spawnZone;

        // if the targeted object is not a "SpawnZone" return immediatly:
        if (_targetedLocation.GetComponent<SpawnZone>())
        {
            _spawnZone = _targetedLocation.GetComponent<SpawnZone>();
        }else
        {
            return false;
        }

        // if it is a "SpawnZone", of the correct deployment area and unoccupied, return true:
        if(_spawnZone.player1Zone && !_spawnZone.occupied ) // currently based on the notion of player vs AI
        {
            return true;
        }else
        {
            return false;
        }
    }

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
        GetComponent<GoldManager>().SubtractGold(1, carriedObject.GetComponent<UnitManager>().baseDeploymentCost);

        #region experiment: Auto Launch
        if (FindAnyObjectByType<GameManager>().autoLaunchEnabled)
        {
            carriedObject.GetComponent<UnitManager>().LaunchUnit();

            Debug.Log("shoulda autolaunched now");
        }
        #endregion

        // empty the carried object (nothing is being dragged anymore):
        carriedObject = null;
    }

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
    public void CreateUnit(GameObject _unit)
    {
        // define the rotation to face the right side of the screen (positive x direction):
        Quaternion rightFacingRotation = Quaternion.Euler(0, 90, 0); // Assuming the unit's forward is along the z-axis

        carriedObject = Instantiate(_unit, _mousePosition, rightFacingRotation, _player1UnitParent);
        //carriedObject = Instantiate(_unit, _mousePosition, Quaternion.identity, _player1UnitParent);

        // setup the picked up units profile:
        carriedObject.GetComponent<UnitManager>().InitializeUnit(1);
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
        GetComponent<GoldManager>().AddGold(1, carriedObject.GetComponent<UnitManager>().baseDeploymentCost);
    }
}