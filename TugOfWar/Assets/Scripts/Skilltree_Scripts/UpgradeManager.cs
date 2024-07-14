using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/// <summary>
/// This class manages all ingame upgrades.
/// </summary>
public class UpgradeManager : MonoBehaviour
{
    [Header("Gold-Infusion-Button Settings:")]
    [SerializeField] float _pl1Tier1GoldInfusion = 50.0f;
    [SerializeField] float _pl1Tier2GoldInfusion = 100.0f; // t1*2
    [SerializeField] float _pl1Tier3GoldInfusion = 200.0f; // t2*2
    private int _pl1CurrentGoldInfusionTier = 0;
    [SerializeField] TextMeshProUGUI _pl1GoldTierLevelDisplay;

    [Space(10)]
    [SerializeField] float _pl2Tier1GoldInfusion = 50.0f;
    [SerializeField] float _pl2Tier2GoldInfusion = 100.0f; // t1*2
    [SerializeField] float _pl2Tier3GoldInfusion = 200.0f; // t2*2
    private int _pl2CurrentGoldInfusionTier = 0;

    ExperienceManager _experienceManager;
    GoldManager _goldManager;
    public void InitializeUpgradeManager()
    {
        // cache references:
        _experienceManager = FindAnyObjectByType<ExperienceManager>();
        _goldManager = FindAnyObjectByType<GoldManager>();

        // setup variables:
        _pl1GoldTierLevelDisplay.text = "T " + _pl1CurrentGoldInfusionTier.ToString();
    }

    #region Upgrades:
    public void UpgradeGoldInfusion(int playerID)
    {
        int xpCost = 1;

        switch (playerID)
        {
            case 1:
                if(_pl1CurrentGoldInfusionTier < 2 && _experienceManager.SufficientXP(playerID, xpCost))
                {
                    _experienceManager.SpendExperience(playerID, xpCost);
                    _pl1CurrentGoldInfusionTier++;
                    _pl1GoldTierLevelDisplay.text = "T " + _pl1CurrentGoldInfusionTier.ToString();
                }
                else
                {
                    Debug.Log("Player " + playerID + " tried to upgrade the money-button, but doesn't have the required experience!", this);
                }
                break;

            case 2:
                if (_pl2CurrentGoldInfusionTier < 2 && _experienceManager.SufficientXP(playerID, xpCost))
                {
                    _experienceManager.SpendExperience(playerID, xpCost);
                    _pl2CurrentGoldInfusionTier++;
                }else
                {
                    Debug.Log("Player " + playerID + " tried to upgrade the money-button, but doesn't have the required experience!", this);
                }
                break;
        }
    }
    #endregion

    #region Use Mechanic:
    /// <summary>
    /// This function is called when the player choses to trade ingame-xp for a immediate gold-infusion. 
    /// Gold Button calls this from the UI.
    /// </summary>
    public void ButtonForGoldInfusion(int playerID)
    {
        float amtOfGold;
        int xpCost;

        switch (playerID)
        {
            case 1:
                switch (_pl1CurrentGoldInfusionTier)
                {
                    case 0:
                        amtOfGold = _pl1Tier1GoldInfusion;
                        xpCost = 1;
                        break;
                    case 1:
                        amtOfGold = _pl1Tier2GoldInfusion;
                        xpCost = 1;
                        break;
                    case 2:
                        amtOfGold = _pl1Tier3GoldInfusion;
                        xpCost = 1;
                        break;

                    default:
                        amtOfGold = 0.0f;
                        xpCost = 0;
                        Debug.LogError("ERROR: Gold-infusion-button pressed but invalid tier assigned!", this);
                        return;
                }

                if (_experienceManager.SufficientXP(playerID, xpCost))
                {
                    _experienceManager.SpendExperience(playerID, xpCost);
                    _goldManager.AddGold(playerID, amtOfGold);
                }
                else
                {
                    Debug.Log("Player " + playerID + "Tried to get money fast, but you don't have the required experience!", this);
                }
                break;

            case 2:
                switch (_pl2CurrentGoldInfusionTier)
                {
                    case 0:
                        amtOfGold = _pl2Tier1GoldInfusion;
                        xpCost = 1;
                        break;
                    case 1:
                        amtOfGold = _pl2Tier2GoldInfusion;
                        xpCost = 1;
                        break;
                    case 2:
                        amtOfGold = _pl2Tier3GoldInfusion;
                        xpCost = 1;
                        break;

                    default:
                        amtOfGold = 0.0f;
                        xpCost = 0;
                        Debug.LogError("ERROR: Gold-infusion-button pressed but invalid tier assigned!", this);
                        return;
                }

                if (_experienceManager.SufficientXP(playerID, xpCost))
                {
                    _experienceManager.SpendExperience(playerID, xpCost);
                    _goldManager.AddGold(playerID, amtOfGold);
                }
                else
                {
                    Debug.Log("Player " + playerID + "Tried to get money fast, but you don't have the required experience!", this);
                }
                break;
        }
    }
    #endregion
}

/*
 switch (playerID)
        {
            case 1:
                break;

            case 2:
                break;
        }
 */