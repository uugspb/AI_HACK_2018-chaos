using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FoxAnimator : MonoBehaviour {
	public bool crouch;
	NavMeshAgent m_agent;
	Animator m_animator;
	// Use this for initialization
	void Start () {
		m_agent = GetComponentInParent<NavMeshAgent> ();
		m_animator = GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		if (m_agent != null) {
			m_animator.SetFloat ("speed", m_agent.velocity.magnitude);
		}
		m_animator.SetBool ("crouching", crouch);
	}
}