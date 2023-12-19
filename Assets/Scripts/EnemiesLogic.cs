using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemiesLogic : MonoBehaviour
{
    private GameObject target;
    private NavMeshAgent agent;
    private Life life;
    private Animator animator;
    private Collider collider;
    private Life playerLife;
    private PlayerLogic playerLogic;
    public bool life0 = false;
    public bool isAttacking = false;
    public float speed = 1.0f;
    public float angularSpeed = 120;
    public float damage = 25;
    public bool looking;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");
        playerLife = target.GetComponent<Life>();
        if (playerLife == null)
        {
            throw new System.Exception("The player object hasn't Life component.");
        }

        playerLogic = target.GetComponent<PlayerLogic>();

        if (playerLogic == null)
        {
            throw new System.Exception("The player object hasn't PlayerLogic component.");
        }

        agent = GetComponent<NavMeshAgent>();
        life = GetComponent<Life>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        LifeReview();
        Pursue();
        AttackReview();
        IsFacingPlayer();

    }

    void IsFacingPlayer()
    {
        Vector3 forward = transform.forward;
        Vector3 targetPlayer = (GameObject.Find("Player").transform.position - transform.position).normalized;

        if (Vector3.Dot(forward, targetPlayer) < 0.6f)
        {
            looking = false;
        }
        else
        {
            looking = true;
        }
    }

    void LifeReview()
    {
        if (life0) return;
        if (life.value <= 0)
        {
            life0 = true;
            agent.isStopped = true;
            collider.enabled = false;
            animator.CrossFadeInFixedTime("Life0", 0.1f);

            // No tuto
            float destructionTime = (gameObject.name == "Zombie1") ? 3f : 8f;
            Destroy(gameObject, destructionTime);
        }
    }

    void Pursue()
    {
        if (life0) return;
        if (playerLogic.life0) return;
        agent.destination = target.transform.position;
    }

    void AttackReview()
    {
        if (life0) return;
        if (isAttacking) return;
        if(playerLogic.life0) return;
        float targetDistance = Vector3.Distance(target.transform.position, transform.position);
        if (targetDistance <= 2.0 && looking)
        {
            Attack();
        }
    }

    void Attack()
    {
        playerLife.TakeDamage(damage);
        agent.speed = 0;
        agent.angularSpeed = 0;
        isAttacking = true;
        animator.SetTrigger("MustAttack");
        Invoke("RestartAttack", 1.5f);
    }

    void RestartAttack()
    {
        isAttacking = false;
        agent.speed = speed;
        agent.angularSpeed = angularSpeed;
    }
}
