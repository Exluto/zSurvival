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
		//OnPoints is true if player has enough points
		if(power){
			timer += Time.deltaTime;
		}
		Debug.Log(timer);
        if(timer >= 10 && power) {
            power = false;
			timer = 0;
		}
	}

	void OnTriggerEnter (Collider other){
        if(other.gameObject.tag == "Player" & OnPoints) {
			power= true;
        }
    }
}
