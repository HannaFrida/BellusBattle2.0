using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Videon som jag har f�ljt
// https://www.youtube.com/watch?v=gaDFNCRQr38&t=23s
public class Aim : MonoBehaviour
{
    public Transform arm;
    float armAngle;
    public float armSpeed;
    private void Update()
    {
        Rotate();
    }
    void Rotate()
    {
        armAngle += Input.GetAxis("Mouse Y") * armSpeed * -Time.deltaTime;
        armAngle = Mathf.Clamp(armAngle, 0, 360);
        arm.localRotation = Quaternion.AngleAxis(armAngle, Vector3.right);
    }
}
