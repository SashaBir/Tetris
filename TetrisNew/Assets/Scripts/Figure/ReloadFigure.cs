using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ReloadFigure : MonoBehaviour, ICounterNumberOfCubesInFugre
{
    private Rigidbody _rb;
    private readonly float _pusherFigureToDown = -2;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    private void PushToDown()
    {
        _rb.velocity = new Vector3(0,_pusherFigureToDown,0);
    }
    
    public virtual IEnumerator СountNumberOfCubesInFigure(float delay)
    {
        PushToDown();
        yield break;
    }
}
