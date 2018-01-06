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
    public Transform joystick;

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


    Quaternion gyroRotation;
    public Text txtDebug;
    public Text txtSpeed;


    Quaternion easedRot;
    public float easeRotationLerp = 0.5f;
    public float easeRotationAmount = 10;

    float upThrustLerped;
    public float upThrustLerp = 2.0f;

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


        float anglePitch = Mathf.Clamp((rawAngles.x - -20) / 30, -1, 1);
        float angleRoll = -Mathf.Clamp((rawAngles.z - 0) / 50, -1, 1);
        anglePitch = touchPadInput.y;
#if UNITY_EDITOR
        angleRoll = Input.GetAxis("Horizontal");
        anglePitch = Input.GetAxis("Vertical");
#endif

        Quaternion joystickAngle = Quaternion.Euler(0, angleRoll, 0);

        gyroRotation = Quaternion.Slerp(gyroRotation, joystickAngle, gyroLerp * Time.deltaTime);

        shipppp.transform.Rotate(gyroRotation.eulerAngles, Space.Self);

        gyroIndicator.localRotation = gyroRotation;



        if (joystick)
        {
            joystick.transform.localRotation = Quaternion.Euler(-angleRoll * 45, anglePitch * 45, 0);
        }

        float updownThrust = touchPadInput.y;
#if UNITY_EDITOR
        updownThrust = -Input.GetAxis("Vertical");
#endif

        upThrustLerped = Mathf.Lerp(upThrustLerped, updownThrust, upThrustLerp * Time.deltaTime);


        float accelerator = IsHoldingTrigger ? 1 : 0;

        easedRot = Quaternion.Euler(
            upThrustLerped * -easeRotationAmount, 
            shipppp.transform.localRotation.eulerAngles.y, 
            angleRoll * -easeRotationAmount);

        shipppp.transform.localRotation = Quaternion.Slerp(shipppp.transform.localRotation, easedRot, easeRotationLerp * Time.deltaTime);

        Vector3 flatForward = new Vector3(shipppp.transform.forward.x, 0, shipppp.transform.forward.z);

        Vector3 accelerationVector = upThrustLerped * Vector3.up + accelerator * flatForward;

        acc = Vector3.Lerp(acc, accelerationVector, accelerationlerp * Time.deltaTime);

        vel += acc;

        vel = Vector3.ClampMagnitude(vel, clampMagnitude);

        shipppp.transform.Translate(vel);

        vel = Vector3.Lerp(vel, Vector3.zero, decelerator * Time.deltaTime);
        acc = Vector3.Lerp(acc, Vector3.zero, accelerationDecelerator * Time.deltaTime);

        cameraRig.transform.position = Vector3.Lerp(cameraRig.transform.position, shipSeat.transform.position, cameraLerp * Time.deltaTime);
        cameraRig.transform.rotation = shipSeat.transform.rotation;

        cameraZoomLens.LookAt(pointerSphere);

        txtDebug.text = OVRPlugin.GetAppFramerate().ToString("00 FPS") + transform.position.ToString();

        txtSpeed.text = (vel.sqrMagnitude * 100000).ToString("#00");

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
    public bool IsHoldingTrigger
    {
        get
        {
#if UNITY_EDITOR
            return Input.GetMouseButton(0);
#else
            return OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
#endif
        }
    }
}
