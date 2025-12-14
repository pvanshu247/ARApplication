using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ARObjectPlacer : MonoBehaviour
{
    [Header("AR")]
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private ARPlaneManager planeManager;

    [Header("Prefab")]
    [SerializeField] private GameObject carPrefab;

    public event Action<GameObject> OnObjectPlaced;
    public event Action OnPlacementReset;

    private GameObject _spawnedCar;
    private bool _placementEnabled = true;

    private static readonly List<ARRaycastHit> hits = new();

    void Update()
    {
        if (!_placementEnabled || Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began)
            return;

        if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
        {
            PlaceObject(hits[0].pose);
        }
    }

    void PlaceObject(Pose pose)
    {
        _spawnedCar = Instantiate(carPrefab, pose.position, carPrefab.transform.rotation);

        DisablePlaneTracking();
        _placementEnabled = false;

        OnObjectPlaced?.Invoke(_spawnedCar);
    }

    public void ResetPlacement()
    {
        if (_spawnedCar != null)
            Destroy(_spawnedCar);

        EnablePlaneTracking();
        _placementEnabled = true;

        OnPlacementReset?.Invoke();
    }

    void DisablePlaneTracking()
    {
        planeManager.enabled = false;
        foreach (ARPlane plane in planeManager.trackables)
            plane.gameObject.SetActive(false);
    }

    void EnablePlaneTracking()
    {
        planeManager.enabled = true;
        foreach (ARPlane plane in planeManager.trackables)
            plane.gameObject.SetActive(true);
    }
}
