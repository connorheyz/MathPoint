using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ITranslatable2D
{
    void MoveTo(Vector2 position);
    void MoveBy(Vector2 position);
}

public interface IPointable2D
{
    void PointTo(Vector2 point);
    void PointBy(Vector2 point);
}

public interface IParametric2D
{
    void SetFunctions(Variable v, Func<float,float> x1, Func<float, float> Y1);
    void ClearFunctions();
}
