/*
 * Created by:
/*
 * Created by:
 * Name: James Sturdgess
 * Sid: 1314371
 * Date Created: 17/10/2019
 * Last Modified 26/10/2019
 * Modified By: Dominik Waldowski,
 *              James Sturdgess
 */
using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Class for the Sound Manager
/// </summary>
public class SoundManager : MonoBehaviour
{

    /// <summary>
    /// Backing field for <see cref="Instance"/>
    /// </summary>
    private static SoundManager _instance;

    /// <summary>
    /// Property for the Music Volume
    /// </summary>
    /// 
    [SerializeField]
    private float musicVol;

    /// <summary>
    /// Property for the Sound Volume
    /// </summary>
    [SerializeField]
    private float soundVol;

    /// <summary>
    /// Property for the Master Volume
    /// </summary>
    [SerializeField]
    private float masterVol;

    /// <summary>
    /// Field for the tempo of the music
    /// </summary>
    private float tempo;
    [SerializeField]
    private bool mute;

    /// <summary>
    /// Array to store audio
    /// </summary>
    [SerializeField]
    private Audio[] backgroundM;

    /// <summary>
    /// Property for getting the Instance of the SoundManager. Backing Field: <see cref="_instance"/>
    /// </summary>
    public static SoundManager Instance { get { return _instance; } }

    public float MusicVol { get => musicVol; set => musicVol = value; }
    public float SoundVol { get => soundVol; set => soundVol = value; }
    public float MasterVol { get => masterVol; set => masterVol = value; }
    public bool Mute { get => mute; set => mute = value; }

    [SerializeField]
    private List<AudioSource> allMusic = new List<AudioSource>();
    void Awake()
    {

        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

        foreach (Audio a in backgroundM)
        {
            // GameObject newSound = new GameObject("SoundObj");
            //  a.SetSource(newSound.AddComponent<AudioSource>());
            //  newSound.transform.SetParent(this.gameObject.transform);
            a.SetSource(this.gameObject.AddComponent<AudioSource>());
            a.Source.clip = a.GetAudio();
        }

        mute = false;

        //TODO: Load values from save once implemented
        SetBGMVol(1);
        SetSFXVol(1);

        DontDestroyOnLoad(gameObject);

    }

    #region Dom's additions
    public void PlayMainTheme()
    {
        SetBGM("Main Menu");
    }

    public void PlayGameTheme()
    {
        SetBGM("Game Theme");
    }
    #endregion

    /// <summary>
    /// Method to set whether or not the audio is muted
    /// </summary>
    /// <param name="m">The value to set <see cref="Mute"/> to</param>
    public void SetMute(bool m)
    {

        Mute = m;
        foreach (Audio a in backgroundM)
            a.Source.mute = m;

    }

    /// <summary>
    /// Method to set the Master Volume
    /// </summary>
    /// <param name="vol">Value to set <see cref="MasterVol"/> to</param>
    public void SetMasterVol(float vol)
    {

        MasterVol = vol;

        //If Master Volume is lower than the Music Volume, use the Master Volume. Else use the Music Volume.
        if (MasterVol < MusicVol)
            foreach (Audio a in backgroundM)
                a.Source.volume = MasterVol;
        else if (MasterVol > MusicVol)
            foreach (Audio a in backgroundM)
                a.Source.volume = MusicVol;


    }

    /// <summary>
    /// Method to set the volume of the music
    /// </summary>
    /// <param name="v">Value to set the volume to</param>
    public void SetBGMVol(float v)
    {
        MusicVol = v;

        foreach (Audio a in backgroundM)
            a.Source.volume = MusicVol;
       // Debug.Log(MusicVol);
    }

    /// <summary>
    /// Method to set the volume of the sounds
    /// </summary>
    /// <param name="v">Value to set the volume to</param>
    public void SetSFXVol(float v)
    {

        SoundVol = v;

    }

    /// <summary>
    /// Method to set and play a music track
    /// </summary>
    /// <param name="name">Name of the track</param>
    public void SetBGM(string name)
    {

        //Find the currently playing music
        Audio playing = Array.Find(backgroundM, sound => sound.Source.isPlaying);
        //Find the music specified in the parameter
        Audio toPlay = Array.Find(backgroundM, sound => sound.Name == name);
        if (toPlay != null && playing != null)
            StartCoroutine(CrossFade(playing.Source, toPlay.Source, 0.5f, 20));
        else if (toPlay != null)
            toPlay.Source.Play();

        SetBGMTempo(1);

    }

    public void PlaySFX(AudioSource src)
    {

        src.volume = SoundVol;
        src.PlayOneShot(src.clip);

    }
    
    public void StopSFX(AudioSource src)
    {

        src.Stop();

    }

    /// <summary>
    /// Method to set the tempo of the music
    /// </summary>
    /// <param name="t">The value to set the tempo to</param>
    public void SetBGMTempo(float t)
    {

        tempo = t;

        //Find the currently playing music and change the pitch
        Audio x = Array.Find(backgroundM, sound => sound.Source.isPlaying);
        if (x != null)
            x.Source.pitch = tempo;

    }

    /// <summary>
    /// IEnumerator to control the crossfade between tracks
    /// </summary>
    /// <param name="a">The AudioSource to fade out</param>
    /// <param name="b">The AudioSource to fade in</param>
    /// <param name="seconds">How many seconds the fade takes</param>
    /// <param name="steps">How many times the loop needs to run through</param>
    private IEnumerator CrossFade(AudioSource a, AudioSource b, float seconds, float steps)
    {

        b.volume = 0;

        //Find how many seconds there will be between each volume step
        float step = seconds / steps;
        //Calculate the value that the volume needs to change by with each step
        float volInt = MusicVol / steps;

        b.Play();

        for (int i = 0; i < steps; i++)
        {

            a.volume -= volInt;
            b.volume += volInt;

            yield return new WaitForSeconds(step);

        }

        a.Stop();

    }
}