using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class Slowmotion : MonoBehaviour
{
    private float slowMo = 0.1f;
    private float normTime = 1.0f;
    private bool doSlowMo = false;

    private float speed;
    private Vector3 oldPosition;

    void Update()
    {
        speed = Vector3.Distance(oldPosition, transform.position);
        oldPosition = transform.position;
        if(speed < 0.01f)
        {
            if (!doSlowMo)
            {
                Time.timeScale = slowMo;
                Time.fixedDeltaTime = Time.timeScale / 72.0f;
                doSlowMo = true;
            }
        }
        else
        {
            if (doSlowMo)
            {
                Time.timeScale = normTime;
                Time.fixedDeltaTime = Time.timeScale / 72.0f;
                doSlowMo = false;
            }
        }
    }
}
