using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(CanvasScaler))]
public class ResponsiveUI : MonoBehaviour
{
    CanvasScaler canvasScaler;

    void Start()
    {
        canvasScaler = GetComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080); // Replace with your UI design resolution
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        canvasScaler.matchWidthOrHeight = 0.5f; // Balanced match between width and height
    }

    // Additional logic for responsive adjustments if needed
}
