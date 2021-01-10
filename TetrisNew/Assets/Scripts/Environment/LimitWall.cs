using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(MeshRenderer))]
public class LimitWall : MonoBehaviour
{
    //насколько сдвинуть от стены
    [SerializeField] private float _side;
    [SerializeField] private float _stepAlpha = 0.075f;
    [SerializeField] private float _delayrUpdateColor = 0.05f;

    private MeshRenderer _meshRenderer;
    private AudioSource _as;
    private Color _color;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _as = GetComponent<AudioSource>();
        
        _color = _meshRenderer.sharedMaterial.color;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<FigureOptions>())
        {
            Vector3 figurePosition = other.gameObject.transform.position;
            other.gameObject.transform.position = new Vector3
            (
                figurePosition.x + _side,
                figurePosition.y,
                figurePosition.z
            );
            
            _as.volume = SaveSome.GetParameter(PrefsSave.VolumeEffects, 0);
            _as.Play();
            StartCoroutine(StartAnimationUp());
        }
    }

    private IEnumerator StartAnimationUp()
    {
        while (_color.a < 1)
        {
            ChangeAlpha(_stepAlpha);
            yield return  new WaitForSeconds(_delayrUpdateColor);
        }
        
        StartCoroutine(StartAnimationDown());
    }
    
    private IEnumerator StartAnimationDown()
    {
        while (_color.a > 0)
        {
            ChangeAlpha(-_stepAlpha);
            yield return  new WaitForSeconds(_delayrUpdateColor);
        }
    }

    private void ChangeAlpha(float stepAlpha)
    {
        _color.a += stepAlpha;
        _meshRenderer.sharedMaterial.color = new Color(_color.r,_color.g, _color.b, _color.a);
    }
}
