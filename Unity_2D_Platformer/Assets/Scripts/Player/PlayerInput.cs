using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float tempmoveInput { get; private set; }

    public Vector2 moveInput { get; private set; }
    public bool isJump { get; private set; }
    public bool isJumpCut { get; private set; }
    public bool isAttack { get; private set; }
    public bool isDash { get; private set; }
    public bool isDashCut { get; private set; }
    void Start()
    {
        Application.targetFrameRate = 144;
    }

    // Update is called once per frame
    void Update()
    {
        tempmoveInput = Input.GetAxisRaw("Horizontal");

        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        isJump = Input.GetKeyDown(KeyCode.Space);
        isJumpCut = Input.GetKeyUp(KeyCode.Space);
        isAttack = Input.GetKeyDown(KeyCode.LeftControl);
        isDash = Input.GetKeyDown(KeyCode.X);
        isDashCut = Input.GetKeyUp(KeyCode.X);
    }
}
