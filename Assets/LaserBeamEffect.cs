using UnityEngine;

public class LaserBeamEffect : MonoBehaviour
{
    [Header("Beam Settings")]
    [SerializeField] private float beamWidth = 0.1f;
    [SerializeField] private float fadeTime = 0.5f;
    [SerializeField] private Material beamMaterial;
    
    private LineRenderer lineRenderer;
    private float currentFadeTime;
    private bool isFading;
    
    private void Awake()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(beamMaterial); // Create instance of material
        lineRenderer.startWidth = beamWidth;
        lineRenderer.endWidth = beamWidth;
        lineRenderer.positionCount = 2;
    }

    public void ShowBeam(Vector3 startPos, Vector3 endPos)
    {
        startPos.y += 0.5f;
        endPos.y += 0.5f;
        
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
        
        currentFadeTime = fadeTime;
        isFading = true;
        
        Color color = lineRenderer.material.color;
        color.a = 1f;
        lineRenderer.material.color = color;
    }

    private void Update()
    {
        if (!isFading) return;

        currentFadeTime -= Time.deltaTime;
        
        Color color = lineRenderer.material.color;
        color.a = currentFadeTime / fadeTime;
        lineRenderer.material.color = color;

        if (currentFadeTime <= 0)
        {
            Destroy(gameObject); // Destroy the object when fade completes
        }
    }

    private void OnDestroy()
    {
        // Clean up the instance material
        if (lineRenderer != null && lineRenderer.material != null)
        {
            Destroy(lineRenderer.material);
        }
    }
}