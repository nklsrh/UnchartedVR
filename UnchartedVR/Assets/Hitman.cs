using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitman : MonoBehaviour
{
	public OVRPlayerController playerController;
    public OVRGearVrController handController;
    public OVRCameraRig cameraRig;

	public Transform pointerSphere;
    public Transform shipppp;
    public Transform shipSeat;
    public Transform monitorTransform;

    public Transform centreCameraTransform;

    public Transform cameraZoomLens;

    void Start ()
    {

	}


	MeshRenderer oldMesh;
	Material oldM;


    Vector3 vel;
    Vector3 acc;

    public float rotationLerp = 0.001f;
    float rot;

    public float clampMagnitude = 10;

    public float accelerationlerp = 1;
    public float decelerator = 10;
    public float accelerationDecelerator = 5f;

    public float cameraLerp = 10.0f;

    float distanceOfLastRaycast;

    void Update ()
    {
        RaycastHit hitInfo;
        int layer = ~(1 << LayerMask.NameToLayer("SHIP"));
		if (Physics.Raycast(handController.m_model.transform.position, handController.m_model.transform.forward, out hitInfo, 10000000))
		{
			MeshRenderer newMesh = hitInfo.collider.gameObject.GetComponent<MeshRenderer>();
			if (oldMesh != newMesh)
			{
				ResetMaterial();
			}
			oldMesh = newMesh;
			oldM = oldMesh.material;
			oldMesh.material = new Material(oldM);
			oldMesh.material.SetColor(0, Color.green);

			pointerSphere.transform.position = hitInfo.point;

            distanceOfLastRaycast = hitInfo.distance;
		}
		else
		{
			ResetMaterial();

            pointerSphere.transform.position = handController.m_model.transform.position + handController.m_model.transform.forward * distanceOfLastRaycast;
		}


        Vector2 touchPadInput =  OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);

        Vector3 accelerator = new Vector3(touchPadInput.x, 0, touchPadInput.y);
#if UNITY_EDITOR
        accelerator = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
#endif


        Vector3 finalAceleration = cameraRig.transform.TransformDirection(accelerator);

        acc += Vector3.Lerp(acc, accelerator, accelerationlerp * Time.deltaTime);

        vel += acc;


        vel = Vector3.Lerp(vel, Vector3.zero, decelerator * Time.deltaTime);
        acc = Vector3.Lerp(acc, Vector3.zero, accelerationDecelerator * Time.deltaTime);

        vel = Vector3.ClampMagnitude(vel, clampMagnitude);

        shipppp.transform.Translate(vel);

        // shipppp.transform.Rotate(Vector3.up * rot * Time.deltaTime);

        shipppp.transform.rotation = centreCameraTransform.transform.rotation; //(Quaternion.LookRotation(cameraRig.transform.forward, cameraRig.transform.up));

        cameraRig.transform.position = Vector3.Lerp(cameraRig.transform.position, shipSeat.transform.position, cameraLerp * Time.deltaTime);
    
        cameraZoomLens.LookAt(pointerSphere);


        bool isJustClicked = OVRInput.GetUp(OVRInput.Button.One);
#if UNITY_EDITOR
        isJustClicked = Input.GetMouseButtonUp(0);
#endif

        if (isJustClicked)
        {
            shipppp.transform.position = this.pointerSphere.transform.position + Vector3.up * 0.5f;
        }

        monitorTransform.position = Vector3.Lerp(monitorTransform.position, centreCameraTransform.TransformPoint(monitorPositioning), monitorLerp * Time.deltaTime);
        monitorTransform.rotation = centreCameraTransform.rotation;
	}

    public Vector3 monitorPositioning;
    public float monitorLerp = 10.0f;

	void ResetMaterial()
	{
		if (oldMesh != null)
		{
			oldMesh.material = oldM;
		}
	}
}
