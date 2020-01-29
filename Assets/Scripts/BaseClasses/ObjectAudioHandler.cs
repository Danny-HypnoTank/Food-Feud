/*
 * Created by:
 * Name: James Sturdgess
 * Sid: 1314371
 * Date Created: 24/10/2019
 * Last Modified 24/10/2019
 * Modified By:
 */
using System;
using UnityEngine;

/// <summary>
/// Class for handling audio on objects
/// </summary>
public class ObjectAudioHandler : MonoBehaviour
{

    /// <summary>
    /// Array to store audio
    /// </summary>
    [SerializeField]
    private Audio[] sfx;

    /// <summary>
    /// Property for the audio source
    /// </summary>
    public AudioSource Audio { get; private set; }

    void Start()
    {

        Audio = GetComponent<AudioSource>();

    }

    /// <summary>
    /// Method to play sound effects
    /// </summary>
    /// <param name="name">Name of the sound effect to play</param>
    public void SetSFX(string name)
    {

        if (SoundManager.Instance != null)
        {
            if (!SoundManager.Instance.Mute)
            {

                //Get the sound effect specified in the parameter
                Audio a = Array.Find(sfx, sound => sound.Name == name);
                if (a != null)
                {
                    Audio.clip = a.GetAudio();

                    //Play sound effect through the SoundManager
                    SoundManager.Instance.PlaySFX(Audio);
                }
            }
        }

    }

    public void StopSFX(string name)
    {

        //Get the sound effect specified in the parameter
        Audio a = Array.Find(sfx, sound => sound.Name == name);
        if (a != null)
            Audio.clip = a.GetAudio();

        //Play sound effect through the SoundManager
        SoundManager.Instance.StopSFX(Audio);

    }

}
