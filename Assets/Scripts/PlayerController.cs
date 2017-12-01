using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private float m_Velocity = 10.0f;
	private float m_Sensitivity = 10.0f;
	CharacterController m_player;
	public GameObject m_eyes;
	private Animator m_Anim;

	float m_moveFB;
	float m_moveRL;
	float m_rotationX;
	float m_rotationY;

	
	// Use this for initialization
	void Start () {
		m_player = GetComponent<CharacterController>();
		m_Anim = GetComponent<Animator>();
		
	}

	void Update() {
		Movement();
	}
	
	// Update is called once per frame
	void Movement() {
		m_moveRL =  Input.GetAxis("Horizontal") * m_Velocity;
		m_moveFB = Input.GetAxis("Vertical") * m_Velocity;
		m_Anim.SetFloat("m_speed", m_Velocity);

		m_rotationX = Input.GetAxis("Mouse X") * m_Sensitivity;
		m_rotationY -= Input.GetAxis("Mouse Y") * m_Sensitivity;

		Vector3 movement = new Vector3(m_moveRL, 0, m_moveFB);

		transform.Rotate(0, m_rotationX, 0);

		movement = transform.rotation * movement;

		m_eyes.transform.localRotation = Quaternion.Euler(m_rotationY, 0, 0);

		m_player.Move (movement * Time.deltaTime);

		
	}
}
