using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : Singleton<GameController> {

    [SerializeField] private string _intro;
    [SerializeField] private string _play;
    [SerializeField] private string _score;
    [SerializeField] private float _scoreScale;

    private float _timeCounter;

    public int Score { get { return Mathf.RoundToInt (_timeCounter * _scoreScale); } }
    public int Seconds { get { return Mathf.RoundToInt (_timeCounter); } }

    private void Start () {
        DontDestroyOnLoad (this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        LoadIntro ();
    }

    private void OnSceneLoaded (Scene scene, LoadSceneMode arg1) {
        if (scene.name == _play) {
            Fox.instance.OnFoxGoalReached += OnGoalReached;
        }
    }

    [EditorButton]
    public void LoadIntro () {
        SceneManager.LoadScene (_intro);
    }

    [EditorButton]
    public void LoadPlay () {
        _timeCounter = 0;
        SceneManager.LoadScene (_play);
    }
    float startTime = 0f;
    public void StartTimer () {
        startTime = Time.time;
    }

    public void StopTimer () {
        _timeCounter = Time.time - startTime;
    }

    [EditorButton]
    public void LoadScore () {
        StopTimer ();
        SceneManager.LoadScene (_score);
    }

    private void Update () {
        // _timeCounter += Time.deltaTime;
    }

    private void OnGoalReached () {
        Fox.instance.OnFoxGoalReached -= OnGoalReached;
        LoadScore ();
    }

}