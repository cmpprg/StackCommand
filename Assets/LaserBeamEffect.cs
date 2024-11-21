using UnityEngine;

public class LaserBeamEffect : MonoBehaviour 
{
    [SerializeField] private float beamWidth = 0.1f;
    [SerializeField] private float fadeTime = 0.5f;
    [SerializeField] private Material beamMaterial;

    private LineRenderer lineRenderer;
    private float timeLeft;

    private void Awake()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = beamMaterial;
        lineRenderer.startWidth = beamWidth;
        lineRenderer.endWidth = beamWidth;
        lineRenderer.positionCount = 2;
    }

    public void ShowBeam(Transform source, Vector3 targetPosition)
    {
        lineRenderer.SetPosition(0, source.position);
        lineRenderer.SetPosition(1, targetPosition);
        timeLeft = fadeTime;
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        Color color = beamMaterial.color;
        color.a = timeLeft / fadeTime;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        if (timeLeft <= 0)
        {
            Destroy(gameObject);
        }
    }
}