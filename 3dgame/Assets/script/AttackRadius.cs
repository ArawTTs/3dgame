    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]

public class AttackRadius : MonoBehaviour
{
    private List<IDamageable> Damageables = new List<IDamageable>();
    public int damage = 10;
    public float attackDelay = 0.5f;
    public delegate void AttackEvent(IDamageable Target);
    public AttackEvent onAttack;
    private Coroutine attackCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the AttackRadius.");
        }
        else if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy entered the AttackRadius.");
        }

        IDamageable damageable = other.GetComponent<IDamageable>();
        if(damageable != null)
        {
            Damageables.Add(damageable);
            if(attackCoroutine == null)
            {
                attackCoroutine = StartCoroutine(Attack());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if(damageable != null)
        {
            Damageables.Remove(damageable);
            if(Damageables.Count == 0)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }
    }

    private IEnumerator Attack()
    {
        WaitForSeconds wait = new WaitForSeconds(attackDelay);

        yield return wait;

        IDamageable closestDamageable = null;
        float closestDistance = float.MaxValue;

        while(Damageables.Count > 0)
        {
            for(int i = 0; i < Damageables.Count; i++)
            {
                Transform damageableTransform = Damageables[i].GetTransform();
                float distance = Vector3.Distance(transform.position, damageableTransform.position);

                if(distance < closestDistance)
                {
                    closestDistance = distance;
                    closestDamageable = Damageables[i];
                }
            }

            if(closestDamageable != null)
            {
                onAttack?.Invoke(closestDamageable);
                closestDamageable.TakeDamage(damage);
            }

            closestDamageable = null;
            closestDistance = float.MaxValue;

            yield return wait;

            Damageables.RemoveAll(DisableDamageables);
        }
        attackCoroutine = null;
    }

    private bool DisableDamageables(IDamageable Damageable)
    {
        return Damageable != null && !Damageable.GetTransform().gameObject.activeSelf;
    }
}
