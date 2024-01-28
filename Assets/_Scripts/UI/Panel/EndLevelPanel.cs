using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndLevelPanel : Panel
{
    [SerializeField]
    private GameObject endGameAnimation, gui;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private StartLevelPanel startLevelPanel;

    private Sequence sequence;

    public override void OnOpen()
    {
        AudioManager.Instance.StopAll(Audio.Type.Music);
        AudioManager.Instance.Play(AudioEnum.Lose);
        base.OnOpen();
        endGameAnimation.SetActive(true);
        endGameAnimation.transform.position = transform.position + Vector3.down * 3;
        gui.SetActive(false);
        PlayElementAppearAnimation();
        scoreText.text = $"{GameManager.Instance.Score}";
    }

    public void PlayElementAppearAnimation()
    {
        sequence?.Kill();
        sequence = DOTween.Sequence();
        rects[2].GetComponent<Image>().color = Color.black;
        rects[3].localScale = Vector3.zero;
        rects[4].localScale = Vector3.zero;
        rects[5].localScale = Vector3.zero;
        sequence.Append(rects[2].GetComponent<Image>().DOColor(Color.clear, 4).SetEase(Ease.InQuad));
        sequence.Append(rects[3].DOScale(1, 2));
        sequence.Append(rects[4].DOScale(1, 0.4f));
        sequence.Append(rects[5].DOScale(1, 0.4f));
    }

    public override void OnClose()
    {
        base.OnClose();
        gui.SetActive(true);
        endGameAnimation.SetActive(false);
    }

    public void OnReplay()
    {
        AudioManager.Instance.Play(AudioEnum.Tap);
        OnClose();
        startLevelPanel.OnOpen();
    }
}
