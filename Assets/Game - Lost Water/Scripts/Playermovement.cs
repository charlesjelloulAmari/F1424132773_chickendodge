using UnityEngine;
using System.Collections;

public enum PlayerForm{ Water, Vapor, Poison, Flubber, Ice, Cloud};

public class Playermovement : MonoBehaviour
{
	
	string GOUTTE_SOUND = "Assets/Resources/Sound/goutte.mp3";
	string FLUBBER_SOUND = "Assets/Resources/Sound/jumpFlubber.mp3";
	string NO_SOUND = "Assets/Resources/Sound/";
	

	public int highJump = 600;
	public int normalJump = 250;
	public int lowJump = 120;

    public float movementSpeed = 5.0f;
	private bool isGrounded = false;
	PlayerForm form = PlayerForm.Water;

	public GameObject water;
	public GameObject vapor;
	public GameObject poison;
	public GameObject flubber;
	public GameObject ice;
	public GameObject cloud;
	
	public GameObject shining;

	public GameObject grilleOnCollision;

	
	public PhysicMaterial bouncy;
	public PhysicMaterial ice_mat;

	public bool salt;

	AudioClip jumpSound; 

	float distToGround;

	void Start(){

		salt = false;
		distToGround = collider.bounds.extents.y;

		water = GameObject.Find("Water");
		vapor = GameObject.Find ("Vapor");
		poison = GameObject.Find ("Poison");
		flubber = GameObject.Find ("Flubber");
		ice = GameObject.Find ("Ice");
		cloud = GameObject.Find ("Cloud");

		shining = GameObject.Find ("Shining");
		
		bouncy = (PhysicMaterial)Resources.Load ("Bouncy_2");
		ice_mat = (PhysicMaterial)Resources.Load ("Ice_2");

		audio.clip = (AudioClip)Resources.LoadAssetAtPath(GOUTTE_SOUND, typeof(AudioClip));
		
		PlayerForm form = PlayerForm.Water;

		vapor.SetActive(false);
		poison.SetActive(false);
		flubber.SetActive(false);
		ice.SetActive (false);
		cloud.SetActive (false);
		shining.SetActive (false);

	}

    void Update() {
		rigidbody.velocity = new Vector3 (0, rigidbody.velocity.y, 0); //Set X and Z velocity to 0

		/*Gravité inversée en mode vapor*/
		if (form == PlayerForm.Vapor ) {
			rigidbody.velocity = new Vector3(0,rigidbody.velocity.y  + GameControl.gravity * Time.deltaTime,0);
		}

        transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed, 0, 0);

		if(form == PlayerForm.Cloud)
			transform.Translate(0,Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed , 0);
		
