using UnityEngine;

public class ClearPad : MonoBehaviour
{
    //Fields
    /// <summary>
    /// Reference to AudioSystem
    /// </summary>
    [SerializeField]
    [Tooltip ("Reference to AudioSystem")]
    private AudioSource _audioSystem;

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
