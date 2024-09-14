using UnityEngine;
using static UnitEnumManager;

[CreateAssetMenu(fileName = "[Unit]_DataSheet", menuName = "Unit Data Card", order = 51)]
public class UnitDataSO : ScriptableObject
{
    [Header("Unit Base Profile:")]
    public string unitName = "[Unit Name]";
    public Race race; // unused...
    public UnitType unitType;
    public Size size;
    [Tooltip("HQ's and environment objects e.g. cannot be manually placed. For now. " +
        "NOTE: Some buildings, like towers, could potentially be placed and picked up again.")]
    public bool canBePlacedManually = true;

    public ArmorDataSO armorData;
    public ItemDataSO mainHandItemData;
    public ItemDataSO secondaryItemData;

    // general:
    public float deploymentCost = 5.0f;
    public float carryCapacity = 0.0f; // how many resources a peasant can carry // unused...
    [Tooltip("This sum gets rewarded for the player who landed the death blow on this unit.")]
    public float rewardForDestruction = 0.0f; // idea: ca. 25% of deployment:

    public float healthPoints = 5.0f;

    public float walkingSpeed = 1.0f;
    public float runningSpeed = 0.0f; // idea: ca. 200% of walking:
    public float chargingSpeed = 0.0f; // idea: ca. 300% of walking:

    public float detectionRange = 5.0f;
    public float alarmRange = 0.0f; // how far will this unit relay a spotted enemy // unused...

    // base combat ability:
    public float toHitSkill = 0.0f; // unused...
    public float toCritSkill = 0.0f; // unused...

    // advanced unit stats:
    public float chargeImpactDamage = 0.0f; // unused...
    public float chargeImpactSplashRadius = 0.0f; // unused...
    public float chargeImpactForce = 0.0f; // unused...


    // ***
    // strength == This could counterbalance additional weight from gear and multiply damage of weapons
    // precision == This could indicate likelyhood to hit in combat and strike critical hits
}
