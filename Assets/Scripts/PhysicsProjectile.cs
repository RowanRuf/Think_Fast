using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
[RequireComponent(typeof(Rigidbody))]
public class PhysicsProjectile : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    public float bulletSpeed = 15f;
    private Rigidbody rigidBody;
    public AudioClip[] gunSounds;
    public GameObject particle;

    public GameObject brokenGlass;

    private void Awake()
    {
        GetComponent<AudioSource>().PlayOneShot(gunSounds[0]);
        GetComponent<AudioSource>().PlayOneShot(gunSounds[1]);
        transform.LookAt(Camera.main.transform.parent.TransformPoint(InputTracking.GetLocalPosition(XRNode.Head)));
        rigidBody = GetComponent<Rigidbody>();
        Vector3 relativePos = Camera.main.transform.parent.TransformPoint(InputTracking.GetLocalPosition(XRNode.Head)) - transform.position;
        rigidBody.AddForce(relativePos * bulletSpeed, ForceMode.Impulse);
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("obstacle"))
        {
            Instantiate(particle, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("glass"))
        {
            Destroy(other.transform.parent.gameObject);
            GameObject glass = Instantiate(brokenGlass, transform.position, transform.rotation);
            GetComponent<AudioSource>().PlayOneShot(gunSounds[3]);
        }
        else if(other.gameObject.CompareTag("Player"))
        {
            MainMenu.Instance.RestartLevel();
            GetComponent<AudioSource>().PlayOneShot(gunSounds[2]);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("obstacle"))
        {
            Instantiate(particle, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("glass"))
        {
            Destroy(other.transform.parent.gameObject);
            GameObject glass = Instantiate(brokenGlass, transform.position, transform.rotation);
            GetComponent<AudioSource>().PlayOneShot(gunSounds[3]);
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            MainMenu.Instance.RestartLevel();
            GetComponent<AudioSource>().PlayOneShot(gunSounds[2]);
        }
    }
}
