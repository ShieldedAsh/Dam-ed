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

    /// <summary>
    /// Loads the Main_Menu scene when the main menu button is clicked.
    /// </summary>
    public void MainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }
}
