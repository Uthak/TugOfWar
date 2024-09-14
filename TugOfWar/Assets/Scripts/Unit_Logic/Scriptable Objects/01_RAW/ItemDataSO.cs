using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnitEnumManager;

[CreateAssetMenu(fileName = "[Weapon]_DataSheet", menuName = "Weapon Data Card", order = 51)]
public class ItemDataSO : ScriptableObject
{
    [Header("Item Profile:")]
    public string itemName = "[Weapon Name]";
    public ItemGrip itemGrip; // unused...
    public ItemType itemType; // used to determine attack-efficiency:

    // base item stats:
    [Tooltip("cost: 1 per point")]
    public float damage = 1.0f;
    
    [Tooltip("cost: start at 10 cost for 1 second speed, then subtract 2 per second longer intervals. " +
        "E.g. a 3 second attack speed costs 6 points")]
    public float attackSpeed = 1.0f;
    
    [Tooltip("cost: 2 per point")]
    public float attackRange = 0.5f;

    // advanced item stats:
    [Space(10)]
    [Tooltip("cost: 1 per point")]
    public float armorPenetrationValue = 0.0f;
    
    public float hitChance = 0.0f; // unused...
    public float critChance = 0.0f; // unused...

    // splash:
    public float splashRadius = 0.0f; // unused...
    public float splashDamage = 0.0f; // unused...

    // bash:
    public float bashChance = 0.0f; // unused...
    public float bashForceFactor = 0.0f;  // unused...// a massiv club will add bashiness over a brittle twig...

    // interaction with terrain:
    public bool canAttackTerrain = false; // e.g. tools, certain warmachines, some magic, etc. allow to attack terrain.

    [Header("Shield Offhand:")]
    public bool shielded = false; // unused...
    [Tooltip("cost: 4 per point")]
    public float armorValue = 0.0f; // unused...
    public float deploymentCost = 0.0f; // unused...
    public float reward = 0.0f; // unused...

    [TextArea(3, 10)][SerializeField] string _comment;
}
