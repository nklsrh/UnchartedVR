using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBVRController : MonoBehaviour 
{
    public BBPlayerController playerController;

    [Header("VR objects")]
    public OVRGearVrController handController;
    public OVRCameraRig cameraRig;
    public Transform centreCameraTransform;

    [Header("Move")]
	public Transform pointerMove;
	public Transform teleportReticule;

    [Header("Attack")]
	public Transform pointerAttack;
    public Transform ball;

    public Transform handTransform;

    [Header("Camera")]
    public float cameraHeight = 2.0f;

    public float rotationLerp = 0.001f;
    public float clampMagnitude = 10;

    public float accelerationlerp = 1;
    public float decelerator = 10;
    public float accelerationDecelerator = 5f;

    public float cameraLerp = 10.0f;
    
    [Header("Grabbing Objects")]
    public float grabDistance = 2.0f;
    public float grabbedObjectLerp = 14.0f;
    public float grabThrowPower = 200.0f;
    public float handRotationLerp = 15f;

    float distanceOfLastRaycast;

    Vector3 teleportLookDirection;
    Vector3 firstPressTeleportPoint;
    Vector3 teleportReticuleUpNormal;

    int raycastLayer;   
    RaycastHit hitInfo;

    float distanceOfLastRaycastAttack;
    RaycastHit hitInfoAttack;
    Collider colliderLastHit;

    public void Setup()
    {
        raycastLayer = LayerMask.GetMask("Walkable", "Grabbable");
    }

    public void Logic()
    {

        // RaycastGaze();

        Vector2 touchPadInput =  OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
        Vector3 accelerator = new Vector3(touchPadInput.x, 0, touchPadInput.y);

#if UNITY_EDITOR
        accelerator = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (!IsHolding && !HasHeld)
        {
            playerController.transform.Rotate(accelerator.z, accelerator.x, 0);
        }
#endif

        bool isJustClicked = OVRInput.GetUp(OVRInput.Button.One);
        
        if (HasHeld)
        {
            //firstPressTeleportPoint = pointerAttack.position;
            //teleportReticule.position = firstPressTeleportPoint;
            //teleportReticule.up = teleportReticuleUpNormal;
             
            //teleportReticule.localScale = Vector3.one * Mathf.Clamp((teleportReticule.position - transform.position).sqrMagnitude * 0.05f, 0.5f, 10);
        }
        else if (IsHolding)
        {
            //teleportLookDirection = Camera.main.transform.TransformVector(accelerator);
            //teleportLookDirection.y = 0;
            //teleportReticule.forward = teleportLookDirection;
        }
        else if (HasLifted)
        {
            isJustClicked = true;
        }


        if (isJustClicked)
        {
            //playerController.mover.TeleportTo(this.pointerAttack.transform.position);
            //playerController.mover.transform.rotation = Quaternion.LookRotation(teleportLookDirection, Vector3.up);
            // TODO - orient using VR controller's forward direction as well

            playerController.PressActionButton();
        }

        transform.position = playerController.transform.position + Vector3.up * cameraHeight;
        transform.rotation = Quaternion.Euler(playerController.transform.rotation.eulerAngles.x, playerController.transform.rotation.eulerAngles.y, 0);


        RaycastHand();

        CheckFiring();
    }

    GameObject currentThrowingObject = null;
    Vector3 smoothenedVelocity;
    Vector3 smoothenedHandMovement;
    Vector3 originalPickupPosition;
    Vector3 lerpHandForward;

    float originalDistanceSquared = 10;

    void CheckFiring()
    {
        Vector3 newSmoothenedHandMovement = transform.InverseTransformPoint(pointerAttack.position).normalized * grabDistance;// Vector3.Lerp(smoothenedHandMovement, (pointerAttack.position - transform.position).normalized * grabDistance, grabbedObjectLerp * Time.deltaTime);
        Vector3 deltaOverFrame = (newSmoothenedHandMovement - smoothenedHandMovement);
        smoothenedVelocity = deltaOverFrame;
        smoothenedHandMovement = Vector3.Lerp(smoothenedHandMovement, newSmoothenedHandMovement, grabbedObjectLerp * Time.deltaTime);

#if UNITY_EDITOR
        handTransform.position = transform.TransformPoint(smoothenedHandMovement);
        lerpHandForward = Vector3.Lerp(lerpHandForward, (pointerAttack.transform.position - handTransform.position), handRotationLerp * Time.deltaTime);
        handTransform.forward = lerpHandForward;
#else
        handTransform.position = transform.TransformPoint(smoothenedHandMovement);
        handTransform.rotation = handController.m_model.transform.rotation;
#endif

        if (IsHoldingTrigger)
        {
            if (colliderLastHit != null)
            {
                if (colliderLastHit.gameObject.layer == LayerMask.NameToLayer("Grabbable"))
                {
                    currentThrowingObject = colliderLastHit.gameObject;
                    originalPickupPosition = currentThrowingObject.transform.position;
                    originalDistanceSquared = distanceOfLastRaycastAttack;
                }
            }


            {
                if (currentThrowingObject == null)
                {
                    //GameObject go = GameObject.Instantiate(ball.gameObject);
                    //go.GetComponent<Rigidbody>().isKinematic = true;
                    //go.transform.position = handTransform.position;
                    //currentThrowingObject = go;
                    //originalPickupPosition = go.transform.position;
                }
                else
                {
                    currentThrowingObject.transform.position = handTransform.position;
                    currentThrowingObject.transform.rotation = handTransform.rotation;
                }
            }

            playerController.FireCurrentWeapon();
        }
        else if (currentThrowingObject != null)
        {
            currentThrowingObject.GetComponent<Rigidbody>().isKinematic = false;
            currentThrowingObject.GetComponent<Rigidbody>().AddForce(smoothenedVelocity * grabThrowPower * Time.deltaTime, ForceMode.VelocityChange);
            currentThrowingObject = null;
        }
    }

    void RaycastGaze()
    {
        // Look at where you want to GO 
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, 10000000, raycastLayer))
		{
			pointerMove.transform.position = hitInfo.point;

            distanceOfLastRaycast = hitInfo.distance;
            teleportReticuleUpNormal = hitInfo.normal;
		}
		else
		{
            pointerMove.transform.position = Camera.main.transform.position + Camera.main.transform.forward * distanceOfLastRaycast;
		}
    }

    void RaycastHand()
    {
#if UNITY_EDITOR
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfoAttack, 10000000, raycastLayer))
		{
            Debug.DrawLine(hitInfoAttack.point, Camera.main.transform.position, Color.green);
			pointerAttack.transform.position = hitInfoAttack.point;

            distanceOfLastRaycastAttack = hitInfoAttack.distance;
		}
		else
		{
            pointerAttack.transform.position = Camera.main.ScreenPointToRay(Input.mousePosition).origin + Camera.main.ScreenPointToRay(Input.mousePosition).direction * distanceOfLastRaycastAttack;

            Debug.DrawLine(hitInfoAttack.point, Camera.main.ScreenPointToRay(Input.mousePosition).origin + Camera.main.ScreenPointToRay(Input.mousePosition).direction * 100, Color.green);
		}
