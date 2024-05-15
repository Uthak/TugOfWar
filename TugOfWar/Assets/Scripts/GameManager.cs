using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject team1Destination;
    public GameObject team2Destination;
    //[Space(10)]
    //public TextMeshProUGUI moneyDisplay;

    public bool useAIEnemy = true;
    //public int goldCoinsMax = 20;
    //public int usedGoldCoinsPlayer = 0;
    //public int usedGoldCoinsAI = 0;

    List<UnitManager> _newWaveUnits = new List<UnitManager>();
    List<UnitManager> _previousWaveUnits = new List<UnitManager>();
    private bool _firstWave = true;

    /// <summary>
    /// This function is called by <see cref="LevelBuilder"/> when the map is setup. 
    /// Only once that is complete should the AI start deploying units.
    /// </summary>
    public void LevelSetupComplete()
    {
        // AI may deploy its army:
        GetComponent<EnemyArmyManager>().DeployStartingArmy();
    }


    /// <summary>
    /// This is the button-function to start a new wave of troops. Core game-loop.
    /// </summary>
    public void LaunchWave()
    {
        // if this is the first wave of the game set up required actions here:
        if (_firstWave)
        {
            // this will disable the visual markers of deployment tiles of the map:
            //GetComponent<LevelBuilder>().TurnOffAllZoneMarkers(); // temporarily disabled, as this has become redundant

            // both players will now start receiving gold:
            GetComponent<GoldManager>().StartGoldFlow();

            // AI will start periodically deploying all its available money into new troops:
            GetComponent<EnemyArmyManager>().StartContineousDeployment();

            _firstWave = false; 
        }

        // reset the deployment zone:
        GetComponent<LevelBuilder>().ResetDeploymentZone();

        // find all active units in the scene and fill an array with them:
        UnitManager[] _allUnits = FindObjectsOfType<UnitManager>();

        // loop through all units and filter out those who have not yet been launched:
        foreach (UnitManager _unit in _allUnits)
        {
            if (!_unit.wasLaunched)
            {
                _newWaveUnits.Add(_unit);
            }
        }

        // loop through the new units and launch them.
        foreach (UnitManager _unit in _newWaveUnits)
        {
            // tell this unit it has been launched:
            _unit.wasLaunched = true;

            // beginn movement of all new units:
            _unit.GetComponent<UnitMovement>().GameStarted();

            // add all friendly units to the cameras follow-list:
            if (_unit.teamAffiliation == 1)
            {
                GetComponent<CameraController>().AddUnit(_unit.transform);
            }
        }

        // reset list for the next wave:
        _newWaveUnits.Clear();
    }


    // Safeties:
    /*
    // this will now get called over and over, which is not smart
    public void LaunchWave()
    {
        // if this is the first wave of the game do additional things here:
        if (_firstWave)
        {
            UnitManager[] _allUnits = FindObjectsOfType<UnitManager>();
            foreach (UnitManager _unit in _allUnits)
            {
                _unit.GetComponent<UnitMovement>().GameStarted();

                // When the wave is launched also update the cameras list of friendly units to pot. follow:
                if (_unit.teamAffiliation == 1)
                {
                    GetComponent<CameraController>().AddUnit(_unit.transform);
                }
            }
            GetComponent<LevelBuilder>().TurnOffAllZoneMarkers();

            _firstWave = false;
        }else
        {

        }
    }*/

    /*
    public void UpdateMoney(int _goldCoinAmt, bool _isUnitOfPlayer)
    {
        if (_isUnitOfPlayer)
        {
            usedGoldCoinsPlayer += _goldCoinAmt; // += as the value gets transferred as if SUBTRACTED when placed and ADDED when picked up
            moneyDisplay.text = usedGoldCoinsPlayer.ToString() + " / " + goldCoinsMax.ToString();
        }else // AI placed a unit. No need for display. This is only for AI to check if it has money left for more!
        {
            usedGoldCoinsAI += _goldCoinAmt;
            //Debug.Log("AI has spent: " + usedGoldCoinsAI);
        }
    }*/
    /*
    public void LaunchWave()
    {
        UnitMovement[] _allUnits = FindObjectsOfType<UnitMovement>();
        foreach (UnitMovement _unit in _allUnits)
        {
            _unit.GetComponent<UnitMovement>().GameStarted();

            // temporary solution to controll camera:
            FindAnyObjectByType<CameraController>().AddUnit(this.transform);
        }
        GetComponent<LevelBuilder>().TurnOffAllZoneMarkers();

        //GetComponent<EnemyArmyManager>().SpawnEnemyArmy();
    }*/
}
