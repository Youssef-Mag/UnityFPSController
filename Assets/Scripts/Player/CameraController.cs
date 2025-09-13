using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRot;
    float yRot;

    float shakeCounterx = 0f;
    float shakeCountery = 0f;
    float shakeStrength;

    public float shakeFreq;
    public float shakeDecay;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Application.targetFrameRate=60;

    }

    // Update is called once per frame
    void Update()
    {
        //mouse input
        float mousex = Input.GetAxisRaw("Mouse X")  * sensX;
        float mousey = Input.GetAxisRaw("Mouse Y")  * sensY;

        yRot += mousex;
        xRot -= mousey;
        xRot = Mathf.Clamp(xRot, -90,90);

        //rotate
        transform.rotation = Quaternion.Euler(xRot, yRot, 0);
        orientation.rotation = Quaternion.Euler(0, yRot, 0);

        //screenshake
        if(shakeStrength < 0.1f){
            shakeCountery = 1;
            shakeCounterx = 1;
            return;
        }
        transform.rotation = Quaternion.Euler(
                                            xRot + (shakeStrength * Mathf.Cos(shakeFreq * shakeCounterx) * Mathf.Min(1.5f, shakeCounterx)), 
                                            yRot + (shakeStrength * Mathf.Cos(shakeFreq * shakeCountery) * Mathf.Min(1.5f, shakeCountery)), 0);

        shakeStrength *= shakeDecay;
        shakeCounterx += Random.Range(1,10)*Time.deltaTime;
        shakeCountery += Random.Range(1,10)*Time.deltaTime;
    }

    public void shake(float strength){
        if(strength > shakeStrength)
            shakeStrength = strength;

    }
}
