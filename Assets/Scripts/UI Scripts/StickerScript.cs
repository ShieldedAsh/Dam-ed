using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Switch;

public class StickerScript : MonoBehaviour
{
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
        //Creates a new child object, gives it a sprite renderer, then sets that sprite to the provided sprite.
        child = new GameObject();
        child.transform.SetParent(transform);
        child.transform.AddComponent<SpriteRenderer>();
        stickersprite = child.GetComponent<SpriteRenderer>();
        stickersprite.sprite = sprite;
        stickersprite.color = Color.white;
        
    }

    //Moves the sticker according the mouse position
    public void OnMouseDrag()
    {
        screenpoint = Mouse.current.position.ReadValue();
        screenpoint.z = 1.0f;
        mousePos = Camera.main.ScreenToWorldPoint(screenpoint);

        child.transform.position = mousePos;
    }

    //Destroys the child if they try to place the sticker outside of the bounding box
    public void OnMouseUp()
    {
        if(!dropField.bounds.Contains(new Vector2(mousePos.x, mousePos.y))){
            Destroy(child);
        }
    }
}
