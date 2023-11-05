using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private ZombieSpawn[] spawners;
    private ZombieSpawn selectedSpawner;
    private NavMeshAgent navAgent;
    private Transform target;
    private Player player;
    private RoundHandler roundHandler;
    private float health;
    private float distanceToPlayer;
    public float damage = 40f;
    private bool canHit = true;
    private bool inMap = true;
    private Animator animator;
    private bool isDead;

    void Start()
    {
        animator = GetComponent<Animator>();
        roundHandler = FindObjectOfType<RoundHandler>();
        spawners = FindObjectsOfType<ZombieSpawn>();
        selectedSpawner = spawners[Random.Range(0, spawners.Length)];
        navAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").gameObject.GetComponent<Player>();
        target = player.transform;
        navAgent.Warp(selectedSpawner.spawnPoint.position);
        health = 100f * roundHandler.round;
    }

    //If the zombie is alive, either chase the player or enter the map through a barricade
    void Update()
    {
        if (!isDead)
        {
            if (inMap) OutsideMap();
            else ChasePlayer();
        }
    }

    //Decreases zombie's health when damage is taken
    public void TakeDamage(float damage)
    {
        health = health - damage;
        if (health <= 0)
        {
            player.score += 100;
            Death();
        }
        else player.score += 10;
    }

    //Chase the player up until distance is 1.7, once distance is 1.7, attack player
    void ChasePlayer()
    {
        distanceToPlayer = Vector3.Distance(transform.position, target.transform.position);
        if (distanceToPlayer > 1.7)
        {
            navAgent.isStopped = false;
            navAgent.SetDestination(target.position);
        }
        else
        {
            navAgent.isStopped = true;
            Attack();
        }
    }

    //Enter the map through the barricade, start and end animation for climbing
    void OutsideMap()
    {
        navAgent.SetDestination(selectedSpawner.transform.position);
        if (Vector3.Distance(transform.position, selectedSpawner.transform.position) <= 2.5f)
        {
            animator.SetBool("isClimbing", true);
        }
        if (Vector3.Distance(transform.position, selectedSpawner.transform.position) <= 0.5f)
        {
            animator.SetBool("isClimbing", false);
            inMap = false;
        }
    }

    //Attack the player by casting a ray in their direction, if hit, apply damage
    void Attack()
    {
        if (canHit)
        {
            canHit = false;
            Vector3 offset = new Vector3(0.0f, 1f, 0.0f);
            RaycastHit hit;

            var lookPos = target.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 360f);

            if (Physics.Raycast(transform.position + offset, transform.TransformDirection(Vector3.forward), out hit, 1))
            {
                Debug.DrawRay(transform.position + offset, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                if (hit.collider.gameObject.tag == "Player")
                {
                    Player player = hit.collider.gameObject.GetComponent<Player>();
                    player.TakeDamage(damage);
                }
            }
            else
            {
                Debug.DrawRay(transform.position + offset, transform.TransformDirection(Vector3.forward), Color.white);
            }
            Invoke("ResetHit", 1f);
        }
    }

    //Resets the zombie's ability to hit the player
    void ResetHit()
    {
        canHit = true;
    }

    //Upon death, play death animation and signal the round handler, then remove from game
    void Death()
    {
        isDead = true;
        navAgent.isStopped = true;
        animator.SetBool("isDead", true);
        roundHandler.ZombieDeath();
        Object.Destroy(GetComponent<CapsuleCollider>());
        Invoke("Destroy", 5f);
    }

    //Destroy the zombie after it is killed
    void Destroy()
    {
        Object.Destroy(this.gameObject);
    }
}
