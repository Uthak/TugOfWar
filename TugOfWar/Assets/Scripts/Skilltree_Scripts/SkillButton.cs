using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public SkillSO _skill;
    private Button _button;
    private SkillManager _skillManager;

    void Start()
    {
        _button = GetComponent<Button>();
        _skillManager = FindAnyObjectByType<SkillManager>();
        _button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        _skillManager.SelectSkill(_skill);
    }
}