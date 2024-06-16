using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DisplayFPS : MonoBehaviour
{
    [Header("FPS Display Settings:")]
    [Tooltip("If checked, FPS will be tracked for the duration of the game running in \"Play\"-mode.")]
    [SerializeField] bool _automaticallyTrackFPS = true;

    [Tooltip("To avoid fluctuating update rates etc we're using a set value to check for fps.")]
    [SerializeField] float _fpsCheckInterval = 0.1f;

    [Tooltip("If the FPS drops to, or below this threshhold you will get a warning.")]
    [SerializeField] float _fpsThreshold = 60.0f;

    [Tooltip("Reference to the TextMeshPro text field.")]
    [SerializeField] TMP_Text _fpsText;

    [Tooltip("Reference to the TextMeshPro text field.")]
    [SerializeField] TMP_Text _avgFpsText;

    // inaccessible variables:
    private bool _trackFPS = false;
    private float _deltaTime;
    private int _controlCount;
    private int _fpsDropCount;

    // Variables for average FPS calculation
    private float _fpsSum = 0.0f;
    private int _fpsSampleCount = 0;


    // start tracking FPS automatically:
    private void Start()
    {
        if(_fpsText != null) // check if a display is assigned:
        {
            if (_automaticallyTrackFPS) 
            {
                // this is required for the Update method of tracking fps:
                StartFPSTracking();

                // use coroutine instead:
                StartCoroutine(CheckFPS());
            }else // if automatic tracking is disabled give warning to manually call tracking:
            {
                Debug.LogWarning("The \"DisplayFPS\"-script is currently not automatically tracking the FPS. " +
                    "Either enable automatic tracking or call the \"StartFPSTracking()\"-function.", this);
            }
        }else
        {
            Debug.LogError("ERROR: No FPS-display assigned!", this);
        }
    }

    /// <summary>
    /// This allows you to start tracking FPS at a certain point. Simply call from anywhere.
    /// </summary>
    public void StartFPSTracking()
    {
        _trackFPS = true;
    }

    /// <summary>
    /// This allows you to stop tracking FPOS at a certain point. Simply call from anywhere.
    /// </summary>
    public void StopFPSTracking()
    {
        _trackFPS = false;
    }

    IEnumerator CheckFPS()
    {
        while (_trackFPS)
        {
            // track every update to later calculate the % of FPS-drops:
            _controlCount++;

            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
            float fps = 1.0f / _deltaTime;
            _fpsText.text = string.Format("FPS: {0:0.}", fps);

            // track any update where the FPS drops below threshold:
            if (fps <= _fpsThreshold)
            {
                _fpsDropCount++;
            }

            // update FPS sum and sample count for average FPS calculation:
            _fpsSum += fps;
            _fpsSampleCount++;
            float avgFps = _fpsSum / _fpsSampleCount;
            _avgFpsText.text = string.Format("Avg. FPS: {0:0.}", avgFps);

            float dropPercentage = (_fpsDropCount / (float)_controlCount) * 100f;

            yield return new WaitForSeconds(_fpsCheckInterval);
        }
    }
    /*
    IEnumerator CheckFPS()
    {
        if (_trackFPS)
        {
            // track every update to later calculate the % of FPS-drops:
            _controlCount++;

            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
            float fps = 1.0f / _deltaTime;
            _fpsText.text = string.Format("FPS: {0:0.}", fps);

            // track any update where the FPS drops below threshold:
            if (fps <= _fpsThreshold)
            {
                _fpsDropCount++;
            }

            //float dropPercentage = (_fpsDropCount / (float)_controlCount) * 100f;
            //_avgFpsText.text = string.Format("Avg. FPS: {0:0.}", fps);

            yield return new WaitForSeconds(_fpsCheckInterval);

            StartCoroutine(CheckFPS());
        }
    }*/

    /*
    void Update()
    {
        if (_trackFPS)
        {
            // track every update to later calculate the % of FPS-drops:
            _controlCount++;

            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
            float fps = 1.0f / _deltaTime;
            fpsText.text = string.Format("FPS: {0:0.}", fps);

            // track any update where the FPS drops below threshold:
            if (fps <= _fpsThreshold)
            {
                _fpsDropCount++;
            }
        }
    }*/

    // when leaving "Play"-mode give out a warning for the amount of fps drops relative to runtime: 
    void OnApplicationQuit()
    {
        float dropPercentage = (_fpsDropCount / (float)_controlCount) * 100f;
        Debug.LogWarning(string.Format("About {0:0.0}% of updates were below the FPS threshold of {1} FPS.", dropPercentage, _fpsThreshold));
    }
}
