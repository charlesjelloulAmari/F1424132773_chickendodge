using UnityEngine;
using System.Collections;

public class LightningBolt : MonoBehaviour
{
	
	// Use this for initialization
	void Start ()
	{		
		Material newMat = renderer.material;
		newMat.SetFloat("_StartSeed",Random.value*10);
		renderer.material = newMat;
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}

