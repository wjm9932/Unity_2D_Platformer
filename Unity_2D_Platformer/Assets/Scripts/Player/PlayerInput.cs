using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float tempmoveInput { get; private set; }

    public Vector2 moveInput { get; private set; }
    public bool isJump { get; private set; }
    public bool isJumpCut { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        tempmoveInput = Input.GetAxisRaw("Horizontal");


        isJump = Input.GetKeyDown(KeyCode.Space);
        isJumpCut = Input.GetKeyUp(KeyCode.Space);

    }
}
