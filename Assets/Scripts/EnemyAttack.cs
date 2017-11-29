using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour {
	public float timeBetweenAttacks = 1f;
    public int attackDamage = 10;
    public float AccelerationTime = 3f;


    Animator anim;
    GameObject player;
    //PlayerHealth playerHealth;                  // Reference to the player's health.
    EnemyHealth enemyHealth;                    // Reference to this enemy's health.
    public bool playerInRange;
    public float runSpeed = 1f;
    float timer;       


    void Awake (){
        player = GameObject.FindGameObjectWithTag ("Player");
        //playerHealth = player.GetComponent <PlayerHealth> ();  //pulls playerHealth script and stores refrence
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent <Animator> ();
    }
    
    void OnTriggerEnter (Collider other){
        if(other.gameObject.tag == "Player") {
            // ... the player is in range.
            playerInRange = true;
			Debug.Log("Range");
        }
    }

    void OnTriggerExit (Collider other){
        if(other.gameObject.tag == "Player") {
            playerInRange = false;
			Debug.Log("OutRange");
			anim.SetBool("Attack", false);
        }
    }


    void Update (){
        timer += Time.deltaTime;
        anim.SetFloat("AccelTime", timer);
        runSpeed = timer * 0.5f;
        if(runSpeed >= AccelerationTime) {
           runSpeed = 3f;
        }
        if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0){
            Attack ();
        }

        /*if(playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger ("PlayerDead");
        }*/
    }


    void Attack (){
        timer = 0f;
		Debug.Log("attack");
		anim.SetBool("Attack", true);
        // If the player has health to lose...
      /*  if(playerHealth.currentHealth > 0)
        {
            // ... damage the player.
            playerHealth.TakeDamage (attackDamage);
        }
		*/    
	}
}
