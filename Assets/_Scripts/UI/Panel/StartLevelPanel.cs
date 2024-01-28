using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevelPanel : Panel
{
    public override void Initialize()
    {
        AudioManager.Instance.Play(AudioEnum.Music);
    }

    public override void OnOpen()
    {
        base.OnOpen();
        AudioManager.Instance.Play(AudioEnum.Music);
    }

    public void OnTapToStart()
    {
        AudioManager.Instance.Play(AudioEnum.Tap);
        LevelManager.Instance.LoadLevel();
        OnClose();
    }
}
