using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameMode mode = GameMode.editor;
    private float _savedTimescale = 1;

    [EditorButton]
    public void SwitchMode()
    {
        switch (mode)
        {
            case GameMode.editor:
                mode = GameMode.play;
                PlayMode();
                break;
            case GameMode.play:
                mode = GameMode.editor;
                EditorMode();
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
        PatrolManager.instance.StopPatrol();
        PatrolManager.instance.StartPatrol();
    }

    public void SetPaused(bool paused)
    {
        if (paused)
            Time.timeScale = 0;
        else
            Time.timeScale = _savedTimescale;
    }
}

public enum GameMode
{
    editor,
    play
}