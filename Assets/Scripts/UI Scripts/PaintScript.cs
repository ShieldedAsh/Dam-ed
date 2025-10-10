using UnityEngine;
using UnityEngine.InputSystem;

public class PaintScript : MonoBehaviour
{
    public Color brushColor = Color.red;
    public float width = 0.2f;
    public Vector3 mousePos;
    public Vector3 screenpoint;
    public LineRenderer brushLine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        screenpoint = Mouse.current.position.ReadValue();
        screenpoint.z = 10.0f;
        mousePos = Camera.main.ScreenToWorldPoint(screenpoint);

        // Add a LineRenderer component
        brushLine = gameObject.AddComponent<LineRenderer>();

        // Set the material
        brushLine.material = new Material(Shader.Find("Sprites/Default"));

        // Set the color
        brushLine.startColor = brushColor;
        brushLine.endColor = brushColor;

        // Set the width
        brushLine.startWidth = width;
        brushLine.endWidth = width;


        brushLine.positionCount = 1;
        brushLine.SetPosition(0, mousePos);
    }

    private void OnMouseDrag()
    {
        screenpoint = Mouse.current.position.ReadValue();
        screenpoint.z = 10.0f;
        mousePos = Camera.main.ScreenToWorldPoint(screenpoint);

        brushLine.positionCount++;
        brushLine.SetPosition(brushLine.positionCount - 1, mousePos);
    }

}
