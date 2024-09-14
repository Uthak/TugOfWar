using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using static UnitEnumManager;


/// <summary>
/// This script manages all health bars in the level.
/// </summary>
public class HealthBarManager : MonoBehaviour
{
    [Header("Health Bar UI Setup:")]
    [SerializeField] float _smallUnitHeight = 1.5f; // size: 0
    [SerializeField] float _mediumUnitHeight = 2.5f; // size: 1
    [SerializeField] float _largeUnitHeight = 3.5f; // size: 2
    [SerializeField] float _veryLargeUnitHeight = 4.5f; // size: 3
    [SerializeField] float _extremelyLargeUnitHeight = 5.5f; // size: 4
    [SerializeField] float _titanicUnitHeight = 6.5f; // size: 5

    [Space(10)]
    public GameObject healthBarPrefab; // assign the prefab Slider

    [Space(10)]
    public Transform unitHealthBarsParent; // assign the "UnitHealthBars" GameObject here

    [Space(10)]
    [SerializeField] bool _debugLogs = false;

    // private variables:
    private Camera _mainCamera;
    private List<UnitManager> _listOfUnitManagerComponents = new List<UnitManager>();
    private List<GameObject> _activeUnitHealthbars = new List<GameObject>();

    void OnEnable()
    {
        StartCoroutine(InitializeCamera());
    }

    IEnumerator InitializeCamera()
    {
        yield return null; // Wait one frame to ensure all objects are initialized
        _mainCamera = Camera.main;

        if (_mainCamera == null)
        {
            Debug.LogError("Camera not found. Make sure there is a Main Camera in the scene.", this);
        }
    }

    // keep all health bars facing the camera:
    void Update()
    {
        if (_mainCamera == null) return;

        foreach (UnitManager _unitManager in _listOfUnitManagerComponents)
        {
            if (_unitManager != null && _unitManager.gameObject.activeInHierarchy)
            {
                GameObject healthBar = GetHealthBarForUnit(_unitManager);

                if (healthBar != null)
                {
                    UpdateHealthBarPosition(_unitManager, healthBar);
                    UpdateHealthBarValue(_unitManager, healthBar); // IMPROVEMENT: call this from each UnitHealth
                }
            }
        }

        // rotate health bars to face the camera:
        foreach (GameObject healthBar in _activeUnitHealthbars)
        {
            healthBar.transform.LookAt(_mainCamera.transform);
            healthBar.transform.rotation = Quaternion.LookRotation(_mainCamera.transform.forward);
        }
    }

