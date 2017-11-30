using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private float m_speed = 10.0f;
	private float m_Sensitivity = 10.0f;
	CharacterController m_player;
	public GameObject m_eyes;

	float m_moveFB;
	float m_moveRL;
	float m_rotationX;
	float m_rotationY;

	
	// Use this for initialization
	void Start () {
		m_player = GetComponent<CharacterController>();
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		m_moveRL =  Input.GetAxis("Horizontal") * m_speed;
		m_moveFB = Input.GetAxis("Vertical") * m_speed;

		m_rotationX = Input.GetAxis("Mouse X") * m_Sensitivity;
		m_rotationY = Input.GetAxis("Mouse Y") * m_Sensitivity;

		Vector3 movement = new Vector3(m_moveRL, 0, m_moveFB);
		transform.Rotate(0, m_rotationX, 0);
		transform.Rotate(-m_rotationY, 0, 0 );

		movement = transform.rotation * movement;
		m_player.Move (movement * Time.deltaTime);

		
	}
}
