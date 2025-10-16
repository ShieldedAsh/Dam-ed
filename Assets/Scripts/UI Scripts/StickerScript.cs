using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Switch;

public class StickerScript : MonoBehaviour
{
    /// <summary>
    /// Reference to the audio system
    /// </summary>
    [SerializeField]
    [Tooltip("Reference to the audio system")]
    private AudioSource _audioSystem;

    //The area within which stickers are allowed to be placed
    public Collider2D dropField;
    //The sprite of the sticker
    public Sprite sprite;
    private SpriteRenderer stickersprite;
    GameObject child;
    
    

    Vector3 screenpoint;
    Vector3 mousePos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        //Creates a new child object
        child = new GameObject();
        //Assigns its parent to being the drawing pad
        child.transform.SetParent(dropField.gameObject.transform);

        //Gives it a spriteRenderer, names it, and adds the box collider and script to move it.
        child.transform.AddComponent<SpriteRenderer>();
        stickersprite = child.GetComponent<SpriteRenderer>();
        stickersprite.sprite = sprite;
        stickersprite.color = Color.white;
        child.transform.name = this.name;
        child.transform.AddComponent<BoxCollider2D>();
        child.transform.AddComponent<MoveSticker>();
        child.transform.GetComponent<MoveSticker>().dropField = dropField;
        child.transform.GetComponent<SpriteRenderer>().sortingLayerName = "Stickers";
        
    }

    //Moves the sticker according the mouse position
    public void OnMouseDrag()
    {
        screenpoint = Mouse.current.position.ReadValue();
        screenpoint.z = 1.0f;
        mousePos = Camera.main.ScreenToWorldPoint(screenpoint);

        child.transform.position = mousePos;

        //Displays whether the sticker is within bounds, tinting it red if out-of-bounds
        if (!dropField.bounds.Contains(new Vector2(mousePos.x, mousePos.y)))
        {
            child.transform.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            child.transform.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    //Destroys the child if they try to place the sticker outside of the bounding box
    public void OnMouseUp()
    {
        if(!dropField.bounds.Contains(new Vector2(mousePos.x, mousePos.y))){
            Destroy(child);
        }

        //Plays a random sticker sound
        _audioSystem.GetComponent<AudioSystem>().PlayActiveAudio(ActiveSoundName.sticker);
    }
}
