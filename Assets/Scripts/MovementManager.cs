using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class MovementManager : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private float minClickDistance = 0.1f;
    
    [Header("Formation Settings")]
    [SerializeField] private float unitSpacing = 1f;  // Minimum distance between units
    [SerializeField] private float formationSpread = 1.5f; // How spread out the formation should be
    
    private SelectionManager selectionManager;
    private Camera mainCamera;
    
    private void Start()
    {
        mainCamera = Camera.main;
        selectionManager = FindObjectOfType<SelectionManager>();
        
        if (!selectionManager)
            Debug.LogError("MovementManager requires a SelectionManager in the scene!");
    }

    private void Update()
    {
        HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, terrainLayer))
            {
                MoveSelectedUnits(hit.point);
            }
        }
    }

    private void MoveSelectedUnits(Vector3 targetPosition)
    {
        List<UnitSelection> selectedUnits = selectionManager.GetSelectedUnits();
        if (selectedUnits.Count == 0) return;

        // For a single unit, just move it directly to the target
        if (selectedUnits.Count == 1)
        {
            Unit unit = selectedUnits[0].GetComponent<Unit>();
            if (unit != null)
            {
                Vector3 finalPosition = GetValidPositionOnNavMesh(targetPosition);
                unit.SetDestination(finalPosition);
            }
            return;
        }

        // For multiple units, create a formation
        Vector3 centerPoint = targetPosition;
        int unitCount = selectedUnits.Count;
        int unitsPerRow = Mathf.CeilToInt(Mathf.Sqrt(unitCount));
        float totalWidth = (unitsPerRow - 1) * unitSpacing * formationSpread;
        
        for (int i = 0; i < unitCount; i++)
        {
            int row = i / unitsPerRow;
            int col = i % unitsPerRow;
            
            float xOffset = col * unitSpacing * formationSpread - totalWidth * 0.5f;
            float zOffset = row * unitSpacing * formationSpread;
            
            Vector3 targetPos = centerPoint + new Vector3(xOffset, 0, zOffset);
            Vector3 finalPosition = GetValidPositionOnNavMesh(targetPos);
            
            Unit unit = selectedUnits[i].GetComponent<Unit>();
            if (unit != null)
            {
                unit.SetDestination(finalPosition);
            }
        }
    }

    private Vector3 GetValidPositionOnNavMesh(Vector3 position)
    {
        if (NavMesh.SamplePosition(position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return position;
    }
}