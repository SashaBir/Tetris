using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class AnimatorNextLVL : MonoBehaviour
{
    [SerializeField] private Text _counterLVLPlayerText;
    [SerializeField] private Image _imageLVL;
    [SerializeField] private float _durationBefor = 2f;
    [SerializeField] private float _durationAfter = 1f;
    [SerializeField] private float _durationBetween = 1f;
    [SerializeField] private float _durationFade = 3f;
    [SerializeField] private float _stepAlphaImage = 0.3f;

    private AudioSource _as;
    private Color _colorImage;
    

    private void Start()
    {
        _as = GetComponent<AudioSource>();
        _colorImage = _imageLVL.color;
    }
    
    internal void StartAnimation(float lvlPlayer)
    {
        _as.Play();
        _counterLVLPlayerText.text = lvlPlayer.ToString();
        StartCoroutine(AnimationImageUp());
        StartCoroutine(AnimationText());
    }

    private IEnumerator AnimationImageUp()
    {
        _imageLVL.gameObject.SetActive(true);
        while (_colorImage.a < 1)
        {
            ChangeAlpha(_stepAlphaImage);
            yield return  new WaitForSeconds(_durationFade);
        }
        StartCoroutine(AnimationImageDown());
    }
    
    private IEnumerator AnimationImageDown()
    {
        while (_colorImage.a > 0)
        {
            ChangeAlpha(-_stepAlphaImage);
            yield return  new WaitForSeconds(_durationFade);
        }
        _imageLVL.gameObject.SetActive(false);
    }

    private IEnumerator AnimationText()
    {
        _counterLVLPlayerText.gameObject.SetActive(true);
        _counterLVLPlayerText.CrossFadeAlpha(0.0f, 0.0f, true);
        _counterLVLPlayerText.CrossFadeAlpha(1.0f, _durationBefor, false);
        yield return  new WaitForSeconds(_durationBetween);
        _counterLVLPlayerText.CrossFadeAlpha(0.0f, _durationAfter, true);
        yield return  new WaitForSeconds(_durationAfter);
        _counterLVLPlayerText.gameObject.SetActive(false);
    }

    private void ChangeAlpha(float stepAlpha)
    {
        _colorImage.a += stepAlpha;
        _imageLVL.color = _colorImage;
    }

}
