using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float edgeScrollMargin = 20f;
    [SerializeField] private bool enableEdgeScrolling = true;
    
    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 4f;
    [SerializeField] private float minZoom = 10f;
    [SerializeField] private float maxZoom = 50f;
    
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 100f;
    
    private Camera mainCamera;
    private Vector3 lastMousePosition;
    
    private void Start()
    {
        mainCamera = GetComponent<Camera>();
        if (!mainCamera)
        {
            Debug.LogError("CameraController requires a Camera component!");
            enabled = false;
        }
    }

    private void Update()
    {
        HandleKeyboardInput();
        HandleEdgeScrolling();
        HandleZoom();
        HandleRotation();
    }

    private void HandleKeyboardInput()
    {
        Vector3 movement = Vector3.zero;

        // WASD/Arrow movement
        movement.x += (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) ? 1 : 0;
        movement.x += (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) ? -1 : 0;
        movement.z += (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) ? 1 : 0;
        movement.z += (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) ? -1 : 0;

        if (movement != Vector3.zero)
        {
            // Normalize for consistent speed in all directions
            movement.Normalize();
            
            // Transform movement relative to camera rotation
            movement = transform.TransformDirection(movement);
            movement.y = 0; // Keep movement horizontal
            
            transform.position += movement * (moveSpeed * Time.deltaTime);
        }
    }

    private void HandleEdgeScrolling()
    {
        if (!enableEdgeScrolling) return;

        Vector3 movement = Vector3.zero;
        Vector2 mousePosition = Input.mousePosition;

        // Check screen edges
        if (mousePosition.x < edgeScrollMargin)
            movement.x = -1;
        else if (mousePosition.x > Screen.width - edgeScrollMargin)
            movement.x = 1;

        if (mousePosition.y < edgeScrollMargin)
            movement.z = -1;
        else if (mousePosition.y > Screen.height - edgeScrollMargin)
            movement.z = 1;

        if (movement != Vector3.zero)
        {
            movement.Normalize();
            movement = transform.TransformDirection(movement);
            movement.y = 0;
            transform.position += movement * (moveSpeed * Time.deltaTime);
        }
    }

    private void HandleZoom()
    {
        float scrollDelta = Input.mouseScrollDelta.y;
        if (scrollDelta != 0)
        {
            Vector3 position = transform.position;
            position.y = Mathf.Clamp(position.y - scrollDelta * zoomSpeed, minZoom, maxZoom);
            transform.position = position;
        }
    }

    private void HandleRotation()
    {
        // E/Q rotation
        float rotation = 0f;
        if (Input.GetKey(KeyCode.Q))
            rotation = -1f;
        else if (Input.GetKey(KeyCode.E))
            rotation = 1f;

        if (rotation != 0)
        {
            transform.Rotate(Vector3.up, rotation * rotationSpeed * Time.deltaTime, Space.World);
        }
    }
}