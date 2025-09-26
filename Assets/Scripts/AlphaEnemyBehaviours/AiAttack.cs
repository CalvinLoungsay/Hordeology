using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AiAttack : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject player, pathObject;
    public LayerMask whatIsGroud, whatIsPlayer, whatIsObject;
    public int aiDamage;


    // Attacking
    public float timeBetweenAttacks, attackKnockback;
    bool alreadAttacked, canAttack, playerInSphere;

    //States 
    public float attackRange;
    bool objectInPath, playerInAttackRange;

    //throw object 
    public Transform cam;
    public Transform attackPoint;
    public LineRenderer line;


    [Header("Shooting")]
    public float attackSpread = 0;

    float timer = 0.0f;
    public float targetUpdateTimer = 0.1f;


    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        canAttack = true;
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0.0f)
        {
            // Check for sight and attack range
            //playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
            cam.transform.LookAt(player.transform);
            if (canAttack) {
                if (Physics.Raycast(cam.position, cam.forward, out RaycastHit rayHit, attackRange)) {
                    {
                        if (LayerMask.LayerToName(rayHit.collider.gameObject.layer) == "Player" 
                            || LayerMask.LayerToName(rayHit.collider.gameObject.layer) == "Object")
                        {
                            float objAggroRange = this.GetComponent<FindPlayer>().objectAggroRange;
                            bool playerNearby = Physics.CheckSphere(rayHit.collider.transform.position, objAggroRange, whatIsPlayer);
                            if (playerNearby) {
                                Attack(rayHit.collider.gameObject);
                            }
                        } else {
                            ResetActions();
                        }
                    }
                } else {
                    ResetActions();
                }
            }  else {
                ResetActions();
            } 
            timer = targetUpdateTimer;
        }    
    }

    private void ResetActions() {
        CancelInvoke("Shoot");
        CancelInvoke(nameof(ResetAttack));
        ResetAttack();
    }
    
    private void Attack(GameObject target)
    {
        agent.SetDestination(transform.position);
        cam.transform.LookAt(target.transform);
        if (!alreadAttacked)
        {
            // Attack code 
            Invoke("Shoot", timeBetweenAttacks);
            alreadAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadAttacked = false;
    }

    private void Shoot()
    {
        Vector3 forceDirection = cam.transform.forward;

        float x = Random.Range(-attackSpread, attackSpread);
        float y = Random.Range(-attackSpread, attackSpread);
        Vector3 direction = cam.forward + cam.up * y + cam.right * x;
        if (Physics.Raycast(cam.position, direction, out RaycastHit rayHit))
        {
            if (rayHit.collider.gameObject.GetComponent<Target>() != null)
            {
                if (LayerMask.LayerToName(rayHit.collider.gameObject.layer) == "Player")
                {
                    player.GetComponent<Target>().Damage(aiDamage);
                    Debug.Log("hit player");
                }
                else if (LayerMask.LayerToName(rayHit.collider.gameObject.layer) == "Object")
                {
                    rayHit.collider.gameObject.GetComponent<Target>().Damage(aiDamage);
                    Debug.Log("hit object");
                    if (rayHit.collider.gameObject.GetComponent<Rigidbody>() != null) {
                        rayHit.collider.gameObject.GetComponent<Rigidbody>().AddForce(direction * attackKnockback, ForceMode.Impulse);
                    }
                }
            }
            
            LineRenderer enemyAttack = Instantiate(line);
                enemyAttack.SetPosition(0, transform.position);
                enemyAttack.SetPosition(1, rayHit.point);
        }

    }

    public void EnableAttack() {
        canAttack = true;
    }

    public void DisableAttack() {
        canAttack = false;
    }

}

// Tutorial : https://www.youtube.com/watch?v=UjkSFoLxesw
