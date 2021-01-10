using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

internal interface IVector3
{ 
    Vector3[] GetVector3();
}

internal interface IRigibody
{
    bool IsUseGravity();
}

internal interface ICounterNumberOfCubesInFugre
{
    IEnumerator СountNumberOfCubesInFigure(float _delay);
}

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class FigureOptions : MonoBehaviour, IVector3, IRigibody, ICounterNumberOfCubesInFugre
{
    [SerializeField] private Vector3[] _vectorCubes;
    [SerializeField] private Material[] _materialsCubes;
    [SerializeField] private GameObject[] _cubes;
    [SerializeField] private GameObject _reloadFigure;
    [SerializeField] private float _speed = -3.5f;
    [SerializeField] private float _boostSpeed = 2;

    public UnityEvent<byte> OnFell;
    
    private GameObject _cubeCurrent;
    private GameObject[] _cubesGameplay;
    private  protected Transform _tr;
    private Rigidbody _rb;
    private BoxCollider _bx;
    private AudioSource _as;
    private byte _counterChildCubes;

    private bool _isFallen = false;
    private readonly byte _costFell = 1;
    private readonly float _pusherFigureToDown = -2;

    private void Start()
    {
        _tr = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody>();
        _bx = GetComponent<BoxCollider>();
        _as = GetComponent<AudioSource>();
        
        _cubesGameplay = new GameObject[_vectorCubes.Length];
        _counterChildCubes = (byte)_vectorCubes.Length;
        _as.volume = SaveSome.GetParameter(PrefsSave.VolumeEffects, 0);
        _cubeCurrent = GetRandomCube();
        
        GetSpeed();
        DoNormalSpeed();
        SpawnCubes();
        CreatZoneFigure();
        RandomRotate();
    }
    
    public Vector3[] GetVector3()
    {
        return _vectorCubes;
    }

    public bool IsUseGravity()
    {
        return _rb.useGravity;
    }

    private void GetSpeed()
    {
        _speed = SaveSome.GetParameter(PrefsSave.SpeedFigure, _speed);
    }

    private GameObject GetRandomCube()
    {
        return _cubes[Random.Range(0, _cubes.Length)];
    }
    
    private void SpawnCubes()
    {
        _cubeCurrent.GetComponent<Renderer>().material = _materialsCubes[Random.Range(0, _materialsCubes.Length)];
        for (byte i = 0; i < _vectorCubes.Length; i++)
        {
            _cubesGameplay[i] = Instantiate(_cubeCurrent, _vectorCubes[i], Quaternion.identity);
            _cubesGameplay[i].transform.position += _tr.position;
            _cubesGameplay[i].transform.SetParent(_tr);
        }
    }

    private void CreatZoneFigure()
    {
        float minX = 0, maxX = 0, minY = 0, maxY = 0, // максимальные и минамальные положения по X и Y
            lengthCollider, heightColleder, widthCollider = 1;

        foreach (Vector3 item in _vectorCubes)
        {
            if (minX > item.x) minX = item.x;
            if (maxX < item.x) maxX = item.x;

            if (minY > item.y) minY = item.y;
            if (maxY < item.y) maxY = item.y;
        }

        // место положение коллайдера по X и Y
        // задание размера коллайдера
        float x = (minX + maxX) / 2,
            y = (minY + maxY) / 2,
            z = 0;
        
        lengthCollider = (Mathf.Abs(minX) + Mathf.Abs(maxX) + 1);
        heightColleder = (Mathf.Abs(minY) + Mathf.Abs(maxY) + 1);
        
        _bx.center = new Vector3(x, y, z);
        _bx.size = new Vector3(lengthCollider, heightColleder, widthCollider);
    }

    internal void Freeze()
    {
        _rb.velocity = new Vector3(0,0,0);
    }
    
    internal void DoNormalSpeed()
    {
        _rb.velocity = new Vector3(0, _speed, 0);
    }

    internal void DoAcceleratedSpeed()
    {
        _rb.velocity = new Vector3(0, _speed * _boostSpeed, 0);
    }
    
    internal void MoveSide(int side)
    {
        _tr.position = new Vector3(_tr.position.x + side, _tr.position.y, _tr.position.z);
    }

    internal virtual void Rotate()
    {
        _tr.RotateAround(transform.position, Vector3.back, 90);
    }

    protected void PushToDown()
    {
        _rb.velocity = new Vector3(0,_pusherFigureToDown,0);
    }
    
    private void RandomRotate()
    {
        byte numberOfRotate = (byte)Random.Range(0, 3);
        
        for (byte i = 0; i < numberOfRotate; i++)
        {
            Rotate();
        }
    }

    public virtual IEnumerator СountNumberOfCubesInFigure(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        if (_tr.childCount == 0)
        {
            Destroy(gameObject);
        }
        else if (_tr.childCount < _counterChildCubes)
        {
            MoveCubes();
        }
    }

    private void MoveCubes()
    {
        if (((_counterChildCubes - _tr.childCount == 1) &
            (_cubesGameplay[0] == null | _cubesGameplay[_cubesGameplay.Length - 1] == null))
            | (_tr.childCount == 1))
        {
            PushToDown();
        }
        else
        {
            SplitFigure();
        }
    }

    private void SplitFigure()
    {
        List<GameObject> cubes = new List<GameObject>();

        for (byte i = 0; i < _cubesGameplay.Length; i++)
        {
            if (_cubesGameplay[i] != null)
            {
                cubes.Add(_cubesGameplay[i]);
            }
            else
            {
                if (cubes.Count > 0)
                {
                    GameObject separateFigure = Instantiate(_reloadFigure, _tr.position, Quaternion.identity);
                    for (byte j = 0; j < cubes.Count; j++)
                    {
                        cubes[j].transform.SetParent(separateFigure.transform);
                    }

                    PushToDown();
                    cubes = new List<GameObject>();
                }
            }
        }
    }

    private void Fell()
    {
        Buffer.BuffSome(PrefsSave.AllFellFiguresInfo);
        _as.Play();
        _rb.useGravity = true;
        _isFallen = true;
        OnFell.Invoke(_costFell);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_isFallen)
        {
            Fell();
        }
    }
}

