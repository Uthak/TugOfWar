using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script manages all the overlaying logic of the game in runtime.
/// </summary>
public class GameManager : MonoBehaviour
{
    // Singleton instance
    //public static GameManager gameManager { get; private set; }

    [Header("Game Manager Setup:")]
    [Tooltip("Upon destruction a fraction of the deployment cost of a unit is rewarded to the victor. Set the " +
        "fraction-factor here. NOTE: destroyed neutral units rewards are not affected by this!")]
    public float goldRewardFactor = 4.0f; // currently NOT used, using the reward tab of the UnitDataSO instead!
    [Tooltip("This is the target objective player 1's units run towards.")]
    public GameObject player1Destination;
    [Tooltip("This is the target objective player 2's units run towards.")]
    public GameObject player2Destination;

    [Space(10)]
    [Tooltip("This should be checked if playing against an AI-opponent.")]
    [SerializeField] bool _playAgainstAI = true;

    // private variables:
    EnemyArmyManager _NPCArmyManager;
    GoldManager _goldManager;
    LevelArchitect _levelArchitect; // manages level creation
    ExperienceManager _experienceManager;
    UpgradeManager _upgradeManager;

    float _player1DeploymentBacklineX;
    float _player2DeploymentBacklineX;

    List<UnitManager> _allUnlaunchedUnits = new List<UnitManager>(); // used for filtering unlaunched units from those launched already
    List<UnitManager> _unlaunchedPlayer1Units = new List<UnitManager>(); // used for launching units of player 1
    List<UnitManager> _unlaunchedPlayer2Units = new List<UnitManager>(); // used for launching units of player 2

    private bool _firstWave = true; // this is used to distinguish the commence of the game, after deployment of the first wave is done

    // for experimentation: Auto Launch & auto deploy!
    public bool autoLaunchEnabled = false;
    public bool autoDeployEnabled = false;

    public void EnableAutoLaunch()
    {
        autoLaunchEnabled = true;
    }

    /// <summary>
    /// The Game Setup commences here.
    /// </summary>
    private void Awake()
    {
        // currently not being used singleton implementation:
        #region Singleton pattern:
        /*
        if (gameManager == null)
        {
            gameManager = this;

            // optional: Makes this instance persist across scenes
            //DontDestroyOnLoad(gameObject);
        }else
        {
            Destroy(gameObject);
            return;
        }*/
        #endregion

        // start game setup:
        _levelArchitect = GetComponent<LevelArchitect>();
        _levelArchitect.BuildLevel();
    }

    void InitializeGameManager()
    {
        // cache components:
        if (GetComponent<EnemyArmyManager>()) // only present in Single Player
        {
            _NPCArmyManager = GetComponent<EnemyArmyManager>();
        }
        _goldManager = GetComponent<GoldManager>();
        //_levelBuilder = GetComponent<LevelBuilder>();

        _experienceManager = GetComponent<ExperienceManager>();
        _experienceManager.InitializeExperienceManager();

        _upgradeManager = GetComponent<UpgradeManager>();
        _upgradeManager.InitializeUpgradeManager();
        // register button events:
        //_goldInfusionButton.onClick.AddListener(() => ButtonForGoldInfusion(_currentGoldInfusionTier, 1,));

        // adaptively cache the respective headquarters:
        player1Destination = _levelArchitect.baseConfig.player1HQ; // this is wrong, but works, the var is falsly assigned!
        player2Destination = _levelArchitect.baseConfig.player2HQ; // this is wrong, but works, the var is falsly assigned!

        //TEMPORARY using hq instead of base line:
        // adaptively cache target x-lie for both players:
        /*
        _player1DeploymentBacklineX = player1Destination.transform.position.x;
        _player2DeploymentBacklineX = player2Destination.transform.position.x;*/
        
        // adaptively cache the respective headquarters:
        player1Destination = _levelArchitect.GetHeadquarter(2);
        player2Destination = _levelArchitect.GetHeadquarter(1);
        

        // adaptively cache target x-lie for both players:
        _player1DeploymentBacklineX = _levelArchitect.GetDeploymentBacklineX(1);
        _player2DeploymentBacklineX = _levelArchitect.GetDeploymentBacklineX(2);
        
    }
    /// <summary>
    /// This function is called by <see cref="LevelArchitect"/> when the map is setup. 
    /// Only once that is complete should the AI start deploying units.
    /// </summary>
    public void LevelSetupComplete()
    {
        InitializeGameManager();

        if (_playAgainstAI)
        {
            _NPCArmyManager.DeployAIStartingArmy();
        }
    }

