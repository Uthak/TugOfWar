using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public SkillSO skill;
    private Button button;
    private SkillManager skillManager;

    void Start()
    {
        button = GetComponent<Button>();
        skillManager = FindAnyObjectByType<SkillManager>();
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        skillManager.SelectSkill(skill);
    }
}