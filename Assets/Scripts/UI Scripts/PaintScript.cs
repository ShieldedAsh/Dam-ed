using UnityEngine;
using UnityEngine.InputSystem;

public class PaintScript : MonoBehaviour
{
    //These public variables can be changed freely
    public Color brushColor = Color.red;
    public float width = 0.2f;
    public Material brush;

    //These are four private variables that get the position of the mouse and assign where to be drawing the line
    Vector3 mousePos;
    Vector3 screenpoint;
    LineRenderer brushLine;

    //A boolean to make sure the lines don't go off the page
    bool offGrid = false;

    //integers and strings for Drawing Layers
    int drawOrder = -1;
    public string layerName = "Pencils";

    GameObject child;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        brush = new Material(Shader.Find("Sprites/Default"));
    }

    // Update is called once per frame
    void Update()
    {
    }

    //Generates the start of the line and assigns the initial components
    private void OnMouseDown()
    {
        drawOrder++;
        offGrid = false;
        //When clicked, it gets the current mouse position and translates it to the worldpoint
        screenpoint = Mouse.current.position.ReadValue();
        screenpoint.z = 10.0f;
        mousePos = Camera.main.ScreenToWorldPoint(screenpoint);

        //This makes the new line
        child = new GameObject();
        child.transform.SetParent(transform);
        child.transform.name = "Line";
        brushLine = child.AddComponent<LineRenderer>();

        // This assigns the material, color, and width of the new line
        brushLine.material = new Material(brush);
        brushLine.startColor = brushColor;
        brushLine.endColor = brushColor;
        brushLine.startWidth = width;
        brushLine.endWidth = width;

        //This sets the first position of the line
        brushLine.positionCount = 1;
        brushLine.SetPosition(0, mousePos);

        brushLine.sortingOrder = drawOrder;
        brushLine.sortingLayerName = layerName;
    }

    private void OnMouseDrag()
    {
        //Every frame, it gets the translated worldpoint position
        screenpoint = Mouse.current.position.ReadValue();
        screenpoint.z = 10.0f;
        mousePos = Camera.main.ScreenToWorldPoint(screenpoint);

        
        //This checks whether the mouse is still drawing on the pad, and cuts off the line if it isn't.
        if (this.GetComponent<Collider2D>().bounds.Contains(new Vector2(mousePos.x, mousePos.y)))
        {
            //This if statement is used to restart the line after it gets cut off whenever the player leaves the drawing bounds.
            if (offGrid)
            {
                OnMouseDown();
            }
            //Then adds another position to the line wherever the mouse is
            brushLine.positionCount++;
            brushLine.SetPosition(brushLine.positionCount - 1, mousePos);
        }
        else
        {
            //Cuts off the line if not in-bounds

            //Simplifies the line to reduce load and assigns it a collider so it can be deleted
            if(brushLine != null)
            {
                brushLine.Simplify(0.02f);
                
            }
            
            brushLine = null;
            offGrid = true;
        }
    }

    private void OnMouseUp()
    {
        if(brushLine != null)
        {
            brushLine.Simplify(0.02f);
            brushLine = null;
        }
    }

}
