using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameplayState : MonoBehaviour
{
    [SerializeField] private SpawnerCoins _spawnerCoins;
    [SerializeField] private FigureMovement _figureMovement;
    [SerializeField] private SpawnerUnistallZone _spawnerUnistallZone;
    [SerializeField] private GameObject _panelPause;
    [SerializeField] private Scrollbar _chargeBar;
    [SerializeField] private Image[] _zaps;

    private AudioSource _as;
    
    private readonly float _stepChargePlus = 0.1f;
    private readonly float _delayCharge = 0.5f;
    private byte _counterZapsCharge;
    private short _realZapCharge;
    
    private void OnEnable()
    {
        _spawnerCoins.CounterCoins = 0;
        _chargeBar.size = 0;
        _counterZapsCharge = (byte)SaveSome.GetParameter(PrefsSave.RedCubes, 0);
        ChangeVolume();
        CountZaps();
    }

    public void Pause()
    {
        ChangeActiveSomeGameObject(0, false);
    }
    
    public void Return()
    {
        ChangeActiveSomeGameObject(1, true);
    }
    
    public void Exit(ReloaderGame reloaderGame)
    {
        ChangeActiveSomeGameObject(1, true);
        reloaderGame.ReloadAndExit();
    }

    public void DeleteCubes()
    {
        _as.Play();
        if (_chargeBar.size != 1) return;

        DeleteCubesMode(false, true, true);
        Buffer.BuffSome(PrefsSave.RedCubes, -1);
        _chargeBar.size = 0;
        _zaps[_realZapCharge].gameObject.SetActive(false);
        _realZapCharge -= 1;

        _spawnerUnistallZone.OnActivePanelMovemevement.AddListener(() =>
        {
            DeleteCubesMode(true, false, false);
            StartCoroutine(ChargeFill());
        });
    }

    private IEnumerator ChargeFill()
    {
        while (_chargeBar.size <= 1 & _realZapCharge >= 0)
        {
            _chargeBar.size += _stepChargePlus;
            yield return new WaitForSeconds(_delayCharge);
        }
    }

    private void CountZaps()
    {
        if (_counterZapsCharge == 0) return;
        
        if (_counterZapsCharge >= 3)
        {
            _realZapCharge = 2;
            foreach (Image item in _zaps)
            {
                item.gameObject.SetActive(true);
            }
        }
        else if (_counterZapsCharge > 0)
        {
            for (byte i = 0; i < _counterZapsCharge; i++)
            {
                _zaps[i].gameObject.SetActive(true);
            }

            _realZapCharge = _counterZapsCharge;
            _realZapCharge -= 1;
        } 
        
        StartCoroutine(ChargeFill());
    }
    
    private void ChangeActiveSomeGameObject(byte isGameActive, bool isActive)
    {
        _as.Play();
        Time.timeScale = isGameActive;
        _figureMovement.enabled = isActive; 
        _panelPause.SetActive(!isActive);
    }
    
    private void DeleteCubesMode(bool isFigureMovement, bool isSpawnerUnistallZone, bool isFreezeCurrentFigure)
    {
        _figureMovement.enabled = isFigureMovement; // отключение управления фигурами
        _spawnerUnistallZone.enabled = isSpawnerUnistallZone; // включение удаление кубов
        _figureMovement.FreezeCurrentFigure(isFreezeCurrentFigure); // заморозка текущей фигуры
    }
    
    private void ChangeVolume()
    {
        _as = GetComponent<AudioSource>();
        _as.volume = SaveSome.GetParameter(PrefsSave.VolumeEffects, 0);
    }
}