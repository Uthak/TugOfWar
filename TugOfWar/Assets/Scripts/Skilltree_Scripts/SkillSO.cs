using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skilltree")]
public class SkillSO : ScriptableObject
{
    public string skillName;
    public string description;
    public Sprite icon;
    //public int requiredLevel;
}
