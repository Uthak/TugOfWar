using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnitEnumManager;

[CreateAssetMenu(fileName = "[Shield]_DataSheet", menuName = "Shield Data Card", order = 51)]

public class ShieldDataSO : ScriptableObject
{
    [Header("Gear Profile:")]
    public string shieldName = "[Shield Name]";
    public ItemType weaponType = ItemType.Shield;

    // item stats:
    [Space(10)]
    public float armorValue = 0.0f; // unused...
    public float deploymentCost = 0.0f; // unused...
    public float reward = 0.0f; // unused...

    [TextArea(3, 10)][SerializeField] string _comment;
}
