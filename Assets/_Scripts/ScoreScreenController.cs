using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScreenController : MonoBehaviour {

    [SerializeField] private Button _yesBtn;
    [SerializeField] private Button _noBtn;
    [SerializeField] private Text _score;
    [SerializeField] private float _scoreDelay;
    [SerializeField] private int _scoreStep;
    [SerializeField] private Text _text;

    // Use this for initialization
    void Start () {
        _yesBtn.onClick.AddListener (OnYesBtnClick);
        _noBtn.onClick.AddListener (OnNoBtnClick);
        _score.text = "00:00";
        StartCoroutine (ProcessScore ());

        int seconds = GameController.instance.Seconds;
        _text.text = "Do you even try?";
        if (seconds >= 10) {
            _text.text = "Too easy for that fox";
        }
        if (seconds >= 20) {
            _text.text = "Better luck next time";
        }
        if (seconds >= 30) {
            _text.text = "Good job!";
        }
        if (seconds >= 45) {
            _text.text = "Wow that was cool!";
        }
        if (seconds >= 60) {
            _text.text = "HOW YOU DO THIS?";
        }
    }

    void OnYesBtnClick () {
        GameController.instance.LoadPlay ();
    }

    void OnNoBtnClick () {
        Application.Quit ();
    }

    // Update is called once per frame
    void Update () {

    }

    private IEnumerator ProcessScore () {
        var wait = new WaitForSeconds (1.5f / Mathf.Max (1, GameController.instance.Seconds));
        int addPerWait = (int) Mathf.Max (1, GameController.instance.Seconds / 1.5f / 60);
        int currentScore = 0;
        int currentSeconds = 0;
        _score.text = (currentSeconds / 60) + ":" + (currentSeconds % 60); // currentScore.ToString ();
        while (currentSeconds < GameController.instance.Seconds) {
            // currentScore += _scoreStep;
            currentSeconds += addPerWait;
            // Debug.Log (currentSeconds);
            currentSeconds = Mathf.Min (currentSeconds, GameController.instance.Seconds);
            // currentScore = Mathf.Min (currentScore, GameController.instance.Score);
            _score.text = (currentSeconds / 60) + ":" + (currentSeconds % 60);

            if (currentSeconds == GameController.instance.Seconds) {
                // yield break;
                break;
            } else {
                yield return wait;
            }
        }

        for (float t = 0; t <= 1f; t += Time.deltaTime * 4) {
            _score.transform.localScale = Vector3.one * (Mathf.Sin (Mathf.PI * t) * 0.5f + 1);
            yield return null;
        }
    }
}