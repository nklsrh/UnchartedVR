using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBHealthController : MonoBehaviour 
{
    public float maxHealth;
    public float current;

	System.Action OnDeath;

    public void Reset()
    {
        current = maxHealth;
    }

    public void Damage(float amount)
    {
        current -= amount;
		
        if (current <= 0)
        {
			if (OnDeath != null)
			{
            	OnDeath.Invoke();
			}
        }
    }
}
