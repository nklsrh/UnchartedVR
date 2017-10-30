using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBMeleeController : MonoBehaviour 
{
    public float attackDamageFirstHit = 30;
    public float attackDamageMultiplier = 1.5f;
    public float attackDamageMax = 150;
    public float attackRadius = 30;

    BBMeleeController lastAttackedEnemy;
    float attackDamage = 0;

    public bool Attack(BBMeleeController e)
    {
        if (lastAttackedEnemy != e)
        {
            attackDamage = attackDamageFirstHit;
        }

        if (e.GetComponent<BBHealthController>())
        {
            e.GetComponent<BBHealthController>().Damage(attackDamage);
            attackDamage = attackDamage * attackDamageMultiplier;
            return true;
        }

        return false;
    }

    internal void GetAttacked(BBMeleeController agent)
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

        Vector3 direction = (transform.position - agent.transform.position + Vector3.up * 1).normalized;
        GetComponent<BBCharacterMover>().GetFlung(direction, 10, agent.transform.position);

        transform.LookAt(agent.transform);
    }
}
