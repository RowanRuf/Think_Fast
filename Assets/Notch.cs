using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(PullMeasurer))]
public class Notch : XRSocketInteractor
{
    [Range(0, 1)] public float releaseThreshold = 0.25f;

    public pullBow PullBow;
    public bool IsReady { get; private set; } = false;

    private CustomInteractionManager CustomManager => interactionManager as CustomInteractionManager;

    protected override void Awake()
    {
        base.Awake();
    }
    public void SetReady(BaseInteractionEventArgs args)
    {
        IsReady = args.interactable.isSelected;
    }

    public override bool CanSelect(XRBaseInteractable interactable)
    {
        return base.CanSelect(interactable) && CanHover(interactable) && IsArrow(interactable) && IsReady;
    }

    private bool IsArrow(XRBaseInteractable interactable)
    {
        return interactable is Arrow;
    }

    public override XRBaseInteractable.MovementType? selectedInteractableMovementTypeOverride
    {
        get { return XRBaseInteractable.MovementType.Instantaneous; }
    }

    public override bool requireSelectExclusive => false;
}
