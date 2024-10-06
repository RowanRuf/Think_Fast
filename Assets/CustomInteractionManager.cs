using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class CustomInteractionManager : XRInteractionManager
{
    public void ForceDeselect(XRBaseInteractor interactor)
    {
        if (interactor.selectTarget)
            SelectExit(interactor, interactor.selectTarget);
    }
}
