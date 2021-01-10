using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RowOfTenCubes : MonoBehaviour
{
    [SerializeField] private AnimatorNextLVL _animatorNext;
    [SerializeField] private float _delayCube = 0.05f;
    [SerializeField] private float _delayCheckCubesInFigure = 0.75f;

    private AudioSource _as;
    
    private Transform _tr;
    private Vector3 _initialPosition; // начальная позиция
    private GameObject[] _row = new GameObject[10];
    private byte _score = 0;
    private readonly sbyte _step = -1;
    private bool _check = false;
    private readonly byte _hieghtTower = 20;
    
    private float _progressBarLVLPlayer = 0;
    private readonly float _scorePlusLVLPlayer = 0.2f;
    private readonly float _maxProgressLVLPlayer = 1f; // 1
    private readonly float _speedPlus = -0.5f;

    private void OnEnable()
    {
        _tr = GetComponent<Transform>();
        _as = GetComponent<AudioSource>();
        
        _initialPosition = _tr.position;
    }

    internal void StartCheckRow()
    {
        _check = true;
        StartCoroutine(StartLoop());
    }

    private IEnumerator StartLoop()
    {
        for (byte i = 0; i < _hieghtTower; i++)
        {
            _score = 0;
            _tr.position += new Vector3(0, _step, 0);

            yield return new WaitForSeconds(_delayCube);
        }

        _tr.position = _initialPosition;
        _check = false;
    }

    private void DeleteRow()
    {
        _as.Play();
        
        foreach (GameObject item in _row)
        {
            Destroy(item);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_check)
        {
            if (other.gameObject.tag == "Cube") //other.gameObject.GetComponent<FigureOptions>()
            {
                _row[_score] = other.gameObject;
                _score++;
                
                if (_score == 10)
                {
                    DeleteRow();
                    CallCheckNumberOfCubesInFigure(_row);
                    UpdateLVLPlayer();
                }
            }
        }
    }

    private void CallCheckNumberOfCubesInFigure(GameObject[] gOs)
    {
        GameObject lastCube = null;
        
        foreach (GameObject item in gOs)
        {
            // вызов фигуры один раз
            if (item.transform.root.gameObject != lastCube)
            {
                StartCoroutine(item.transform.root.gameObject.GetComponent<ICounterNumberOfCubesInFugre>().СountNumberOfCubesInFigure(_delayCheckCubesInFigure));
            }
            
            lastCube = item.transform.root.gameObject;
        }
    }
    
    private void UpdateLVLPlayer()
    {
        Buffer.BuffSome(PrefsSave.AllCubesDeleteInfo, 10);
        _progressBarLVLPlayer = SaveSome.GetParameter(PrefsSave.LVLPlayerBar, _progressBarLVLPlayer);
        
        if (_progressBarLVLPlayer >= _maxProgressLVLPlayer)
        {
            Buffer.BuffSome(PrefsSave.LVLPlayer);
            Buffer.BuffSome(PrefsSave.SpeedFigure, _speedPlus);
            _progressBarLVLPlayer = 0;
           // _animatorNext.StartAnimation(SaveSome.GetParameter(PrefsSave.LVLPlayer, 0));
        }
        
        _progressBarLVLPlayer += _scorePlusLVLPlayer;
        SaveSome.SetParametr(PrefsSave.LVLPlayerBar, _progressBarLVLPlayer);
    }
}
