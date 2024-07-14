using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GoldManager : MonoBehaviour
{
    [Header("Gold Manager Setup:")]
    // these are kept public for now to be able to read them from anywhere and modify them in runtime as needed:
    public float team1Wallet = 100.0f;
    public float team1GoldPerInterval = 1.0f;
    public float team1Interval = 1.0f;
    public TextMeshProUGUI team1GoldDisplay;
    public TextMeshProUGUI team1GPIDisplay; // GPI = gold per interval


    [Space(10)]
    public float team2Wallet = 100.0f;
    public float team2GoldPerInterval = 1.0f;
    public float team2Interval = 1.0f;
    public TextMeshProUGUI team2GoldDisplay; // this is only for testing if AI working correctly

    // currently NOT using initialization:
    public void InitializeGoldManager()
    {
        // setup the gold manager here...
        // e.g. perhaps skills allow one player more starting capital, higher interval rate or more income etc...
    }

    /// <summary>
    /// Call this when the 1. wave launches. Gold will now be automatically generated at the provided rate.
    /// </summary>
    public void StartGoldFlow()
    {
        StartCoroutine(GenerateTeam1Gold());
        StartCoroutine(GenerateTeam2Gold());
        Debug.Log("Starting gold-generation now.", this);
    }
    IEnumerator GenerateTeam1Gold()
    {
        AddGold(1, team1GoldPerInterval);

        yield return new WaitForSeconds(team1Interval);

        StartCoroutine(GenerateTeam1Gold());
    }
    IEnumerator GenerateTeam2Gold()
    {
        AddGold(2, team2Interval);

        yield return new WaitForSeconds(team2Interval);

        StartCoroutine(GenerateTeam2Gold());
    }

    /// <summary>
    /// This allows to gain various gold amounts at certain points / interactions in the game. 
    /// This is called from within the "DragNDrop"-script when a player refunds a previously placed unit.
    /// </summary>
    /// <param name="amtOfGold"></param>
    public void AddGold(int recipientPlayerID, float amtOfGold)
    {
        switch (recipientPlayerID)
        {
            case 1:
                team1Wallet += amtOfGold;
                break;
        
            case 2:
                team2Wallet += amtOfGold;
                break;
        }

        UpdateGoldDisplay();
    }

    /// <summary>
    /// Placing a unit in your deployment zone will subtract that units costs from your wallet. 
    /// If you have insufficient funds, error feedback will be given.
    /// </summary>
    /// <param name="_goldCost"></param>
    public void SubtractGold(int _playerID, float _goldCost)
    {
        //Debug.Log("player " + _playerID + " is paying to a unit with: " + _goldCost);   
        switch (_playerID)
        {
            case 1:
                if (SufficientGold(_playerID, _goldCost)) // the check for sufficient funds is redundant here, as this is done when trying to deploy a unit.
                {
                    team1Wallet -= _goldCost;
                }else
                {
                    Debug.Log("ERROR: Not enough gold for this unit!");
                    // error feedback to player; e.g. alarm sound and flaring up of money tab....
                }
                break;

            case 2:
                if (SufficientGold(_playerID, _goldCost)) // the check for sufficient funds is redundant here, as this is done when trying to deploy a unit.
                {
                    team2Wallet -= _goldCost;
                }
                else
                {
                    // error feedback to player; e.g. alarm sound and flaring up of money tab....
                }
                break;
        }

        UpdateGoldDisplay();
    }

    /// <summary>
    /// Check if there is enough gold before being able to purchase.
    /// </summary>
    /// <param name="_goldCost"></param>
    /// <returns></returns>
    public bool SufficientGold(int _playerID, float _goldCost)
    {
        switch (_playerID)
        {
            case 1:
                return team1Wallet >= _goldCost;

            case 2:
                return team2Wallet >= _goldCost;

            default: 
                return false;
        }
    }

    /*
    private void LateUpdate()
    {
        team1GoldDisplay.text = "Team 1 Gold: " + team1Wallet.ToString();
        team2GoldDisplay.text = "Team 2 Gold: " + team2Wallet.ToString();
    }*/

    void UpdateGoldDisplay()
    {
        //team1GoldDisplay.text = "Team 1 Gold: " + team1Wallet.ToString();
        team1GoldDisplay.text = team1Wallet.ToString();
        team1GPIDisplay.text = "+" + team1GoldPerInterval.ToString();

        team2GoldDisplay.text = "Team 2 Gold: " + team2Wallet.ToString();
    }
}
