using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public int clampMin;
    public int clampMax;
    public KeyCode yPositive;
    public KeyCode yNegative;
    public KeyCode xPositive;
    public KeyCode xNegative;
    public float rotationScale;
    Vector3 trueRotation;

    private void Start()
    {
        trueRotation = this.transform.eulerAngles;
        trueRotation.x = Mathf.Clamp(trueRotation.x, clampMin, clampMax);
        this.transform.eulerAngles = trueRotation;
    }
    private void Update()
    {
        if (Input.GetKey(yPositive))
        {
            trueRotation.x += rotationScale * Time.deltaTime;
            trueRotation.x = Mathf.Clamp(trueRotation.x, clampMin, clampMax);
            this.transform.eulerAngles = trueRotation;
        }
        else if (Input.GetKey(yNegative))
        {
            trueRotation.x -= rotationScale * Time.deltaTime;
            trueRotation.x = Mathf.Clamp(trueRotation.x, clampMin, clampMax);
            this.transform.eulerAngles = trueRotation;
        }
        if (Input.GetKey(xPositive))
        {
            trueRotation.y += rotationScale * Time.deltaTime;
            this.transform.eulerAngles = trueRotation;
        }
        else if (Input.GetKey(xNegative))
        {
            trueRotation.y -= rotationScale * Time.deltaTime;
            this.transform.eulerAngles = trueRotation;
        }
    }
}