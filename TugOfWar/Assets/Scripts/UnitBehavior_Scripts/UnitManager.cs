using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class UnitManager : MonoBehaviour
{
    public enum Race { Human, Elf, Orc }
    public enum UnitType { Infantry, Cavalry, Monstreous }
    //public enum AttackType { Melee, Ranged } // this is to make ranged units attack from afar
    public enum ClassType { DamageDealer, Tank, Sniper } // DamageDealer > Tank, Tank > Sniper, Sniper > DamageDealer


    [Header("Unit Profile")]
    public string unitName = "default unit";
    // faction enum?
    public Race race = Race.Human;
    public UnitType unitType = UnitType.Infantry;
    //public AttackType attackType = AttackType.Melee;
    public ClassType classType = ClassType.DamageDealer; // DD > armored, armored > ranged, ranged > DD
    public int myClassID;
    public int teamAffiliation = 0; // 0 player, 1 enemy
    //public int unitTier = 0;
    public int deploymentCost = 1;
    [Space(10)]
    [Header("Unit Stats:")]
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
    //public bool isRangedUnit = false;
    public float rangedArmorPenetrationRating = 0.0f;
    public float rangedAttackRange = 0.0f;
    public float rangedAttackSpeed = 0.0f;
    public float rangedDamage = 0.0f;

    [Space(20)]
    [Header("Do Not Touch!")]
    public GameObject myLocation;
    public bool wasLaunched = false;

    /// <summary>
    /// Tell the unit it was placed. int = 1 is the players team, int = 2 is the AI's team.
    /// </summary>
    /// <param name="_teamID"></param>
    public void SetupThisUnit(bool _placedByPlayer, GameObject _spawnZoneOfPlacement)
    {
        myLocation = _spawnZoneOfPlacement;

        if (_placedByPlayer)
        {
            teamAffiliation = 1;
        }
        else
        {
            teamAffiliation = 2;
        }

        // this is both for kill-rewards and camera control (for players):
        GetComponent<UnitHealth>().teamAffiliation = teamAffiliation;

        // transform my class type into an ID:
        switch (classType)
        {
            case ClassType.DamageDealer:
                myClassID = 0;
                break;

            case ClassType.Tank:
                myClassID= 1;
                break;

            case ClassType.Sniper:
                myClassID = 2;
                break;
        }

        //PayForThisUnit();

        GetComponent<UnitMovement>().UpdateUnitMovement();
        GetComponent<UnitCombat>().UpdateUnitCombat();
        GetComponent<UnitHealth>().UpdateUnitHealth();
    }

    
    public void ResetThisUnit()
    {
        //GetMoneyBack();
        myLocation.GetComponent<SpawnZone>().VacateDeploymentTile();
        myLocation = null;
    }

    /*
    void PayForThisUnit()
    {
        if(teamAffiliation == 1)
        {
            FindObjectOfType<GameManager>().UpdateMoney(deploymentCost, true);
        }else
        {
            FindObjectOfType<GameManager>().UpdateMoney(deploymentCost, false);
        }
    }

    void GetMoneyBack()
    {
        FindObjectOfType<GameManager>().UpdateMoney(-deploymentCost, true); // no false variant needed, as AI cannot use this function!
    }*/

    /*
    // TO TEST ENCOUNTERS:
    void Start()
    {
        UnitWasPlaced(false, this.gameObject);
    }*/

    /// <summary>
    /// Allow player to pick this placed unit back up.
    /// </summary>
    private void OnMouseDown()
    {
        if(FindObjectOfType<DragNDrop>().carriedObject == null)
        {
            FindObjectOfType<DragNDrop>().PickUpAgain(this.gameObject);

            ResetThisUnit();
        }
    }
}
