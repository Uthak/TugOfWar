using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfGame : MonoBehaviour
{
    [Header("Game Ending Setup:")]
    [SerializeField] GameObject _ingameGameUI;
    [SerializeField] GameObject _endGameGameUI;

    [Space(10)]
    [SerializeField] GameObject _victoryBackgroundUI;
    [SerializeField] GameObject _victoryBannerUI;

    [Space(10)]
    [SerializeField] GameObject _lossBackgroundUI;
    [SerializeField] GameObject _lossBannerUI;

    [Space(10)]
    [SerializeField] GameObject _progressionFeedbackUI; // optional
    
    [Space(10)]
    [SerializeField] GameObject _thisRoundsStatisticUI;

    [Header("Automatically transition through end screens:")]
    [SerializeField] bool _automaticTransition = false;
    [SerializeField] float _delayBeforeCallingFirstUI = 5.0f;
    [SerializeField] float _delayBeforeAutomaticallyCallingSecondUI = 20.0f;
    [SerializeField] float _delayBeforeAutomaticallyCallingThirdUI = 20.0f;

    // inaccessible variables:
    [Tooltip("This should be set to true, if a button was pressed to get to the next UI. This " +
        "will disable the automatic transition between end-of-round-ui's.")]
    private bool _manualInputDetected = false;
    private bool _victoryAchieved = false;
    GameObject _previousUI;

    private void OnEnable()
    {
        if (EndOfGameUIElementsAssigned())
        {
            Debug.Log("End of round UI's have been assigned!");
        }else
        {
            Debug.LogError("ERROR: End of round UI's haven't been assigend - stop now!");
        }
    }

    /// <summary>
    /// Call this function when a round reaches it's conclusion.
    /// The transmitted playerID is the one of the player who lost the round.
    /// </summary>
    /// <param name="victory"></param>
    //public void RoundFinished(bool victory)
    public void RoundFinished(int _idOfPlayerWhoLost)
    {
        #region What Happens here?
        // 1. play destruction of HQ animation

        // 2. camerea-flight around HQ while it collapses

        // 3. show victory/loss screen

        // 4. optional: show impact of this round on your online stats/game progression

        // 5. on click show the results/stats

        // 6. on click return to main menu
        #endregion

        // check if all required UI elements are assigned:
        if (EndOfGameUIElementsAssigned())
        {
            // cache wheather a victory was achieved, or not:
            if(_idOfPlayerWhoLost == 2)
            {
                _victoryAchieved = true;
            }else
            {
                _victoryAchieved = false;
            }
            //_victoryAchieved = victory;

            StartCoroutine(AutomaticEndOfRoundUIProgression());
        }
        else
        {
            Debug.LogError($"ERROR: At least one UI in { nameof(EndOfGame)} has not been assigned yet. " +
                $"Cannot end game properly.", this);
        }
    }

    public void RegisterManualInput()
    {
        _manualInputDetected = true;
    }
    public void CallEndOfRoundUI(int _uiID)
    {
        // disable whichever UI was on previously, if any:
        if(_previousUI != null)
        {
            StaticUIManager.DeactivateUIElement(_previousUI);
        }

        switch (_uiID)
        {
            case 0:
                StaticUIManager.DeactivateUIElement(_ingameGameUI);
                StaticUIManager.ActivateUIElement(_endGameGameUI);

                if (_victoryAchieved)
                {
                    StaticUIManager.ActivateUIElement(_victoryBannerUI);
                    StaticUIManager.ActivateUIElement(_victoryBackgroundUI);

                    _previousUI = _victoryBannerUI;
                }else
                {
                    StaticUIManager.ActivateUIElement(_lossBannerUI);
                    StaticUIManager.ActivateUIElement(_lossBackgroundUI);

                    _previousUI = _lossBannerUI;
                }
                break;  
                
            case 1:
                StaticUIManager.ActivateUIElement(_progressionFeedbackUI);
                _previousUI = _progressionFeedbackUI;
                break;
            
            case 2:
                StaticUIManager.ActivateUIElement(_thisRoundsStatisticUI);
                _previousUI = _thisRoundsStatisticUI;
                break;

            default:
                Debug.LogError($"ERROR: {nameof(EndOfGame)}: {nameof(CallEndOfRoundUI)} was called with an invalid UI-ID.", this);
                break;
        }
    }

    IEnumerator AutomaticEndOfRoundUIProgression()
    {
        yield return new WaitForSeconds(_delayBeforeCallingFirstUI);

        if (!_manualInputDetected)
        {
            CallEndOfRoundUI(0);
        }

        yield return new WaitForSeconds(_delayBeforeAutomaticallyCallingSecondUI);

        // if the player didn't manually proceed to the next UI, do it for him/her:
        if (!_manualInputDetected && _automaticTransition)
        {
            CallEndOfRoundUI(1);
        }

        yield return new WaitForSeconds(_delayBeforeAutomaticallyCallingThirdUI);

        // if the player didn't manually proceed to the next UI, do it for him/her:
        if (!_manualInputDetected && _automaticTransition)
        {
            CallEndOfRoundUI(2);
        }
    }

    /// <summary>
    /// This bool-function checks, whether all end-of-round-UI's have been assigned in the inspector.
    /// </summary>
    /// <returns></returns>
    private bool EndOfGameUIElementsAssigned()
    {
        if (_victoryBannerUI == null)
        {
            Debug.LogWarning($"{nameof(EndOfGame)}: {nameof(_victoryBannerUI)} is not assigned.", this);
            return false;
        }
        if (_lossBannerUI == null)
        {
            Debug.LogWarning($"{nameof(EndOfGame)}: {nameof(_lossBannerUI)} is not assigned.", this);
            return false;
        }
        if (_victoryBackgroundUI == null)
        {
            Debug.LogWarning($"{nameof(EndOfGame)}: {nameof(_victoryBackgroundUI)} is not assigned.", this);
            return false;
        }
        if (_lossBackgroundUI == null)
        {
            Debug.LogWarning($"{nameof(EndOfGame)}: {nameof(_lossBackgroundUI)} is not assigned.", this);
            return false;
        }
        if (_progressionFeedbackUI == null)
        {
            Debug.LogWarning($"{nameof(EndOfGame)}: {nameof(_progressionFeedbackUI)} is not assigned.", this);
            return false;
        }
        if (_thisRoundsStatisticUI == null)
        {
            Debug.LogWarning($"{nameof(EndOfGame)}: {nameof(_thisRoundsStatisticUI)} is not assigned.", this);
            return false;
        }

        // if everything up to now was true, all UI's have been assigned:
        return true;
    }
}