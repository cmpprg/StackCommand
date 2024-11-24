using UnityEngine;

[RequireComponent(typeof(Unit))]
public class UnitSelection : MonoBehaviour
{
    private Unit unit;
    private Renderer unitRenderer;
    private Material defaultMaterial;
    private bool isSelected = false;

    [Header("Selection Visuals")]
    [SerializeField] private GameObject selectionIndicator;
    [SerializeField] private float indicatorOffset = 0.1f;

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

        if (selectionIndicator != null)
        {
            selectionIndicator.SetActive(false);
        }
    }

    public void OnSelected()
    {
        isSelected = true;
        
        if (selectionIndicator != null)
        {
            selectionIndicator.SetActive(true);
            UpdateIndicatorPosition();
        }
    }

    public void OnDeselected()
    {
        isSelected = false;
        
        if (selectionIndicator != null)
        {
            selectionIndicator.SetActive(false);
        }

        if (unitRenderer != null)
        {
            unitRenderer.material = defaultMaterial;
        }
    }

    // Simplified indicator positioning
    public void UpdateIndicatorPosition()
    {
        if (selectionIndicator != null)
        {
            Vector3 position = transform.position;
            position.y += indicatorOffset;
            selectionIndicator.transform.position = position;
        }
    }

    public void SetSelectionMaterial(Material selectionMaterial)
    {
        if (unitRenderer != null && selectionMaterial != null)
        {
            unitRenderer.material = selectionMaterial;
        }
    }
}