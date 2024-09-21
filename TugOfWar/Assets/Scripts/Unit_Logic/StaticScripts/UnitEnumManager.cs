using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEnumManager
{
    [Tooltip("Currently no effect.")]
    public enum Race { Human, Orc }


    [Tooltip("UnitType influences the combat-effectiveness of units. It interacts with other UnitTypes and ItemTypes.")]
    public enum UnitType { Peasant, Soldier, LightCavalry, HeavyCavalry, Warmachine, Building, Environment }


    [Tooltip("Size influences the distance of knock-back-effects as well as the offset of health bars above. " +
        "For reference: 0 small, 1 humanoid, 2 cavalry, 3 monster, 4 monstrousCavalry, 5 giant (e.g. HQ)")]
    public enum Size { small, humanoid, cavalry, monster, monstrousCavalry, giant }


    [Tooltip("Currently no effect.")]
    public enum ItemGrip { OneHanded, TwoHanded }


    [Tooltip("ItemType influences the combat-effectiveness of units. It interacts with UnitTypes and armor.")]
    public enum ItemType { Unarmed, Tool, Sword, Axe, Mace, Spear, Lance, Bow, Warmachine, MagicalAttack, Shield }


    [Tooltip("The placement-state indicates which colors/materials the active sample unit should have.")]
    public enum SampleUnitMaterial { Default, Valid, Invalid } // default is currently not used.
}
