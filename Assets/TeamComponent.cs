using UnityEngine;

[RequireComponent(typeof(Unit))]
public class TeamComponent : MonoBehaviour
{
    [SerializeField] private TeamDefinition teamDefinition;
    
    private Unit unit;
    private Renderer unitRenderer;
    private Material originalMaterial;

    // Public accessor for team definition
    public TeamDefinition Team => teamDefinition;

    private void Awake()
    {
        unit = GetComponent<Unit>();
        unitRenderer = GetComponent<Renderer>();
        
        if (unitRenderer != null)
        {
            originalMaterial = unitRenderer.material;
        }

        ApplyTeamVisuals();
    }

    public void Initialize(TeamDefinition team)
    {
        teamDefinition = team;
        ApplyTeamVisuals();
    }

    private void ApplyTeamVisuals()
    {
        if (unitRenderer == null || teamDefinition == null) return;

        if (teamDefinition.UnitMaterial != null)
        {
            unitRenderer.material = teamDefinition.UnitMaterial;
        }
        else
        {
            // Fallback to modifying the original material's color
            unitRenderer.material.color = teamDefinition.PrimaryColor;
        }
    }

    public bool IsHostileTo(TeamComponent other)
    {
        if (other == null || other.teamDefinition == null || teamDefinition == null)
            return false;
            
        return other.teamDefinition != teamDefinition;
    }

    public bool IsSameTeam(TeamComponent other)
    {
        if (other == null || other.teamDefinition == null || teamDefinition == null)
            return false;
            
        return other.teamDefinition == teamDefinition;
    }

    private void OnDestroy()
    {
        // Cleanup: Restore original material if we modified it
        if (unitRenderer != null && originalMaterial != null)
        {
            unitRenderer.material = originalMaterial;
        }
    }
}