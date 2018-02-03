using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BBCharacterMover : MonoBehaviour 
{
    public NavMeshAgent nav;

    Vector3 targetEnd;
    public new Rigidbody rigidbody;
    public float walkSpeed = 1;
    public float attackSpeed = 10;
    public float range = 10;

    Vector3 end;
    Vector3 posLastFrame;
    bool isWalking = false;
    float curSpeed = 1;
	
    public float flungPower = 2.0f;

    public virtual void Setup()
    {
        end = transform.position;
    }

    public virtual void Logic()
    {
        if (end != transform.position && isWalking)
        {
            end = new Vector3(end.x, 0, end.z);
            if (nav.isOnNavMesh)
            {
                nav.SetDestination(end);
            }

            // transform.position += (end - transform.position) * curSpeed * Time.deltaTime;
            // nav.Move((nav.nextPosition - nav.transform.position).normalized * curSpeed * Time.deltaTime);
            //rigidbody.MovePosition((end - transform.position) * curSpeed * Time.deltaTime);
            // if ((end - transform.position).sqrMagnitude > 1.0f)
            {
                transform.LookAt(end);
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            }
        }
        posLastFrame = transform.position;
    }

    internal void IndicateMoveTo(Vector3 end)
    {
        // float smag = Mathf.Min((end - transform.position).sqrMagnitude, range * range);

        // targetEnd = transform.position + (end - transform.position).normalized * (float)System.Math.Sqrt(smag);
        // lineRenderer.SetPositions(nesw Vector3[2] { transform.position + Vector3.up * 0.1f, targetEnd + Vector3.up * 0.1f });
    }

    NavMeshPath pahttt;
    bool pathCalculated;

    internal virtual void MoveTo(Vector3 end)
    {
        this.end = end;
        isWalking = true;
        curSpeed = walkSpeed;
        if (nav.isOnNavMesh)
        {
            nav.SetDestination(end);
        }
    }

    public virtual void GetPushed(Vector3 push)
    {
        isWalking = false;
    }

    internal virtual void GetFlung(Vector3 direction, float force, Vector3 from)
    {
        isWalking = false;

        rigidbody.AddForce(direction.normalized * force, ForceMode.Impulse);
        rigidbody.AddRelativeTorque(Vector3.Cross(direction, transform.right), ForceMode.Impulse);

        transform.LookAt(from);
    }

	public virtual void TeleportTo(Vector3 position)
	{
		transform.position = position;
	}

    public void Ragdoll(Vector3 direction)
    {
        nav.enabled = false;
        rigidbody.isKinematic = false;
        GetFlung(direction, flungPower, transform.position);
    }

    public void Reset()
    {
        nav.enabled = true;
        rigidbody.isKinematic = true;
    }
}
