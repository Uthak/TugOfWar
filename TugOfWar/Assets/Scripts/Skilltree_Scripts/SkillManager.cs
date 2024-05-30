using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public List<SkillSO> availableSkills;
    private List<SkillSO> selectedSkills = new List<SkillSO>();
    private string savePath;

    void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "selectedSkills.json");
        LoadSkills();
    }

    public void SelectSkill(SkillSO skill)
    {
        if (!selectedSkills.Contains(skill))
        {
            selectedSkills.Add(skill);
            SaveSkills();
        }
    }

    public void DeselectSkill(SkillSO skill)
    {
        if (selectedSkills.Contains(skill))
        {
            selectedSkills.Remove(skill);
            SaveSkills();
        }
    }

    private void SaveSkills()
    {
        List<string> skillNames = new List<string>();
        foreach (var skill in selectedSkills)
        {
            skillNames.Add(skill.skillName);
        }
        File.WriteAllText(savePath, JsonUtility.ToJson(new SkillListWrapper(skillNames)));
    }

    private void LoadSkills()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SkillListWrapper skillListWrapper = JsonUtility.FromJson<SkillListWrapper>(json);
            foreach (string skillName in skillListWrapper.skillNames)
            {
                SkillSO skill = availableSkills.Find(s => s.skillName == skillName);
                if (skill != null)
                {
                    selectedSkills.Add(skill);
                }
            }
        }
    }

    [System.Serializable]
    private class SkillListWrapper
    {
        public List<string> skillNames;
        public SkillListWrapper(List<string> skillNames)
        {
            this.skillNames = skillNames;
        }
    }
}