using System.Collections.Generic;
using UnityEngine;

public class AllComponentAnimator : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> interactableBehaviours;

    private List<IInteractable> _interactables = new();

    void Awake()
    {
        foreach (var mb in interactableBehaviours)
        {
            if (mb is IInteractable interactable)
                _interactables.Add(interactable);
        }
    }

    public void CallInteractOnAll()
    {
        foreach (var interactable in _interactables)
        {
            interactable.Interact();
        }
    }
}
