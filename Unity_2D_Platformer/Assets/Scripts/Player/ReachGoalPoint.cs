using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachGoalPoint : MonoBehaviour
{
    private bool isGoalPoint;
    private Player owner;

    private void Awake()
    {
        owner = GetComponent<Player>();
    }

    void Start()
    {
        isGoalPoint = false;
    }

    void Update()
    {
        if(owner.input.moveInput.y < 0 && isGoalPoint == true && owner.keyCount > 0)
        {
            owner.StopPlayer();
            SceneLoadManager.Instance.LoadNextScene(null);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Goal"))
        {
            isGoalPoint = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Goal"))
        {
            isGoalPoint = false;
        }
    }
}
