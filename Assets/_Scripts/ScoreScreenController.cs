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


    // Use this for initialization
    void Start () {
        _yesBtn.onClick.AddListener(OnYesBtnClick);
        _noBtn.onClick.AddListener(OnNoBtnClick);
        _score.text = "0";
        StartCoroutine(ProcessScore());
    }
	
    void OnYesBtnClick()
    {
        GameController.instance.LoadPlay();
    }

    void OnNoBtnClick()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update () {
		
	}

    private IEnumerator ProcessScore()
    {
        var wait = new WaitForSeconds(_scoreDelay);
        int currentScore = 0;
        _score.text = currentScore.ToString();
        while (currentScore < GameController.instance.Score)
        {
            currentScore += _scoreStep;
            currentScore = Mathf.Min(currentScore, GameController.instance.Score);
            _score.text = currentScore.ToString();

            if (currentScore == GameController.instance.Score)
            {
                yield break;
            }
            else
            {
                yield return wait;
            }            
        }
    }
}
