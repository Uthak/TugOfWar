using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This script sits on ever object in the scene with a healthpool.
/// </summary>
public class UnitManager : MonoBehaviour
{
    public enum Race { Human, Orc }
    public enum UnitType { Building, Infantry, Cavalry }
    public enum ClassType { DamageDealer, Tank, Sniper } // DamageDealer > Tank, Tank > Sniper, Sniper > DamageDealer

    [Header("Unit Setup:")]
    public string unitName = "default unit";
    public Race race = Race.Human;
    public UnitType unitType = UnitType.Infantry;
    public ClassType classType = ClassType.DamageDealer; // DD > armored, armored > ranged, ranged > DD
    public int myClassID;
    public int playerAffiliation = 0;
    public int deploymentCost = 1;
    
    [Space(10)]
    public float healthPoints = 1.0f;
    public float armorValue = 0.0f;
    public float movementSpeed = 1.0f;
    public float spottingRange = 5.0f;
    
    [Space(10)]
    public float meleeArmorPenetrationRating = 0.0f;
    public float meleeAttackRange = 0.5f;
    public float meleeAttackSpeed = 1.0f;
    public float meleeDamage = 1.0f;
    
    [Space(10)]
    public float rangedArmorPenetrationRating = 0.0f;
    public float rangedAttackRange = 0.0f;
    public float rangedAttackSpeed = 0.0f;
    public float rangedDamage = 0.0f;

    [Space(20)]
    [Header("Do Not Touch!")]
    public GameObject myLocation;
    public bool wasLaunched = false;

    private void Start()
    {
        // check if unit is a headquaters
        if (unitType == UnitType.Building)
        {
            SetupThisUnit(true, this.gameObject);
            GetComponent<UnitHealth>().UpdateUnitHealth();
        }
    }

    /// <summary>
    /// Tell the unit it was placed. int = 1 is the players team, int = 2 is the AI's team.
    /// </summary>
    /// <param name="_teamID"></param>
    public void SetupThisUnit(bool _placedByPlayer, GameObject _spawnZoneOfPlacement)
    {
        // check if unit is a headquaters
        if(unitType != UnitType.Building)
        {
            myLocation = _spawnZoneOfPlacement;

            if (_placedByPlayer)
            {
                playerAffiliation = 1;
            }
            else
            {
                playerAffiliation = 2;
            }

            // this is both for kill-rewards and camera control (for players):
            GetComponent<UnitHealth>().teamAffiliation = playerAffiliation;

            // transform my class type into an ID:
            switch (classType)
            {
                case ClassType.DamageDealer:
                    myClassID = 0;
                    break;

                case ClassType.Tank:
                    myClassID = 1;
                    break;

                case ClassType.Sniper:
                    myClassID = 2;
                    break;
            }

            GetComponent<UnitMovement>().UpdateUnitMovement();
            GetComponent<UnitCombat>().UpdateUnitCombat();
            GetComponent<UnitHealth>().UpdateUnitHealth();
        }
        else
        {
            Debug.Log("I am a castle, I have " + healthPoints + " and of player " + playerAffiliation);
        }
    }

    
    public void ResetThisUnit()
    {
        // check if unit is a headquaters
        if (unitType != UnitType.Building)
        {
            myLocation.GetComponent<SpawnZone>().VacateDeploymentTile();
            myLocation = null;
        }
    }

    /// <summary>
    /// Allow player to pick this placed unit back up.
    /// </summary>
    private void OnMouseDown()
    {
        // check if unit is a headquaters
        if (unitType != UnitType.Building)
        {
            if (FindObjectOfType<DragNDrop>().carriedObject == null)
            {
                FindObjectOfType<DragNDrop>().PickUpAgain(this.gameObject);

                ResetThisUnit();
            }
        }
    }
}
