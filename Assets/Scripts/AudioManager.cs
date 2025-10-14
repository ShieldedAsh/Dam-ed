using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// The audio system for Dam-ed.
/// </summary>
public class AudioSystem : MonoBehaviour
{
    //Fields
    /// <summary>
    /// A number that determines what track/sound plays when.
    /// </summary>
    [SerializeField]
    [Tooltip ("A number that determines what track/sound plays when.")]
    private float _nextAudio;

    /// <summary>
    /// The amound of time since the game has been opened, in seconds
    /// </summary>
    [SerializeField]
    [Tooltip("The amound of time since the game has been opened, in seconds")]
    private float _audioTime;

    /// <summary>
    /// A reference to the BeaverManager gameObject.
    /// </summary>
    [SerializeField]
    [Tooltip ("A reference to the BeaverManager gameObject.")]
    private GameObject _beaverManager;

    /// <summary>
    /// The number of beavers left the players know about.
    /// </summary>
    [SerializeField]
    [Tooltip ("The number of beavers left the players know about.")]
    private int _knownBeaversLeft;

    /// <summary>
    /// Jumpscare-ish tracks such as panic1-4
    /// </summary>
    [SerializeField]
    [Tooltip ("Jumpscare-ish tracks such as panic1-4.")]
    private List<AudioClip> _jumpscareMus;

    /// <summary>
    /// Regular music that plays during gameplay.
    /// </summary>
    [SerializeField]
    [Tooltip ("Regular music that plays during gameplay.")]
    private List<AudioClip> _normalMus;

    /// <summary>
    /// Ambient sounds that play during gamplay.
    /// </summary>
    [SerializeField]
    [Tooltip ("Ambient sounds that play during gamplay.")]
    private List<AudioClip> _ambientSounds;

    /// <summary>
    /// Tracks corresponding to the amount of living beavers the player knows about."
    /// </summary>
    [SerializeField]
    [Tooltip ("Tracks corresponding to the amount of living beavers the player knows about.")]
    private List<AudioClip> _deadBeaverTracks;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetNextAudioTime();
    }

    // Update is called once per frame
    void Update()
    {
        //Updating _knownBeaversLeft each frame
        //Should probably change this to whenever the _knownBeaversLeft changes, but meh
        //_knownBeaversLeft = _beaverManager.GetComponent<BeaverManager>()._knownBeaversLeft;

        AdvanceAudio(Time.deltaTime);
    }

    /// <summary>
    /// Advances the 
    /// </summary>
    /// <param name="time"></param>
    public void AdvanceAudio(float time)
    {
        _audioTime += time;
        PlaySound();
    }

    /// <summary>
    /// Play an ambient track/noise if conditions are met.
    /// </summary>
    public void PlaySound()
    {
        AudioSource aS = gameObject.GetComponent<AudioSource>();

        //Checks if enough time has passed and a sound isn't already being played.
        if (_audioTime >= _nextAudio && !aS.isPlaying)
        {
            _audioTime -= _nextAudio;
            //Roll 1d4.
            //1-Plays a random track from panic1-4
            //2-Plays a random regular track
            //3-Plays an ambient sound
            //4-Plays the mass beaver death track.
            int die = Random.Range(1, 5);
            int index = 0;

            switch (die)
            {
                case 1:
                    index = Random.Range(0, _jumpscareMus.Count);
                    aS.clip = _jumpscareMus[index];
                    break;

                case 2:
                    index = Random.Range(0, _normalMus.Count);
                    aS.clip = _normalMus[index];
                    break;

                case 3:
                    index = Random.Range(0, _ambientSounds.Count);
                    aS.clip = _ambientSounds[index];
                    break;

                case 4:
                    index = _knownBeaversLeft - 1;
                    aS.clip = _deadBeaverTracks[index];
                    break;
            }

            //Put volume/other settings here if needed.
            aS.Play();
            
            //If _audioTime is greater than _nextAudio, then a sound will always play one after another. This can be
            //avoided by setting _audioTime to 0.
            if(_audioTime > _nextAudio)
            {
                _audioTime = 0;
            }

            GetNextAudioTime();
        }
    }

    /// <summary>
    /// Gets the next audio time.
    /// </summary>
    public void GetNextAudioTime()
    {
        //A random amount of time, from 0-15 minutes, in seconds.
        //30s is 30f, 15min is 900f
        _nextAudio = Random.Range(0f, 900f);
    }
}
