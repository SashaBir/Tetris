using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class StarterGame : MonoBehaviour
{
    [SerializeField] private Text[] _texts;
    [SerializeField] private float _delayLoadPanel = 0.025f;
    [SerializeField] private float _delayUnLoadPane = 0.05f;
    [SerializeField] private float _alphaPlus = 0.075f;

    public UnityEvent OneAnimationEnd;
    
    private Image _imagePanel;
    private float _alpha = 0;
    private bool _isStartGame = true;

    private void OnEnable()
    {
        _imagePanel = GetComponent<Image>();
    }

    internal void StartAnimation(bool isStartGame)
    {
        _isStartGame = isStartGame;
        StartCoroutine(LoadPanel());
    }
    
    //загрузка панели
    internal IEnumerator LoadPanel()
    {
        gameObject.SetActive(true);

        while (_alpha < 1.75)
        {
            ChangeAlpha(_alphaPlus);
            yield return new WaitForSeconds(_delayLoadPanel); //задержка при спавне
        }

        StartCoroutine(UnLoadPanel());
    }

    //разгрузка пенели
    private IEnumerator UnLoadPanel()
    {
        while (_alpha > 0)
        {
            ChangeAlpha(-_alphaPlus);
            yield return new WaitForSeconds(_delayUnLoadPane); //задержка при спавне
        }

        if (_isStartGame)
        {
            OneAnimationEnd.Invoke();
        }
        
        gameObject.SetActive(false);
    }

    private void ChangeAlpha(float alphaPlus)
    {
        _alpha += alphaPlus;

        for (byte i = 0; i < _texts.Length; i++)
            _texts[i].color = new Color(_texts[i].color.r, _texts[i].color.g, _texts[i].color.b, _alpha);

        _imagePanel.color = new Color(_imagePanel.color.r, _imagePanel.color.g, _imagePanel.color.b, _alpha);
    }
}
