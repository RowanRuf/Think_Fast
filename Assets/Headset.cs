using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class Headset : MonoBehaviour
{
    GameObject collid;
    // Start is called before the first frame update
    void Start()
    {
        collid = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        collid.transform.position = Camera.main.transform.parent.TransformPoint(InputTracking.GetLocalPosition(XRNode.Head));
        collid.transform.rotation = InputTracking.GetLocalRotation(XRNode.Head);
    }
}
