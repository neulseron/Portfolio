using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    bool IsInteractable { get; }
    float Distance { get; }

    void Interact(GameObject other);
    void StopInteract(GameObject other);
}
