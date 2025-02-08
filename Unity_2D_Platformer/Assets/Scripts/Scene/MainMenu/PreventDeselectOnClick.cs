using UnityEngine;
using UnityEngine.EventSystems;

public class PreventDeselectOnClick : MonoBehaviour
{
    private GameObject lastSelected;

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            lastSelected = EventSystem.current.currentSelectedGameObject;
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(lastSelected);
        }
    }
}
