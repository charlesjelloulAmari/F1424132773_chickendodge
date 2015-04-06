using UnityEngine;
using System.Collections;

public class Playermovement : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    private bool isGrounded = false;

	float distToGround;
	
	void Start(){
		// get the distance to ground
		distToGround = collider.bounds.extents.y;
	}


    void Update() {
        rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, 0); //Set X and Z velocity to 0
 
        transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed, 0, 0);
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
			Debug.Log("OUI");
            Jump(); //Manual jumping

        }
	}

    void Jump()
    {
        if (!isGrounded) {
			return;
		} else {

			//GameObject.Find("Sparks").SetActive(true);
			GameControl.sparks.SetActive(false);
			isGrounded = false;
			rigidbody.velocity = new Vector3 (0, 0, 0);
			rigidbody.AddForce (new Vector3 (0, 250, 0), ForceMode.Force);
		}
    }

    void FixedUpdate()
    {
        //isGrounded = Physics.Raycast(transform.position, -Vector3.up, 1.0f);
		isGrounded = Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.5f);
        if (isGrounded)
        {
            //Jump(); //Automatic jumping
        }
    }
       

}
