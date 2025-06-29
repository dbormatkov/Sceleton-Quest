using UnityEngine;
using UnityEngine.InputSystem;

public class TouchRotation : MonoBehaviour
{
    public float rotationSpeed = 0.2f;
    public float zoomSpeed = 0.01f;
    public float panSpeed = 0.005f;

    public float minZoom = 0.3f;
    public float maxZoom = 3f;

    private Camera mainCamera;

    private Vector2 lastSingleTouchPos;
    private Vector2 lastPanAverage;
    private float lastPinchDistance;
    private bool wasTwoFingerLastFrame = false;
    private bool isFirstSingleTouchFrame = true;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Touchscreen.current == null) return;

        int activeTouches = 0;
        Vector2[] positions = new Vector2[2];

        foreach (var touch in Touchscreen.current.touches)
        {
            if (touch.press.isPressed)
            {
                if (activeTouches < 2)
                    positions[activeTouches] = touch.position.ReadValue();
                activeTouches++;
            }
        }

        if (activeTouches == 1)
        {
            Vector2 current = positions[0];

            if (wasTwoFingerLastFrame)
            {
                lastSingleTouchPos = current;
                isFirstSingleTouchFrame = true;
                wasTwoFingerLastFrame = false;
                return;
            }

            if (isFirstSingleTouchFrame)
            {
                lastSingleTouchPos = current;
                isFirstSingleTouchFrame = false;
                return;
            }

            Vector2 delta = current - lastSingleTouchPos;
            RotateModel(delta);
            lastSingleTouchPos = current;
        }
        else if (activeTouches == 2)
        {
            Vector2 current0 = positions[0];
            Vector2 current1 = positions[1];

            float currentDistance = Vector2.Distance(current0, current1);
            Vector2 currentAverage = (current0 + current1) / 2f;

            if (!wasTwoFingerLastFrame)
            {
                lastPinchDistance = currentDistance;
                lastPanAverage = currentAverage;
                wasTwoFingerLastFrame = true;
                return;
            }

            float deltaDistance = currentDistance - lastPinchDistance;
            ZoomModel(deltaDistance);
            lastPinchDistance = currentDistance;

            Vector2 deltaAverage = currentAverage - lastPanAverage;
            PanModel(deltaAverage);
            lastPanAverage = currentAverage;

            isFirstSingleTouchFrame = true;
        }
        else
        {
            wasTwoFingerLastFrame = false;
            isFirstSingleTouchFrame = true;
        }
    }

    void RotateModel(Vector2 delta)
    {
        float rotationY = -delta.x * rotationSpeed;
        float rotationX = delta.y * rotationSpeed;

        transform.Rotate(Vector3.up, rotationY, Space.World);
        transform.Rotate(Vector3.right, rotationX, Space.Self);
    }

    void ZoomModel(float delta)
    {
        float scaleFactor = 1f + delta * zoomSpeed;
        float newScale = Mathf.Clamp(transform.localScale.x * scaleFactor, minZoom, maxZoom);
        transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    void PanModel(Vector2 delta)
    {
        Vector3 move = mainCamera.transform.right * delta.x * panSpeed +
                       mainCamera.transform.up * delta.y * panSpeed;

        transform.position += move;
    }
}

