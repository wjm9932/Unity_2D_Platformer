using UnityEngine;
using UnityEngine.EventSystems;

public class PreventDeselectOnClick : MonoBehaviour, IPointerDownHandler
{
    private GameObject lastSelected;

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            lastSelected = EventSystem.current.currentSelectedGameObject;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (EventSystem.current.currentSelectedGameObject == null && lastSelected != null)
        {
            EventSystem.current.SetSelectedGameObject(lastSelected);
        }
    }
}
