using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class Unit
{
    public GameObject unitPrefab;
    public Vector3 offsetToUnitCenter;
}

public class UnitPhotoBooth : MonoBehaviour
{
    [Header("Photobooth Setup:")]
    [Tooltip("Enter desired screenshot resolution here.")]
    [SerializeField] bool _useGreenscreen = true;
    [SerializeField] bool _usePhotoStage = false;

    [Space(10)]
    [SerializeField] Vector2 _pictureResultion = new Vector2(512.0f, 512.0f);
    [SerializeField] Camera _camera;
    [Tooltip("Enter desired file path here, or leave on PersistentDataPath.")]
    [SerializeField] string _exportFilePath = "PersistentDataPath";
    [Tooltip("Add all GameObjects to take pictures of here. Make sure their position is 0,0,0 and they're scaled correctly. " +
        "Furthermore make sure you're using an appropriate naming convention, as their name will be used in the title of the screenshot.")]
    [SerializeField] Unit[] _arrayOfPhotoObjects; // add all units to have pictures taken here:

    [Space(10)]
    [SerializeField] string _exportFolderName = "Object Button Raw Screenshots";
    [SerializeField] string _nameExtension = "_RawIcon";
    [SerializeField] Transform _camPosition;

    [Space(10)]
    [SerializeField] GameObject _parentForInstantiatedObjects;

    [Space(10)]
    [SerializeField] Material _greenscreenMaterial;

    [Space(10)]
    [SerializeField] GameObject _photoStage;
    [SerializeField] GameObject _greenscreen;

    [Space(10)]
    [SerializeField] GameObject[] _arrayOfObjectsToTurnOff;

    string _fileFormat = ".png";

    // in case no camera has been manually assigned, cache the default:
    private void OnEnable()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
        }
    }

    /// <summary>
    /// Loop through the array of GameObjects and take screenshots.
    /// </summary>
    public void TakeScreenshots()
    {
        StartCoroutine(CaptureScreenshotsWithDelay());
    }

    private IEnumerator CaptureScreenshotsWithDelay()
    {
        // turn off anything you don't want in a screenshot:
        ToggleObjectOnOff();

        for (int i = 0; i < _arrayOfPhotoObjects.Length; i++) // double check if the last unit was pictured
        {
            GameObject _currentObject = Instantiate(_arrayOfPhotoObjects[i].unitPrefab, new Vector3(0, 0, 0), Quaternion.identity, _parentForInstantiatedObjects.transform);
            _currentObject.SetActive(true);

            yield return TakePicture(_currentObject, i, _arrayOfPhotoObjects[i].offsetToUnitCenter);

            _currentObject.SetActive(false);
        }

        // turn them back on when done:
        ToggleObjectOnOff();
    }

    /// <summary>
    /// Take a picture of the active asset.
    /// NOTE: CaptureScreenshot does not allow you to set resolution, hence here is RenderTexture being used.
    /// </summary>
    private IEnumerator TakePicture(GameObject _currentObject, int _unitIndex, Vector3 _vec3Offset)
    {
        // create screenshot folder if it doesn't exist yet:
        string exportFolderPath = GetExportFilePath();
        if (!Directory.Exists(exportFolderPath))
        {
            Directory.CreateDirectory(exportFolderPath);
        }

        // set camera position:
        if (_camPosition != null)
        {
            _camera.transform.position = _camPosition.position;
        }

        // focus on object center and, if required, give an offset depending on object size:
        _camera.transform.LookAt(_currentObject.GetComponentInChildren<MeshRenderer>().bounds.center + _vec3Offset);

        // create a RenderTexture with the desired resolution:
        RenderTexture renderTexture = new RenderTexture((int)_pictureResultion.x, (int)_pictureResultion.y, 24);
        _camera.targetTexture = renderTexture;

        // render the camera's view:
        _camera.Render();

        // create a Texture2D to read the RenderTexture:
        RenderTexture.active = renderTexture;
        Texture2D screenshot = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        screenshot.Apply();

        // reset the camera's target texture and RenderTexture.active:
        _camera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        // save the screenshot to a file at the given path:
        byte[] bytes = screenshot.EncodeToPNG();
        string _compiledFileName = GetExportFilePath() + "/" + _currentObject.name + _nameExtension + _fileFormat;
        File.WriteAllBytes(_compiledFileName, bytes);

        Debug.Log("Screenshot taken of object: " + _currentObject.name + ", at index: " + _unitIndex + ". File-name: " + _compiledFileName, _currentObject);

        yield return null;
    }

    /// <summary>
    /// Toggles select array of GO's state for screenshots: Turns things like UI etc off and then back on.
    /// </summary>
    void ToggleObjectOnOff()
    {
        foreach (GameObject gameObject in _arrayOfObjectsToTurnOff)
        {
            gameObject.SetActive(!gameObject.activeInHierarchy);
        }
    }

    /// <summary>
    /// Save the export to desired location, if none assigned, use persistentDataPath as default.
    /// </summary>
    /// <returns></returns>
    private string GetExportFilePath()
    {
        if (_exportFilePath == "PersistentDataPath") // no custom path assigned:
        {
            return Application.persistentDataPath + "/" + _exportFolderName;
        }
        else // custom path assigned - use this instead:
        {
            return _exportFilePath + "/" + _exportFolderName;
        }
    }
}


