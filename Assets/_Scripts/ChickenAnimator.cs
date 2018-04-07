using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChickenAnimator : MonoBehaviour {
	Animator m_animator;
	NavMeshAgent m_agent;

	void Start () {
		m_animator = GetComponent<Animator> ();
		m_agent = GetComponentInParent<NavMeshAgent> ();
		Eat ();
	}

	void Eat () {
		m_animator.SetTrigger ("eat");
		Invoke ("Eat", Random.Range (0.05f, 1f));
	}

	void Update () {
		m_animator.SetFloat ("speed", m_agent.velocity.magnitude);
	}
}