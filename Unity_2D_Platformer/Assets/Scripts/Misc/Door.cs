using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public float duration = 1f;
    
    private bool isOpen = false;
    private Coroutine doorCoroutine = null;
    public void Trigger()
    {
        if (doorCoroutine != null)
        {
            StopCoroutine(doorCoroutine);
        }

        if (!isOpen)
        {
            doorCoroutine = StartCoroutine(OpenDoor(duration));
        }
        else
        {
            doorCoroutine = StartCoroutine(CloseDoor(duration));
        }
    }
    private IEnumerator OpenDoor(float duration)
    {
        isOpen = true;

        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = new Vector3(initialScale.x, 0f, initialScale.z);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float percentage = elapsedTime / duration;
            percentage = Mathf.Pow(percentage, 1 / 2f);

            transform.localScale = Vector3.Lerp(initialScale, targetScale, percentage);
            yield return null;
        }

        transform.localScale = targetScale;
        doorCoroutine = null;
    }

    private IEnumerator CloseDoor(float duration)
    {
        isOpen = false;

        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = new Vector3(initialScale.x, 1f, initialScale.z);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float percentage = elapsedTime / duration;
            percentage = Mathf.Pow(percentage, 1 / 2f);

            transform.localScale = Vector3.Lerp(initialScale, targetScale, percentage);
            yield return null;
        }

        transform.localScale = targetScale;
        doorCoroutine = null;
    }
}
