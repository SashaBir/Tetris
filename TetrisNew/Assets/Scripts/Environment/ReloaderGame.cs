using UnityEngine;
using UnityEngine.UI;

public class ReloaderGame : MonoBehaviour
{
    [SerializeField] private StarterGame _starterGame;
    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _game;
    [SerializeField] private Text _counterFiguresFellText;
    [SerializeField] private Text _counterCoinsTookText;

    private readonly string _allCountersGameplay = "0";
    
    internal void ReloadAndExit()
    {
        DeleteGameplayObjects();
        ClenInterface();
        LaunchMenu();
    }

    private void DeleteGameplayObjects()
    {
        // удаление фигур
        foreach (FigureOptions item in FindObjectsOfType<FigureOptions>())
        {
            Destroy(item.gameObject);
        }
        
        // удаление кубов
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Cube"))
        {
            Destroy(item);
        }
        
        // удаления монет
        foreach (Coin item in FindObjectsOfType<Coin>())
        {
            Destroy(item.gameObject);
        }
    }

    private void ClenInterface()
    {
        _counterFiguresFellText.text = _counterCoinsTookText.text = _allCountersGameplay;
    }
    
    private void LaunchMenu()
    {
        _starterGame.gameObject.SetActive(true);
        _starterGame.StartAnimation(false);
        _game.SetActive(false);
        _menu.SetActive(true);
    }
}