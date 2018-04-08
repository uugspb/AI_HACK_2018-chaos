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

    public void ResetAnim()
    {
        m_animator.gameObject.SetActive(false);
        m_animator.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update () {
		if (m_agent != null) {
			m_animator.SetFloat ("speed", m_agent.velocity.magnitude);
		}
		crouch = DeathStrandingManager.instance != null && DeathStrandingManager.instance.IsCloseToKillingPoint (transform.position);
		m_animator.SetBool ("crouching", crouch);



        if(!Fox.instance.IsKillInProgress)
        {
            if (!crouch && !walk.isPlaying && GameManager.instance.mode == GameMode.play)
            {
                walk.Play();
            }
            else if (crouch && walk.isPlaying)
            {
                walk.Stop();
            }
        }
        else
        {
            if (Fox.instance.PlayKillAnim)
            {
                Fox.instance.PlayKillAnim = false;
                m_animator.SetTrigger("die");
                m_animator.SetBool("crouching", false);
                walk.Stop();
            }
        }

        
	}
}