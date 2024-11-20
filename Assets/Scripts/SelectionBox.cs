using UnityEngine;
using UnityEngine.UI;

public class SelectionBox : MonoBehaviour
{
    private Image selectionBoxImage;
    private RectTransform rectTransform;
    
    private void Awake()
    {
        selectionBoxImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        // Start with the selection box hidden
        selectionBoxImage.enabled = false;
    }

    public void Show()
    {
        selectionBoxImage.enabled = true;
    }

    public void Hide()
    {
        selectionBoxImage.enabled = false;
    }

    public void UpdateSize(Vector2 size)
    {
        rectTransform.sizeDelta = size;
    }

    public void UpdatePosition(Vector2 position)
    {
        rectTransform.position = position;
    }
}