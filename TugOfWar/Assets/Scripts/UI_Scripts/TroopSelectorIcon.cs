using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class TroopSelectorIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public GameObject itemToPickUpHere;

    private DragNDrop _dragNDrop;

    private bool mouse_over = false;

    /// <summary>
    /// Search the game to find the the DragNDrop-script.
    /// </summary>
    void OnEnable()
    {
        _dragNDrop = FindObjectOfType<DragNDrop>();
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
        //carriedObject = Instantiate(itemToPickUpHere, _mousePosition, Quaternion.identity);
        //_dragNDrop.carriedObject = itemToPickUpHere;
        //_dragNDrop.carryingSomething = true;
        //Debug.Log(this.gameObject.name + " Was Clicked.");
    }
}
