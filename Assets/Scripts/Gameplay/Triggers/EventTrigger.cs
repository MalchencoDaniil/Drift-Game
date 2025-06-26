using UnityEngine;

public class EventTrigger : Trigger
{
    public override void Interact()
    {
        _triggerEvent?.Invoke();
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.gameObject.tag == TagDictionary.player)
        {
            Interact();
        }
    }
}