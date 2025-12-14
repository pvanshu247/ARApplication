using System.Collections;
using UnityEngine;

public class ComponentAnimator : MonoBehaviour, IInteractable
{
    [SerializeField] private Vector3 openRotation;
    [SerializeField] private float speed = 2f;

    private bool _isOpen;
    private Quaternion _closedRot;
    private Quaternion _openRot;

    void Start()
    {
        _closedRot = transform.localRotation;
        _openRot = Quaternion.Euler(openRotation);
    }

    public void Interact()
    {
        _isOpen = !_isOpen;
        StopAllCoroutines();
        StartCoroutine(RotateDoor(_isOpen ? _openRot : _closedRot));
    }

    IEnumerator RotateDoor(Quaternion target)
    {
        while (Quaternion.Angle(transform.localRotation, target) > 0.1f)
        {
            transform.localRotation = Quaternion.Slerp(
                transform.localRotation,
                target,
                Time.deltaTime * speed
            );
            yield return null;
        }
    }
}
