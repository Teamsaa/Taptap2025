using System;
using UnityEngine;

[DisallowMultipleComponent]
public class Mover : MonoBehaviour
{
    [Header("移动速度（单位/秒）")]
    public float moveSpeed = 4f;

    [Header("停止距离")]
    public float stopDistance = 0.05f;

    /// <summary>抵达事件</summary>
    public event Action OnArrived;

    // 仅记录目标X
    float? targetX;

    /// <summary>是否正在移动</summary>
    public bool IsMoving => targetX.HasValue;

    void Update()
    {
        if (!targetX.HasValue) return;

        Vector3 cur = transform.position;
        float dx = targetX.Value - cur.x;
        float dist = Mathf.Abs(dx);

        if (dist <= stopDistance)
        {
            transform.position = new Vector3(targetX.Value, cur.y, cur.z);
            targetX = null;
            OnArrived?.Invoke();
            return;
        }

        // 计算本帧位移
        float step = Mathf.Sign(dx) * moveSpeed * Time.deltaTime;
        
        if (Mathf.Abs(step) > dist) step = Mathf.Sign(dx) * dist;

        transform.position = new Vector3(cur.x + step, cur.y, cur.z);
    }

    /// 移动到世界坐标
    public void MoveTo(Vector2 worldPos) => targetX = worldPos.x;

    /// 移动到X
    public void MoveToX(float x) => targetX = x;

    /// 取消当前移动
    public void Cancel()
    {
        targetX = null;
    }
}
