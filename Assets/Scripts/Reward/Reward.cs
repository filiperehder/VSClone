using UnityEngine;

public abstract class Reward
{
    public string title { get; protected set; }
    public string description { get; protected set; }
    public Sprite icon { get; protected set; } // Novo campo para ícone

    public abstract void Apply();
}