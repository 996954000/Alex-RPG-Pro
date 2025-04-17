using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 状态数据包装类 */
[System.Serializable]
public class Stat
{
    [SerializeField] private float value;
    private List<float> modifier;

    public float getValue()
    {
        foreach (float modifierValue in modifier)
        {
            value += modifierValue;
        }
        return value;
    }

    public void setValue(float _val)
    {
        value = _val;
    }   

    public float getMultiValue(float multi)
    {
        return value * multi;
    }

    public void AddModifier(float value)
    {
        modifier.Add(value);
    }

    public void RemoveModifier(float value)
    {
        modifier.Remove(value);
    }
}
