using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBHealthController : MonoBehaviour 
{
    public float maxHealth;
    public float current;

    public System.Action OnDeath;
    public System.Action<float> OnDamage;

    public void Setup()
    {
        Reset();
    }

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
        else
        {
            if (OnDamage != null)
            {
                OnDamage.Invoke(amount);
            }
        }
    }

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        // Debug.LogWarning("HIT : " + LayerMask.LayerToName(other.gameObject.layer));
        if (other.gameObject.layer == LayerMask.NameToLayer("Projectile"))
        {
            Debug.LogWarning("ON HITTTED " + current);
            Damage(25);
            other.gameObject.SetActive(false);
        }
    }
}
