using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SpawnerUnistallZone : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Transform _zoneDeleteTransform;
    [SerializeField] private ZoneUnistaller _zoneUnistaller;
    [SerializeField] private Vector3 _coefficientDistance = new Vector3(2f, 2.2f, 22);
    [SerializeField] private Vector2 _shiftZone = new Vector2(0.5f, -12);
    
    public UnityEvent OnActivePanelMovemevement;

    private Vector3 _downTouch, _upTouch;

    public void OnPointerDown(PointerEventData eventData)
    {
        _downTouch = InstallCoordinates(eventData,_shiftZone.x, _shiftZone.y);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _upTouch = InstallCoordinates(eventData,_shiftZone.x,_shiftZone.y);
        СonfigureUninstallZone();
        OnActivePanelMovemevement.Invoke();
    }

    private void СonfigureUninstallZone()
    {
        // место положение коллайдера по X и Y
        _zoneDeleteTransform.position = new Vector3
        (
            (_upTouch.x + _downTouch.x) / 2,
            (_upTouch.y + _downTouch.y) / 2 ,//(_upTouch.y + _downTouch.y) / 2 - 0.5f,
            _upTouch.z
        );

        // задание размера коллайдера
        _zoneDeleteTransform.localScale = new Vector3
        (
            Mathf.Abs(_upTouch.x - _downTouch.x),
            Mathf.Abs(_upTouch.y - _downTouch.y),
            1
        );
        
        _zoneDeleteTransform.gameObject.SetActive(true);
    }

    private Vector3 InstallCoordinates(PointerEventData eventData, float shiftX = 0, float shiftY = 0)
    {
        return new Vector3
        (
            eventData.pointerCurrentRaycast.worldPosition.x * _coefficientDistance.x + shiftX,
            (eventData.pointerCurrentRaycast.worldPosition.y * _coefficientDistance.y) + shiftY, //(eventData.pointerCurrentRaycast.worldPosition.y * _coefficientDistance.y) + (shiftY * 2) + -1, 
            _coefficientDistance.z
        );
    }
}
