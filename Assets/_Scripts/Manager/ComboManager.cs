using Dasis.DesignPattern;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ComboManager : Singleton<ComboManager>
{
    [SerializeField]
    private float comboDuration;

    [SerializeField]
    private TextMeshProUGUI comboText;

    private float timer;
    private int combo;
    private Sequence sequence;

    public int Combo => combo;

    public void CheckCombo()
    {
        if (timer > 0)
        {
            combo++;
            ShowCombo();
            AudioManager.Instance.Play(AudioEnum.Bonus);
        }
        else
        {
            combo = 1;
            comboText.text = string.Empty;
        }
        timer = comboDuration;
    }

    public void ShowCombo()
    {
        comboText.text = $"x{combo} Combo!";
        sequence?.Kill();
        sequence = DOTween.Sequence();
        comboText.transform.localScale = Vector3.zero;
        sequence.Append(comboText.transform.DOScale(1, 0.4f).SetEase(Ease.OutBack));
        sequence.AppendInterval(0.4f);
        sequence.Append(comboText.transform.DOScale(0, 0.3f));
    }

    public void ResetCombo()
    {
        timer = 0;
        combo = 0;
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }
        timer = 0;
    }
}
