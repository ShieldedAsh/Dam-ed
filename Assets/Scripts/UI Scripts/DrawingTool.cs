using UnityEngine;

public class DrawingTool : MonoBehaviour
{

    public PaintScript paintScript;
    public Color color = Color.green;
    
    public float width = 0.2f;

    public string layer = "Pencil";
    public Material brush;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        brush = new Material(Shader.Find("Sprites/Default"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        paintScript.tool = this;
    }
}
