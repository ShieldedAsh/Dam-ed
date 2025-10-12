using UnityEngine;
using UnityEngine.InputSystem;

public class MoveSticker : MonoBehaviour
{
    public Collider2D dropField;

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

    private void OnMouseDrag()
    {
        screenpoint = Mouse.current.position.ReadValue();
        screenpoint.z = 1.0f;
        mousePos = Camera.main.ScreenToWorldPoint(screenpoint);

        this.transform.position = mousePos;
        if (!dropField.bounds.Contains(new Vector2(mousePos.x, mousePos.y)))
        {
            this.transform.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            this.transform.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    private void OnMouseUp()
    {
        if (!dropField.bounds.Contains(new Vector2(mousePos.x, mousePos.y)))
        {
            Destroy(gameObject);
        }
    }
}
