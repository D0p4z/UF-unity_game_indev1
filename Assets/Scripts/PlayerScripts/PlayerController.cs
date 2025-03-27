using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    
    [SerializeField]
    [Range(0f, 20f)]
    float speed;

    [Header("Basic lerp variables")]

    [SerializeField]
    [Range(0, 20)]
    float accelerate;

    [SerializeField]
    [Range(0,20)]
    float deAccelerate;

    [Space]

    Vector2 movementInput;
    //Basically just trying to replicate movement input code lol
    float repairInput;
    //Dumbest work around ever for tapping and not holding
    bool holdingR;

    [Header("Input table(?)")]
    [SerializeField]
    InputActionReference move;
    //Retreived by adding InputAction to GamePlay actions
    [SerializeField]
    InputActionReference repair;

    // Start is called before the first frame update
    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        holdingR = false;
    }

    // Update is called once per frame
    private void Update() {
        takeInput();
    }

    private void FixedUpdate() {
        movementLogic();
        repairLogic();
    }

    void takeInput() {
        movementInput = move.action.ReadValue<Vector2>();
        //Reads boolean: true if pressed
        repairInput = repair.action.ReadValue<float>();
    }

    void movementLogic() {
        //Basic lerp movement
        Vector2 velocity = rb.linearVelocity;
        float lerpFloatValue = accelerate;

        if(movementInput == Vector2.zero)
        {
            lerpFloatValue = deAccelerate;
        }
        else
        {
            movementInput.Normalize();
        }

        velocity = Vector2.Lerp(velocity, movementInput * speed, lerpFloatValue * Time.fixedDeltaTime);
        rb.linearVelocity = velocity;
    }
    void repairLogic()
    {
        if (repairInput != 0)
        {
            if (!holdingR)
                gameObject.GetComponent<PlayerHand>().repair();
            holdingR = true;
        }
        else holdingR = false;
    }
}
