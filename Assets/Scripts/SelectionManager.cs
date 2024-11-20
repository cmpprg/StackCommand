using UnityEngine;
using System.Collections.Generic;

public class SelectionManager : MonoBehaviour
{
    private Camera mainCamera;
    private List<Unit> selectedUnits = new List<Unit>();
    
    // Selection box visualization
    private Vector3 selectionStartPos;
    private bool isDragging = false;
    
    [SerializeField] private LayerMask selectableLayer;
    [SerializeField] private Material selectionMaterial; // Assign a material for selection highlight
    
    // Selection box visual properties
    [SerializeField] private RectTransform selectionBoxVisual;
    
    private void Start()
    {
        mainCamera = Camera.main;
        
        // Ensure selection box is initially hidden
        if (selectionBoxVisual != null)
            selectionBoxVisual.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Start selection
        if (Input.GetMouseButtonDown(0))
        {
            selectionStartPos = Input.mousePosition;
            isDragging = false;
            
            // Single unit selection check
            if (!Input.GetKey(KeyCode.LeftShift)) // If shift isn't held, clear previous selection
                DeselectAll();
                
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, selectableLayer))
            {
                Unit unit = hit.collider.GetComponent<Unit>();
                if (unit != null)
                {
                    SelectUnit(unit);
                }
            }
        }
        
        // Update selection box
        if (Input.GetMouseButton(0))
        {
            if ((Input.mousePosition - selectionStartPos).magnitude > 5f)
            {
                isDragging = true;
                UpdateSelectionBox();
            }
        }
        
        // End selection
        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                SelectUnitsInBox();
            }
            
            // Hide selection box
            if (selectionBoxVisual != null)
                selectionBoxVisual.gameObject.SetActive(false);
            
            isDragging = false;
        }
    }
    
    private void SelectUnit(Unit unit)
    {
        if (!selectedUnits.Contains(unit))
        {
            selectedUnits.Add(unit);
            // Add visual feedback for selection (e.g., highlight effect)
            Renderer renderer = unit.GetComponent<Renderer>();
            if (renderer != null && selectionMaterial != null)
            {
                renderer.material = selectionMaterial;
            }
        }
    }
    
    private void DeselectUnit(Unit unit)
    {
        if (selectedUnits.Contains(unit))
        {
            selectedUnits.Remove(unit);
            // Remove visual feedback
            Renderer renderer = unit.GetComponent<Renderer>();
            if (renderer != null)
            {
                // Reset to original material (you'll need to store this)
                renderer.material = unit.GetComponent<Unit>().DefaultMaterial;
            }
        }
    }
    
    private void DeselectAll()
    {
        foreach (Unit unit in selectedUnits)
        {
            Renderer renderer = unit.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = unit.GetComponent<Unit>().DefaultMaterial;
            }
        }
        selectedUnits.Clear();
    }
    
    private void UpdateSelectionBox()
    {
        if (selectionBoxVisual == null) return;
        
        selectionBoxVisual.gameObject.SetActive(true);
        
        float width = Input.mousePosition.x - selectionStartPos.x;
        float height = Input.mousePosition.y - selectionStartPos.y;
        
        selectionBoxVisual.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBoxVisual.anchoredPosition = selectionStartPos + new Vector3(width/2, height/2, 0);
    }
    
    private void SelectUnitsInBox()
    {
        Vector2 min = Vector2.Min(selectionStartPos, Input.mousePosition);
        Vector2 max = Vector2.Max(selectionStartPos, Input.mousePosition);
        
        // Find all units in selection box
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            Vector3 screenPos = mainCamera.WorldToScreenPoint(unit.transform.position);
            
            if (screenPos.x > min.x && screenPos.x < max.x && 
                screenPos.y > min.y && screenPos.y < max.y)
            {
                SelectUnit(unit);
            }
        }
    }
    
    // Public method to get currently selected units
    public List<Unit> GetSelectedUnits()
    {
        return selectedUnits;
    }
}