using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class SpawnerCoins : MonoBehaviour
{
    [SerializeField] private GameObject _coin;
    [SerializeField] private byte _chanceAppear = 3;
    [SerializeField] private Text _counterCoinsTookText;
    [SerializeField] private Vector3 _point1 = new Vector3(-6, 1, 22);
    [SerializeField] private Vector3 _point2 = new Vector3(4, 20, 22);

    private AudioSource _as;
    
    private Vector3 _correctPosition = new Vector3(0, 0, 0);
    internal short CounterCoins { private get;  set; } = 0;

    private void Start()
    {
        _as = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _counterCoinsTookText.text = "0";
    }

    internal void SpawnCoin()
    {
        ChangeVolume();
        
        // шанс на то что монета заспаунится
        if (GetChanсeAppear(_chanceAppear) == 0)
        {
            _correctPosition = GetLastCube();
            
            GameObject someCoin = Instantiate(_coin, _correctPosition, Quaternion.identity);
            someCoin.transform.Rotate(Vector3.back, 90);
            
            // добавление монет в счетчик
            someCoin.GetComponent<Coin>().OnTook.AddListener((_cost) =>
            {
                _as.Play();
                CounterCoins += _cost;
                _counterCoinsTookText.text = CounterCoins.ToString();
            });
        }
    }
    
    // получить шанс появитсья
    private int GetChanсeAppear(int chance) => UnityEngine.Random.Range(0, chance);

    // нахождения координат последнего куба
    private Vector3 GetLastCube()
    {
        Vector3 lastCubePosition;
        float maxY = 0;

        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Cube"))
        {
            if (maxY < item.transform.parent.position.y)
            {
                maxY = item.transform.parent.position.y;
            }
        }

        lastCubePosition = new Vector3
        (
            Mathf.FloorToInt(Random.Range(_point1.x + 1, _point2.x - 1)),
            Mathf.Ceil(Random.Range(maxY + 1, _point2.y)),
            _point2.z
        );
        
        return lastCubePosition;
    }
    
    private void ChangeVolume()
    {
        _as.volume = SaveSome.GetParameter(PrefsSave.VolumeEffects, 0);
    }
}