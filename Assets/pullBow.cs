using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class pullBow : XRGrabInteractable
{
    public Transform linePos1;
    public Transform linePos2;
    public Notch _notch;
    private XRDirectInteractor interactor = null;
    private bool pullingBow = false;
    public ActionBasedController xrController = null;
    private Arrow currentArrow = null;
    public Transform bow;
    public AudioClip[] bowSFX;

    private CustomInteractionManager CustomManager => interactionManager as CustomInteractionManager;
    void Start()
    {
        transform.position = linePos1.position;
    }
    void FixedUpdate()
    {
        if (pullingBow)
        {
            VibrateOnPull();
            transform.LookAt(bow.position);
        }
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        if (CanFire())
        {
            currentArrow.Launch(_notch);
            bow.root.gameObject.GetComponent<AudioSource>().PlayOneShot(bowSFX[1]);
        }
        transform.position = linePos1.position;
        transform.rotation = linePos1.rotation;
        transform.localScale = new Vector3(1f, 1f, 1f);
        pullingBow = false;
    }
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if(_notch.selectTarget != null)
        {
            currentArrow = _notch.selectTarget.GetComponent<Arrow>();
            pullingBow = true;
            bow.root.gameObject.GetComponent<AudioSource>().PlayOneShot(bowSFX[0]);
        }

        xrController = args.interactor.GetComponent<ActionBasedController>();
    }
    private bool CanFire()
    {
        if (Vector3.Distance(transform.position, linePos1.transform.position) > 0.5f && _notch.selectTarget is Arrow)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void VibrateOnPull()
    {
        if (CanFire())
        {
            xrController.SendHapticImpulse(0.4f, 0.5f); //float amplitude, float duration
        }
    }
}

