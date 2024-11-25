using UnityEngine;

[CreateAssetMenu(fileName = "NewTeam", menuName = "StackCommand/Team Definition")]
public class TeamDefinition : ScriptableObject
{
    [Header("Team Identity")]
    [SerializeField] private string teamName = "New Team";
    [SerializeField] private int teamId;  // Unique identifier for the team

    [Header("Visual Properties")]
    [SerializeField] private Color primaryColor = Color.white;
    [SerializeField] private Color secondaryColor = Color.gray;
    [SerializeField] private Material unitMaterial;

    [Header("Gameplay Properties")]
    [SerializeField] private LayerMask targetableLayers;
    
    // Public accessors
    public string TeamName => teamName;
    public int TeamId => teamId;
    public Color PrimaryColor => primaryColor;
    public Color SecondaryColor => secondaryColor;
    public Material UnitMaterial => unitMaterial;
    public LayerMask TargetableLayers => targetableLayers;
}