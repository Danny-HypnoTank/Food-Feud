using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Audio
{

    [SerializeField]
    private string _name;
    public string Name
    {
        get { return _name; }
        private set { _name = value; }
    }

    [SerializeField]
    private AudioClip _audio;

    public AudioClip GetAudio()
    { return _audio; }
    private void SetAudio(AudioClip value)
    { _audio = value; }

    private AudioSource _source;
    public AudioSource Source
    {

        get { return _source; }

    }

    public void SetSource(AudioSource AS)
    {

        _source = AS;

    }

}
