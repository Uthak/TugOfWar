using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    [SerializeField] string _savePath;
    [SerializeField] string _currentRunSkilltreeSafeFile = "currentRunSkillSelection";
    [SerializeField] string _fileType = ".json";

    private List<SkillSO> _availableSkills;
    private List<SkillSO> _selectedSkills = new List<SkillSO>();


    #region Skill Management:
    // temporarily until calling it in a more sophisticated way:
    void Awake() // should be a controlled initializsation
    {
        LoadCurrentSkilltree();
    }


    public void LoadCurrentSkilltree()
    {
        _savePath = Path.Combine(Application.persistentDataPath, _currentRunSkilltreeSafeFile + _fileType);
        LoadSkills();
    }

    /// <summary>
    /// This gets clalled by the <see cref="SkillButton"/> when a skill was chosen by the player.
    /// Each button is assigned a specific skill-SO to hand over. 
    /// </summary>
    /// <param name="skill"></param>
    public void SelectSkill(SkillSO skill)
    {
        if (!_selectedSkills.Contains(skill))
        {
            _selectedSkills.Add(skill);
            SaveSkills();
        }
    }

    /// <summary>
    /// If the player clicks on a selected skill again (before saving), it gets deselected. 
    /// Each button is assigned a specific skill-SO to hand over. 
    /// </summary>
    /// <param name="skill"></param>
    public void DeselectSkill(SkillSO skill)
    {
        if (_selectedSkills.Contains(skill))
        {
            _selectedSkills.Remove(skill);
            SaveSkills();
        }
    }

    /// <summary>
    /// Write a learnt skill into the safe-file to load on next play-session.
    /// </summary>
    private void SaveSkills()
    {
        List<string> skillNames = new List<string>();
        foreach (var skill in _selectedSkills)
        {
            skillNames.Add(skill._skillName);
        }
        File.WriteAllText(_savePath, JsonUtility.ToJson(new SkillListWrapper(skillNames)));
    }

    /// <summary>
    /// When playing a previous safe-game load the previously selected skills first.
    /// </summary>
    private void LoadSkills()
    {
        if (File.Exists(_savePath))
        {
            string json = File.ReadAllText(_savePath);
            SkillListWrapper skillListWrapper = JsonUtility.FromJson<SkillListWrapper>(json);
            foreach (string skillName in skillListWrapper.skillNames)
            {
                SkillSO skill = _availableSkills.Find(s => s._skillName == skillName);
                if (skill != null)
                {
                    _selectedSkills.Add(skill);
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
#endregion
}