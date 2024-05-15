using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNDrop : MonoBehaviour
{
    [SerializeField] float _offsetOnMap = 1.0f;
    [SerializeField] float _offsetOffMap = 2.0f; 

    public GameObject carriedObject;

    private GameObject _targetedGameObject;
    Vector3 _mousePosition;

    [SerializeField] LayerMask _UI_layerMask;
    List<SpawnZone> _usedSpawnZones = new List<SpawnZone>();

    private void Update()
    {
        // cast a ray from the camera to the mouse position:
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity,_UI_layerMask))
        {
            _targetedGameObject = hit.collider.gameObject;

            if (hit.collider.CompareTag("Ground"))
            {
                _mousePosition = new Vector3(hit.point.x, hit.point.y + _offsetOnMap, hit.point.z);
            }/*else if (hit.collider.CompareTag("Surroundings")) // why??? is this to hover over terrain?
            {
                _mousePosition = new Vector3(hit.point.x, hit.point.y + _offsetOffMap, hit.point.z);
            }*/

            // snap mouse (and carried object) to deployment-grid:
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
            if(LegalTargetLocation() && EnoughRessources())
            {
                PlaceUnit();
            }else
            {
                DestroyItem();
            }
        }
    }

    /// <summary>
    /// Check with the "GoldManager"-script if enough resources are available for this unit.
    /// </summary>
    /// <returns></returns>
    bool EnoughRessources()
    {
        if (GetComponent<GoldManager>().SufficientGold(1, carriedObject.GetComponent<UnitManager>().deploymentCost))
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
    bool LegalTargetLocation()
    {
        if(_targetedGameObject.GetComponent<SpawnZone>() && !_targetedGameObject.GetComponent<SpawnZone>().occupied)
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
    void PlaceUnit()
    {
        // pay for the unit at hand:
        GetComponent<GoldManager>().SubtractGold(1, carriedObject.GetComponent<UnitManager>().deploymentCost);

        // tell the targeted deployment-tile that it is now occupied:
        _targetedGameObject.GetComponent<SpawnZone>().OccupyDeploymentTile();

        // place the carried object at the center of the chosen tile:
        carriedObject.transform.position = _mousePosition;

        // setup the placed unit after placing it. You can still revert this by picking it up again:
        carriedObject.GetComponent<UnitManager>().SetupThisUnit(true, _targetedGameObject);

        // empty the carried object (nothing is being dragged anymore):
        carriedObject = null;
    }
    /*
    void PlaceItem()
    {
        _targetedGameObject.GetComponent<SpawnZone>().ItemWasPlaced();
        carriedObject.transform.position = _mousePosition;
        
        if (carriedObject.GetComponent<UnitManager>())
        {
            carriedObject.GetComponent<UnitManager>().UnitWasPlaced(true, _targetedGameObject);
        }
        carriedObject = null;
    }*/

    void DestroyItem()
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
        carriedObject = Instantiate(_unit, _mousePosition, Quaternion.identity);

        /*
        if(GetComponent<GameManager>().usedGoldCoinsPlayer < GetComponent<GameManager>().goldCoinsMax)
        {
            carriedObject = Instantiate(_item, _mousePosition, Quaternion.identity);
        }*/
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
        GetComponent<GoldManager>().AddGold(1, carriedObject.GetComponent<UnitManager>().deploymentCost);

        /*
        if (carriedObject.GetComponent<UnitManager>()) // why?
        {
            carriedObject.GetComponent<UnitManager>().myLocation.GetComponent<SpawnZone>().VacateDeploymentTile();
            carriedObject.GetComponent<UnitManager>().UnitWasPickedUp();
        }*/
    }
}