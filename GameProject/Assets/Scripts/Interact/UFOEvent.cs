using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class UfoEvent : MonoBehaviour
{
    [Header("UFO本体")]
    [Tooltip("UFO的Animator")]
    public Animator ufoAnimator;

    [Tooltip("UFO的transform")]
    public Transform ufoRoot;

    [Tooltip("触发UFO旋转的Trigger")]
    public string ufoRotateTrigger = "Rotate";

    [Tooltip("UFO悬停在角色头顶的高度偏移")]
    public float ufoHoverOffsetY = 2.5f;

    [Header("用 Mover 来移动 UFO")]
    [Tooltip("挂在 UFO 上的 Mover。")]
    public Mover ufoMover;

    [Header("光束")]
    [Tooltip("光束物体")]
    public GameObject beamObject;

    [Tooltip("当无法获取事件")]
    public float beamHitFallbackDelay = 0.6f;

    [Header("角色参数")]
    [Tooltip("角色 Transform")]
    public Transform character;

    [Tooltip("垂直上升高度")]
    public float ascendHeight = 3.5f;

    [Tooltip("上升用时")]
    public float ascendTime = 1.2f;

    [Tooltip("吸入完成后是否隐藏角色")]
    public bool hidePlayerOnFinish = true;

    bool _running;

    public void StartAbduction()
    {
        if (_running) return;
        StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        _running = true;

        // 引用兜底
        if (character == null)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go == null) go = GameObject.FindGameObjectWithTag("character_M/F");
            if (go) character = go.transform;
        }
        if (ufoRoot == null && ufoAnimator != null) ufoRoot = ufoAnimator.transform;
        if (ufoMover == null && ufoRoot != null) ufoMover = ufoRoot.GetComponent<Mover>();

        if (character == null || ufoRoot == null || ufoMover == null || beamObject == null)
        {
            Debug.LogWarning("[UfoEvent] 缺少必要引用");
            _running = false;
            yield break;
        }

        // 触发 UFO 旋转
        if (ufoAnimator && !string.IsNullOrEmpty(ufoRotateTrigger))
            ufoAnimator.SetTrigger(ufoRotateTrigger);

        // 移动
        var pos = ufoRoot.position;
        ufoRoot.position = new Vector3(pos.x, character.position.y + ufoHoverOffsetY, pos.z);

        bool arrived = false;
        System.Action arrivedHandler = () => { arrived = true; };
        ufoMover.OnArrived += arrivedHandler;

        ufoMover.MoveToX(character.position.x);

        // 等待到达
        float maxWait = 5f;
        float t = 0f;
        while (!arrived && t < maxWait)
        {
            t += Time.deltaTime;
            yield return null;
        }
        ufoMover.OnArrived -= arrivedHandler;
    }
}