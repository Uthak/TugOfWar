using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // This is a reference to the corresponding UnitManager attached to this prefab instance
    public UnitManager unitManagerReference;

    public void InitializeHealthBar(UnitManager _unitManager)
    {
        unitManagerReference = _unitManager;
        UnitHealth unitHealthReference = unitManagerReference.unitHealth;

        Slider slider = GetComponentInChildren<Slider>();

        if (slider != null && unitHealthReference != null)
        {
            slider.maxValue = unitHealthReference.baseHealthPoints;
            slider.value = unitHealthReference.baseHealthPoints;
        }
        else
        {
            Debug.LogError("Slider or UnitHealth component not found.");
        }
    }

    /*
    // this is a reference to the corresponding unit attached to this prefab instance:
    public UnitHealth unitHealthReference;

    public void InitializeHealthBar(UnitHealth _unitHealth,float _startingHealth)
    {
        
        unitHealthReference = _unitHealth;
        Slider slider = GetComponentInChildren<Slider>();
        // old:
        //slider.maxValue = unit.GetMaxHealth();
        //slider.value = unit.GetCurrentHealth();

        //new:
        if (slider != null)
        {
            slider.maxValue = unitHealthReference.baseHealthPoints; //_startingHealth;
            slider.value = unitHealthReference.baseHealthPoints; //_startingHealth;
        }
        else
        {
            Debug.LogError("Slider component not found in health bar prefab.");
        }*/
}

    //old :
    /*public UnitHealthBar unitHealthBar;

    public void Initialize(UnitHealthBar _thisUnitHealthbar)
    {
        this.unitHealthBar = _thisUnitHealthbar;
        Slider slider = GetComponentInChildren<Slider>();
        // old:
        //slider.maxValue = unit.GetMaxHealth();
        //slider.value = unit.GetCurrentHealth();

        //new:
        if (slider != null)
        {
            slider.maxValue = _thisUnitHealthbar.GetMaxHealth();
            slider.value = _thisUnitHealthbar.GetCurrentHealth();
        }
        else
        {
            Debug.LogError("Slider component not found in health bar prefab.");
        }
    }*/
//}