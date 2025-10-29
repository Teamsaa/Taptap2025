using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class UfoEvent : MonoBehaviour
{
    [Header("UFO本体")]
    public Animator ufoAnimator;
    public Transform ufoRoot;
    public string ufoRotateTrigger = "Rotate";
    public float ufoHoverOffsetY = 2.5f;
    public float flyingHeight = 50f;

    public float flyingDuration = 2f;

    [Header("移动控制")]
    public Mover ufoMover;

    [Header("光束控制")]
    public UFOlight beamLight;
    public float beamFillSpeed = 0.8f;
    
    [Header("角色参考")]
    public Transform character;

    bool _running;

    void Start()
    {
        StartAbduction();
    }
    public void StartAbduction()
    {   
        if (_running) return;
        StartCoroutine(RunAbduction());
    }

    IEnumerator RunAbduction()
    {
        _running = true;
        
        // 基础引用检查
        if (character == null)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go == null) go = GameObject.FindGameObjectWithTag("character_M/F");
            if (go) character = go.transform;
        }

        if (ufoRoot == null && ufoAnimator != null)
            ufoRoot = ufoAnimator.transform;
        if (ufoMover == null && ufoRoot != null)
            ufoMover = ufoRoot.GetComponent<Mover>();

        if (character == null || ufoRoot == null || ufoMover == null || beamLight == null)
        {
            Debug.LogWarning("[UfoEvent] 缺少必要引用");
            _running = false;
            yield break;
        }
        
        // 重置光束状态
        beamLight.fill = 0f;
        beamLight.speed = beamFillSpeed;

        // 设置UFO初始位置
        Vector3 targetPos = new Vector3(
            character.position.x,
            character.position.y + ufoHoverOffsetY,
            ufoRoot.position.z
        );
        ufoRoot.position = targetPos;
        
        // 触发UFO旋转动画
        if (!string.IsNullOrEmpty(ufoRotateTrigger))
            ufoAnimator?.SetTrigger(ufoRotateTrigger);

        // 等待一帧确保UFO激活
        yield return null;
        
        // 移动到精确位置
        bool arrived = false;
        System.Action arrivedHandler = () => { arrived = true; };
        ufoMover.OnArrived += arrivedHandler;
        
        Debug.Log("123");
        
        ufoMover.MoveToX(character.position.x);
        
        Debug.Log("123");
        // 等待到达
        float waitTime = 0f;
        float maxWait = 3f;
        while (!arrived && waitTime < maxWait)
        {
            waitTime += Time.deltaTime;
            yield return null;
        }

        ufoMover.OnArrived -= arrivedHandler;

        // 到达后启动光束填充动画
        if (arrived)
        {
            // 启动光束
            if (!beamLight.gameObject.activeInHierarchy)
                beamLight.gameObject.SetActive(true);

            // 等待光束完全填充
            float beamWaitTime = 0f;
            while (beamLight.fill < 0.99f)
            {
                beamWaitTime += Time.deltaTime;
                yield return null;
            }


            // 吸收角色
            if (character != null)
            {

            }

            // 等待吸收过程完成
            yield return new WaitForSeconds(1f);


            // 隐藏角色
            if (character != null)
            {
                character.gameObject.SetActive(false);
            }
            
            // UFO飞出屏幕
            yield return StartCoroutine(FlyAway());
        }

        _running = false;
    }
    
    IEnumerator FlyAway()
    {

        // 飞向屏幕上方
        Vector3 startPosition = ufoRoot.position;
        Vector3 endPosition = startPosition + Vector3.up * flyingHeight;

        float flyTime = 0f;
        while (flyTime < flyingDuration)
        {
            flyTime += Time.deltaTime;
            float progress = flyTime / flyingDuration;
            
            // 使用缓动函数
            float easedProgress = Mathf.SmoothStep(0f, 1f, progress);
            ufoRoot.position = Vector3.Lerp(startPosition, endPosition, easedProgress);
            
            yield return null;
        }

        ufoRoot.position = endPosition;

        // 飞走后隐藏UFO
        gameObject.SetActive(false);
    }

}