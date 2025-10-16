using Unity.VisualScripting;
using UnityEngine;

public class DrawingTool : MonoBehaviour
{
    //Fields
    /// <summary>
    /// Reference to the audio system
    /// </summary>
    [SerializeField]
    [Tooltip ("Reference to the audio system")]
    private AudioSource _audioSystem;

    public PaintScript paintScript;
    public Color color = Color.green;
    
    public float width = 0.2f;

    public string layer = "Pencil";
    public Material brush;

    public Vector3 restPos;
    Vector3 targetPos;

    public float speed = 0.01f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        brush = new Material(Shader.Find("Sprites/Default"));
        restPos = transform.position;
        targetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position != targetPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed);
        }
    }

    private void OnMouseDown()
    {
        paintScript.layerName = layer;
        paintScript.tool = this;
        targetPos = restPos + Vector3.left * 0.4f;
        this.transform.parent.BroadcastMessage("ToolChanged");
    }

    private void OnMouseEnter()
    {
        if(paintScript.tool != this)
        {
            targetPos = restPos + Vector3.left * 0.2f;
        }
        
    }
    private void OnMouseExit()
    {
        if( paintScript.tool != this)
        {
            targetPos = restPos;
        }

    }

    public void OnMouseUp()
    {
        string tempLayer = layer;
        tempLayer = tempLayer.ToLower();

        if(tempLayer == "pencils")
        {
            _audioSystem.GetComponent<AudioSystem>().PlayActiveAudio(ActiveSoundName.pencil);
        }
        else
        {
            _audioSystem.GetComponent<AudioSystem>().PlayActiveAudio(ActiveSoundName.highlighter);
        }
    }



    private void ToolChanged()
    {
        OnMouseExit();
    }
}
