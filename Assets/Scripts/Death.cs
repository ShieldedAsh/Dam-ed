using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages behavior around the player dying.
/// </summary>
public class Death : MonoBehaviour
{
    //Fields
    [SerializeField]
    [Tooltip("Reference to the audio system")]
    private AudioSource _audioSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "Death")
        {
            _audioSystem.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Function that's called when the player dies.
    /// Changes the scene to the death scene.
    /// </summary>
    public void PlayerDeath()
    {
        SceneManager.LoadScene("Death");
    }
}
