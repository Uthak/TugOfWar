using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "[Armor]_DataSheet", menuName = "Armor Data Card", order = 51)]
public class ArmorDataSO : ScriptableObject
{
    public enum ArmorType { Unarmored, LightArmor, MediumArmor, HeavyArmor, PlateArmor, Building, Magic }

    [Header("Armor Profile:")]
    public string armorName = "[Armor Name]"; // unused...
    public ArmorType armorType;

    // base item stats:
    [Tooltip("cost: 4 per point")]
    public float armorValue = 0.0f;
    //public float armorWeight = 0.0f; // unused...

    // deployment cost:
    public float deploymentCost = 0.0f;
}
