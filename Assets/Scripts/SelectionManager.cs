using UnityEngine;
using System.Collections.Generic;

public class SelectionManager : MonoBehaviour
{
    private Camera mainCamera;
    private List<Unit> selectedUnits = new List<Unit>();
    
    // Selection box visualization
    private Vector3 selectionStartPos;
    private bool isDragging = false;
    
    [Header("Selection Settings")]
    [SerializeField] private LayerMask selectableLayer;
    [SerializeField] private Material selectionMaterial;
    [SerializeField] private SelectionBox selectionBox;
    [SerializeField] private float minDragDistance = 5f;
    
    private void Start()
    {
        mainCamera = Camera.main;
        
        if (selectionBox == null)
        {
            Debug.LogError("Selection Box reference not set in Selection Manager!");
        }
    }

    private void Update()
    {
        HandleSelection();
    }
    
    private void HandleSelection()
    {
        // Start selection
        if (Input.GetMouseButtonDown(0))
        {
            selectionStartPos = Input.mousePosition;
            isDragging = false;
            
            if (!Input.GetKey(KeyCode.LeftShift))
                DeselectAll();
                
            // Single unit selection
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
            Vector3 dragDelta = Input.mousePosition - selectionStartPos;
            if (dragDelta.magnitude > minDragDistance)
            {
                if (!isDragging)
                {
                    isDragging = true;
                    selectionBox.Show();
                }
                UpdateSelectionBox();
            }
        }
        
        // End selection
        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                SelectUnitsInBox();
                selectionBox.Hide();
            }
            isDragging = false;
        }
    }
    
    private void UpdateSelectionBox()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 boxStart = selectionStartPos;
        
        // Calculate corners
        Vector2 bottomLeft = new Vector2(
            Mathf.Min(boxStart.x, mousePos.x),
            Mathf.Min(boxStart.y, mousePos.y)
        );
        Vector2 topRight = new Vector2(
            Mathf.Max(boxStart.x, mousePos.x),
            Mathf.Max(boxStart.y, mousePos.y)
        );
        
        // Calculate size and center position
        Vector2 size = topRight - bottomLeft;
        Vector2 center = bottomLeft + size / 2f;
        
        // Update selection box UI
        selectionBox.UpdateSize(size);
        selectionBox.UpdatePosition(center);
    }
    
    private void SelectUnit(Unit unit)
    {
        if (!selectedUnits.Contains(unit))
        {
            selectedUnits.Add(unit);
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
            Renderer renderer = unit.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = unit.DefaultMaterial;
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
                renderer.material = unit.DefaultMaterial;
            }
        }
        selectedUnits.Clear();
    }
    
    private void SelectUnitsInBox()
    {
        Vector2 min = Vector2.Min(selectionStartPos, Input.mousePosition);
        Vector2 max = Vector2.Max(selectionStartPos, Input.mousePosition);
        
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
    
    public List<Unit> GetSelectedUnits()
    {
        return selectedUnits;
    }
}