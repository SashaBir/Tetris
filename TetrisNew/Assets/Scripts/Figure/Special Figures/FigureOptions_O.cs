using System.Collections;

public class FigureOptions_O : FigureOptions
{
    internal override void Rotate()
    {
    }

    public override IEnumerator СountNumberOfCubesInFigure(float delay)
    {
        PushToDown();
        yield break;
    }
}
