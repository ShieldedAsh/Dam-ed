using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// When this is attached to a button and the button is clicked, it will change the scene to the main menu.
/// </summary>
public class ToMainMenu : MonoBehaviour
{
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
        SceneManager.LoadScene("Main_Menu");
    }
}
