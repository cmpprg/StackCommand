using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage;
    [SerializeField] private Image backgroundImage;
    
    [Header("Color Settings")]
    [SerializeField] private Color healthyColor = new Color(0.35f, 0.85f, 0.35f);
    [SerializeField] private Color warningColor = new Color(0.85f, 0.85f, 0.35f);
    [SerializeField] private Color criticalColor = new Color(0.85f, 0.35f, 0.35f);
    [SerializeField] private Color backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
    
    [Header("Threshold Settings")]
    [SerializeField] private float warningThreshold = 0.6f;
    [SerializeField] private float criticalThreshold = 0.3f;
    
    [Header("Position Settings")]
    [SerializeField] private float heightOffset = 1.5f;
    [SerializeField] private float smoothSpeed = 5f;
    
    [Header("Visibility Settings")]
    [SerializeField] private float hideAfterFullHealth = 3f;
    [SerializeField] private float fadeSpeed = 2f;
    
    private Camera mainCamera;
    private Unit unit;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private float hideTimer;
    private float lastHealth = 1f;

    private void Awake()
    {
        mainCamera = Camera.main;
        unit = GetComponentInParent<Unit>();
        canvas = GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        
        if (!canvasGroup)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
            
        if (!unit)
            Debug.LogError("HealthBarController requires a Unit component on the parent!");
            
        InitializeHealthBar();
    }

    private void InitializeHealthBar()
    {
        healthSlider.value = 1f;
        fillImage.color = healthyColor;
        
        if (backgroundImage)
            backgroundImage.color = backgroundColor;
            
        hideTimer = hideAfterFullHealth;
    }

    private void LateUpdate()
    {
        UpdatePosition();
        UpdateHealthDisplay();
        UpdateVisibility();
    }

    private void UpdatePosition()
    {
        if (mainCamera != null)
        {
            // Simplified positioning - just use unit position plus offset
            Vector3 position = unit.transform.position;
            position.y += heightOffset;
            transform.position = position;
            transform.rotation = mainCamera.transform.rotation;
        }
    }

    private void UpdateHealthDisplay()
    {
        float healthPercent = unit.GetHealthPercent();
        
        healthSlider.value = Mathf.Lerp(healthSlider.value, healthPercent, Time.deltaTime * smoothSpeed);
        
        Color targetColor = GetHealthColor(healthPercent);
        fillImage.color = Color.Lerp(fillImage.color, targetColor, Time.deltaTime * smoothSpeed);
        
        if (healthPercent != lastHealth)
        {
            hideTimer = hideAfterFullHealth;
            canvasGroup.alpha = 1f;
            lastHealth = healthPercent;
        }
    }

    private Color GetHealthColor(float healthPercent)
    {
        if (healthPercent <= criticalThreshold)
            return criticalColor;
        if (healthPercent <= warningThreshold)
            return warningColor;
        return healthyColor;
    }

    private void UpdateVisibility()
    {
        if (healthSlider.value >= 0.999f)
        {
            hideTimer -= Time.deltaTime;
            if (hideTimer <= 0)
            {
                canvasGroup.alpha = Mathf.Max(0, canvasGroup.alpha - Time.deltaTime * fadeSpeed);
            }
        }
    }
}