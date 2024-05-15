using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UnitHealthBar : MonoBehaviour
{
    Canvas _canvas;
    Slider _healthBar;
    //Image _healthFillingImg;

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
    private void Update()
    {
        if (_gameStarted)
        {
            _currentHealthPoints = GetComponent<UnitHealth>().currentHealthPoints;
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
}
