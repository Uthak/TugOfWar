using UnityEngine;

[CreateAssetMenu(fileName = "[Unit]_DataSheet", menuName = "Unit Data Card", order = 51)]
public class UnitDataSO : ScriptableObject
{
    public enum Race { Human, Orc }
    public enum UnitType { Peasant, Soldier, LightCavalry, HeavyCavalry, Warmachine, Building, Environment }

    [Header("Unit Base Profile:")]
    public string unitName = "[Unit Name]";
    public Race race;
    public UnitType unitType;
    [Tooltip("HQ's and environment objects e.g. cannot be manually placed. For now. " +
        "NOTE: Some buildings, like towers, can be placed and picked up again.")]
    public bool canBeManuallyPlaced = true;
    [Tooltip("Weight class determines how far a unit can get knocked/bashed back and how far it in " +
        "turn would knock/bash the opponent.")]
    public float size = 1.0f; // 0 small, 1 humanoid, 2 cav, 3 monster, 4 monster cav, 5 titan / HQ
    public bool chargeImmunity = false; // "sturdy"!

    // base unit stats:
    public float deploymentCost = 5.0f;
    public float healthPoints = 5.0f;
    public float walkingSpeed = 1.0f;
    public float runningSpeed = 0.0f; // should usually be around 200% of walking:
    public float chargingSpeed = 0.0f;
    public float spottingRange = 5.0f;
    public float alarmRange = 0.0f; // unused...// how far will this unit relay a spotted enemy

    // base combat ability:
    public float toHitSkill = 0.0f; // unused...
    public float toCritSkill = 0.0f; // unused...

    // special stats:
    public float carryCapacity = 0.0f; // how many resources a peasant can carry off the field 

    [Space(10)]
    // advanced unit stats:
    //public float runningSpeed = 0.0f; // should usually be around 200% of walking:
    public float chargeImpactDamage = 0.0f; // unused...
    public float chargeImpactSplashRadius = 0.0f; // unused...
    public float chargeImpactForce = 0.0f; // unused...

    [Header("Reward when destroyed:")]
    [Tooltip("This sum gets rewarded for the player who landed the death blow on this unit.")]
    public float monetaryReward = 0.0f;



    // strength == This could counterbalance additional weight from gear and multiply damage of weapons
    // precision == This could indicate likelyhood to hit in combat and strike critical hits
}
