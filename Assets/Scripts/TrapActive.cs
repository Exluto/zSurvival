using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapActive : MonoBehaviour {

	public bool power = false;
	private float timer;
	public bool OnPoints;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		/* 
		if (PlayerPoints >= 1500){
			OnPoints = true;
			

		} */

        timer += Time.deltaTime;
        if(timer >= 10 && power) {
            power = false;
			OnPoints = false;
		}
	}

	void OnTriggerEnter (Collider other){
        if(other.gameObject.tag == "Player" & OnPoints) {
            while(OnPoints){
				power= true;
			}
			
			Debug.Log("touch");
			//PlayerPoints =- 1500;
        }
    }
}
