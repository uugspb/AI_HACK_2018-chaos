using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameMode mode = GameMode.editor;
    private float _savedTimescale = 1;
    [SerializeField] private GameObject _editor;

    [EditorButton]
    public void SwitchMode()
    {
        switch (mode)
        {
            case GameMode.editor:
                mode = GameMode.play;
                PlayMode();
                _editor.active = false;
                ChangeTimeScale(1.0f);
                break;
            case GameMode.play:
                mode = GameMode.editor;
                ChangeTimeScale(1.0f);
                EditorMode();
                _editor.active = true;
                break;
        }
    }

    void PlayMode()
    {
        DeathStrandingManager.instance.StartPlayMode();
        PatrolManager.instance.StartPatrol();
        Fox.instance.StartWalking();
    }

    void EditorMode()
    {
        Fox.instance.StopWalking();
        PatrolManager.instance.StopPatrol();
        DeathStrandingManager.instance.StopPlayMode();
    }

    [EditorButton]
    public void KillFox()
    {
        Fox.instance.Kill();
    }

    public void SetPaused(bool paused)
    {
        if (paused)
            Time.timeScale = 0;
        else
            Time.timeScale = _savedTimescale;
    }

    public void ChangeTimeScale(float value)
    {
        Time.timeScale = value;
        _savedTimescale = value;
    }
}

public enum GameMode
{
    editor,
    play
}