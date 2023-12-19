using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Camera playerCamera;
	[SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private Animator animator;
    private AgentLinkMover linkMover;
    
    private RaycastHit[] hits = new RaycastHit[1];
    
    private const string isRunning = "isRunning";
    private const string isJumping = "isJumping";
    private const string isLanding = "isLanding";
   
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        
        linkMover = GetComponent<AgentLinkMover>();
        
        linkMover.OnLinkStart += HandleLinkStart;
        linkMover.OnLinkEnd += HandleLinkEnd;
    }
    
    private void HandleLinkStart()
    {
		animator.SetTrigger(isJumping);
	}

	private void HandleLinkEnd()
    {
		animator.SetTrigger(isLanding);
	}
	
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.RaycastNonAlloc(ray, hits) > 0)
            {
                agent.SetDestination(hits[0].point);
            }
        }
        animator.SetBool(isRunning, agent.velocity.magnitude > 0.01f);
    }
}
