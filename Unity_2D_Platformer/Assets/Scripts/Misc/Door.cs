using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isOpen = false;
    [SerializeField] private float duration = 1f;

    private Coroutine doorCoroutine = null;
    private Vector3 originScale;
    private void Awake()
    {
        originScale = transform.localScale;
    }

    private void Start()
    {
        if(isOpen == true)
        {
            transform.localScale = new Vector3(transform.localScale.x, 0f, transform.localScale.z);
        }
    }

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

        Vector3 initialScale = originScale;
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
        Vector3 targetScale = originScale;
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
