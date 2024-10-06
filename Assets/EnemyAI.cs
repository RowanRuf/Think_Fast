using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;
public class EnemyAI : MonoBehaviour
{
    public float timeTillMove = 0.0f;
    public NavMeshAgent agent;
    public LayerMask layerMask;
    public Transform headPos;
    public Transform eyePos;
    private Animator anim;

    [Header("Gun Settings")]
    public Transform gunShotT;
    public float timeBetweenShots = 2f;
    public GameObject bulletPrefab = null;
    public float shootingDistance = 6f;
    public bool canShoot = true;
    Vector3 pos;

    void Start()
    {
        anim = transform.root.GetChild(0).gameObject.GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(TimeBeforeMove());
    }

    void Update()
    {
        pos = headPos.position - eyePos.position;
        RaycastHit hit;
        if (Physics.Raycast(eyePos.position, pos, out hit, shootingDistance, layerMask))
        {
            if (hit.collider.gameObject.CompareTag("Player") == false)
            {
                return;
            }
            else if (canShoot && hit.collider.gameObject.CompareTag("Player"))
            {
                canShoot = false;
                Attack();
                StartCoroutine(ShootDelay());
            }
            //if Player is dead
            //else
        }
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    //reached StoppingDistance/Destination
                    anim.SetBool("idle", true);
                }
            }
        }
    }

    private void MoveToPlayer()
    {
        agent.SetDestination(headPos.position);
        anim.SetBool("idle", false);
    }
    private void Attack()
    {
        GameObject bullet = Instantiate(bulletPrefab, gunShotT.position, gunShotT.rotation);

    }
    IEnumerator TimeBeforeMove()
    {
        anim.SetBool("idle", true);
        yield return new WaitForSeconds(timeTillMove);
        MoveToPlayer();
    }

    public IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(timeBetweenShots);
        if(GetComponentInChildren<Animator>().enabled == true)
            canShoot = true;
    }
}
