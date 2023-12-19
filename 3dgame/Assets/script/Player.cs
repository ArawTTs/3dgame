using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] 
    private AttackRadius attackRadius;

    [SerializeField] 
    private Animator animator;
    private Coroutine lookCoroutine;

    [SerializeField]
    private int Health = 300;
    private int currentHealth;

    [SerializeField] FloatingStatusBar statusBar;

    

    private void Awake()
    {
        attackRadius.onAttack += OnAttack;
        currentHealth = Health;
        statusBar = GetComponentInChildren<FloatingStatusBar>();
    }

    private void OnAttack(IDamageable Target)
    {
        animator.SetTrigger("Attack");

        if(lookCoroutine != null)
        {
            StopCoroutine(lookCoroutine);
        }

        lookCoroutine = StartCoroutine(LookAt(Target.GetTransform()));

        Debug.Log("ATAK!");
    }

    private IEnumerator LookAt(Transform Target)
    {
        Quaternion lookRotation = Quaternion.LookRotation(Target.position - transform.position);
        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);

            time += Time.deltaTime * 2;
            yield return null;
        }

        transform.rotation = lookRotation;
        Debug.Log("HELLOTHERE");
    }


    public void TakeDamage(int Damage)
    {
        currentHealth -= Damage;
        statusBar.UpdateStatusBar(currentHealth, Health);
        if(currentHealth <= 0)
        {
            animator.SetTrigger("Die");
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
