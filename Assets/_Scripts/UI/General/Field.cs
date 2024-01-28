using DG.Tweening;
using TMPro;
using UnityEngine;

public abstract class Field : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI text;

    protected int virtualAmount = -1;

    protected void Awake()
    {
        Initialize();
    }

    public abstract void Initialize();

    public void UpdateDisplay(int amount)
    {
        if (virtualAmount == -1)
        {
            virtualAmount = amount;
            text.text = amount.ToString();
            return;
        }

        DOVirtual.Float(virtualAmount, amount, 0.5f, value =>
        {
            virtualAmount = Mathf.RoundToInt(value);
            text.text = virtualAmount.ToString();
        }).SetUpdate(true);
    }
}
