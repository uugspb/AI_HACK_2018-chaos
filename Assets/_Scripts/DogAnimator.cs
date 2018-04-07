using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogAnimator : MonoBehaviour {
	public bool crouch;
	NavMeshAgent m_agent;
	Animator m_animator;
	public ParticleSystem gunSmoke;
	// Use this for initialization
	void Start () {
		m_agent = GetComponentInParent<NavMeshAgent> ();
		m_animator = GetComponent<Animator> ();
		HideGun ();
	}

	// Update is called once per frame
	void Update () {
		m_animator.SetFloat ("speed", m_agent.velocity.magnitude);
		m_animator.SetBool ("crouching", crouch);
	}
	public Transform gunInHand;
	public Transform gunOnSpine;
	[EditorButton]
	public void Shoot () {
		gunInHand.localScale = Vector3.one;
		gunOnSpine.localScale = Vector3.zero;
		m_animator.SetTrigger ("shoot");
		Invoke ("HideGun", 1f);
		Invoke ("GunSmoke", 0.25f);
	}

	void GunSmoke () {
		gunSmoke.Play ();
	}

	void HideGun () {
		gunInHand.localScale = Vector3.zero;
		gunOnSpine.localScale = Vector3.one;
	}
}