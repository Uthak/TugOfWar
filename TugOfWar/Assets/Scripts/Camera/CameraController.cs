using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float _panSpeed = 60f;
    [SerializeField] float _zoomSpeed = 15f;
    [SerializeField] float _minZoom = 10f;
    [SerializeField] float _maxZoom = 60f;
    [SerializeField] float _panLimitLeft = 0.0f;
    [SerializeField] float _panLimitRight = 60.0f;
    [SerializeField] float _panLimitTop = 5.0f;
    [SerializeField] float _panLimitBottom = -17.5f;

    [SerializeField] float _followDelay = 30f; // Time to wait before starting to follow the most forward unit
    [SerializeField] float _followSmoothSpeed = 0.125f; // Speed of the camera smooth follow
    [SerializeField] float _followTolerance = 1f; // Tolerance to avoid jumping between units frantically

    private Camera _camera;
    private bool _isPanning = false;
    private float _lastInputTime;
    private Transform _mostForwardUnit;
    private List<Transform> _listOfUnits = new List<Transform>();

    void OnEnable()
    {
        StartCoroutine(InitializeCamera());
    }

    IEnumerator InitializeCamera()
    {
        yield return null; // Wait one frame to ensure all objects are initialized
        _camera = Camera.main;
        _lastInputTime = Time.time;

        if (_camera == null)
        {
            Debug.LogError("Camera not found. Make sure there is a Main Camera in the scene.", this);
        }
    }

    /*
    void Start()
    {
        _camera = Camera.main;
        _lastInputTime = Time.time;
    }*/

    void Update()
    {
        if (_camera == null) return;

        HandleCameraMovement();
        if (!_isPanning && Time.time - _lastInputTime > _followDelay)
        {
            FollowMostForwardUnit();
        }
    }

    void HandleCameraMovement()
    {
        // check if player is trying to pan the camera:
        if (Input.GetMouseButtonDown(1))
        {
            _isPanning = true;
        }

        /*
        // check if the mouse is being touched and stop following forward unit:
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(2) || Input.GetAxis("Mouse X") > 0.0f)
        {
            //isFollowing = false;
        }*/

        // check for any mouse button release to indicate end of panning:
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
        {
            _isPanning = false;
            _lastInputTime = Time.time;
        }

        if (_isPanning)
        {
            // get input:
            float mouseX = Input.GetAxis("Mouse X") * _panSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * _panSpeed * Time.deltaTime;

            // apply input:
            Vector3 newPosition = _camera.transform.position + new Vector3(-mouseX, 0, -mouseY); // minus to invert the movement:

            // limit the field of view:
            newPosition.x = Mathf.Clamp(newPosition.x, _panLimitLeft, _panLimitRight);
            newPosition.z = Mathf.Clamp(newPosition.z, _panLimitBottom, _panLimitTop);

            _camera.transform.position = newPosition;
            _lastInputTime = Time.time;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel") * _zoomSpeed;
        _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView - scroll, _minZoom, _maxZoom);

        if (scroll != 0)
        {
            _lastInputTime = Time.time;
        }
    }

    void FollowMostForwardUnit()
    {
        if (_listOfUnits.Count == 0)
        {
            Debug.LogWarning("No units to follow.");
            return;
        }

        Transform newMostForwardUnit = _listOfUnits[0];
        foreach (Transform unit in _listOfUnits)
        {
            if (unit.position.x > newMostForwardUnit.position.x)
            {
                newMostForwardUnit = unit;
            }
        }

        if (_mostForwardUnit == null || Mathf.Abs(newMostForwardUnit.position.x - _mostForwardUnit.position.x) > _followTolerance)
        {
            _mostForwardUnit = newMostForwardUnit;
        }

        if (_mostForwardUnit != null)
        {
            Vector3 targetPosition = new Vector3(_mostForwardUnit.position.x, _camera.transform.position.y, _camera.transform.position.z);

            // only jump to the most forward unit if the camera's X position is not already further than the unit's X position
            if (_camera.transform.position.x < _mostForwardUnit.position.x)
            {
                _camera.transform.position = Vector3.Lerp(_camera.transform.position, targetPosition, _followSmoothSpeed);
            }
        }
    }

    public void AddUnitToFollow(Transform unit)
    {
        if (!_listOfUnits.Contains(unit))
        {
            _listOfUnits.Add(unit);
        }
    }

    public void RemoveUnitToFollow(Transform unit)
    {
        if (_listOfUnits.Contains(unit))
        {
            _listOfUnits.Remove(unit);
        }
    }
}