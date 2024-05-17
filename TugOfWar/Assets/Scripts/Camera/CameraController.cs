using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // zoom not working
    // follow is hindered by max pan values
    // cam jumps backwards to follow a "forward" unit

    public float panSpeed = 60f;
    public float zoomSpeed = 15f;
    public float minZoom = 10f;
    public float maxZoom = 60f;
    public float panLimitLeft = 10f;
    public float panLimitRight = 50f;
    public float followDelay = 30f; // Time to wait before starting to follow the most forward unit
    public float followSmoothSpeed = 0.125f; // Speed of the camera smooth follow
    public float followTolerance = 1f; // Tolerance to avoid jumping between units frantically

    private Camera _camera;
    private bool isPanning = false;
    //private bool isFollowing = false;
    private float lastInputTime;
    private Transform mostForwardUnit;
    private List<Transform> units = new List<Transform>();

    void Start()
    {
        _camera = Camera.main;
        lastInputTime = Time.time;
    }

    void Update()
    {
        HandleCameraMovement();
        if (!isPanning && Time.time - lastInputTime > followDelay)
        {
            FollowMostForwardUnit();
        }
    }

    void HandleCameraMovement()
    {
        // check if player is trying to pan the camera:
        if (Input.GetMouseButtonDown(1))
        {
            isPanning = true;
        }

        // check if the mouse is being touched and stop following forward unit:
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(2) || Input.GetAxis("Mouse X") > 0.0f)
        {
            //isFollowing = false;
        }

        // check for any mouse button release to indicate end of panning:
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
        {
            isPanning = false;
            lastInputTime = Time.time;
            //isFollowing = false; // why?
        }


        if (isPanning)
        {
            float mouseX = Input.GetAxis("Mouse X") * panSpeed * Time.deltaTime;
            Vector3 newPosition = _camera.transform.position + new Vector3(-mouseX, 0, 0); // minus to invert the movement:
            newPosition.x = Mathf.Clamp(newPosition.x, panLimitLeft, panLimitRight);
            _camera.transform.position = newPosition;

            lastInputTime = Time.time;
            //isFollowing = false;
        }


        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView - scroll, minZoom, maxZoom);

        if (scroll != 0)
        {
            lastInputTime = Time.time;
            Debug.Log("is scrolling!");
        }
    }

    void FollowMostForwardUnit()
    {
        if (units.Count == 0)
        {
            Debug.LogWarning("No units to follow.");
            return;
        }

        Transform newMostForwardUnit = units[0];
        foreach (Transform unit in units)
        {
            if (unit.position.x > newMostForwardUnit.position.x)
            {
                newMostForwardUnit = unit;
            }
        }

        if (mostForwardUnit == null || Mathf.Abs(newMostForwardUnit.position.x - mostForwardUnit.position.x) > followTolerance)
        {
            mostForwardUnit = newMostForwardUnit;
        }

        if (mostForwardUnit != null)
        {
            Vector3 targetPosition = new Vector3(mostForwardUnit.position.x, _camera.transform.position.y, _camera.transform.position.z);

            // only jump to the most forward unit if the camera's X position is not already further than the unit's X position
            if (_camera.transform.position.x < mostForwardUnit.position.x)
            {
                _camera.transform.position = Vector3.Lerp(_camera.transform.position, targetPosition, followSmoothSpeed);
                //isFollowing = true;
            }
        }
    }

    public void AddUnit(Transform unit)
    {
        if (!units.Contains(unit))
        {
            units.Add(unit);
        }
    }

    public void RemoveUnit(Transform unit)
    {
        if (units.Contains(unit))
        {
            units.Remove(unit);
        }
    }


    /*
    public float followCheckInterval = 3f; // Interval to check for following the most forward unit
    public float followTolerance = 1f; // Tolerance to avoid jumping between units frantically

    private Camera _camera;
    private bool isPanning = false;
    private bool isFollowing = false;
    private float lastInputTime;
    private Transform mostForwardUnit;
    private List<Transform> units = new List<Transform>();

    void Start()
    {
        _camera = Camera.main;
        lastInputTime = Time.time;
        StartCoroutine(CheckForMostForwardUnit());
    }

    void Update()
    {
        if (PlayerInput())
        {
            ManuallyMoveCamera();
        }else if(!PlayerInput() && isFollowing)
        {
            _camera.transform.position = new Vector3(mostForwardUnit.transform.position.x, _camera.transform.position.y, _camera.transform.position.z);
        }
    }

    void ManuallyMoveCamera()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isPanning = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isPanning = false;
        }

        if (isPanning)
        {
            isFollowing = false;

            float mouseX = Input.GetAxis("Mouse X") * panSpeed * Time.deltaTime;
            Vector3 newPosition = _camera.transform.position + new Vector3(-mouseX, 0, 0);
            newPosition.x = Mathf.Clamp(newPosition.x, panLimitLeft, panLimitRight);
            _camera.transform.position = newPosition;

            lastInputTime = Time.time;
        }
        
        //isFollowing = false;

        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - scroll, minZoom, maxZoom);
        
        if (scroll != 0)
        {
            //lastInputTime = Time.time;
            //isFollowing = false;
        }
}
    bool PlayerInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isFollowing = false;
            return true;
        }else
        {
            return false;
        }
    }
    IEnumerator CheckForMostForwardUnit()
    {
        while (true)
        {
            yield return new WaitForSeconds(followCheckInterval);

            if (Time.time - lastInputTime > followCheckInterval)
            {
                FollowMostForwardUnit();
            }
        }
    }

    void FollowMostForwardUnit()
    {
        if (units.Count == 0)
        {
            Debug.LogWarning("No units to follow.");
            return;
        }

        Transform newMostForwardUnit = units[0];
        foreach (Transform unit in units)
        {
            if (unit.position.x > newMostForwardUnit.position.x)
            {
                newMostForwardUnit = unit;
            }
        }

        if (mostForwardUnit == null || Mathf.Abs(newMostForwardUnit.position.x - mostForwardUnit.position.x) > followTolerance)
        {
            mostForwardUnit = newMostForwardUnit;
            Vector3 newPosition = new Vector3(
                Mathf.Clamp(mostForwardUnit.position.x, panLimitLeft, panLimitRight),
                _camera.transform.position.y,
                _camera.transform.position.z
            );
            //Vector3 newPosition = new Vector3(mostForwardUnit.position.x,_camera.transform.position.y,_camera.transform.position.z);
            
            //_camera.transform.position = newPosition;
            //isFollowing = true;
        }
        else
        {
            Debug.Log("No significant change in forward unit position.");
        }
        if(mostForwardUnit.transform.position.x >= _camera.transform.position.x)
        {
            isFollowing = true;
        }
    }

    public void AddUnit(Transform unit)
    {
        if (!units.Contains(unit))
        {
            units.Add(unit);
        }

        Debug.Log("i counted this many friendly units: " + units.Count);
    }

    public void RemoveUnit(Transform unit)
    {
        if (units.Contains(unit))
        {
            units.Remove(unit);
        }
    }*/

    /*
    public float panSpeed = 20f;
    public float zoomSpeed = 2f;
    public float minZoom = 5f;
    public float maxZoom = 20f;
    public float panLimitLeft = -20f;
    public float panLimitRight = 20f;
    public float followCheckInterval = 1f; // Interval to check for following the most forward unit
    public float followTolerance = 1f; // Tolerance to avoid jumping between units frantically

    private Camera _camera;
    private bool isPanning = false;
    private bool isFollowing = false;
    private float lastInputTime;
    private Transform mostForwardUnit;
    private List<Transform> units = new List<Transform>();

    void Start()
    {
        _camera = Camera.main;
        lastInputTime = Time.time;
        StartCoroutine(CheckForMostForwardUnit());
    }

    void Update()
    {
        HandleCameraMovement();
    }

    void HandleCameraMovement()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isPanning = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isPanning = false;
        }

        if (isPanning)
        {
            float mouseX = Input.GetAxis("Mouse X") * panSpeed * Time.deltaTime;
            Vector3 newPosition = transform.position + new Vector3(mouseX, _camera.transform.position.y, _camera.transform.position.z);
            newPosition.x = Mathf.Clamp(newPosition.x, panLimitLeft, panLimitRight);
            //transform.position = newPosition;
            _camera.transform.position = newPosition;

            lastInputTime = Time.time;
            isFollowing = false;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - scroll, minZoom, maxZoom);

        if (scroll != 0)
        {
            lastInputTime = Time.time;
            isFollowing = false;
        }
    }

    IEnumerator CheckForMostForwardUnit()
    {
        while (true)
        {
            yield return new WaitForSeconds(followCheckInterval);

            if (Time.time - lastInputTime > followCheckInterval)
            {
                FollowMostForwardUnit();
            }
        }
    }

    void FollowMostForwardUnit()
    {
        if (units.Count == 0) return;

        Transform newMostForwardUnit = units[0];
        foreach (Transform unit in units)
        {
            if (unit.position.x > newMostForwardUnit.position.x)
            {
                newMostForwardUnit = unit;
            }
        }

        if (mostForwardUnit == null || Mathf.Abs(newMostForwardUnit.position.x - mostForwardUnit.position.x) > followTolerance)
        {
            mostForwardUnit = newMostForwardUnit;
            Vector3 newPosition = new Vector3(
                Mathf.Clamp(mostForwardUnit.position.x, panLimitLeft, panLimitRight),
                _camera.transform.position.y,
                _camera.transform.position.z
            );

            _camera.transform.position = newPosition;
            isFollowing = true;
        }
    }

    public void AddUnit(Transform unit)
    {
        if (!units.Contains(unit))
        {
            units.Add(unit);
        }
    }

    public void RemoveUnit(Transform unit)
    {
        if (units.Contains(unit))
        {
            units.Remove(unit);
        }
    }*/


    /*
    public Transform[] units; // Array to hold all units in the battlefield
    public float panSpeed = 20f;
    public float zoomSpeed = 2f;
    public float minZoom = 5f;
    public float maxZoom = 20f;
    public float panLimitLeft = -20f;
    public float panLimitRight = 20f;
    public float followCheckInterval = 1f; // Interval to check for following the most forward unit

    private Camera _camera;
    private bool isPanning = false;
    private bool isFollowing = false;
    private float lastInputTime;

    void Start()
    {
        _camera = Camera.main;
        lastInputTime = Time.time;
        StartCoroutine(CheckForMostForwardUnit());
    }

    void Update()
    {
        HandleCameraMovement();
    }

    void HandleCameraMovement()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isPanning = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isPanning = false;
        }

        if (isPanning)
        {
            float mouseX = Input.GetAxis("Mouse X") * panSpeed * Time.deltaTime;
            Vector3 newPosition = transform.position + new Vector3(mouseX, 0, 0);
            newPosition.x = Mathf.Clamp(newPosition.x, panLimitLeft, panLimitRight);
            transform.position = newPosition;

            lastInputTime = Time.time;
            isFollowing = false;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - scroll, minZoom, maxZoom);

        if (scroll != 0)
        {
            lastInputTime = Time.time;
            isFollowing = false;
        }
    }

    IEnumerator CheckForMostForwardUnit()
    {
        while (true)
        {
            yield return new WaitForSeconds(followCheckInterval);

            if (Time.time - lastInputTime > followCheckInterval)
            {
                FollowMostForwardUnit();
            }
        }
    }

    void FollowMostForwardUnit()
    {
        if (units.Length == 0) return;

        Transform mostForwardUnit = units[0];
        foreach (Transform unit in units)
        {
            if (unit.position.z > mostForwardUnit.position.z)
            {
                mostForwardUnit = unit;
            }
        }

        Vector3 newPosition = new Vector3(
            Mathf.Clamp(mostForwardUnit.position.x, panLimitLeft, panLimitRight),
            transform.position.y,
            transform.position.z
        );

        transform.position = newPosition;
        isFollowing = true;
    }

    public void AddUnit(Transform unit)
    {
        // Add a new unit to the list of units (if necessary)
        List<Transform> unitsList = new List<Transform>(units);
        unitsList.Add(unit);
        units = unitsList.ToArray();
    }

    public void RemoveUnit(Transform unit)
    {
        // Remove a unit from the list of units (if necessary)
        List<Transform> unitsList = new List<Transform>(units);
        unitsList.Remove(unit);
        units = unitsList.ToArray();
    }*/
}
