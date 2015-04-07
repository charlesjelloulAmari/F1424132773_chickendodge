using UnityEngine;
using System.Collections;

public enum PlayerForm{ Water, Vapor};

public class Playermovement : MonoBehaviour

{
    public float movementSpeed = 5.0f;
	private bool isGrounded = false;
	private bool touchAWall = false;
	PlayerForm form = PlayerForm.Water;

	public GameObject water;
	public GameObject vapor;

	float distToGround;

	void Start(){

		distToGround = collider.bounds.extents.y;
		water = GameObject.Find("Water");
		vapor = GameObject.Find ("Vapor");
		//vapor = GameObject.FindGameObjectWithTag ("VaporForm");

		PlayerForm form = PlayerForm.Water;

		vapor.SetActive(false);
	}

    void Update() {
		rigidbody.velocity = new Vector3 (0, rigidbody.velocity.y, 0); //Set X and Z velocity to 0

		if (form == PlayerForm.Vapor ) {
			rigidbody.velocity = new Vector3(0,rigidbody.velocity.y  + GameControl.gravity * Time.deltaTime,0);
		}

        transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed, 0, 0);

		/*if (touchAWall) {
			transform.Translate (0, Input.GetAxis ("Vertical") * Time.deltaTime * movementSpeed, 0);
		}*/

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump(); //Manual jumping
        }
	}

    void Jump()
    {	     
		switch (form) {
		case PlayerForm.Water:
			if (isGrounded) {
				isGrounded = false;
				rigidbody.velocity = new Vector3 (0, 0, 0);
				rigidbody.AddForce (new Vector3 (0, 250, 0), ForceMode.Force);
			}  
			break;
		case PlayerForm.Vapor:
			if (isGrounded) {
				isGrounded = false;
				rigidbody.velocity = new Vector3 (0, 0, 0);
				rigidbody.AddForce (new Vector3 (0, -250, 0), ForceMode.Force);
			}
			break;
		}

    }

    void FixedUpdate()
    {
		/*touchAWall = (Physics.Raycast(transform.position, Vector3.left, distToGround + 0.1f) 
		              || Physics.Raycast(transform.position, Vector3.right, distToGround + 0.1f) );*/
		switch (form) {
		case PlayerForm.Water:
			isGrounded = Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
			break;
		case PlayerForm.Vapor:
			isGrounded = Physics.Raycast(transform.position, Vector3.up, distToGround + 1.1f);
			break;
		}
        if (isGrounded){}

		/*if (touchAWall) {
			Debug.Log("Oui ici");
			rigidbody.useGravity = false;
		}
			else
			rigidbody.useGravity = true;*/
    }

	void OnCollisionEnter(Collision col){
		if (col.gameObject.name.Contains("Radiateur")) {
			form = PlayerForm.Vapor;
			vapor.SetActive(true);
			water.SetActive(false);
			rigidbody.useGravity = false;
			//rigidbody.AddForce (new Vector3 (0, 100, 0), ForceMode.Force);
		}
		if (col.gameObject.name.Contains("JetGel")) {
			form = PlayerForm.Water;
			vapor.SetActive(false);
			water.SetActive(true);
			rigidbody.useGravity = true;
			//rigidbody.AddForce (new Vector3 (0, 100, 0), ForceMode.Force);
		}
	}

	void OnCollisionExit(Collision col){
		if (form == PlayerForm.Vapor){
			//rigidbody.AddForce (new Vector3 (0, 100, 0), ForceMode.Force);
		}
	}

}
