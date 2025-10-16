using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// The active sound to play.
/// Generic name-a random selection between its varients.
/// Numbered name-that specific audio clip.
/// </summary>
public enum ActiveSoundName
{
    erasing,          //Eraser erasing something
    erasing1,
    erasing2,
    erasing3,
    erasing4,

    footsteps,       //Footsteps 
    footsteps1,
    footsteps2,
    footsteps3,
    footsteps4,

    growling,        //A deep, low, roar
    growling1,
    growling2,
    growling3,

    highlighter,    //A highlighter marking something
    highlighter1,
    highlighter2,
    highlighter3,
    highlighter4,

    paper,          //Paper being ripped
    paper1,
    paper2,
    paper3,
    paper4,

    pencil,         //A pencil writing something
    pencil1,
    pencil2,
    pencil3,
    pencil4,

    repairing,      //Metallic objects being hit together
    repairing1,
    repairing2,

    sticker,        //Stickers being placed
    sticker1,
    sticker2,
    sticker3,
    sticker4
};

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

    /// <summary>
    /// Clips of an eraser being used.
    /// </summary>
    [SerializeField]
    [Tooltip("Clips of an eraser being used.")]
    private List<AudioClip> _eraser;

    /// <summary>
    /// Clips of footsteps
    /// </summary>
    [SerializeField]
    [Tooltip("Clips of footsteps.")]
    private List<AudioClip> _footsteps;

    /// <summary>
    /// Clips of growling
    /// </summary>
    [SerializeField]
    [Tooltip("Clips of growling.")]
    private List<AudioClip> _growl;

    /// <summary>
    /// Clips of a highlighter.
    /// </summary>
    [SerializeField]
    [Tooltip("Clips of a highlighter.")]
    private List<AudioClip> _highlighter;

    /// <summary>
    /// Clips of paper being ripped.
    /// </summary>
    [SerializeField]
    [Tooltip("Clips of paper being ripped.")]
    private List<AudioClip> _paper;

    /// <summary>
    /// Clips of a pencil being used.
    /// </summary>
    [SerializeField]
    [Tooltip("Clips of a pencil being used.")]
    private List<AudioClip> _pencil;

    /// <summary>
    /// Clips of a barricade being repaired.
    /// </summary>
    [SerializeField]
    [Tooltip("Clips of a barricade being repaired.")]
    private List<AudioClip> _repair;

    /// <summary>
    /// Clips of a sticker being placed.
    /// </summary>
    [SerializeField]
    [Tooltip("Clips of a sticker being placed.")]
    private List<AudioClip> _sticker;

    /// <summary>
    /// A new temporary audio source.
    /// </summary>
    [SerializeField]
    [Tooltip ("A new temporary audio source.")]
    private AudioSource _tempAS;

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
        PlayAutoSound();
    }

    /// <summary>
    /// Advances the 
    /// </summary>
    /// <param name="time"></param>
    public void AdvanceAudio(float time)
    {
        _audioTime += time;
    }

    /// <summary>
    /// Play an ambient track/noise if conditions are met.
    /// </summary>
    public void PlayAutoSound()
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

    /// <summary>
    /// Generic name-a random selection between its varients.
    /// Numbered name-that specific audio clip.
    /// Play an audio caused by a specific event.
    /// </summary>
    public void PlayActiveAudio(ActiveSoundName sound)
    {
        AudioSource aS = Instantiate(_tempAS, Vector3.zero, Quaternion.identity);
        int index = 0;

        //If the number is not specified, play a random sound of that type.
        //If the number is specified, play that specific sound.
        switch (sound)
        {
            case ActiveSoundName.erasing:
                index = Random.Range(0, _eraser.Count);
                aS.clip = _eraser[index];
                break;
            case ActiveSoundName.erasing1:
                aS.clip = _eraser[0];
                break;
            case ActiveSoundName.erasing2:
                aS.clip = _eraser[1];
                break;
            case ActiveSoundName.erasing3:
                aS.clip = _eraser[2];
                break;
            case ActiveSoundName.erasing4:
                aS.clip = _eraser[3];
                break;

            case ActiveSoundName.footsteps:
                index = Random.Range(0, _footsteps.Count);
                aS.clip = _footsteps[index];
                break;
            case ActiveSoundName.footsteps1:
                aS.clip = _footsteps[0];
                break;
            case ActiveSoundName.footsteps2:
                aS.clip = _footsteps[1];
                break;
            case ActiveSoundName.footsteps3:
                aS.clip = _footsteps[2];
                break;
            case ActiveSoundName.footsteps4:
                aS.clip = _footsteps[3];
                break;

            case ActiveSoundName.growling:
                index = Random.Range(0, _growl.Count);
                aS.clip = _growl[index];
                break;
            case ActiveSoundName.growling1:
                aS.clip = _growl[0];
                break;
            case ActiveSoundName.growling2:
                aS.clip = _growl[1];
                break;
            case ActiveSoundName.growling3:
                aS.clip = _growl[2];
                break;

            case ActiveSoundName.highlighter:
                index = Random.Range(0, _highlighter.Count);
                aS.clip = _highlighter[index];
                break;
            case ActiveSoundName.highlighter1:
                aS.clip = _highlighter[0];
                break;
            case ActiveSoundName.highlighter2:
                aS.clip = _highlighter[1];
                break;
            case ActiveSoundName.highlighter3:
                aS.clip = _highlighter[2];
                break;
            case ActiveSoundName.highlighter4:
                aS.clip = _highlighter[3];
                break;

            case ActiveSoundName.paper:
                index = Random.Range(0, _paper.Count);
                aS.clip = _paper[index];
                break;
            case ActiveSoundName.paper1:
                aS.clip = _paper[0];
                break;
            case ActiveSoundName.paper2:
                aS.clip = _paper[1];
                break;
            case ActiveSoundName.paper3:
                aS.clip = _paper[2];
                break;
            case ActiveSoundName.paper4:
                aS.clip = _paper[3];
                break;

            case ActiveSoundName.pencil:
                index = Random.Range(0, _pencil.Count);
                aS.clip = _pencil[index];
                break;
            case ActiveSoundName.pencil1:
                aS.clip = _pencil[0];
                break;
            case ActiveSoundName.pencil2:
                aS.clip = _pencil[1];
                break;
            case ActiveSoundName.pencil3:
                aS.clip = _pencil[2];
                break;
            case ActiveSoundName.pencil4:
                aS.clip = _pencil[3];
                break;

            case ActiveSoundName.repairing:
                index = Random.Range(0, _repair.Count);
                aS.clip = _repair[index];
                break;
            case ActiveSoundName.repairing1:
                aS.clip = _repair[0];
                break;
            case ActiveSoundName.repairing2:
                aS.clip = _repair[1];
                break;

            case ActiveSoundName.sticker:
                index = Random.Range(0, _sticker.Count);
                aS.clip = _sticker[index];
                break;
            case ActiveSoundName.sticker1:
                aS.clip = _sticker[0];
                break;
            case ActiveSoundName.sticker2:
                aS.clip = _sticker[1];
                break;
            case ActiveSoundName.sticker3:
                aS.clip = _sticker[2];
                break;
            case ActiveSoundName.sticker4:
                aS.clip = _sticker[3];
                break;
        }

        aS.Play();


    }
}
