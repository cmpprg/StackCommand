using UnityEngine;

public class SystemTestScript : MonoBehaviour
{
    [SerializeField] private TeamDefinition redTeam;
    [SerializeField] private TeamDefinition blueTeam;
    [SerializeField] private Unit unitPrefab;

    private void SpawnTestUnits()
    {
        // Spawn a red unit
        Unit redUnit = Instantiate(unitPrefab);
        redUnit.GetComponent<TeamComponent>().Initialize(redTeam);

        // Spawn a blue unit
        Unit blueUnit = Instantiate(unitPrefab);
        blueUnit.GetComponent<TeamComponent>().Initialize(blueTeam);

        // Test targeting
        bool canTarget = redUnit.CanTarget(blueUnit); // Should return true
        bool canTargetSameTeam = redUnit.CanTarget(redUnit); // Should return false
    }
}