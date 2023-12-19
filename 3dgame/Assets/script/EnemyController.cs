using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{

    [SerializeField]
    private Animator enemyAnimator;
    private AgentLinkMover linkMover;

    public Transform target;
    public float updateSpeed = 0.1f;

    private NavMeshAgent agent;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        linkMover = GetComponent<AgentLinkMover>();

        linkMover.OnLinkStart += HandleLinkStart;
        linkMover.OnLinkEnd += HandleLinkEnd;
    }

    private void Start()
    {
        StartCoroutine(followTarget());
    }


    private void HandleLinkStart()
    {
        enemyAnimator.SetTrigger("Jump");
    }

    private void HandleLinkEnd()
    {
        enemyAnimator.SetTrigger("Landed");
    }

    // Update is called once per frame
    private IEnumerator followTarget()
    {
        WaitForSeconds wait = new WaitForSeconds(updateSpeed);

        while (enabled)
        {
            agent.SetDestination(target.position);

            if (agent.velocity != Vector3.zero)
            {
                enemyAnimator.SetBool("isRunning", true);
            }

            else if (agent.velocity == Vector3.zero)
            {
                enemyAnimator.SetBool("isRunning", false);
            }

            yield return wait;
        }
    }

    /*public void StartChasing()
    {
        agent.SetDestination(target.position);

    }*/


}