    private GameObject GetHealthBarForUnit(UnitManager _unitManager)
    {
        if (_debugLogs)
        {
            Debug.Log($"Finding health bar for unit: {_unitManager.gameObject.name}");
        }

        GameObject healthBar = _activeUnitHealthbars.Find(hb => hb.GetComponent<HealthBar>().unitManagerReference == _unitManager);

        if (healthBar == null)
        {
            if (_debugLogs)
            {
                Debug.Log($"Creating new health bar for unit: {_unitManager.gameObject.name}");
            }

            healthBar = Instantiate(healthBarPrefab, unitHealthBarsParent);

            // tell the respective UnitHealth-component, which is it's health bar:
            _unitManager.unitHealth.GiveHealthBarToUnitHealthScript(healthBar); // currently not being used!

            var healthBarComponent = healthBar.GetComponent<HealthBar>();
            if (healthBarComponent != null)
            {
                healthBarComponent.InitializeHealthBar(_unitManager);
                _activeUnitHealthbars.Add(healthBar);
            }else
            {
                Debug.LogError("HealthBar component not found on healthBarPrefab.");
                Destroy(healthBar);
                return null;
            }

            switch (_unitManager.GetComponent<UnitManager>().unitProfile.size)
            {
                case Size.small:
                    break;

                case Size.humanoid:
                    break;

                case Size.cavalry:
                    break;

                case Size.monster:
                    break;

                case Size.monstrousCavalry:
                    break;

                case Size.giant:
                    healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 45);
                    break;
            }
        }
        return healthBar;
    }

    private void UpdateHealthBarPosition(UnitManager _unitManager, GameObject _healthBar)
    {
        float _healthBarHeight = 0.0f;

        // adjust each health bars floating height to it's respective unit:
        switch (_unitManager.unitProfile.unitData.size)
        {
            case Size.small:
                _healthBarHeight = _smallUnitHeight;
                break;

            case Size.humanoid:
                _healthBarHeight = _mediumUnitHeight;
                break;

            case Size.cavalry:
                _healthBarHeight = _largeUnitHeight;
                break;

            case Size.monster:
                _healthBarHeight = _veryLargeUnitHeight;
                break;

            case Size.monstrousCavalry:
                _healthBarHeight = _extremelyLargeUnitHeight;
                break;

            case Size.giant:
                _healthBarHeight = _titanicUnitHeight;
                break;

            default:
                Debug.LogError("ERROR: Unit's size not listed!", _healthBar.gameObject);
                break;
        }

        Vector3 worldPosition = _unitManager.transform.position + Vector3.up * _healthBarHeight; // Adjust the height as needed
        Vector3 screenPosition = _mainCamera.WorldToScreenPoint(worldPosition);

        RectTransform healthBarRectTransform = _healthBar.GetComponent<RectTransform>();
        if (healthBarRectTransform != null)
        {
            // Convert screen position to canvas local position
            Vector2 localPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(unitHealthBarsParent as RectTransform, screenPosition, _mainCamera, out localPosition);
            healthBarRectTransform.localPosition = localPosition;
        }else
        {
            Debug.LogError("ERROR: RectTransform not found on health bar.");
        }
    }

    // instead of calling this in an update, being called by the respective UnitHealth-script may be better...
    private void UpdateHealthBarValue(UnitManager _unitManager, GameObject healthBar)
    {
        Slider slider = healthBar.GetComponentInChildren<Slider>();

        if (slider != null)
        {
            slider.value = _unitManager.unitHealth.currentHealthPoints;
        }else
        {
            Debug.LogError("ERROR: Slider component not found on health bar.");
        }
    }


    public void RegisterUnit(UnitManager _unitManager)
    {
        if (_debugLogs)
        {
            Debug.Log($"Registering unit: {_unitManager.gameObject.name}");
        }

        _listOfUnitManagerComponents.Add(_unitManager);
    }

    public void UnregisterUnit(UnitManager _unitManager)
    {
        if (_debugLogs)
        {
            Debug.Log($"Unregistering unit: {_unitManager.gameObject.name}");
        }

        _listOfUnitManagerComponents.Remove(_unitManager);
        
        GameObject _healthBar = _activeUnitHealthbars.Find(hb => hb.GetComponent<HealthBar>().unitManagerReference == _unitManager);

        if (_healthBar != null)
        {
            _activeUnitHealthbars.Remove(_healthBar);
            Destroy(_healthBar);
        }
    }
}


