using UnityEngine;

public class MovementMarker : MonoBehaviour 
{
    [SerializeField] private GameObject markerPrefab;
    [SerializeField] private float markerDuration = 1.0f;
    [SerializeField] private float fadeSpeed = 2.0f;
    [SerializeField] private float hoverHeight = 0.1f;
    
    private GameObject activeMarker;
    private float timeRemaining;
    private Material markerMaterial;

    public void ShowMarker(Vector3 position)
    {
        if (activeMarker != null)
        {
            Destroy(activeMarker);
        }

        position.y += hoverHeight;
        activeMarker = Instantiate(markerPrefab, position, Quaternion.identity);
        markerMaterial = activeMarker.GetComponent<Renderer>().material;
        timeRemaining = markerDuration;
    }

    private void Update()
    {
        if (activeMarker == null) return;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0)
        {
            Color color = markerMaterial.color;
            color.a = Mathf.Max(0, color.a - (fadeSpeed * Time.deltaTime));
            markerMaterial.color = color;

            if (color.a <= 0)
            {
                Destroy(activeMarker);
            }
        }
    }

    private void OnDestroy()
    {
        if (activeMarker != null)
        {
            Destroy(activeMarker);
        }
    }
}