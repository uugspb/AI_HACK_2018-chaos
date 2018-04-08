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
        gunOnSpine.gameObject.SetActive (true);
        gunInHand.gameObject.SetActive (false);
        myFov.OnOpponentVisible += OnFoxVisible;
        Fox.instance.OnFoxKilled += KillFox;
        m_agent = GetComponentInParent<NavMeshAgent> ();
        m_animator = GetComponent<Animator> ();
    }

    private void OnFoxVisible (FovTarget arg1, FovTarget arg2) {
        if (_gunAnimation == null) {
            _gunAnimation = StartCoroutine (AnimateGun ());
        }
    }

    private void KillFox () {
        if (_gunAnimation != null) {
            gunSmoke.Play ();
        }
    }

    // Update is called once per frame
    void Update () {
        m_animator.SetFloat ("speed", m_agent.velocity.magnitude);
        m_animator.SetBool ("crouching", crouch);
    }

    public Transform gunInHand;
    public Transform gunOnSpine;
    public FovSource myFov;

    private Coroutine _gunAnimation;

    private IEnumerator AnimateGun () {
        m_animator.SetTrigger ("shoot");
        gunOnSpine.gameObject.SetActive (false);
        gunInHand.gameObject.SetActive (true);

        yield return new WaitForSeconds (1f);

        gunOnSpine.gameObject.SetActive (true);
        gunInHand.gameObject.SetActive (false);
        _gunAnimation = null;
    }

    [EditorButton]
    public void Shoot () {
        gunInHand.localScale = Vector3.one;
        gunOnSpine.localScale = Vector3.zero;
        m_animator.SetTrigger ("shoot");
        Invoke ("HideGun", 1f);
        Invoke ("GunSmoke", 0.05f);
    }

    void GunSmoke () {
        gunSmoke.Play ();
    }

    //void HideGun () {
    //	gunInHand.localScale = Vector3.zero;
    //	gunOnSpine.localScale = Vector3.one;
    //}
}