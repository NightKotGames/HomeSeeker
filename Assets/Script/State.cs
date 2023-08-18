using UnityEngine;

public abstract class State : ScriptableObject
{
    public bool IsFinished { get; protected set; }
    public bool DogIsFinished { get; protected set; }

    [HideInInspector] public NPC Character;
    [HideInInspector] public DogNPC DogNPC;

    public virtual void Init() { }
    public abstract void Run();
}