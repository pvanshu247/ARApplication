using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SVImageControl : MonoBehaviour, IDragHandler,IPointerClickHandler, IPointerUpHandler
{
    [SerializeField] private UIController uiController;
    [SerializeField] private Image pickerImage;
    private RawImage _svImage;
    private ColorPickerControl _colorPickerControl;
    private RectTransform _rectTransform, _pickerTransform;

    private void Awake()
    {
        _svImage = GetComponent<RawImage>();
        _colorPickerControl = GetComponentInParent<ColorPickerControl>();
        _rectTransform = GetComponent<RectTransform>();
        _pickerTransform = pickerImage.GetComponent<RectTransform>();
        _pickerTransform.position = new Vector2(-(_rectTransform.sizeDelta.x *0.5f), -(_rectTransform.sizeDelta.y *0.5f));
    }

    void UpdateColor(PointerEventData eventData)
    {
        Vector3 pos = _rectTransform.InverseTransformPoint(eventData.position);

        float deltaX = _rectTransform.sizeDelta.x * 0.5f;
        float deltaY = _rectTransform.sizeDelta.y * 0.5f;

        if (pos.x < -deltaX)
        {
            pos.x = -deltaX;
        }else if (pos.x > deltaX)
        {
            pos.x = deltaX;
        }

        if (pos.y < -deltaY)
        {
            pos.y = -deltaY;
        }else if (pos.y > deltaY)
        {
            pos.y = deltaY;
        }
        
        float x = pos.x + deltaX;
        float y = pos.y + deltaY;

        float xNorm = x / _rectTransform.sizeDelta.x;
        float yNorm = y / _rectTransform.sizeDelta.y;

        _pickerTransform.localPosition = pos;
        pickerImage.color = Color.HSVToRGB(0, 0, 1 - yNorm);
        _colorPickerControl.SetSV(xNorm, yNorm);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateColor(eventData);
        uiController.isUIInteracting = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UpdateColor(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        uiController.isUIInteracting = false;
    }
}
