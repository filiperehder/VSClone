// ShowIfAttribute.cs
using UnityEngine;

public class ShowIfAttribute : PropertyAttribute
{
    public string conditionMethod;

    public ShowIfAttribute(string conditionMethod)
    {
        this.conditionMethod = conditionMethod;
    }
}