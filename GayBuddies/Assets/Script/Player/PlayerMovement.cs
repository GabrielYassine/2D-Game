using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;

public class PlayerMovement : NetworkBehaviour {

    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    public CharacterController2D controller;
    public Rigidbody2D rb;

    void Update() {
        if (!IsOwner) return; // Only allow the local player to control their character

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump")) {
            jump = true;
        }

        if (Input.GetButton("Crouch")) {
            if (controller.m_Grounded) {
                crouch = true;
            }
            else {
                rb.gravityScale = 7f;
            }
        } else {
            rb.gravityScale = 4f;
            crouch = false;
        }
    }

    void FixedUpdate() {
        if (!IsOwner) return; // Only allow the local player to control their character

        Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    [ServerRpc]
    private void Move(float move, bool crouch, bool jump) {
        controller.Move(move, crouch, jump);
        UpdateClientMove(move, crouch, jump);
    }

    [ObserversRpc]
    private void UpdateClientMove(float move, bool crouch, bool jump) {
        if (IsOwner) { // Skip updating the owner's client (no need to update the owner's client since they are the one controlling the character)
            return;
        }

        controller.Move(move, crouch, jump);
    }
}