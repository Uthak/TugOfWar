using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class TroopSelectorIcon : MonoBehaviour
{
    public GameObject unitToPickUpHere; // Reference to the unit prefab this button selects

    public void OnButtonClick()
    {
        //Debug.Log("1");

        // Tell the pool manager to prepare the pool for this unit type
        UnitPoolManager.Instance.RequestPoolForUnit(unitToPickUpHere);
        ///Debug.Log("2");

        // Tell the UnitPlacement system that this unit is selected
        UnitPlacement.unitPlacementSingleton.SelectUnit(unitToPickUpHere);
        //Debug.Log("3");

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("1");

        // Tell the pool manager to prepare the pool for this unit type
        UnitPoolManager.Instance.RequestPoolForUnit(unitToPickUpHere);
        //Debug.Log("2");

        // Tell the UnitPlacement system that this unit is selected
        UnitPlacement.unitPlacementSingleton.SelectUnit(unitToPickUpHere);
        //Debug.Log("3");
    }
}


    /*
     * public class TroopSelectorIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public GameObject itemToPickUpHere;
    public GameObject unitModell; // reference to the modell of this unit, used by the UnitPlacement-class to sample the unit over legal deployment zones

    private DragNDrop _dragNDrop;

    private bool mouse_over = false;

    /// <summary>
    /// Search the game to find the the DragNDrop-script.
    /// </summary>
    void OnEnable()
    {
        _dragNDrop = FindAnyObjectByType<DragNDrop>();
    }

    void Update()
    {
        if (mouse_over)
        {
            //Debug.Log("Mouse Over");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
        //Debug.Log("Mouse enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        //Debug.Log("Mouse exit");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(_dragNDrop.carriedObject == null)
        {
            _dragNDrop.CreateUnit(itemToPickUpHere);
        }
    }
}*/
