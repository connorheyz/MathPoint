using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variable
{
    public delegate void ConstantEventHandler();
    public event ConstantEventHandler OnValueChanged;

    public void ActivateOnValueChanged()
    {
        if (OnValueChanged != null)
        {
            OnValueChanged();
        }
    }

    public char index;

    public Variable(float initialValue, char c)
    {
        value = initialValue;
        index = c;
    }

    private float _value;

    public float value
    {
        get { return _value; } 
        set { _value = value;  ActivateOnValueChanged(); }
    }
}