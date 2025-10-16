using System.Collections.Generic;
using UnityEngine;

public class WolfManager : MonoBehaviour
{
    //Fields
    [SerializeField]
    [Tooltip ("The left hand audio source for growling.")]
    private GameObject _leftGrowlManager;

    [SerializeField]
    [Tooltip("The right hand audio source for growling.")]
    private GameObject _rightGrowlManager;

    [SerializeField]
    [Tooltip("Reference to the audio system")]
    private AudioSource _audioSystem;

    private List<Wolf> wolves;
    private int wolfCount;
    private DamGroup theDam;

    public DamGroup TheDam { get => theDam; }

    public int WolfCount { get => wolfCount; set => wolfCount = value; }

    public List<Wolf> Wolves { get { return wolves; } }

    private void Start()
    {
        theDam = FindAnyObjectByType<DamGenerator>().Dam;
        wolfCount = 30;
        wolves = new List<Wolf>();
        //should increase how many cells are excluded
        List<DamCell> exclude = new List<DamCell>() { theDam.HQ };

        for (int i = 0; i < wolfCount; i++)
        {
            wolves.Add(new Wolf(this, theDam.GetRandomCell(exclude)));
            wolves[i].ChangeTarget(theDam.GetRandomCell(exclude));
        }
    }

    private void Update()
    {
        foreach (Wolf wolf in wolves)
        {
            if (wolf.MainTarget == null)
            {
                wolf.ChangeTarget(theDam.GetRandomCell(new List<DamCell> { TheDam.HQ }));
            }
            wolf.UpdateWolf();
        }
    }

    /// <summary>
    /// Makes the AudioSystem play a random growling noise when a wolf is at either door to the HQ.
    /// </summary>
    /// <param name="right">Whether or not the wolf is at the right side.</param>
    public void Growl(bool right)
    {
        if(right)
        {
            _audioSystem.panStereo = 1;
        }
        else
        {
            _audioSystem.panStereo = -1;
        }

        //Plays a random growling sound
        _audioSystem.GetComponent<AudioSystem>().PlayActiveAudio(ActiveSoundName.growling);
        _audioSystem.panStereo = 0;
    }
}
