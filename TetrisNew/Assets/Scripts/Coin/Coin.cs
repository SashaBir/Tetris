using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{
    [SerializeField] private float _delaySecondRotate = 0.05f;
    [SerializeField] private float _angleRotate = 2;

    public UnityEvent<byte> OnTook;
    
    private Transform _tr;
    private readonly byte _costTool = 1;

    private void Start()
    {
        _tr = GetComponent<Transform>();
        StartCoroutine(RotateAround());
    }

    private IEnumerator RotateAround()
    {
        while (true)
        {
            _tr.RotateAround(_tr.position, Vector3.down, _angleRotate);
            yield return new WaitForSeconds(_delaySecondRotate);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Cube")
        {
            Buffer.BuffSome(PrefsSave.Money);
            Buffer.BuffSome(PrefsSave.AllCoinsTookInfo);
            OnTook.Invoke(_costTool);
            Destroy(gameObject);
        }
    }
}
