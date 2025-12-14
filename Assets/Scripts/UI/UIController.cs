using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 0.4f;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button resetButton;
    public bool isUIInteracting;

    [Header("References")]
    [SerializeField] private ARObjectPlacer placementController;

    void OnEnable()
    {
        placementController.OnObjectPlaced += HandleObjectPlaced;
        placementController.OnPlacementReset += HandlePlacementReset;

        resetButton.onClick.AddListener(placementController.ResetPlacement);
    }

    void OnDisable()
    {
        placementController.OnObjectPlaced -= HandleObjectPlaced;
        placementController.OnPlacementReset -= HandlePlacementReset;

        resetButton.onClick.RemoveListener(placementController.ResetPlacement);
    }

    void HandleObjectPlaced(GameObject placedObject)
    {
        var interact = placedObject.GetComponent<AllComponentAnimator>();
        interactButton.onClick.RemoveAllListeners();
        interactButton.onClick.AddListener(interact.CallInteractOnAll);

        StopAllCoroutines();
        StartCoroutine(FadeCanvas(0f, 1f));

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    void HandlePlacementReset()
    {
        StopAllCoroutines();
        StartCoroutine(FadeCanvas(1f, 0f));

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        interactButton.onClick.RemoveAllListeners();
    }

    IEnumerator FadeCanvas(float from, float to)
    {
        float elapsed = 0f;
        canvasGroup.alpha = from;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = to;
    }
}
