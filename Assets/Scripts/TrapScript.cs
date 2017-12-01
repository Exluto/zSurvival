using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrapScript : MonoBehaviour {
	ParticleSystem hitParticles;
    TrapActive trapActive;
	private Vector3 hitPoint;
    bool  trapOn = false;
    private float timer;

    Collider col;

	void Start (){
        trapActive = GetComponentInChildren<TrapActive>();
		hitParticles = GetComponentInChildren<ParticleSystem>();

        foreach(Transform child in transform)
        {
            if(child.name == "TrapActivator")
            {
                col = child.GetComponent<Collider>();
            }
        }
	}

    void Update(){
        trapOn = trapActive.power;
        if(trapOn){
            //Debug.Log("on");
            runTrap();
        } else if (!trapOn) {
            stopTrap();
        }
    }

    void runTrap() {
        hitParticles.Play();
        trapOn = true;    
    }

    void stopTrap() {
        hitParticles.Stop();
        trapOn = false;
    }

	/*void OnTriggerEnter (Collider other){
		if(other.gameObject.tag == "Enemy"){
			if(trapOn){
			EnemyHealth enemyHealth = other.GetComponent <EnemyHealth> ();
                if(enemyHealth != null){
                enemyHealth.TakeDamage (100, hitPoint);
                }
            }
        }
    }*/

    void OnTriggerStay (Collider other ){
        Debug.Log("fire");
        if(other.gameObject.tag == "Enemy"){
			if(trapOn){
			EnemyHealth enemyHealth = other.GetComponent <EnemyHealth> ();
                if(enemyHealth != null){
                enemyHealth.TakeDamage (100, hitPoint);
                }
            }
        }
    }
}
