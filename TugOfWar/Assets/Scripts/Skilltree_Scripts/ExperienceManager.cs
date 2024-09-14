using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ExperienceManager : MonoBehaviour
{
    [SerializeField] Slider _player1ExperienceBar;
    [SerializeField] TextMeshProUGUI _player1ExperienceCounterDisplay;
    [SerializeField] TextMeshProUGUI _TESTcurrentXP;

    [SerializeField] Slider _player2ExperienceBar;
    [SerializeField] TextMeshProUGUI _player2ExperienceCounterDisplay;

    // xp fragments add up to AvailableExperiencePoints
    private float _currentXPplayer1 = 0.0f;
    private float _currentXPplayer2 = 0.0f;

    // this theshold needs to be met to get the next AvailableExperiencePoint:
    [SerializeField] float _player1XPThreshold = 50.0f;
    [SerializeField] float _pl1ThresholdIcreaseIncrement = 25.0f;
    [SerializeField] float _player2XPThreshold = 50.0f;
    [SerializeField] float _pl2ThresholdIcreaseIncrement = 25.0f;

    // These points can be spent on ingame-upgrades:
    int _pl1AvailableExperiencePoints = 0;
    int _pl2AvailableExperiencePoints = 0;

    /// <summary>
    /// Initializes the ExperienceManager. This is called from the Initialization of <see cref="GameManager"/>.
    /// </summary>
    public void InitializeExperienceManager()
    {
        ResetExperienceBar(1);
        UpdateExperienceBar(1);
        UpdateExperiencePointCounter(1);

        ResetExperienceBar(2);
        UpdateExperienceBar(2);
        UpdateExperiencePointCounter(2);

        _currentXPplayer1 = 0;
        _currentXPplayer2 = 0;
    }

    void ResetExperienceBar(int playerID)
    {
        switch (playerID)
        {
            case 1:
                _player1ExperienceBar.value = 0;
                break;

            case 2:
                if(_player2ExperienceBar != null) // this is only not null if I'm testing!
                {
                    _player2ExperienceBar.value = 0;
                }
                break;
        }
    }
    void UpdateExperienceBar(int playerID)
    {
        //Debug.Log("4");
        switch (playerID)
        {
            case 1:
                _player1ExperienceBar.maxValue = _player1XPThreshold;
                //Debug.Log("5");

                _player1ExperienceBar.value = _currentXPplayer1;
                //Debug.Log("6");

                _TESTcurrentXP.text = _currentXPplayer1.ToString();
                //Debug.Log("7");

                break;

            case 2:
                if(_player2ExperienceBar != null) // this is only not null if I'm testing!
                {
                    _player2ExperienceBar.maxValue = _player2XPThreshold;
                    _player2ExperienceBar.value = _currentXPplayer2;
                }
                break;
        }
    }

    /// <summary>
    /// Check if this player has enough accumulated xp to pay for this upgrade.
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="requiredXP"></param>
    /// <returns></returns>
    public bool SufficientXP(int playerID, int requiredXP)
    {
        switch (playerID)
        {
            case 1:
                if (requiredXP <= _pl1AvailableExperiencePoints)
                {
                    return true;
                }else
                {
                    return false;
                }

            case 2:
                if (requiredXP <= _pl2AvailableExperiencePoints)
                {
                    return true;
                }else
                {
                    return false;
                }
    
            default:
                return false;
        }
    }

    public void GainExperience(int receivingPlayerID, float experiencePoints)
    {
        Debug.Log("1");

        switch (receivingPlayerID)
        {
            case 1:
                _currentXPplayer1 += experiencePoints;

                // if the threshold is met, grant a spendable ExperiencePoint:
                if (_currentXPplayer1 >= _player1XPThreshold)
                {
                    _currentXPplayer1 -= _player1XPThreshold; // this allows to spill over excess xp!
                    _pl1AvailableExperiencePoints ++;

                    // increase the threshold:
                    _player1XPThreshold += _pl1ThresholdIcreaseIncrement;
                    
                    // update xp-counter:
                    UpdateExperiencePointCounter(receivingPlayerID);
                }

                // update xp-bar:
                UpdateExperienceBar(receivingPlayerID);

                //Debug.Log("2");

                break;

            case 2:
                _currentXPplayer2 += experiencePoints;

                // if the threshold is met, grant a spendable ExperiencePoint:
                if (_currentXPplayer2 == _player2XPThreshold)
                {
                    _currentXPplayer2 -= _player2XPThreshold; // this allows to spill over excess xp!
                    _pl2AvailableExperiencePoints++;

                    // increase the threshold:
                    _player2XPThreshold += _pl2ThresholdIcreaseIncrement;

                    // update xp-counter:
                    UpdateExperiencePointCounter(receivingPlayerID);
                }
                //Debug.Log("3");
                // update xp-bar:
                UpdateExperienceBar(receivingPlayerID);

                break;

            default:
                return;
        }
    }

    /// <summary>
    /// This method is called when experience is exchanged for an upgrade.
    /// Callers: ButtonForGoldInfusion inside <see cref="GameManager"/>
    /// </summary>
    /// <param name=""></param>
    public void SpendExperience(int spendingPlayersID, int experiencePointCost) 
    {
        switch (spendingPlayersID)
        {
            case 1:
                _pl1AvailableExperiencePoints -= experiencePointCost;
                break;
            case 2:
                _pl1AvailableExperiencePoints -= experiencePointCost;
                break;
        }

        UpdateExperiencePointCounter(spendingPlayersID);
    }

    void UpdateExperiencePointCounter(int playerID)
    {
        switch (playerID)
        {
            case 1:
                _player1ExperienceCounterDisplay.text = _pl1AvailableExperiencePoints.ToString();
                break;
            case 2:
                if(_player2ExperienceCounterDisplay != null) // this is only not null if I'm testing!
                {
                    _player2ExperienceCounterDisplay.text = _pl2AvailableExperiencePoints.ToString();
                }
                break;
        }
    }
}
