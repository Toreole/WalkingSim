
using UnityEngine;

public class PickUpAndExamine : MonoBehaviour
{

	private Transform hitObject;
	private bool objectIsPickedUp = false;
	private bool examiningObject = false;

	private PickUpObject myPickUpObjectScript;

	[SerializeField]
	private CharacterController myCharacterController;

	[SerializeField]
	private Transform handPosition; //we only ever need the Transform of the hand, no reason to use the GameObject reference.
	[SerializeField]
	private Transform examinePosition;

	[SerializeField]
	private float thrust = 3f;

	[SerializeField]
	private float zoomFOV = 30.0f;

	[SerializeField]
	private float zoomSpeed = 9f;

	private float targetFOV;
	private float baseFOV;

	private new Camera camera;

	// Use this for initialization
	void Start ()
	{
		camera = GetComponent<Camera>();
		SetBaseFOV(camera.fieldOfView);

		// get the CharacterController of the parent object (player)
		myCharacterController = transform.GetComponentInParent<CharacterController>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			if (!objectIsPickedUp)
			{
				// check if objects in front of the camera
				// important! player must be on layer "IgnoreRaycast"! -- not true:
				// "Start Query Inside Collider" setting for raycasts can be disabled, or any arbitrary LayerMask that does not include the layer the player is on can be used.
				Ray ray = camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)); //Camera.main is slow and unnecessary. The camera is attached to this object. We use GetComponent to get it in Start
      			RaycastHit hit;
        		if (Physics.Raycast(ray, out hit))
				{
					//All PickUpObjects have a Rigidbody, no need to check for it.
					// check if object has pickup-script
					myPickUpObjectScript = hit.transform.GetComponent<PickUpObject>(); //never use GetComponent with a string. also no need for transform.gameObject
					if (myPickUpObjectScript) 
					{
						// get object
						hitObject = hit.transform;

						// get script duplicate
						//myPickUpObjectScript = hitObject.GetComponent<PickUpObject>();

						// pick up object
						hitObject.parent = handPosition;
						hitObject.localPosition = Vector3.zero; //simplified to just reset the local position to zero
						myPickUpObjectScript.Rigidbody.isKinematic = true;

						objectIsPickedUp = true;
					}
					
				}
			}
			else
			{
				hitObject.parent = null;
				hitObject.position = transform.position; //throw from the camera position, otherwise it very easily goes through walls.
				//Replace unnecessary GetComponent calls now that the pickup script has a Rigidbody property.
				myPickUpObjectScript.Rigidbody.isKinematic = false;
				myPickUpObjectScript.Rigidbody.AddForce(transform.forward * thrust, ForceMode.Impulse); 
				//using a normal Force for instantaneous changes is bad, use ForceMode.Impulse instead.
				//Forces are dependent on time, Impulses arent. Impulse gives more direct and more intuitive control over the "speed" of the object thrown.
				//Impulses are (velocity * mass), with a mass of 1, its just the change in velocity in the object.
				//Forces are (acceleration * mass), to calculate the velocity change, unity internally multiplies it with the deltaTime.
				hitObject = null;
				
				objectIsPickedUp = false;
				examiningObject = false;
				myCharacterController.enabled = true;
				GetComponent<MouseLook>().enabled = true;
			}
		}

		// ===========================

		if (objectIsPickedUp && !examiningObject)
		{
			//hitObject.transform.eulerAngles = new Vector3(handPosition.transform.eulerAngles.x, handPosition.transform.eulerAngles.y, handPosition.transform.eulerAngles.z);
		}

		// ===========================

		if (examiningObject)
		{
			if (myPickUpObjectScript.rotateHorizontal)
			{
				hitObject.Rotate(new Vector3(Input.GetAxis("Mouse Y"), 0, 0));
			}
			if (myPickUpObjectScript.rotateVertical)
			{
				hitObject.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0));
			}
		}

		// ===========================
		// start examining

		if (Input.GetButtonDown("Fire2") && objectIsPickedUp)
		{
			if (!examiningObject)
			{
				hitObject.position = examinePosition.position;
				if (myPickUpObjectScript.adjustObject)
				{
					hitObject.eulerAngles = transform.eulerAngles;
				}
				examiningObject = true;
				myCharacterController.enabled = false;
				GetComponent<MouseLook>().enabled = false;
			}
		}

		// ===========================
		// end examining

		if (Input.GetButtonUp("Fire2") && objectIsPickedUp)
		{
			if (examiningObject)
			{
				hitObject.position = handPosition.position;
				examiningObject = false;
				myCharacterController.enabled = true;
				GetComponent<MouseLook>().enabled = true;
			}
		}

		// ===========================
		// zoom

		if (Input.GetButton("Fire2") && !objectIsPickedUp)
		{
			targetFOV = zoomFOV;
		}
		else
		{
			targetFOV = baseFOV;
		}

		UpdateZoom();

	}

	private void UpdateZoom()
	{
		camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, targetFOV, zoomSpeed * Time.deltaTime);
	}

	public void SetBaseFOV(float fov)
	{
		baseFOV = fov;
	}
}
