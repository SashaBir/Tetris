using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CheckerLimitHeight : MonoBehaviour
{
    [SerializeField] private ReloaderGame _reloaderGame;

    private AudioSource _as;
    
    private void Start()
    {
        _as = GetComponent<AudioSource>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<FigureOptions>())
        {
            if (other.gameObject.GetComponent<IRigibody>().IsUseGravity())
            {
                _as.volume = SaveSome.GetParameter(PrefsSave.VolumeEffects, 0);
                _as.Play();
                _reloaderGame.ReloadAndExit();
            }
        }
    }
}
