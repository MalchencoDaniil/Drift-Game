using UnityEngine;
using UnityEngine.Events;

public abstract class Trigger : MonoBehaviour
{
    public UnityEvent _triggerEvent;

    public virtual void Interact() { }
}