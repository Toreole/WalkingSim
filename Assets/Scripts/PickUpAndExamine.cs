
using UnityEngine;

public class PickUpAndExamine : MonoBehaviour
{

	private GameObject hitObject;
	private bool objectIsPickedUp = false;
	private bool examiningObject = false;

	public GameObject handPosition;
	public GameObject xPosition;

	public float thrust = 300f;

	public float zoomFOV = 30.0f;
	public float zoomSpeed = 9f;

	private float targetFOV;
	private float baseFOV;

	// Use this for initialization
	void Start ()
	{
		SetBaseFOV(GetComponent<Camera>().fieldOfView);
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

						// check if object has tag
						if (hit.transform.gameObject.tag == "PickUp")
						{

							// get object
							hitObject = hit.transform.gameObject;

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
			hitObject.transform.eulerAngles = new Vector3(handPosition.transform.eulerAngles.x, handPosition.transform.eulerAngles.y, handPosition.transform.eulerAngles.z);
		}

		// ===========================

		if (examiningObject)
		{
			hitObject.transform.eulerAngles = new Vector3(0.0f, xPosition.transform.eulerAngles.y, xPosition.transform.eulerAngles.z);
		}

		// ===========================
		// start examining

		if (Input.GetButtonDown("Fire2") && objectIsPickedUp)
		{
			if (!examiningObject)
			{
				hitObject.transform.position = xPosition.transform.position;
				examiningObject = true;
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
