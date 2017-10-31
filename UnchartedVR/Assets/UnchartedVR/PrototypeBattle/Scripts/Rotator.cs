using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

    public float speed = 10;
    public Vector3 axis = Vector3.one;

    static float lastFrameTime;

    void Update ()
    {
        transform.Rotate(speed * axis * Time.deltaTime, Space.Self);
	}
}
