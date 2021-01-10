using UnityEngine;

public class FigureOptions_SZI : FigureOptions
{
    private bool isForward = false;
    private readonly float _angleTurn = 90;
    
    internal override void Rotate()
    {
        if (isForward)
            _tr.RotateAround(_tr.position, Vector3.forward, _angleTurn);
        else
            _tr.RotateAround(_tr.position, Vector3.back, _angleTurn);

        isForward = !isForward;
    }
}
