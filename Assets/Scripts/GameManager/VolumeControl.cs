using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{

    [SerializeField]
    private Slider volSlider;

    public void SetBGMVol()
    {

        SoundManager.Instance.SetBGMVol(volSlider.value);

    }

    public void SetSFXVol()
    {

        SoundManager.Instance.SetSFXVol(volSlider.value);

    }

}
