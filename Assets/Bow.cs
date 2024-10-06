using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Bow : XRGrabInteractable
{
    public bool removeSurroundings = false;
    public GameObject[] surroundings;
    private Notch notch = null;
    public static Bow Instance;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
        notch = GetComponentInChildren<Notch>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        //set notch ready;
        selectEntered.AddListener(notch.SetReady);
        selectExited.AddListener(notch.SetReady);
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        if (removeSurroundings)
        {
            foreach (GameObject thing in surroundings)
            {
                Destroy(thing);
            }
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        selectEntered.RemoveListener(notch.SetReady);
        selectExited.RemoveListener(notch.SetReady);
    }
}
