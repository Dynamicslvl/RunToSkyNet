using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimeScaleTweener
{
    private static Tween tween;
    public void TweenTimeScale(float target, float duration)
    {
        tween?.Kill();
        tween = DOVirtual.Float(Time.timeScale, target, duration, value =>
        {
            Time.timeScale = value;
        });
        tween.SetUpdate(true);
    }
}
