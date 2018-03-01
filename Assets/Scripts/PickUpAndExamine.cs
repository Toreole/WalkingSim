
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;


public class PickUpAndExamine : MonoBehaviour
{

	private GameObject hitObject;
	private bool objectIsPickedUp = false;
	private bool examiningObject = false;

	private PickUpObject myPickUpObjectScript;

	private FirstPersonController myFirstPersonController;

	public GameObject handPosition;
	public GameObject examinePosition;

	public float thrust = 300f;

	public float zoomFOV = 30.0f;
	public float zoomSpeed = 9f;

	private float targetFOV;
	private float baseFOV;

	// Use this for initialization
	void Start ()
	{
		SetBaseFOV(GetComponent<Camera>().fieldOfView);

		// get the FirstPersonController of the parent object (player)
		myFirstPersonController = transform.parent.gameObject.GetComponent<FirstPersonController>();

	}

	// Update is called once per frame
	void Update()
	{

		if (Input.GetButtonDown("Fire1"))
		{

			if (!objectIsPickedUp)
			{

				// check if objects in front of the camera
				// important! player must be on layer "IgnoreRaycast"!
				Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
      			RaycastHit hit;
        		if (Physics.Raycast(ray, out hit))
				{

					// debug
					//print("Loking at " + hit.transform.name);

					// check if object has rigidbody
					if (hit.transform.gameObject.GetComponent<Rigidbody>() != null)
					{

						// check if object has pickup-script
						if (hit.transform.gameObject.GetComponent("PickUpObject"))
						{

							// get object
							hitObject = hit.transform.gameObject;

							// get script
							myPickUpObjectScript = hitObject.GetComponent<PickUpObject>();

							// pick up object
							hitObject.transform.parent = handPosition.transform;
							hitObject.transform.position = handPosition.transform.position;
							hitObject.GetComponent<Rigidbody>().isKinematic = true;

							objectIsPickedUp = true;
						}
					}
				}
			}
			else
			{
				hitObject.transform.parent = null;
				hitObject.GetComponent<Rigidbody>().isKinematic = false;
				hitObject.GetComponent<Rigidbody>().AddForce(transform.forward * thrust);
				hitObject = null;

				objectIsPickedUp = false;
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
			if (myPickUpObjectScript.rotateHorizontal){
				hitObject.transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), 0, 0));

			}
			if (myPickUpObjectScript.rotateVertical){
				hitObject.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0));

			}
		}

		// ===========================
		// start examining

		if (Input.GetButtonDown("Fire2") && objectIsPickedUp)
		{
			if (!examiningObject)
			{
				hitObject.transform.position = examinePosition.transform.position;
				//hitObject.transform.rotation = gameObject.transform.rotation;
				examiningObject = true;
				myFirstPersonController.enabled = false;

			}
		}

		// ===========================
		// end examining

		if (Input.GetButtonUp("Fire2") && objectIsPickedUp)
		{
			if (examiningObject)
			{
				hitObject.transform.position = handPosition.transform.position;
				examiningObject = false;
				myFirstPersonController.enabled = true;
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
		GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, targetFOV, zoomSpeed * Time.deltaTime);
	}

	public void SetBaseFOV(float fov)
	{
		baseFOV = fov;
	}
}
