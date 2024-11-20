using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class MovementManager : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private LayerMask unitLayer;
    
    [Header("Formation Settings")]
    [SerializeField] private float unitSpacing = 1f;  // Minimum distance between units
    [SerializeField] private float formationSpread = 1.5f; // How spread out the formation should be
    
    private SelectionManager selectionManager;
    private Camera mainCamera;
    private MovementMarker movementMarker;
    private CombatManager combatManager;
    
private void Start()
    {
        mainCamera = Camera.main;
        selectionManager = GetComponent<SelectionManager>();
        movementMarker = GetComponent<MovementMarker>();
        combatManager = GetComponent<CombatManager>();
        
        if (!selectionManager)
            Debug.LogError("MovementManager requires a SelectionManager in the scene!");
        if (!movementMarker)
            Debug.LogError("MovementManager requires a MovementMarker component!");
        if (!combatManager)
            Debug.LogError("MovementManager requires a CombatManager component!");
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
{
    if (Input.GetMouseButtonDown(1)) // Right click
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // First check for unit hits (for combat)
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, unitLayer))
        {
            Unit targetUnit = hit.collider.GetComponent<Unit>();
            if (targetUnit != null && IsAttackModifierPressed())
            {
                HandleCombatCommand(targetUnit);
                return;
            }
        }

        // If no unit was hit or Command/Option wasn't held, handle movement
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrainLayer))
        {
            MoveSelectedUnits(hit.point);
        }
    }
}

    private void HandleCombatCommand(Unit targetUnit)
    {
        List<UnitSelection> selectedUnits = selectionManager.GetSelectedUnits();
        foreach (UnitSelection unitSelection in selectedUnits)
        {
            Unit attackingUnit = unitSelection.GetComponent<Unit>();
            if (attackingUnit != null && attackingUnit != targetUnit) // Can't attack self
            {
                combatManager.InitiateAttack(attackingUnit, targetUnit);
            }
        }
    }

    private void MoveSelectedUnits(Vector3 targetPosition)
    {
        List<UnitSelection> selectedUnits = selectionManager.GetSelectedUnits();
        if (selectedUnits.Count == 0) return;

        // Show marker at target position
        movementMarker.ShowMarker(targetPosition);

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

    private bool IsAttackModifierPressed()
    {
        bool isPressed = Input.GetKey(KeyCode.LeftAlt) ||     // Alt/Option key
            Input.GetKey(KeyCode.RightAlt) ||    // Right Alt/Option key
            Input.GetKey(KeyCode.LeftCommand) || // Left Command (Mac)
            Input.GetKey(KeyCode.RightCommand) || // Right Command (Mac)
            Input.GetKey(KeyCode.LeftControl) ||  // Adding Control key support
            Input.GetKey(KeyCode.RightControl);   // Adding Right Control key support
        
        return isPressed;
    }
}