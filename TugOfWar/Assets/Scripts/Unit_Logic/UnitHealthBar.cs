using UnityEngine;

/// <summary>
/// This script is attached to each unit and manages information to be displayed on its health bar.
/// </summary>
public class UnitHealthBar : MonoBehaviour
{
    /*
    private float _startingHealthPoints;
    private float _currentHealthPoints;
    private HealthBarManager _healthBarManager;
    private UnitManager _unitManager;

    public void InitializeUnitHealthBar()
    {
        // cache components & references:
        _healthBarManager = FindAnyObjectByType<HealthBarManager>();
        _unitManager = GetComponent<UnitManager>();

        // setup variables:
        _startingHealthPoints = _unitManager.baseHealthPoints;
        _currentHealthPoints = _startingHealthPoints;
    }
    /// <summary>
    /// Called from the UnitManager upon being launched. 
    /// </summary>
    public void StartUnitHealthBar()
    {
        _healthBarManager.RegisterUnit(this);
    }

    
    //void Start()
    //{
    //   _healthBarManager = FindObjectOfType<HealthBarManager>();
    //    _healthBarManager.RegisterUnit(this);
    //}

    void OnDestroy()
    {
        _healthBarManager.UnregisterUnit(this);
    }

    //public void InitializeHealthBar(float baseHealthPoints)
    //{
    //    _startingHealthPoints = baseHealthPoints;
    //    _currentHealthPoints = _startingHealthPoints;
    //}

    public void UpdateUnitHealthBar(float _currentHealthPoints)
    {
        this._currentHealthPoints = _currentHealthPoints;
    }

    public float GetMaxHealth()
    {
        return _startingHealthPoints;
    }

    public float GetCurrentHealth()
    {
        return _currentHealthPoints;
    }

    // what does this even do?
    /*public float GetHealthFraction()
    {
        return _currentHealthPoints / _startingHealthPoints;
    }*/
}

// This should work, but isnt very elegant!
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHealthBar : MonoBehaviour
{
    Canvas _canvas;
    Slider _healthBar;
    UnitManager _unitManager;

    float _startingHealthPoints = 0;
    float _currentHealthPoints = 0;

    bool _gameStarted = false;

    public void InitializeHealthBar(float _baseHealthPoints)
    {
        // cache components:
        _canvas = transform.GetComponentInChildren<Canvas>();
        _healthBar = _canvas.GetComponentInChildren<Slider>();
        _unitManager = GetComponent<UnitManager>();

        // set up variables:
        _startingHealthPoints = _baseHealthPoints;
        _healthBar.maxValue = _startingHealthPoints;
    }

    /// <summary>
    /// Call this function to update this units health-bar.
    /// </summary>
    /// <param name="_currentHealthpoints"></param>
    public void UpdateUnitHealthBar(float _currentHealthpoints)
    {
        _currentHealthPoints = _currentHealthpoints;
        _healthBar.value = _currentHealthPoints;

        _gameStarted = true;
    }

    // this update could also be updated in "TakeDamage" and may save performance.
    private void LateUpdate()
    {
        if (_unitManager.wasLaunched) 
        {
            _canvas.transform.LookAt(transform.position + Camera.main.transform.rotation * -Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }
    }
}*/



/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UnitHealthBar : MonoBehaviour
{
    Canvas _canvas;
    Slider _healthBar;
    float _startingHealthPoints = 0;  
    float _currentHealthPoints = 0;

    bool _gameStarted = false;

    public void UpdateUnitHealthBar()
    {
        _canvas = transform.GetComponentInChildren<Canvas>();
        _healthBar = _canvas.GetComponentInChildren<Slider>();
        //_healthFillingImg = _canvas.GetComponentInChildren<Image>();

        _startingHealthPoints = GetComponent<UnitManager>().healthPoints;
        _healthBar.maxValue = _startingHealthPoints;

        _gameStarted = true;
    }
    // disabled for testing *************************************
    private void Update()
    {
        if (_gameStarted)
        {
            _currentHealthPoints = GetComponent<UnitHealth>()._currentHealthPoints;
            _healthBar.value = _currentHealthPoints;
        }
    }

    // this update could also be updated in "TakeDamage" and may save performance.
    private void LateUpdate()
    {
        if(_gameStarted)
        {
            _canvas.transform.LookAt(transform.position + Camera.main.transform.rotation * -Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }
    }
}*/
