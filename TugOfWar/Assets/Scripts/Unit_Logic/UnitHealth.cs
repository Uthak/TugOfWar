using System.Collections;
using System.Collections.Generic;
//using System.Drawing;
using UnityEngine;

/// <summary>
/// Damage to objects is handled here.
/// </summary>
public class UnitHealth : MonoBehaviour
{
    public float baseHealthPoints = 0.0f; // public to communicate with health bar
    public float currentHealthPoints = 0.0f; // public to communicate with health bar
    //public float mySize = 0.0f; // public to update the health bar height to acoount for large creatures/buildings:
    //public Size mySize; // public to update the health bar height to acoount for large creatures/buildings:
    
    //bool _isActive = false;

    bool _isAlive = true;
    float _myArmorValue = 0.0f;
    float _goldRewardFactor = 0.0f;
    float _myRewardValue = 0.0f;

    ArmorDataSO.ArmorType _armorType;

    UnitManager _unitManager;
    UnitAnimationController _myUnitAnimationController;

    //UnitHealthBar _healthBar;

    // to manage this units health bar:
    HealthBarManager _healthBarManager;
    GoldManager _goldManager;
    ExperienceManager _experienceManager;
    GameObject _myHealthBar;


    /// <summary>
    /// Called by the UnitManager to initialize the units health.
    /// </summary>
    public void InitializeUnitHealth()
    {
        // cache component references:
        _unitManager = GetComponent<UnitManager>();
        _goldManager = FindAnyObjectByType<GoldManager>();
        _healthBarManager = FindAnyObjectByType<HealthBarManager>();
        _experienceManager = FindAnyObjectByType<ExperienceManager>();

        // asign & populate variables:
        baseHealthPoints = _unitManager.unitProfile.healthPoints;
        currentHealthPoints = baseHealthPoints;
        _myArmorValue = _unitManager.unitProfile.armorValue;
        _armorType = _unitManager.unitData.armorData.armorType; // currently not used...

        // calculate my value when killed:
        _goldRewardFactor = FindAnyObjectByType<GameManager>().goldRewardFactor; // currently not used!
        _myRewardValue = _unitManager.unitProfile.rewardValue;

        // NOTE: the unit health bar is initialized and managed by hte HealthBarManager,
        // a managing component of the Game Manager.

        _myUnitAnimationController = GetComponent<UnitAnimationController>();
    }

    /// <summary>
    /// This is currently not being used for anything. My idea was to optimize the code by updating 
    /// health bar values from each respective UnitHealth script, instead of an update inside the
    /// HealthBarManager (as values don't change that frequently.
    /// </summary>
    /// <param name="_correspondingHealthBar"></param>
    public void GiveHealthBarToUnitHealthScript(GameObject _correspondingHealthBar)
    {
        _myHealthBar = _correspondingHealthBar;
    }

    public void LaunchHealthBar()
    {
        _healthBarManager.RegisterUnit(_unitManager);
    }

    /// <summary>
    /// Target unit takes damage after considering its armor and the attacks penetration value.
    /// </summary>
    /// <param name="_rawIncomingDamage"></param>
    /// <param name="_penetrationValueOfAttack"></param>
    public void TakeDamage(float _rawIncomingDamage, float _penetrationValueOfAttack, int idOfAttacker)
    {
        // animate damage taken:
        _myUnitAnimationController.TakeDamageAnimation();

        // calculate the armor efficiency to get the effective damage of this attack:
        float _armorEfficiency = CombatCalculator.CalculateArmorEfficiency(_myArmorValue, _penetrationValueOfAttack);
        float _effectiveDamage = _rawIncomingDamage * (1 - _armorEfficiency);

        // subtract the effective damage from current health points:
        currentHealthPoints -= _effectiveDamage;

        if (currentHealthPoints <= 0.0f && _isAlive)
        {
            _isAlive = false;

            StartCoroutine(DeathSequence(idOfAttacker));
        }
    }

    public IEnumerator DeathSequence(int idOfAttacker)
    {
        // shut down all functions:
        //_unitManager.DisableUnfollowAndDisconnectUnit();

        // currently disabled, could be interesting to use for performance-settings! 
        #region Destroy Unit immediatly after Death Animation:

        // use a callback to handle the animation length:
        bool isLengthRetrieved = false;
        float lengthOfDeathAnimation = 0f;

        _myUnitAnimationController.DeathAnimation(length =>
        {
            lengthOfDeathAnimation = length;
            isLengthRetrieved = true;
        });

        // wait until the length is retrieved:
        yield return new WaitUntil(() => isLengthRetrieved);

        // wait for the length of the death animation:
        yield return new WaitForSeconds(lengthOfDeathAnimation);
        #endregion
        

        // reward xp to the killer:
        _experienceManager.GainExperience(idOfAttacker, _unitManager.unitProfile.deploymentCost);

        // reward money to the killer:
        _goldManager.AddGold(idOfAttacker, _myRewardValue);

        // turn off health bar:
        _healthBarManager.UnregisterUnit(_unitManager);

        // shut down all functions:
        _unitManager.DisableUnfollowAndDisconnectUnit();

        yield return new WaitForSeconds(60.0f); // this time should be managed by performance-settings!

        // delete unit entirely, if desired:
        _unitManager.DestroyUnit();
    }
}