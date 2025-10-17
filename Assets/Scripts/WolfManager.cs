using System.Collections.Generic;
using UnityEngine;

public class WolfManager : MonoBehaviour
{
    //Fields
    [SerializeField]
    [Tooltip("Reference to the audio system")]
    private AudioSource _audioSystem;

    private List<Wolf> wolves;
    [SerializeField] private int wolfCount;
    private DamGroup theDam;
    [SerializeField] private bool generateWolves;

    public DamGroup TheDam { get => theDam; }

    public int WolfCount { get => wolfCount; set => wolfCount = value; }

    public List<Wolf> Wolves { get { return wolves; } }

    private void Start()
    {
        theDam = FindAnyObjectByType<DamGenerator>().Dam;
        if (wolfCount <= 0 && generateWolves)
        {
            wolfCount = 2;
        }
        if (!generateWolves)
        {
            wolfCount = 0;
        }
        wolves = new List<Wolf>();
        //should increase how many cells are excluded

        for (int i = 0; i < wolfCount; i++)
        {
            List<DamCell> exclude = new List<DamCell>() { theDam.HQ };
            wolves.Add(new Wolf(this, theDam.GetRandomCell(exclude)));
            exclude.Add(wolves[i].CurrentLocation);
            wolves[i].ChangeTarget(theDam.GetRandomCell(exclude));
        }
    }

    private void Update()
    {
        foreach (Wolf wolf in wolves)
        {
            if (wolf.MainTarget == null)
            {
                wolf.ChangeTarget(theDam.GetRandomCell(new List<DamCell> { TheDam.HQ, wolf.CurrentLocation }));
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