    public float GetEnemyBaseLine(int _playerID)
    {
        switch (_playerID)
        {
            case 1:
                return _player2DeploymentBacklineX; // this throws error when HQ is destroyed

            case 2:
                return _player1DeploymentBacklineX; // this throws error when HQ is destroyed

            default:
                Debug.LogError("ERROR: A unit-destination was requested, but invalid player-ID given!", this);
                return 0.0f;
        }
    }

    /// <summary>
    /// The <see cref="UnitManager"/> requests this location when entering the enemy deployment zone.
    /// This is to redirect units possibly to far away from it to spot it towards it.
    /// The <see cref="UnitMovement"/> uses this as their ultimate target.
    /// </summary>
    /// <param name="_playerID"></param>
    /// <param name="_destinationTransform"></param>
    public Transform GetEnemyCastleLocation(int _playerID)
    {
        switch (_playerID)
        {
            case 1:
                return player1Destination.transform; // this throws error when HQ is destroyed

            case 2:
                return player2Destination.transform; // this throws error when HQ is destroyed

            default:
                Debug.LogError("ERROR: A unit-destination was requested, but invalid player-ID given!", this);
                return null;
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
            _goldManager.StartGoldFlow();

            // AI will start periodically deploying all its available money into new troops:
            _NPCArmyManager.StartContineousDeployment();

            // launch the first AI-wave on the first press of button:
            LaunchWave(2);
        }

        // Get the required lists of active units:
        CreateListsOfNewUnits();

        // ALL OF THIS SHOULD BE OPTIMIZED BY FILLING LISTS AUTOMATICALLY with placed units!
        // depending on who called this function launch the corresponding wave:
        switch (_playerID)
        {
            case 1: // player 1:
                // reset player 1 deployment zone: (the only human player at this time)
                _levelArchitect.ResetDeploymentZone();

                // loop through the unlaunched units of player 1 and launch them:
                foreach (UnitManager _unit in _unlaunchedPlayer1Units)
                {
                    // beginn movement of all new units:
                    _unit.GetComponent<UnitManager>().LaunchUnit();
                }
                return;

                case 2: // player 2:
                // loop through the unlaunched units of player 2 and launch them:
                foreach (UnitManager _unit in _unlaunchedPlayer2Units)
                {
                    // beginn movement of all new units:
                    _unit.GetComponent<UnitManager>().LaunchUnit();
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
            if (!_unit.isActive && !_unit.isDead)
            {
                _allUnlaunchedUnits.Add(_unit);
            }
        }

        // loop through all new, unlaunched units and filter them by player-ownership:
        foreach (UnitManager _unit in _allUnlaunchedUnits)
        {
            if (_unit.myPlayerAffiliation == 1)
            {
                _unlaunchedPlayer1Units.Add(_unit);
            }
            else if (_unit.myPlayerAffiliation == 2)
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
        _allUnlaunchedUnits.Clear();
    }
    /*
    /// <summary>
    /// This function is called when the player choses to trade ingame-xp for a immediate gold-infusion. 
    /// Gold Button in the UI.
    /// </summary>
    //public void ButtonForGoldInfusion(int tierLevel,int usingPlayerID, float amtOfGold, int xpCost)
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
                }else
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
                }else
                {
                    Debug.Log("Player " + playerID + "Tried to get money fast, but you don't have the required experience!", this);
                }
                break;
        }
    }*/

    /*
    void OnDisable()
    {
        //un-register button events:
        _goldInfusionButton.onClick.RemoveAllListeners();
    }*/
}