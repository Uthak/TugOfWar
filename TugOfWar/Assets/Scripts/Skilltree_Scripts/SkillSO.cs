using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]
public class SkillSO : ScriptableObject
{
    public string _skillName;
    public string _description;
    public Sprite _icon;

    [Header("Skill Components")]
    public EffectOnStatlines effectOnStatlines; // e.g. +2% more attack-speed
    public EffectOnEnvironment effectOnEnvironment; // e.g. +3 rows of deployment area
    public EffectOnGameMechanic effectOnGameMechanic; // e.g. spawn a decoy HQ for the enemy soldiers to be drawn to
}
