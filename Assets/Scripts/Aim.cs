using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Aim : MonoBehaviour
{
    enum AngleRotations{ FullAngleRotation, HalvAngleRotation, EightFixedAnglesRotation, FourFixedAnglesRotation }
    [SerializeField] private AngleRotations rotations;
    [SerializeField] private AngleRotations rotationsOverride;

    private Vector3 mousePos;
    private Vector3 direction;
    private Quaternion rotation;
    private float angle;
    private bool usingOverride = false;

    private void Update()
    {
        //MouseInputToAngleCalculation();
        transform.rotation = rotation;
    }
    private void MouseInputToAngleCalculation()
    {
        mousePos = Input.mousePosition;
        mousePos.z = 10;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        direction = mousePos - transform.position;
        direction.Normalize();
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
    public void DefualtJoystickInputToAngleCalculation(InputAction.CallbackContext context)
    {
        Vector2 t = context.ReadValue<Vector2>();
        if (t.x == 0 && t.y == 0 || usingOverride) return;
        direction.Normalize();
        angle = Mathf.Atan2(t.y, t.x) * Mathf.Rad2Deg; // -90 degrees
        ChooseAngleRotation(rotations);
    }
    public void OverrideJoystickInputToAngleCalculation(InputAction.CallbackContext context)
    {
        Vector2 t = context.ReadValue<Vector2>();
        if (t.x == 0 && t.y == 0)
        {
            usingOverride = false;
            return;
        }
        usingOverride = true;
        direction = t - (Vector2)transform.position;
        direction.Normalize();
        angle = Mathf.Atan2(t.y, t.x) * Mathf.Rad2Deg; // -90 degrees
        ChooseAngleRotation(rotationsOverride);
    }
    private void ChooseAngleRotation(AngleRotations type)
    {
        switch (type)
        {
            case AngleRotations.FullAngleRotation:
                FullAngleRotation();
                break;
            case AngleRotations.HalvAngleRotation:
                HalvAngleRotation();
                break;
            case AngleRotations.EightFixedAnglesRotation:
                EightFixedAnglesRotation();
                break;
            case AngleRotations.FourFixedAnglesRotation:
                FourFixedAnglesRotation();
                break;
            default: break;
        }
    }
    private void FullAngleRotation()
    {
        rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    private void HalvAngleRotation()
    {
        if (angle > 90) rotation = Quaternion.AngleAxis(90, Vector3.forward);
        else if (angle < -90) rotation = Quaternion.AngleAxis(-90, Vector3.forward);
        else rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    private void EightFixedAnglesRotation()
    {
        if ((angle >= -180 && angle < -157.5) || (angle >= 157.5 && angle < 180))
        {
            rotation = Quaternion.AngleAxis(-180, Vector3.forward);
            return;
        }
        for (float i = -157.5f; i < 157.5; i += 45)
        {
            if (angle >= i && angle < i +45)
            {
                rotation = Quaternion.AngleAxis(i + 22.5f , Vector3.forward);
            }
        }
    }
    private void FourFixedAnglesRotation()
    {
        if ((angle >= -180 && angle < -135) || (angle >= 135 && angle < 180))
        {
            rotation = Quaternion.AngleAxis(-180, Vector3.forward);
            return;
        }
        for (int i = -135; i < 135; i+= 90)
        {
            if (angle >= i && angle < i + 90)
            {
                rotation = Quaternion.AngleAxis(i+45, Vector3.forward);
            }
        }
    }
}
