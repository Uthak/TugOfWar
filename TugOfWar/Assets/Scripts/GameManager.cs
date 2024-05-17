using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Manager Setup:")]
    [Tooltip("This is the target objective player 1's units run towards.")]
    public GameObject player1Destination;
    [Tooltip("This is the target objective player 2's units run towards.")]
    public GameObject player2Destination;

    [Space(10)]
    [Tooltip("This should be checked if playing against an AI-opponent.")]
    [SerializeField] bool _playAgainstAI = true;

    // private variables:
    List<UnitManager> _unlaunchedUnits = new List<UnitManager>(); // used for filtering unlaunched units from those launched already

    List<UnitManager> _unlaunchedPlayer1Units = new List<UnitManager>(); // used for launching units of player 1
    List<UnitManager> _unlaunchedPlayer2Units = new List<UnitManager>(); // used for launching units of player 2

    private bool _firstWave = true; // this is used to distinguish the commence of the game, after deployment of the first wave is done


    /// <summary>
    /// This function is called by <see cref="LevelBuilder"/> when the map is setup. 
    /// Only once that is complete should the AI start deploying units.
    /// </summary>
    public void LevelSetupComplete()
    {
        if (_playAgainstAI)
        {
            GetComponent<EnemyArmyManager>().DeployAIStartingArmy();
        }
    }

    /// <summary>
    /// This is the button-function to start a new wave of troops. Core game-loop.
    /// </summary>
    public void LaunchWave(int _playerID)
    {
        // if this is the first wave of the game set up required actions here:
        if (_firstWave)
        {
            // prevent this section from being called twice:
            _firstWave = false;

            // both players will now start receiving gold:
            GetComponent<GoldManager>().StartGoldFlow();

            // AI will start periodically deploying all its available money into new troops:
            GetComponent<EnemyArmyManager>().StartContineousDeployment();

            // Launch the first AI-wave on the first press of button:
            LaunchWave(2);
        }

        // Get the required lists of active units:
        CreateListsOfNewUnits();

        // depending on who called this function launch the corresponding wave:
        switch (_playerID)
        {
            case 1: // player 1:
                // reset the deployment zone:
                GetComponent<LevelBuilder>().ResetDeploymentZone();

                // loop through the unlaunched units of player 1 and launch them:
                foreach (UnitManager _unit in _unlaunchedPlayer1Units)
                {
                    // tell this unit it has been launched:
                    _unit.wasLaunched = true;

                    // beginn movement of all new units:
                    _unit.GetComponent<UnitMovement>().GameStarted();

                    // add all friendly units to the cameras follow-list:
                    GetComponent<CameraController>().AddUnit(_unit.transform);
                }
                return;

                case 2: // player 2:
                // loop through the unlaunched units of player 2 and launch them:
                foreach (UnitManager _unit in _unlaunchedPlayer2Units)
                {
                    // tell this unit it has been launched:
                    _unit.wasLaunched = true;

                    // beginn movement of all new units:
                    _unit.GetComponent<UnitMovement>().GameStarted();
                }
                return;
        }
    }

    /// <summary>
    /// Finds all active units in the scene and then filters out the ones which haven't been launched yet. 
    /// In the next step this list of unlaunched-units gets split by player. These lists are the basis for 
    /// launching the next wave for either player. When called these lists are reset.
    /// </summary>
    void CreateListsOfNewUnits()
    {
        // clean up first: 
        ResetUnitLists();

        // find all active units in the scene and fill an array with them:
        UnitManager[] _allUnits = FindObjectsOfType<UnitManager>();

        // loop through all units and filter out those who have not yet been launched:
        foreach (UnitManager _unit in _allUnits)
        {
            if (!_unit.wasLaunched)
            {
                _unlaunchedUnits.Add(_unit);
            }
        }

        // loop through all new, unlaunched units and filter them by player-ownership:
        foreach (UnitManager _unit in _unlaunchedUnits)
        {
            if (_unit.playerAffiliation == 1)
            {
                _unlaunchedPlayer1Units.Add(_unit);
            }
            else if (_unit.playerAffiliation == 2)
            {
                _unlaunchedPlayer2Units.Add(_unit);
            }
            else
            {
                Debug.LogError("ERROR: Unit with wrong player-affiliation detected!", _unit);
                return;
            }
        }
    }

    /// <summary>
    /// Reset all unit-lists before the next wave is called.
    /// </summary>
    void ResetUnitLists()
    {
        _unlaunchedPlayer1Units.Clear();
        _unlaunchedPlayer2Units.Clear();
        _unlaunchedUnits.Clear();
    }
}
