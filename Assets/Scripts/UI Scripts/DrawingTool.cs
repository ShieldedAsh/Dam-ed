using UnityEngine;

public class DrawingTool : MonoBehaviour
{

    public PaintScript paintScript;
    public Color color = Color.green;
    
    public float width = 0.2f;

    public string layer = "Pencil";

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
        paintScript.brushColor = color;
        paintScript.width = width;
        paintScript.layerName = layer;
    }
}
