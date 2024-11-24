using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class UserInputManager : MonoBehaviour
{
    [Header("Layer Settings")]
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private LayerMask unitLayer;

    // Component references
    private SelectionManager selectionManager;
    private Camera mainCamera;
    private MovementMarker movementMarker;
    // private CombatManager combatManager;

    private void Start()
    {
        Debug.Log("UserInputManager#Start");
        // Initialize components
        mainCamera = Camera.main;
        selectionManager = GetComponent<SelectionManager>();
        movementMarker = GetComponent<MovementMarker>();
        // combatManager = GetComponent<CombatManager>();
        
        ValidateComponents();
    }

    private void ValidateComponents()
    {
        if (!selectionManager)
            Debug.LogError("UserInputManager requires a SelectionManager!");
        if (!movementMarker)
            Debug.LogError("UserInputManager requires a MovementMarker component!");
        // if (!combatManager)
        //     Debug.LogError("UserInputManager requires a CombatManager component!");
    }

    private void Update()
    {
        HandleUnitCommand();
    }

    private void HandleUnitCommand()
    {
        if (Input.GetMouseButtonDown(1)) // Right click
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            ProcessUnitCommand(ray);
        }
    }

    private void ProcessUnitCommand(Ray ray)
    {
        // First check for unit targets (for combat)
        if (Physics.Raycast(ray, out RaycastHit unitHit, Mathf.Infinity, unitLayer))
        {
            Unit targetUnit = unitHit.collider.GetComponent<Unit>();
            if (targetUnit != null && IsAttackModifierPressed())
            {
                IssueCombatCommand(targetUnit);
                return;
            }
        }

        // If no unit was hit or attack modifier wasn't pressed, handle movement
        if (Physics.Raycast(ray, out RaycastHit terrainHit, Mathf.Infinity, terrainLayer))
        {
            IssueMoveCommand(terrainHit.point);
        }
    }

    private void IssueMoveCommand(Vector3 targetPosition)
    {
        List<UnitSelection> selectedUnits = selectionManager.GetSelectedUnits();
        if (selectedUnits.Count == 0) return;

        // Visual feedback
        movementMarker.ShowMarker(targetPosition);

        // Move each selected unit
        foreach (UnitSelection unitSelection in selectedUnits)
        {
            if (TryGetUnitStateController(unitSelection, out UnitStateController controller))
            {
                controller.MoveTo(targetPosition);
            }
        }
    }

    private void IssueCombatCommand(Unit targetUnit)
    {
        List<UnitSelection> selectedUnits = selectionManager.GetSelectedUnits();
        foreach (UnitSelection unitSelection in selectedUnits)
        {
            if (TryGetUnitStateController(unitSelection, out UnitStateController controller))
            {
                Unit attackingUnit = unitSelection.GetComponent<Unit>();
                if (attackingUnit != null && attackingUnit != targetUnit)
                {
                    controller.Attack(targetUnit);
                }
            }
        }
    }

    private bool TryGetUnitStateController(UnitSelection unitSelection, out UnitStateController controller)
    {
        controller = unitSelection.GetComponent<UnitStateController>();
        return controller != null;
    }

    private bool IsAttackModifierPressed()
    {
        return Input.GetKey(KeyCode.LeftAlt) || 
               Input.GetKey(KeyCode.RightAlt) ||
               Input.GetKey(KeyCode.LeftCommand) ||
               Input.GetKey(KeyCode.RightCommand) ||
               Input.GetKey(KeyCode.LeftControl) ||
               Input.GetKey(KeyCode.RightControl);
    }
}