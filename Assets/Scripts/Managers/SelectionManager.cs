using UnityEngine;
using System.Collections.Generic;

public class SelectionManager : MonoBehaviour
{
    private Camera mainCamera;
    private List<UnitSelection> selectedUnits = new List<UnitSelection>();
    
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
                UnitSelection unitSelection = hit.collider.GetComponent<UnitSelection>();
                if (unitSelection != null)
                {
                    SelectUnit(unitSelection);
                }
            }
        }
        
        // Handle drag selection box
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
                UpdateBoxVisual();
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

    private void UpdateBoxVisual()
    {
        float width = Input.mousePosition.x - selectionStartPos.x;
        float height = Input.mousePosition.y - selectionStartPos.y;
        
        // Update box size and position
        selectionBox.UpdateSize(new Vector2(Mathf.Abs(width), Mathf.Abs(height)));
        selectionBox.UpdatePosition(selectionStartPos + new Vector3(width/2, height/2, 0));
    }

    private void SelectUnit(UnitSelection unitSelection)
    {
        if (!selectedUnits.Contains(unitSelection))
        {
            selectedUnits.Add(unitSelection);
            unitSelection.OnSelected();
            unitSelection.SetSelectionMaterial(selectionMaterial);
        }
    }
    
    private void DeselectUnit(UnitSelection unitSelection)
    {
        if (selectedUnits.Contains(unitSelection))
        {
            selectedUnits.Remove(unitSelection);
            unitSelection.OnDeselected();
        }
    }
    
    private void DeselectAll()
    {
        foreach (UnitSelection unitSelection in selectedUnits)
        {
            unitSelection.OnDeselected();
        }
        selectedUnits.Clear();
    }
    
    private void SelectUnitsInBox()
    {
        Vector2 min = Vector2.Min(selectionStartPos, Input.mousePosition);
        Vector2 max = Vector2.Max(selectionStartPos, Input.mousePosition);
        
        foreach (UnitSelection unitSelection in FindObjectsByType<UnitSelection>(FindObjectsSortMode.None))
        {
            Vector3 screenPos = mainCamera.WorldToScreenPoint(unitSelection.transform.position);
            
            if (screenPos.x > min.x && screenPos.x < max.x && 
                screenPos.y > min.y && screenPos.y < max.y)
            {
                SelectUnit(unitSelection);
            }
        }
    }
    
    public List<UnitSelection> GetSelectedUnits()
    {
        return selectedUnits;
    }
}