/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScreenshotExporter : MonoBehaviour
{
    [Header("Screenshot Exporter Setup:")]
    [SerializeField] Camera _camera; // Assign your camera in the Inspector
    [SerializeField] string _exportFilePath = "PersistentDataPath"; // Folder path to save screenshots
    [SerializeField] GameObject[] _arrayOfUnits; // add all units to have pictures taken here:

    [Space(10)]
    //[SerializeField] string _unitName = "[Unit Name]_Screenshot_RawIcon";
    [SerializeField] Transform _camPosition;
    //[SerializeField] Transform _targetUnit;

    string _nameExtension = "_Unit_Screenshot";
    string _fileFormat = ".png";
    string _exportFolderName = "Unit Screenshots";


    /// <summary>
    /// Loop through the array of units and take screenshots.
    /// </summary>
    public void TakeScreenshots()
    {
        StartCoroutine(CaptureScreenshotsWithDelay());
    }

    private IEnumerator CaptureScreenshotsWithDelay()
    {
        for (int i = 0; i < _arrayOfUnits.Length; i++) // double check if the last unit was pictured
        {
            GameObject _currentUnit = _arrayOfUnits[i];
            _currentUnit.SetActive(true);

            TakePicture(_currentUnit, i);

            yield return new WaitForSeconds(0.1f); // Adjust the delay as needed
            _currentUnit.SetActive(false);

        }
    }

    /// <summary>
    /// Take a picture of the active unit.
    /// </summary>
    void TakePicture(GameObject _currentUnit, int _unitIndex)
    {
        // create screenshot folder if it doesn't exist yet:
        string exportFolderPath = GetExportFilePath();
        if (!Directory.Exists(exportFolderPath))
        {
            Directory.CreateDirectory(exportFolderPath);
        }

        if(_camPosition != null)
        {
            _camera.transform.position = _camPosition.position; // using set pos so i can move the camera later:
        }

        // focus on core of unit:
        _camera.transform.LookAt(_currentUnit.transform);

        string _compiledFileName =  _currentUnit.GetComponent<UnitManager>().unitName + _nameExtension + _fileFormat;

        _compiledFileName = GetExportFilePath() + "/" + _compiledFileName;
        Debug.Log(_compiledFileName.ToString());
        ScreenCapture.CaptureScreenshot(_compiledFileName);

        Debug.Log("screenshot taken of unit: " + _currentUnit.name + ", index: " + _unitIndex + 
            ". File-name: " + _compiledFileName, _currentUnit);
    }

    /// <summary>
    /// Save the export to desired location, if none assigned, use persistentDataPath as default.
    /// </summary>
    /// <returns></returns>
    private string GetExportFilePath()
    {
        if (_exportFilePath == "PersistentDataPath") // no custom path assigned:
        {
            return Application.persistentDataPath + "/" + _exportFolderName;
        }
        else // custom path assigned - use this instead:
        {
            return _exportFilePath + "/" + _exportFolderName;
        }
    }
}*/