using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private float m_speed = 10.0f;
	private float m_Sensitivity = 10.0f;
	CharacterController m_player;

	
	// Use this for initialization
	void Start () {
		GetComponent<CharacterController>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
