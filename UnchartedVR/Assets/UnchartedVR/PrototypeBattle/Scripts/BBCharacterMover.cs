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
	
    // Use this for initialization
    public void Setup()
    {
        end = transform.position;
    }

    // Update is called once per frame
    public void Logic()
    {
        if (end != transform.position && isWalking && pathCalculated)
        {
            end = new Vector3(end.x, 0, end.z);

            // transform.position += (end - transform.position) * curSpeed * Time.deltaTime;
            // nav.Move((nav.nextPosition - nav.transform.position).normalized * curSpeed * Time.deltaTime);
            //rigidbody.MovePosition((end - transform.position) * curSpeed * Time.deltaTime);
            transform.LookAt(end);
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

    internal void MoveTo(Vector3 end)
    {
        this.end = end;
        isWalking = true;
        curSpeed = walkSpeed;
        nav.SetDestination(end);
    }

    public void GetPushed(Vector3 push)
    {
        isWalking = false;

    }

    internal void GetFlung(Vector3 direction, int force, Vector3 from)
    {
        isWalking = false;

        rigidbody.AddForce(direction * force, ForceMode.Impulse);

        transform.LookAt(from);
    }

	public void TeleportTo(Vector3 position)
	{
		transform.position = position;
	}
}