#else
        // Point at where you want to FIGHT
        if (Physics.Raycast(handController.m_model.transform.position, handController.m_model.transform.forward, out hitInfoAttack, 10000000, raycastLayer))
		{
			pointerAttack.transform.position = hitInfoAttack.point;

            distanceOfLastRaycastAttack = hitInfoAttack.distance;
		}
		else
		{
            pointerAttack.transform.position = handController.m_model.transform.position + handController.m_model.transform.forward * distanceOfLastRaycastAttack;
		}
#endif

        playerController.AimAt(pointerAttack.transform.position);
        colliderLastHit = hitInfoAttack.collider;
    }

    /// Returns true the frame during which the user pushed down on the button
    public bool HasHeld
    {
        get
        {
            #if UNITY_EDITOR
            return Input.GetKeyDown(KeyCode.Space);
            #else
            return OVRInput.GetDown(OVRInput.Button.One);
            #endif
        }
    }

    /// Return true if currently holding
    public bool IsHolding
    {
        get
        {
            #if UNITY_EDITOR
            return Input.GetKey(KeyCode.Space);
            #else
            return OVRInput.Get(OVRInput.Button.One);
            #endif
        }
    }


    /// Returns true the frame during which user lifted from the button
    public bool HasLifted
    {
        get
        {
            #if UNITY_EDITOR
            return Input.GetKeyUp(KeyCode.Space);
            #else
            return OVRInput.GetUp(OVRInput.Button.One);
            #endif
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
