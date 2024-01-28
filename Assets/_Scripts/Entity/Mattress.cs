using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(EdgeCollider2D))]
public class Mattress : MonoBehaviour
{
    [SerializeField]
    private float maxLength;

    [SerializeField]
    private float slowdownDuration;

    [SerializeField]
    private float fastenDuration;

    [SerializeField]
    private int numberOfPoints = 31;

    private TouchHandler touchHandler;
    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;
    private readonly TimeScaleTweener timeScaleTweener = new();
    private readonly BezierCurve bezierCurve = new();
    private Vector2 center, bouncePos;
    private Sequence bounce;

    public TimeScaleTweener TimeScaleTweener => timeScaleTweener;

    private void Awake()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();
        touchHandler = GetComponent<TouchHandler>();
        touchHandler.OnTouch += OnTouch;
        touchHandler.OnDrag += OnDrag;
        touchHandler.OnUnTouch += OnUnTouch;
    }

    public void Initialize()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, Vector2.zero);
        lineRenderer.SetPosition(1, Vector2.zero);
        edgeCollider.enabled = false;
    }

    public void OnTouch(Vector2 mousePos)
    {
        bounce?.Kill();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, mousePos);
        lineRenderer.SetPosition(1, mousePos);
        edgeCollider.enabled = false;
        timeScaleTweener.TweenTimeScale(0.1f, slowdownDuration);
    }

    public void OnDrag(Vector2 mousePos)
    {
        Vector2 diff = mousePos - (Vector2)lineRenderer.GetPosition(0);
        float length = diff.magnitude;
        if (length > maxLength)
        {
            mousePos = (Vector2)lineRenderer.GetPosition(0) + diff * maxLength / length;
        }
        lineRenderer.SetPosition(1, mousePos);
    }

    public void OnUnTouch(Vector2 mousePos)
    {
        edgeCollider.SetPoints(new List<Vector2>
        {
            lineRenderer.GetPosition(0),
            lineRenderer.GetPosition(1),
        });
        edgeCollider.enabled = true;
        timeScaleTweener.TweenTimeScale(1f, fastenDuration);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        AudioManager.Instance.Play(AudioEnum.Spring);

        edgeCollider.enabled = false;

        bezierCurve.X = lineRenderer.GetPosition(0);
        bezierCurve.Y = lineRenderer.GetPosition(1);
        center = (bezierCurve.X + bezierCurve.Y) / 2;

        Vector2 other = collision.gameObject.transform.position;

        Player ball = collision.gameObject.GetComponent<Player>();
        other += 0.5f * ball.Force * (other - center) / ball.MaxForce;

        Vector2 lineDirection = (bezierCurve.X - bezierCurve.Y).normalized;
        Vector2 perpendicular = new(-lineDirection.y, lineDirection.x);
        bouncePos = other - 2 * Vector2.Dot(other - bezierCurve.Y, perpendicular) * perpendicular;

        Bounce(0.2f);
    }

    private void Bounce(float duration)
    {
        bounce?.Kill();
        bounce = DOTween.Sequence();
        bounce.Append(DOVirtual.Float(0, 1, duration / 2, value =>
        {
            bezierCurve.Z = Vector2.Lerp(center, bouncePos, value);
            DrawCurve();
        }));
        bounce.Append(DOVirtual.Float(0, 1, duration / 2, value =>
        {
            bezierCurve.Z = Vector2.Lerp(bouncePos, center, value);
            DrawCurve();
        }));
        bounce.OnComplete(() =>
        {
            bounce = DOTween.Sequence();
            bounce.Append(DOVirtual.Int(numberOfPoints, 0, 0.3f, value =>
            {
                lineRenderer.positionCount = value;
            }).SetEase(Ease.OutSine));
        });
    }

    private void DrawCurve()
    {
        lineRenderer.positionCount = numberOfPoints;
        for (int i = 0; i < numberOfPoints; i++)
        {
            lineRenderer.SetPosition(i, bezierCurve.GetPoint(i * 1f / (numberOfPoints - 1)));
        }
    }
}
