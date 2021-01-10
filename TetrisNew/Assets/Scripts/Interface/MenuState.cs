using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuState : MonoBehaviour
{
    [SerializeField] private StarterGame _starterGame;
    
    [SerializeField] private List<GameObject> _allMenu;
    
    [SerializeField] private GameObject _game;
    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _panelBuyRedCubesError;
    [SerializeField] private GameObject _settingsPanel;

    [SerializeField] private Scrollbar _progressLVLBar;
    [SerializeField] private Text _currentLVLPlayer;
    [SerializeField] private Text _counterCoins;
    [SerializeField] private Text _counterRedCubes;

    [SerializeField] private Text _coinsTakeInfo;
    [SerializeField] private Text _figureFallInfo;
    [SerializeField] private Text _cubesDeletedInfo;
    [SerializeField] private Text _speedFigureInfo;

    [SerializeField] private Scrollbar _volumeSingBar;
    [SerializeField] private Scrollbar _volumeEffects;

    [SerializeField] private AudioSource _themeMenu;

    private AudioSource _as;
    private readonly byte _costRedCubes = 15;

    private void Start()
    {
        _as = GetComponent<AudioSource>();
        ChangeVolume();
    }

    private void OnEnable()
    {
        GetAllInformationAboutMenu();
    }

    public void StartGame()
    {
        _starterGame.gameObject.SetActive(true);
        _starterGame.StartAnimation(true);
        _menu.SetActive(false);
        _game.SetActive(true);
    }

    private void GetAllInformationAboutMenu()
    {
        _progressLVLBar.size = SaveSome.GetParameter(PrefsSave.LVLPlayerBar, 0);
        _currentLVLPlayer.text = SaveSome.GetParameter(PrefsSave.LVLPlayer, 0).ToString();
        _counterCoins.text = SaveSome.GetParameter(PrefsSave.Money, 0).ToString();
        _counterRedCubes.text = SaveSome.GetParameter(PrefsSave.RedCubes, 0).ToString();
        
        _coinsTakeInfo.text = SaveSome.GetParameter(PrefsSave.AllCoinsTookInfo, 0).ToString();
        _figureFallInfo.text = SaveSome.GetParameter(PrefsSave.AllFellFiguresInfo, 0).ToString();
        _cubesDeletedInfo.text = SaveSome.GetParameter(PrefsSave.AllCubesDeleteInfo, 0).ToString();
        _speedFigureInfo.text = Mathf.Abs(SaveSome.GetParameter(PrefsSave.SpeedFigure, -3.5f)).ToString();
        
        _volumeSingBar.value = SaveSome.GetParameter(PrefsSave.VolumeSing, 1);
        _volumeEffects.value = SaveSome.GetParameter(PrefsSave.VolumeEffects, 1);
    }

    public void BuyRedCubes()
    {
        if (int.Parse(_counterCoins.text) >= _costRedCubes)
        {
            _counterCoins.text = (int.Parse(_counterCoins.text) - _costRedCubes).ToString();
            _counterRedCubes.text = (int.Parse(_counterRedCubes.text) + 1).ToString();
            Buffer.BuffSome(PrefsSave.Money, -_costRedCubes);
            Buffer.BuffSome(PrefsSave.RedCubes);
            _as.Play();
        }
        else
        {
            IsActiveMenuElements(_panelBuyRedCubesError);
            _panelBuyRedCubesError.SetActive(true);
        }
    }

    public void CloseErrorPanelMoney(GameObject panelError)
    {
        IsActiveMenuElements(panelError, false, true,_settingsPanel);
        _panelBuyRedCubesError.SetActive(false);
    }

    public void Settings(GameObject panelSettings)
    {
        IsActiveMenuElements(panelSettings);
    }

    public void SaveSettings(GameObject panelSettings)
    {
        SaveSome.SetParametr(PrefsSave.VolumeSing, _volumeSingBar.value);
        SaveSome.SetParametr(PrefsSave.VolumeEffects, _volumeEffects.value);
        ChangeVolume();
        print(SaveSome.GetParameter(PrefsSave.VolumeEffects, 0));
        IsActiveMenuElements(panelSettings, false, true);
    }

    public void CanelSettings(GameObject panelSettings)
    {
        IsActiveMenuElements(panelSettings, false, true);
    }
    
    public void Info(GameObject panelInfo)
    {
        panelInfo.SetActive(!panelInfo.activeSelf);
        _as.Play();
    }

    private void IsActiveMenuElements(GameObject gO, bool isActive = true, bool isActiveOther = false, GameObject specialGO = null)
    {
        _as.Play();
        
        foreach (GameObject item in _allMenu)
        {
            if (item == gO)
            {
                gO.SetActive(isActive);
            }
            else
            {
                item.SetActive(isActiveOther);
            }
        }
        
        if (specialGO != null)
        {
            specialGO.SetActive(isActive);
        }
    }

    private void ChangeVolume()
    {
        _as.volume = SaveSome.GetParameter(PrefsSave.VolumeEffects, 0);
        _themeMenu.volume = SaveSome.GetParameter(PrefsSave.VolumeSing, 0);
    }
}