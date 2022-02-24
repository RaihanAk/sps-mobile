using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    public FixedJoystick Move;
    public FixedButton Fire, 
        Jump,
        Crouch,
        Aim;
    public FixedTouchField Look;
    public PlayerCharacterController Controller;

    // Update is called once per frame
    void Update()
    {
        // Controller.touchFire = Fire.Pressed;
        Controller.touchJump = Jump.Pressed;
        Controller.touchCrouch= Crouch.Pressed;
        Controller.touchAim = Aim.Pressed;

        Controller.touchHinput = Move.input.x;
        Controller.touchVinput = Move.input.y;
        Controller.touchLookAxis = Look.TouchDist;
    }
}
