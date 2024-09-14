using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "[Armor]_DataSheet", menuName = "Armor Data Card", order = 51)]
public class ArmorDataSO : ScriptableObject
{
    public enum ArmorType { Unarmored, LeatherArmor, LightArmor, HeavyArmor, PlateArmor, Building, Magic }

    [Header("Armor Profile:")]
    //public string armorName = "[Armor Name]"; // unused...
    public ArmorType armorType;

    // base item stats:
    public float armorValue = 0.0f; // unused
    //public float armorWeight = 0.0f; // unused...

    // deployment cost:
    [Tooltip("avg. cost: 4 per point")]
    public float deploymentCost = 0.0f; // unused
}
