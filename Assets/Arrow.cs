using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class Arrow : XRGrabInteractable
{

    public float speed = 5.0f;


    public Transform tip = null;
    public LayerMask layerMask = ~Physics.IgnoreRaycastLayer;

    private new Collider collider = null;
    private new Rigidbody rigidbody = null;

    public AudioClip[] hitSFX;
    private Vector3 lastPosition = Vector3.zero;
    private bool launched = false;

    public GameObject brokenGlass;

    protected override void Awake()
    {
        base.Awake();
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        if (args.interactor is XRDirectInteractor)
        {
            Clear();
        }

        base.OnSelectEntering(args);
    }

    private void Clear()
    {
        SetLaunch(false);
        TogglePhysics(true);
    }

    public void Launch(Notch notch)
    {
        notch.enabled = false;
        SetLaunch(true);
        UpdateLastPosition();
        TogglePhysics(true);
        notch.enabled = true;
        ApplyForce(notch.PullBow);
    }

    private void SetLaunch(bool value)
    {
        launched = value;
    }

    private void UpdateLastPosition()
    {
        lastPosition = tip.position;
    }

    private void ApplyForce(pullBow pullMeasurer)
    {
        float power = Vector3.Distance(pullMeasurer.transform.position, pullMeasurer.linePos1.transform.position); //maybe x2?
        Vector3 force = transform.forward * (power * 3f * speed);
        rigidbody.AddForce(force, ForceMode.Impulse);
    }
    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (launched)
        {
            // Check for collision as often as possible
            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
            {
                UpdateLastPosition();
            }

            // Only set the direction with each physics update
            //if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Fixed)
            //    SetDirection();
        }
    }

    private void SetDirection()
    {
        // Look in the direction the arrow is moving
        if (rigidbody.velocity.z > 0.5f)
            transform.forward = rigidbody.velocity;
    }
    private bool CheckForCollision()
    {
        if(Physics.Linecast(lastPosition, tip.position, out RaycastHit hit, layerMask))
        {
            TogglePhysics(false);
        }

        return hit.collider != null;
    }

    void OnCollisionEnter(Collision other)
    {
        if (launched)
        {
            if (other.gameObject.CompareTag("ENEMY"))
            {
                other.transform.root.gameObject.GetComponent<EnemyAI>().agent.SetDestination(other.transform.root.position);
                other.transform.root.gameObject.GetComponent<EnemyAI>().canShoot = false;
                StopCoroutine(other.transform.root.gameObject.GetComponent<EnemyAI>().ShootDelay());
                other.transform.root.GetChild(0).gameObject.GetComponent<Animator>().enabled = false; //enables ragdoll
                other.transform.root.GetChild(0).gameObject.GetComponent<Collider>().enabled = false;
                Destroy(other.transform.root.gameObject, 5f);
                tip.gameObject.GetComponent<AudioSource>().PlayOneShot(hitSFX[1]);
                EnemyManager.Instance.enemiesLeft -= 1;
            }
            else if (other.gameObject.CompareTag("glass"))
            {
                Destroy(other.transform.parent.gameObject);
                GameObject glass = Instantiate(brokenGlass, transform.position, transform.rotation);
                tip.gameObject.GetComponent<AudioSource>().PlayOneShot(hitSFX[3]);
            }
            else
            {
                tip.gameObject.GetComponent<AudioSource>().PlayOneShot(hitSFX[0]);
                ChildArrow(other);
                TogglePhysics(false);
            }
            launched = false;
        }
    }

    private void TogglePhysics(bool value)
    {
        rigidbody.isKinematic = !value;
        rigidbody.useGravity = value;
    }

    private void ChildArrow(Collision hit)
    {
        Transform newParent = hit.transform;
        transform.SetParent(newParent);
    }
}
