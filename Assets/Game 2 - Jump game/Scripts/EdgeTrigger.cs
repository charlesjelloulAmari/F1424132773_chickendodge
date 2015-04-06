using UnityEngine;
using System.Collections;

public class EdgeTrigger : MonoBehaviour {

    //You could implement this yourself..

   void OnCollisionEnter(){
       Debug.Log("OnTriggerEnter..");
		GameControl.sparks.SetActive(true);
   }
}
