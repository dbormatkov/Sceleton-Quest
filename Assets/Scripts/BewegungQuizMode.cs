using UnityEngine;
using System.Collections;

public class BewegungQuizMode : MonoBehaviour
{
    public float rotationSpeed = 0.1f;
    public float zoomSpeedTouch = 0.1f;
    public float panSpeed = 0.05f; 
    private float currentZoomDistance;

    public float minZoom = 0.3f;
    public float maxZoom = 3f; 

    private Vector2 lastTouchPosition;
    private bool isDragging = false;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        currentZoomDistance = Vector3.Distance(mainCamera.transform.position, transform.position);
    }

    void Update()
    {
        HandleTouchInput();
    }

    void HandleTouchInput()
    {
         if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

            float prevTouchDeltaMag = (touch0PrevPos - touch1PrevPos).magnitude;
            float touchDeltaMag = (touch0.position - touch1.position).magnitude;

            float deltaMagnitudeDiff = touchDeltaMag - prevTouchDeltaMag;

            float angle = Vector2.Angle(touch0.deltaPosition, touch1.deltaPosition);
            bool isPanning = angle < 30f;

            if (isPanning)
            {
                Vector2 avgDelta = (touch0.deltaPosition + touch1.deltaPosition) / 2f;
                PanCamera(avgDelta);
            }
            else
            {
                ZoomCamera(deltaMagnitudeDiff * zoomSpeedTouch);
            }
        }
    }

    void ZoomCamera(float deltaMagnitudeDiff)
    {
        float scaleFactor = 1f + deltaMagnitudeDiff * zoomSpeedTouch;
        float newScale = Mathf.Clamp(transform.localScale.x * scaleFactor, minZoom, maxZoom);
        transform.localScale = Vector3.Lerp(
            transform.localScale, 
            new Vector3(newScale, newScale, newScale),
            0.1f
        );
    }

    void PanCamera(Vector2 touchDelta)
    {
        Vector3 move = mainCamera.transform.up * touchDelta.y * panSpeed;
        move += mainCamera.transform.right * touchDelta.x * panSpeed;
        transform.position += move;
    }
}
