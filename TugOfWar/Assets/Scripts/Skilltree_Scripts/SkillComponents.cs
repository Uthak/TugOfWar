using UnityEngine;

[CreateAssetMenu(fileName = "New TargetComponent", menuName = "SkillComponents/TargetComponent")]
public class EffectOnStatlines : ScriptableObject
{
    public enum TargetType { Unit, Environment, Economy }
    public TargetType targetType;
    // Add more target-related properties here
}

[CreateAssetMenu(fileName = "New EffectComponent", menuName = "SkillComponents/EffectComponent")]
public class EffectOnEnvironment : ScriptableObject
{
    public string effectName;
    public float effectValue;
    // Add more effect-related properties here
}

[CreateAssetMenu(fileName = "New RequirementComponent", menuName = "SkillComponents/RequirementComponent")]
public class EffectOnGameMechanic : ScriptableObject
{
    public int requiredLevel;
    public string prerequisiteSkill;
    // Add more requirement-related properties here
}
