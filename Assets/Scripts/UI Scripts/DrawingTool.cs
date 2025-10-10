using UnityEngine;

public class DrawingTool : MonoBehaviour
{

    public PaintScript PaintScript;
    public Color color = Color.green;
    public float width = 0.2f;
    public float layer = 10.0f;

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
        //Cannot get the color to update right now for some reason...
        //PaintScript.brushColor = color;
        PaintScript.width = width;
        PaintScript.layer = layer;
    }
}
