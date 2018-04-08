using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FoxAnimator : MonoBehaviour {
	public bool crouch;
	NavMeshAgent m_agent;
	Animator m_animator;

    public AudioSource walk;
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
		crouch = DeathStrandingManager.instance != null && DeathStrandingManager.instance.IsCloseToKillingPoint (transform.position);
		m_animator.SetBool ("crouching", crouch);


        if(!crouch && !walk.isPlaying && GameManager.instance.mode == GameMode.play)
        {
            walk.Play();
        }
        else if(crouch && walk.isPlaying)
        {
            walk.Stop();
        }
	}
}