using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "[Weapon]_DataSheet", menuName = "Weapon Data Card", order = 51)]
public class WeaponDataSO : ScriptableObject
{
    public enum WeaponGrip { OneHanded, TwoHanded }
    public enum WeaponType { Unarmed, Tool, Sword, Axe, Mace, Spear, Lance, Bow, Warmachine, MagicalAttack }

    [Header("Weapon Profile:")]
    public string weaponName = "[Weapon Name]";
    public WeaponGrip weaponGrip;
    public WeaponType weaponType;
    public bool shieldOffhand = false;

    // base item stats:
    [Tooltip("cost: 1 per point")]
    public float damage = 1.0f;
    
    [Tooltip("cost: start at 10 cost for 1 second speed, then subtract 2 per second longer intervals. " +
        "E.g. a 3 second attack speed costs 6 points")]
    public float attackSpeed = 1.0f;
    
    [Tooltip("cost: 2 per point")]
    public float attackRange = 0.5f;

    [Space(10)]
    // advanced item stats:
    [Tooltip("cost: 1 per point")]
    public float armorPenetrationValue = 0.0f;
    
    public float toHitFactor = 0.0f; // unused...
    public float criticalHitFactor = 0.0f; // unused...

    // splash:
    public float splashRadius = 0.0f; // unused...
    public float splashDamage = 0.0f; // unused...

    // bash:
    public float bashChance = 0.0f; // unused...
    public float bashForceFactor = 0.0f;  // unused...// a massiv club will add bashiness over a brittle twig...

    [Header("Shield Offhand:")]
    [Tooltip("cost: 4 per point")]
    public float armorValue = 0.0f;

    // deployment cost:
    public float deploymentCost = 0.0f;
}
