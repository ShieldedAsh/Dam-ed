using UnityEngine;

public class ClearPad : MonoBehaviour
{
    public PaintScript drawPad;
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
        foreach(Transform child in drawPad.transform)
        {
            Destroy(child.gameObject);
        }
        drawPad.drawOrder = -1;
    }
}
