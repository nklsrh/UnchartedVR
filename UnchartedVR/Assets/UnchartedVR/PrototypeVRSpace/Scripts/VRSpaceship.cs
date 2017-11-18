using UnityEngine;
using UnityEngine.UI;

public class VRSpaceship : MonoBehaviour
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

    public Transform gyroIndicator;

    public float rotationLerp = 0.001f;

    public float clampMagnitude = 10;

    public float accelerationlerp = 1;
    public float decelerator = 10;
    public float accelerationDecelerator = 5f;

    public float cameraLerp = 10.0f;

    public float gyroMultiplier = 0.01f;
    public float gyroLerp = 10.0f;


	MeshRenderer oldMesh;
	Material oldM;

    float rot;
    Vector3 vel;
    Vector3 acc;
    float distanceOfLastRaycast;


    Vector3 gyroRotation;
    public Text txtDebug;
    public Text txtSpeed;


    void Update ()
    {
        Vector3 originPoint = handController.m_model.transform.position;
        Vector3 originDirection = handController.m_model.transform.forward;

#if UNITY_EDITOR
        originPoint = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
        originDirection = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
#endif

        RaycastHit hitInfo;
        int layer = ~(1 << LayerMask.NameToLayer("SHIP"));
		if (Physics.Raycast(originPoint, originDirection, out hitInfo, 10000000))
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



        Quaternion handRotation = handController.m_model.transform.rotation;



        Vector3 rawAngles = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote).eulerAngles;

        if (rawAngles.x > 180)
        {
            rawAngles.x -= 360;
        }
        if (rawAngles.y > 180)
        {
            rawAngles.y -= 360;
        }
        if (rawAngles.z > 180)
        {
            rawAngles.z -= 360;
        }

        Vector3 cleanedAngles = new Vector3(
            Mathf.Clamp((rawAngles.x - -20) / 30, -1, 1),
            0,
            Mathf.Clamp((rawAngles.z - 0) / 50, -1, 1)
        );

        Vector3 joystickAngle = cleanedAngles;

#if UNITY_EDITOR
        joystickAngle = new Vector3(Input.GetAxis("Vertical"), 0, -Input.GetAxis("Horizontal"));
#endif

        gyroRotation = Vector3.Slerp(gyroRotation, joystickAngle, gyroLerp * Time.deltaTime);

        shipppp.transform.Rotate(gyroRotation, Space.Self);

        gyroIndicator.localRotation = Quaternion.Euler(gyroRotation);

        float thrust = (Input.GetButton("Thrust") ? 1.0f : 0f);

        Vector3 accelerator = new Vector3(0, 0, touchPadInput.y);
#if UNITY_EDITOR
        accelerator = new Vector3(0, 0, thrust);
#endif


        Vector3 finalAceleration = shipppp.transform.TransformDirection(accelerator);

        acc = Vector3.Lerp(acc, accelerator, accelerationlerp * Time.deltaTime);

        vel += acc;

        vel = Vector3.ClampMagnitude(vel, clampMagnitude);

        shipppp.transform.Translate(vel);

        vel = Vector3.Lerp(vel, Vector3.zero, decelerator * Time.deltaTime);
        acc = Vector3.Lerp(acc, Vector3.zero, accelerationDecelerator * Time.deltaTime);

        cameraRig.transform.position = Vector3.Lerp(cameraRig.transform.position, shipSeat.transform.position, cameraLerp * Time.deltaTime);
        cameraRig.transform.rotation = shipSeat.transform.rotation;

        cameraZoomLens.LookAt(pointerSphere);


        txtDebug.text = rawAngles.ToString() + "\n" + cleanedAngles.ToString();

        txtSpeed.text = (vel.sqrMagnitude * 100000).ToString("#00");

        // TELEPORTING

        //         bool isJustClicked = OVRInput.GetUp(OVRInput.Button.One);
        // #if UNITY_EDITOR
        //         isJustClicked = Input.GetMouseButtonUp(0);
        // #endif

        //         if (isJustClicked)
        //         {
        //             shipppp.transform.position = this.pointerSphere.transform.position + Vector3.up * 0.5f;
        //         }



        // MOVE MONITOR TO FRONT OF EYE

        // monitorTransform.position = Vector3.Lerp(monitorTransform.position, centreCameraTransform.TransformPoint(monitorPositioning), monitorLerp * Time.deltaTime);
        // monitorTransform.rotation = centreCameraTransform.rotation;


        gyroIndicator.position = Vector3.Lerp(gyroIndicator.position, centreCameraTransform.TransformPoint(monitorPositioning), monitorLerp * Time.deltaTime);
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
