using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class Panel : MonoBehaviour
{
    /* Static */
    public static int NumberOfPanels = 0;

    /* Input */
    [Header("Animation Settings")]
    [SerializeField]
    [PropertyOrder(Order = -3)]
    protected Animation openAnimation;
    [SerializeField]
    [PropertyOrder(Order = -3)]
    protected Animation closeAnimation;

    [Header("Animation Registers")]
    [Tooltip("Required Rects[0] = <Background> and Rects[1] = <Container> to run animations")]
    [SerializeField]
    [PropertyOrder(Order = -2)]
    protected List<RectTransform> rects = new List<RectTransform>();

    [Space]
    [Tooltip("Disinteractable these buttons on Close to prevent multiple click event")]
    [SerializeField]
    [PropertyOrder(Order = -1)]
    protected List<Button> buttons;

    [SerializeField]
    [PropertyOrder(Order = -1)]
    protected Color disableColor = Color.white;

    protected Vector2 panelSizeDelta;
    protected readonly List<Vector2> originPositions = new List<Vector2>();
    protected CanvasGroup canvasGroup;
    protected State state = State.Closing;

    protected CanvasGroup CanvasGroup
    {
        get
        {
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
            }
            return canvasGroup;
        }
    }

    protected void Awake()
    {
        PrepareAnimation();
        Initialize();
    }

    protected void PrepareAnimation()
    {
        if (rects == null || rects.Count == 0) return;
        if (rects[0] != null)
        {
            panelSizeDelta = new Vector2(rects[0].rect.width, rects[0].rect.height);
        }
        originPositions.Clear();
        foreach (var rect in rects)
        {
            originPositions.Add(rect.localPosition);
        }
    }

    public virtual void Initialize() { }

    protected void OnEnable()
    {
        NumberOfPanels++;
    }

    protected void OnDisable()
    {
        NumberOfPanels--;
    }

    [PropertyOrder(Order = -2)]
    [Button(Icon = SdfIconType.LightningFill)]
    public void QuickAddRegisters()
    {
        rects.Clear();
        for (int i = 0; i < 2; i++)
        {
            if (i >= transform.childCount)
            {
                break;
            }
            rects.Add(transform.GetChild(i).GetComponent<RectTransform>());
        }
    }

    [ButtonGroup("Buttons")]
    [PropertyOrder(Order = -1)]
    [Button(Icon = SdfIconType.LightningFill)]
    public void QuickAddButtons()
    {
        buttons.Clear();

        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(transform);

        while (queue.Count > 0)
        {
            var tf = queue.Dequeue();
            if (tf.TryGetComponent(out Button button))
            {
                buttons.Add(button);
            }
            for (int i = 0; i < tf.childCount; i++)
            {
                queue.Enqueue(tf.GetChild(i));
            }
        }
    }

    [ButtonGroup("Buttons")]
    [PropertyOrder(Order = -1)]
    [Button(Icon = SdfIconType.PaletteFill)]
    public void SetDisinteractableColor()
    {
        foreach (var button in buttons)
        {
            var buttonColor = button.colors;
            buttonColor.disabledColor = disableColor;
            button.colors = buttonColor;
        }
    }

    [PropertyOrder(Order = -3)]
    [ButtonGroup]
    [DisableInEditorMode]
    public virtual void OnOpen()
    {
        gameObject.SetActive(true);
        state = State.Opening;
        SetAllButtonsInteractable(true);
        AudioManager.Instance.Play(openAnimation.audioName, true);
        Sequence sq = DOTween.Sequence();
        switch (openAnimation.type)
        {
            case Animation.Type.None:
                CanvasGroup.alpha = 1;
                break;
            case Animation.Type.Fade:
                sq.Append(Fade(State.Opening, openAnimation));
                break;
            case Animation.Type.Zoom:
                sq.Append(Zoom(State.Opening, openAnimation));
                sq.Join(Fade(State.Opening, openAnimation));
                break;
            case Animation.Type.Floating:
                sq.Append(Floating(State.Opening, openAnimation));
                sq.Join(Fade(State.Opening, openAnimation));
                break;
        }
        sq.OnComplete(() =>
        {
            OnOpenCompletely();
        });
        sq.SetUpdate(true);
    }

    [PropertyOrder(Order = -3)]
    [ButtonGroup]
    [DisableInEditorMode]
    public virtual void OnClose()
    {
        state = State.Closing;
        SetAllButtonsInteractable(false);
        AudioManager.Instance.Play(closeAnimation.audioName, true);
        Sequence sq = DOTween.Sequence();
        switch (closeAnimation.type)
        {
            case Animation.Type.None:
                break;
            case Animation.Type.Fade:
                sq.Append(Fade(State.Closing, closeAnimation));
                break;
            case Animation.Type.Zoom:
                sq.Append(Zoom(State.Closing, closeAnimation));
                sq.Join(Fade(State.Closing, closeAnimation));
                break;
            case Animation.Type.Floating:
                sq.Append(Floating(State.Closing, closeAnimation));
                sq.Join(Fade(State.Closing, closeAnimation));
                break;
        }
        sq.OnComplete(() =>
        {
            gameObject.SetActive(false);
            OnCloseCompletely();
        });
        sq.SetUpdate(true);
    }

    public virtual void OnOpenCompletely() { }
    public virtual void OnCloseCompletely() { }

    public void SetAllButtonsInteractable(bool interactable)
    {
        if (buttons == null) return;
        foreach (var button in buttons)
        {
            button.interactable = interactable;
        }
    }

    #region Animations
    public Sequence Fade(State state, Animation animation)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(animation.delay);
        if (state == State.Opening)
        {
            CanvasGroup.alpha = 0;
            sequence.Append(CanvasGroup.DOFade(1, animation.duration));
        }
        if (state == State.Closing)
        {
            CanvasGroup.alpha = 1;
            sequence.Append(CanvasGroup.DOFade(0, animation.duration));
        }
        sequence.SetEase(animation.ease);
        return sequence;
    }

    public Sequence Zoom(State state, Animation animation)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(animation.delay);
        if (state == State.Opening)
        {
            rects[1].localScale = Vector3.zero;
            sequence.Join(rects[1].DOScale(1, animation.duration).SetEase(animation.ease));
        }
        if (state == State.Closing)
        {
            rects[1].localScale = Vector3.one;
            sequence.Join(rects[1].DOScale(0, animation.duration).SetEase(animation.ease));
        }
        return sequence;
    }

    public Sequence Floating(State state, Animation animation)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(animation.delay);
        Vector2 floatedContainerPosition = animation.GetDirection() * panelSizeDelta;
        if (state == State.Opening)
        {
            rects[1].localPosition = -floatedContainerPosition;
            sequence.Join(rects[1].transform.DOLocalMove(originPositions[1], animation.duration).SetEase(animation.ease));
        }
        if (state == State.Closing)
        {
            rects[1].localPosition = originPositions[1];
            sequence.Join(rects[1].transform.DOLocalMove(floatedContainerPosition, animation.duration).SetEase(animation.ease));
        }
        return sequence;
    }
    #endregion

    #region Structure
    public enum State
    {
        Opening,
        Closing,
    }

    [System.Serializable]
    public class Animation
    {
        public Type type;

        [ShowIf("@type == Type.Floating")]
        public Direction direction;

        [ShowIf("@type != Type.None")]
        public Ease ease;

        [Range(0, 3f)]
        public float delay = 0f; // Common

        [ShowIf("@type != Type.None")]
        [Range(0, 2f)]
        public float duration = 0.4f; // Recommended

        [LabelText("Audio")]
        [ValueDropdown("@AudioDictionary.names")]
        public string audioName;

        public Animation()
        {
            audioName = AudioManager.NULL_NAME;
        }

        public enum Type
        {
            None,
            Fade,
            Zoom,
            Floating,
        }

        public enum Direction
        {
            TopToBottom,
            BottomToTop,
            LeftToRight,
            RightToLeft,
        }

        public Vector2 GetDirection()
        {
            return direction switch
            {
                Direction.TopToBottom => Vector2.down,
                Direction.BottomToTop => Vector2.up,
                Direction.LeftToRight => Vector2.right,
                Direction.RightToLeft => Vector2.left,
                _ => Vector2.zero,
            };
        }
    }
    #endregion
}