/*using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    public GameObject healthBarPrefab; // Assign the prefab with a Canvas and Slider
    public Transform unitHealthBarsParent; // Assign the "UnitHealthBars" GameObject here

    private Camera _mainCamera;
    private List<UnitHealthBar> _listOfUnitHealthBars = new List<UnitHealthBar>();
    private List<GameObject> _activeUnitHealthBars = new List<GameObject>();

    void Start()
    {
        _mainCamera = Camera.main;

        //Debug.Log("cam pos " + mainCamera.transform.position + " and the rotation " + mainCamera.transform.rotation);
    }

    void Update()
    {
        foreach (UnitHealthBar _unitHealthBar in _listOfUnitHealthBars)
        {
            if (_unitHealthBar != null && _unitHealthBar.gameObject.activeInHierarchy)
            {
                GameObject healthBar = GetHealthBarForUnit(_unitHealthBar);
                
                if (healthBar != null)
                {
                    UpdateHealthBarPosition(_unitHealthBar, healthBar);
                    UpdateHealthBarValue(_unitHealthBar, healthBar);
                }
            }
            // old:
            //if (_unitHealthBar != null && _unitHealthBar.gameObject.activeInHierarchy)
            //{
            //    GameObject healthBar = GetHealthBarForUnit(_unitHealthBar);
            //    UpdateHealthBarPosition(_unitHealthBar, healthBar);
            //    UpdateHealthBarValue(_unitHealthBar, healthBar);
            //}
        }

        // Rotate health bars to face the camera
        foreach (GameObject healthBar in _activeUnitHealthBars)
        {
            // old:
            //healthBar.transform.LookAt(_mainCamera.transform.position);
            //new:
            healthBar.transform.LookAt(_mainCamera.transform);

            healthBar.transform.rotation = Quaternion.LookRotation(_mainCamera.transform.forward);

            //healthBar.transform.LookAt(transform.position + Camera.main.transform.rotation * -Vector3.forward, Camera.main.transform.rotation * Vector3.up);

            // no more -Vector3.forward test:
            //healthBar.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);

        }
    }

    private GameObject GetHealthBarForUnit(UnitHealthBar unit)
    {
        Debug.Log($"Finding health bar for unit: {unit.gameObject.name}");

        GameObject healthBar = _activeUnitHealthBars.Find(hb => hb.GetComponent<HealthBar>().unitHealthBar == unit);
        
        if (healthBar == null)
        {
            Debug.Log($"Creating new health bar for unit: {unit.gameObject.name}");

            healthBar = Instantiate(healthBarPrefab, unitHealthBarsParent);
            // old
            //healthBar.GetComponent<HealthBar>().Initialize(unit);
            //_activeUnitHealthBars.Add(healthBar);

            // new:
            var healthBarComponent = healthBar.GetComponent<HealthBar>();
            if (healthBarComponent != null)
            {
                healthBarComponent.Initialize(unit);
                _activeUnitHealthBars.Add(healthBar);
            }
            else
            {
                Debug.LogError("HealthBar component not found on healthBarPrefab.");
                Destroy(healthBar);
                return null;
            } // end new
        }
        return healthBar;
    }

    private void UpdateHealthBarPosition(UnitHealthBar unit, GameObject healthBar)
    {
        Vector3 worldPosition = unit.transform.position + Vector3.up * 2.5f; // Adjust the height as needed
        Vector3 screenPosition = _mainCamera.WorldToScreenPoint(worldPosition);
        //old:
        //healthBar.transform.position = screenPosition;
        //old 2:
        //healthBar.GetComponent<RectTransform>().position = screenPosition;
        
        //new:
        RectTransform healthBarRectTransform = healthBar.GetComponent<RectTransform>();
        if (healthBarRectTransform != null)
        {
            // Convert screen position to canvas local position
            Vector2 localPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                unitHealthBarsParent as RectTransform, screenPosition, _mainCamera, out localPosition);
            healthBarRectTransform.localPosition = localPosition;
        }
        else
        {
            Debug.LogError("RectTransform not found on health bar.");
        }

        // old version:
        //Vector3 screenPosition = mainCamera.WorldToScreenPoint(unit.transform.position + Vector3.up * 2); // Adjust as needed
        //healthBar.transform.position = screenPosition;

        // my attempts:
        //Vector3 screenPosition = mainCamera.ScreenToWorldPoint(unit.transform.position + Vector3.up * 2); // Adjust as needed
        //healthBar.transform.TransformDirection(screenPosition);
    }

    private void UpdateHealthBarValue(UnitHealthBar unit, GameObject healthBar)
    {
        //old:
        //Slider slider = healthBar.GetComponentInChildren<Slider>();
        //slider.value = unit.GetHealthFraction();

        //new:
        Slider slider = healthBar.GetComponentInChildren<Slider>();
        if (slider != null)
        {
            // super weird approach:
            //slider.value = unit.GetHealthFraction();

            //new:
            slider.value = unit.GetCurrentHealth();
        }
        else
        {
            Debug.LogError("Slider component not found on health bar.");
        }
    }

    public void RegisterUnit(UnitHealthBar unit)
    {
        Debug.Log($"Registering unit: {unit.gameObject.name}");

        _listOfUnitHealthBars.Add(unit);
    }

    public void UnregisterUnit(UnitHealthBar unit)
    {
        Debug.Log($"Unregistering unit: {unit.gameObject.name}");

        _listOfUnitHealthBars.Remove(unit);
        GameObject healthBar = _activeUnitHealthBars.Find(hb => hb.GetComponent<HealthBar>().unitHealthBar == unit);
        
        if (healthBar != null)
        {
            _activeUnitHealthBars.Remove(healthBar);
            Destroy(healthBar);
        }
    }
}*/