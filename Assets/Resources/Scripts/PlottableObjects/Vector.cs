using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector : ITranslatable2D, IPointable2D, IParametric2D
{
    public delegate void VectorEventHandler(Vector origin);
    public event VectorEventHandler OnMove;
    public event VectorEventHandler OnPoint;

    public Vector2 point { get { return _point; } set { _point = value; ActivateOnPoint(); } }

    private Vector2 _point;

    private Vector2 _position;

    public Vector2 position { get { return _position; } set { _position = value; ActivateOnMove(); } }

    public Func<float, float> x1;

    public Func<float, float> y1;

    public Func<float, float> x2;

    public Func<float, float> y2;

    public Variable variable;

    void ActivateOnMove()
    {
        if (OnMove != null)
        {
            OnMove(this);
        }
    }

    void ActivateOnPoint()
    {
        if (OnPoint != null)
        {
            OnPoint(this);
        }
    }

    public void PointTo(Vector2 p)
    {
        point = p;
    }

    public void PointBy(Vector2 p)
    {
        point += p;
    }

    public void MoveTo(Vector2 p)
    {
        position = p;
    }

    public void MoveBy(Vector2 p)
    {
        position += p;
    }

    public void SetFunctions(Variable v, Func<float, float> x, Func<float, float> y)
    {
        x1 = x;
        y1 = y;
        if (v != null)
        {
            variable = v;
            variable.OnValueChanged += UpdatePositions;
        }
    }

    public void AttachToVector(Vector v)
    {
        VectorEventHandler update = (Vector b) => { MoveTo(b.position + b.point); };
        v.OnPoint += update;
        v.OnMove += update;
    }



    private void UpdatePositions()
    {
        PointTo(new Vector2(x1(variable.value), y1(variable.value)));
    }

    public Vector(Vector2 pt, Vector2 pos)
    {
        point = pt;
        position = pos;
    }

    public void ClearFunctions()
    {
        if (variable != null)
        {
            variable.OnValueChanged -= UpdatePositions;
        }
        variable = null;
        x1 = null;
        y1 = null;
    }
}