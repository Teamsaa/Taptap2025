using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 2D 物品交互：悬停放大；点击：
/// 最后一个可点物品：可选播放动画，动画播完后淡入淡出切换场景。
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class ClickableItem2D : MonoBehaviour
{
    [Header("悬停效果")]
    public float hoverScale = 1.1f;
    public bool useOutline = true;

    [Header("角色移动")]
    [Tooltip("拖入玩家/主角身上的Mover。用于点击后移动与到达判定。")]
    public Mover playerMover;

    [Tooltip("判定到达的距离（米）")]
    public float arriveDistance = 0.05f;

    [Header("点击行为")]
    [Tooltip("若为 true：等角色到达本物体位置后再触发 onArrived；否则立刻触发 onClicked")]
    public bool triggerAfterArrive = false;   // 等角色到了再触发
    public UnityEvent onClicked;              // 立即触发（若不等到达）
    public UnityEvent onArrived;              // 到达后触发

    [Header("动画 + 切场景（仅当本物体是场景最后一个时才执行）")]
    public bool playAnimBeforeTransition = true;  // 切场景前先播动画
    public Animator targetAnimator;               // 要播放的 Animator
    public string animTrigger = "OnClick";        // 触发器名
    public string waitStateName = "";             // 可填需要等待的状态名
    public float fallbackDelay = 0.5f;            // 若未填状态名/状态检测失败，用这个秒数兜底
    public string nextSceneName;                  // 目标场景
    public bool useFade = true;

    [Header("一次性规则")]
    public bool oneShot = true;                   // 点击后锁死（关闭悬停与点击）

    // 运行时
    SpriteRenderer _sr;
    Collider2D _col;
    MaterialPropertyBlock _mpb;
    Vector3 _originScale;

    bool _waitingArrive;
    bool _interactable = true;
    bool _isFinal;                                // 标记最后一个物品

    void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _col = GetComponent<Collider2D>();
        _mpb = new MaterialPropertyBlock();
        _originScale = transform.localScale;
    }

    void OnEnable()
    {
        InteractablesRegistry.Instance?.Register(this);

        if (triggerAfterArrive && playerMover != null)
            playerMover.OnArrived += HandleArrive;
    }

    void OnDisable()
    {
        InteractablesRegistry.Instance?.Unregister(this);

        if (playerMover != null)
            playerMover.OnArrived -= HandleArrive;
    }

    // Registry 调用
    public void SetIsFinal(bool v) => _isFinal = v;

    //悬停
    void OnMouseEnter()
    {
        if (!_interactable) return;
        transform.localScale = _originScale * hoverScale;
    }

    void OnMouseExit()
    {
        if (!_interactable) return;
        transform.localScale = _originScale;
    }

    void OnMouseDown()
    {
        if (!_interactable) return;

        // 移动角色到此
        if (playerMover != null)
            playerMover.MoveTo((Vector2)transform.position);

        if (!triggerAfterArrive) ExecuteLogic();
        else _waitingArrive = true;

        if (oneShot) LockInteraction();  // 立刻锁掉本物品
    }

    void HandleArrive()
    {
        if (!_waitingArrive || playerMover == null) return;

        var p = playerMover.transform.position;
        // 判定是否到达当前物体
        if ((p - transform.position).sqrMagnitude <= arriveDistance * arriveDistance)
        {
            _waitingArrive = false;
            ExecuteLogic();
        }
    }

    async void ExecuteLogic()
    {
        // 自定义事件
        if (triggerAfterArrive) onArrived?.Invoke();
        else onClicked?.Invoke();

        if (!_isFinal) return;

        // 先播动画
        if (playAnimBeforeTransition && targetAnimator)
        {
            if (!string.IsNullOrEmpty(animTrigger))
                targetAnimator.SetTrigger(animTrigger);

            bool waited = false;

            // 等待指定状态播放完一轮
            if (!string.IsNullOrEmpty(waitStateName))
            {
                // 等待切到目标状态
                float t = 0f, maxWait = 3f;
                while (t < maxWait)
                {
                    var s = targetAnimator.GetCurrentAnimatorStateInfo(0);
                    if (s.IsName(waitStateName)) break;
                    t += Time.deltaTime;
                    await System.Threading.Tasks.Task.Yield();
                }
                // 再等一帧
                var s2 = targetAnimator.GetCurrentAnimatorStateInfo(0);
                float len = s2.length > 0.01f ? s2.length : fallbackDelay;
                float e = 0f;
                while (e < len)
                {
                    e += Time.deltaTime;
                    await System.Threading.Tasks.Task.Yield();
                }
                waited = true;
            }

            // 兜底延时
            if (!waited && fallbackDelay > 0f)
            {
                float e = 0f;
                while (e < fallbackDelay)
                {
                    e += Time.deltaTime;
                    await System.Threading.Tasks.Task.Yield();
                }
            }
        }

        // 切场景（淡入淡出）
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            if (useFade && SceneFader.Instance)
                SceneFader.Instance.FadeToScene(nextSceneName);
            else
                SceneManager.LoadScene(nextSceneName);
        }
    }

    // 点击后锁死
    void LockInteraction()
    {
        _interactable = false;
        transform.localScale = _originScale;
        if (_col) _col.enabled = false;

        // 通知 Registry
        InteractablesRegistry.Instance?.NotifyItemBecameInactive(this);
    }
}
