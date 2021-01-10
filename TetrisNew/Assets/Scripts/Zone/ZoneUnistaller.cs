using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class ZoneUnistaller : MonoBehaviour
{
    [SerializeField] private float _delayBeforDelete = 0.3f;
    [SerializeField] private float _delaySplitFigure = 0.3f;
    
    private AudioSource _as;
    private List<GameObject> _figures;

    private void OnEnable()
    {
        _as = GetComponent<AudioSource>();
        _figures = new List<GameObject>();
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        _as.Play();
        yield return new WaitForSeconds(_delayBeforDelete);
        SplitFigures();
        yield return new WaitForSeconds(_delaySplitFigure);
        gameObject.SetActive(false);
    }

    private void AddNewFigure(GameObject gO)
    {
        if (!_figures.Contains(gO))
        {
            _figures.Add(gO);
        }
    }

    private void SplitFigures()
    {
        foreach (GameObject item in _figures)
        {
            StartCoroutine( item.GetComponent<ICounterNumberOfCubesInFugre>().СountNumberOfCubesInFigure(_delaySplitFigure));
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Cube")
        {
            if (other.gameObject.transform.parent.GetComponent<FigureOptions>())
            {
                if (other.gameObject.transform.parent.GetComponent<IRigibody>().IsUseGravity())
                {
                    FoundCube(other);
                }
            }

            if (other.gameObject.transform.parent.GetComponent<ReloadFigure>())
            {
                FoundCube(other);
            }
        }
    }

    private void FoundCube(Collider other)
    {
        AddNewFigure(other.gameObject.transform.parent.gameObject);
        Buffer.BuffSome(PrefsSave.AllCubesDeleteInfo);
        Destroy(other.gameObject);
    }
}
