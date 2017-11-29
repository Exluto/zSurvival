using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieScript : MonoBehaviour {
	public float m_moveSpeed = 0.0f;
	private Transform player;
	//public PlayerHealth playerHealth;
	EnemyHealth enemyHealth;
	EnemyAttack enemyAttack;
	

	private NavMeshAgent nav;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").transform;
		//playerHealth = player.GetComponent <PlayerHealth>();
		enemyHealth = GetComponent <EnemyHealth>();
		enemyAttack = GetComponent <EnemyAttack>();
		nav = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		nav.speed = enemyAttack.runSpeed;
		//Debug.Log(nav.speed);
		if(enemyHealth.currentHealth > 0 /*e&& playerHealth.currentHealth > 0*/){
			nav.SetDestination(player.position);
		} else{
			nav.enabled = false;
		}

	}
}
