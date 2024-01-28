using Dasis.Common;
using UnityEngine;
using Dasis.Extensions;
using System.Collections.Generic;

public class Player : TwoDimensionObject
{
    [SerializeField]
    private CameraFollower cameraFollower;

    [SerializeField]
    private TrailRenderer trail;

    [SerializeField]
    private EndLevelPanel endLevelPanel;

    [SerializeField]
    private Mattress mattress;

    [SerializeField]
    private SpriteRenderer faceRender;

    [SerializeField]
    private List<Sprite> faces;

    private const float slowEffectMultiply = 0.3f;
    private bool beCaptured = false;

    public float YBottomLimit => cameraFollower.YBottomLimit - CameraSizeInfo.Instance.InnerRange.y;

    public float MaxForce => 25;

    public float Force { get; set; }

    public void Initialize()
    {
        faceRender.sprite = faces[0];
        Transform.rotation = Quaternion.identity;
        XYPosition = Vector2.zero;
        Rigidbody2D.velocity = Vector2.zero;
        Rigidbody2D.angularVelocity = 0;
        beCaptured = false;
    }

    private void Update()
    {
        if (YPosition < YBottomLimit && !beCaptured)
        {
            OnBeCaptured();
        }
    }

    private void FixedUpdate()
    {
        Force = Rigidbody2D.velocity.magnitude;
        if (Force > MaxForce)
        {
            Rigidbody2D.velocity *= MaxForce / Force;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            OnCollectCoin(collision.gameObject.GetComponent<Coin>());
        }
        if (collision.CompareTag("Slower"))
        {
            OnSlower(collision.gameObject.GetComponent<Slower>());
        }
        if (collision.CompareTag("Enemy") || collision.CompareTag("PopCat"))
        {
            OnBeCaptured();
        }
    }

    public void OnCollectCoin(Coin coin)
    {
        faceRender.sprite = faces[1];
        AudioManager.Instance.Play(AudioEnum.Star);
        ComboManager.Instance.CheckCombo();
        GameManager.Instance.Score += 1 * Mathf.Max(1, ComboManager.Instance.Combo);
        coin.Recall();
        this.WaitForSeconds(0.5f, () =>
        {
            faceRender.sprite = faces[0];
        });
    }

    public void OnSlower(Slower slower)
    {
        faceRender.sprite = faces[2];
        AudioManager.Instance.Play(AudioEnum.Laugh);
        Rigidbody2D.velocity *= slowEffectMultiply;
        slower.Recall();
        this.WaitForSeconds(0.5f, () =>
        {
            faceRender.sprite = faces[0];
        });
    }

    public void OnBeCaptured()
    {
        mattress.TimeScaleTweener.TweenTimeScale(1, 0);
        beCaptured = true;
        trail.Clear();
        endLevelPanel.OnOpen();
        LevelManager.Instance.ClearLevel();
    }
}
