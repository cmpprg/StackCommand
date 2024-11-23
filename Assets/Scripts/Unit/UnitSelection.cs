using UnityEngine;

[RequireComponent(typeof(Unit))]
public class UnitSelection : MonoBehaviour
{
    private Unit unit;
    private Renderer unitRenderer;
    private Material defaultMaterial;
    private bool isSelected = false;

    [Header("Selection Visuals")]
    [SerializeField] private GameObject selectionIndicator; // Optional visual ring/highlight
    [SerializeField] private float indicatorOffset = 0.1f; // Height offset for the indicator

    public bool IsSelected => isSelected;
    public Material DefaultMaterial => defaultMaterial;

    private void Awake()
    {
        unit = GetComponent<Unit>();
        unitRenderer = GetComponent<Renderer>();
        
        if (unitRenderer != null)
        {
            defaultMaterial = unitRenderer.material;
        }

        // Initialize selection indicator if present
        if (selectionIndicator != null)
        {
            selectionIndicator.SetActive(false);
        }
    }

    public void OnSelected()
    {
        isSelected = true;
        
        // Show selection indicator if present
        if (selectionIndicator != null)
        {
            selectionIndicator.SetActive(true);
            UpdateIndicatorPosition();
        }
    }

    public void OnDeselected()
    {
        isSelected = false;
        
        // Hide selection indicator
        if (selectionIndicator != null)
        {
            selectionIndicator.SetActive(false);
        }

        // Reset material to default if using material-based highlighting
        if (unitRenderer != null)
        {
            unitRenderer.material = defaultMaterial;
        }
    }

    // Update indicator position when unit moves or changes stack height
    public void UpdateIndicatorPosition()
    {
        if (selectionIndicator != null)
        {
            Vector3 position = transform.position;
            position.y = unit.GetBaseHeight() + indicatorOffset;
            selectionIndicator.transform.position = position;
        }
    }

    // Method to change the selection material (called by SelectionManager)
    public void SetSelectionMaterial(Material selectionMaterial)
    {
        if (unitRenderer != null && selectionMaterial != null)
        {
            unitRenderer.material = selectionMaterial;
        }
    }
}