		gererInput ();

	}

	void gererInput(){
		if (Input.GetButtonDown("Jump") )
		{
			Jump();
		}
		if (Input.GetButtonDown("Fire1") )
		{
			if(form == PlayerForm.Poison && grilleOnCollision != null){
				grilleOnCollision.GetComponent<Collider>().isTrigger = true;
			}
		}
	}

	void OnTriggerExit(){

		GameObject[] list_gel = GameObject.FindGameObjectsWithTag ("JetGel");
		foreach(GameObject jetgel in list_gel){
			jetgel.collider.isTrigger = false;
		}

		if (grilleOnCollision != null) {
			grilleOnCollision.GetComponent<Collider> ().isTrigger = false;
			grilleOnCollision = null;
		}
	}
	
	void Jump()
    {	     
		switch (form) {
		case PlayerForm.Water:
		case PlayerForm.Poison:
			rigidbody.useGravity = true;
			if (isGrounded) {
				isGrounded = false;
				rigidbody.velocity = new Vector3 (0, 0, 0);
				audio.Play ();
				rigidbody.AddForce (new Vector3 (0, normalJump, 0), ForceMode.Force);
			}  
			break;
		case PlayerForm.Vapor:
			if (isGrounded) {
				isGrounded = false;
				passTo(PlayerForm.Water);
			}
			break;
		case PlayerForm.Flubber:
			if (isGrounded) {
				isGrounded = false;
				audio.Play ();
				rigidbody.velocity = new Vector3 (0, 0, 0);
				rigidbody.AddForce (new Vector3 (0, highJump, 0), ForceMode.Force);
			}
			break;
		case PlayerForm.Ice:
			if (isGrounded) {
				isGrounded = false;
				rigidbody.velocity = new Vector3 (0, 0, 0);
				audio.Play ();
				rigidbody.AddForce (new Vector3 (0, lowJump, 0), ForceMode.Force);
			}
			break;
		}

    }

    void FixedUpdate()
    {
		switch (form) {
		case PlayerForm.Water:
		case PlayerForm.Poison:
		case PlayerForm.Flubber:
		case PlayerForm.Ice:
			isGrounded = Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
			break;
		case PlayerForm.Vapor:
			isGrounded = Physics.Raycast(transform.position, Vector3.up, distToGround + 1.1f);
			break;
		}
        if (isGrounded){}
    }

	void OnCollisionEnter(Collision col){
		if (col.gameObject.name.Contains("Radiateur")) {
			rigidbody.useGravity = false;
			if(!salt)
				passTo(PlayerForm.Vapor);
			else
				passTo (PlayerForm.Cloud);
		}
		if (col.gameObject.name.Contains("JetGel")) {
			passTo(PlayerForm.Ice);
			rigidbody.useGravity = true;
			col.gameObject.collider.isTrigger = true;
		}
		if (col.gameObject.name.Contains("Flaque_Poison")) {
			passTo(PlayerForm.Poison);
			rigidbody.useGravity = true;
		}
		if (col.gameObject.name.Contains("Flaque_Flubber")) {
			passTo(PlayerForm.Flubber);
			rigidbody.useGravity = true;
		}
		if (col.gameObject.name.Contains("Tas_Sel")) {
			salt = true;
			shining.SetActive (true);
		}

		if ((form == PlayerForm.Vapor ||form == PlayerForm.Cloud) && col.gameObject.name.Contains("EdgeTrigger") ){
			passTo (PlayerForm.Water);
			rigidbody.useGravity = false;
		}

		if(col.gameObject.name.Contains("Grille")){
			grilleOnCollision = col.gameObject;
		}

		if(col.gameObject.name.Contains("Contact") && salt){
			string parentName = col.gameObject.transform.parent.name;
			if(parentName.Equals("Lightning_system")){
				col.gameObject.GetComponentInParent<Animator>().Play("condensation_generator");
			}
			else if(parentName.Equals("Door_system")){
				col.gameObject.GetComponentInParent<Animator>().Play("acdctodoor");
			}
		}

		if(col.gameObject.name.Contains("GameOver")){
			Vector3 temp = new Vector3(-12,10,0);
			transform.position = temp;
		}
	}

	public void passTo(PlayerForm to){
		water.SetActive(false);
		vapor.SetActive(false);
		flubber.SetActive(false);
		poison.SetActive(false);
		ice.SetActive(false);
		cloud.SetActive (false);

		collider.material = null;
		
		switch (to) {
		case PlayerForm.Water:
			form = PlayerForm.Water;
			audio.clip = (AudioClip)Resources.LoadAssetAtPath(GOUTTE_SOUND, typeof(AudioClip));
			water.SetActive(true);
			break;
		case PlayerForm.Poison:
			form = PlayerForm.Poison;
			audio.clip = (AudioClip)Resources.LoadAssetAtPath(GOUTTE_SOUND, typeof(AudioClip));
			poison.SetActive(true);
			break;
		case PlayerForm.Flubber:
			form = PlayerForm.Flubber;
			audio.clip = (AudioClip)Resources.LoadAssetAtPath(FLUBBER_SOUND, typeof(AudioClip));
			collider.material = bouncy;
			flubber.SetActive(true);
			break;
		case PlayerForm.Vapor:
			form = PlayerForm.Vapor;
			audio.clip = (AudioClip)Resources.LoadAssetAtPath(GOUTTE_SOUND, typeof(AudioClip));
			vapor.SetActive(true);
			break;
		case PlayerForm.Ice:
			form = PlayerForm.Ice;
			audio.clip = (AudioClip)Resources.LoadAssetAtPath(NO_SOUND, typeof(AudioClip));
			collider.material = ice_mat;
			ice.SetActive(true);
			break;
		case PlayerForm.Cloud:
			form = PlayerForm.Cloud;
			audio.clip = (AudioClip)Resources.LoadAssetAtPath(NO_SOUND, typeof(AudioClip));
			cloud.SetActive(true);
			break;
		}
	}

	void OnCollisionExit(Collision col){
		if (form != PlayerForm.Vapor && !IsGroundedUp() && form!= PlayerForm.Cloud) {
			rigidbody.useGravity = true;
		}
	}

	bool IsGroundedUp (){
		return Physics.Raycast(transform.position, Vector3.up, distToGround + 1.1f);
	}

	public void setSalt(bool sel){
		salt = sel;
		shining.SetActive (sel);
	}
	
}
