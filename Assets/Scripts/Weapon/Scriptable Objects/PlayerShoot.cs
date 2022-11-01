using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public static Action shootInput;

    public void OnGunShot(InputAction.CallbackContext ctx)
    {
        shootInput?.Invoke();
        // used for effects
    }
}
