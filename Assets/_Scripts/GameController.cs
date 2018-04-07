using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : Singleton<GameController> {

    [SerializeField] private string _intro;
    [SerializeField] private string _play;
    [SerializeField] private string _score;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    [EditorButton]
    public void LoadIntro()
    {
        SceneManager.LoadScene(_intro);
    }

    [EditorButton]
    public void LoadPlay()
    {
        SceneManager.LoadScene(_play);
    }

    [EditorButton]
    public void LoadScore()
    {
        SceneManager.LoadScene(_score);
    }

}
