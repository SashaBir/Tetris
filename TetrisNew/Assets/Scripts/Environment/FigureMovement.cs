using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FigureMovement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private StarterGame _starterGame;
    [SerializeField] private SpawnerCoins _spawnerCoins;
    [SerializeField] private RowOfTenCubes _rowOfTenCubes;
    [SerializeField] private Transform _pointSpawnFigure;
    [SerializeField] private Transform _trPointNextFigure;
    [SerializeField] private GameObject[] _figures;
    [SerializeField] private GameObject _cubeNextFigure;
    [SerializeField] private Text _counterFiguresFellText;

    private FigureOptions _figureOptions; // опции фигуры
    private readonly float _delayTouch = 0.35f; // задержка при нажатии
    private float _touchDown, _touchUp;
    private bool _isTouch = false, _isBoost = true;
    private readonly int _screenWidth = Screen.width / 2;
    private byte _numberCurrent = 0, _numberNext = 0;
    private readonly float _spaceBetweenTouch = 6.5f;

    private short _counterFeigureFell = 0;
    private Coroutine _boostRoutine;
        

    private void Start()
    {
        _starterGame.OneAnimationEnd.AddListener(SpawnNewFigure);
        //SetRandomNumberFigure();
    }

    private void OnDisable()
    {
        _counterFeigureFell = 0;
    }

    private void SpawnNewFigure()
    {
        _figureOptions = Instantiate(_figures[_numberCurrent], _pointSpawnFigure.position, Quaternion.identity).GetComponent<FigureOptions>();
        _figureOptions.OnFell.AddListener((_costFell) =>
        {
            _counterFeigureFell += _costFell;
            _counterFiguresFellText.text = _counterFeigureFell.ToString();
            SpawnNewFigure();
        });

        SetRandomNumberFigure(); // номера нвоых фигнур
        SpawmNextFigure(_figures[_numberNext].GetComponent<IVector3>().GetVector3());
        _rowOfTenCubes.StartCheckRow(); // запуск проверки кубов в ряд
        _spawnerCoins.SpawnCoin();
    }

    internal void FreezeCurrentFigure(bool isFreeze)
    {
        if (isFreeze)
            _figureOptions.Freeze();
        else
            _figureOptions.DoNormalSpeed();
    }
    
    private void SpawmNextFigure(Vector3[] vector)
    {
        DeleteChildrenCubes();

        for (byte i = 0; i < vector.Length; i++)
        {
            GameObject separateСube = Instantiate(_cubeNextFigure, vector[i], Quaternion.identity); // создание нового отдельного куба
            separateСube.transform.position += _trPointNextFigure.position;
            separateСube.transform.SetParent(_trPointNextFigure);
        }
    }

    // удаленеи кубов
    private void DeleteChildrenCubes()
    {
        foreach (Transform item in _trPointNextFigure)
        {
            Destroy(item.gameObject);
        }
    }

    private void SetRandomNumberFigure()
    {
        do
        {
            _numberNext = (byte)Random.Range(0, _figures.Length);
        }
        while (_numberCurrent == _numberNext);

        _numberCurrent = _numberNext;
    }

    private IEnumerator BoostFigure()
    {
        yield return new WaitForSeconds(_delayTouch);

        if (_isTouch & _isBoost /*& _localCurrentTouch == _currentTouch*/)
        {
            _figureOptions.DoAcceleratedSpeed();
            _isBoost = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _boostRoutine = StartCoroutine(BoostFigure());
        _isTouch = true;
        _touchDown = eventData.position.x - _screenWidth;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopCoroutine(_boostRoutine);
        _isTouch = false;
        _touchUp = eventData.position.x - _screenWidth;

        // ускоренние фигуры
        if (!_isBoost)
        {
            _figureOptions.DoNormalSpeed();
            _isBoost = true;
        } // поворот фигуры
        else if ( (Mathf.Abs(Mathf.Abs(_touchDown) - Mathf.Abs(_touchUp))) <= _spaceBetweenTouch)
        {
            _figureOptions.Rotate();
        } // передвижение фигуры
        else
        {
            _figureOptions.MoveSide((_touchUp - _touchDown) > 0 ? 1 : -1);
        }
    }
}
