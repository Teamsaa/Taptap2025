using System;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public enum MoveType
    {
        X_Axis,     // 只移动X轴
        Y_Axis,     // 只移动Y轴
        XY_Sequence // 先X后Y
    }

    [Header("移动设置")]
    public MoveType moveType = MoveType.X_Axis;
    public float moveSpeed = 4f;
    public float stopDistance = 0.05f;

    [Header("事件响应")]
    public bool respondToEvents = true;
    
    /// <summary>抵达后可能播放动画</summary>
    public event Action OnArrived;

    // 移动目标
    private Vector2? targetPosition;
    private bool isMoving = false;

    /// <summary>是否正在移动</summary>
    public bool IsMoving => isMoving;

    void Update()
    {
        if (!isMoving || !targetPosition.HasValue) return;

        Vector3 currentPos = transform.position;
        Vector2 target = targetPosition.Value;

        switch (moveType)
        {
            case MoveType.X_Axis:
                MoveXAxis(currentPos, target);
                break;
            case MoveType.Y_Axis:
                MoveYAxis(currentPos, target);
                break;
            case MoveType.XY_Sequence:
                MoveXYSequence(currentPos, target);
                break;
        }
    }

    void MoveXAxis(Vector3 currentPos, Vector2 target)
    {
        float dx = target.x - currentPos.x;
        float dist = Mathf.Abs(dx);

        if (dist <= stopDistance)
        {
            transform.position = new Vector3(target.x, currentPos.y, currentPos.z);
            CompleteMovement();
            return;
        }

        float step = Mathf.Sign(dx) * moveSpeed * Time.deltaTime;
        if (Mathf.Abs(step) > dist) step = Mathf.Sign(dx) * dist;

        transform.position = new Vector3(currentPos.x + step, currentPos.y, currentPos.z);
    }

    void MoveYAxis(Vector3 currentPos, Vector2 target)
    {
        float dy = target.y - currentPos.y;
        float dist = Mathf.Abs(dy);

        if (dist <= stopDistance)
        {
            transform.position = new Vector3(currentPos.x, target.y, currentPos.z);
            CompleteMovement();
            return;
        }

        float step = Mathf.Sign(dy) * moveSpeed * Time.deltaTime;
        if (Mathf.Abs(step) > dist) step = Mathf.Sign(dy) * dist;

        transform.position = new Vector3(currentPos.x, currentPos.y + step, currentPos.z);
    }

    void MoveXYSequence(Vector3 currentPos, Vector2 target)
    {
        // 先移动X轴
        float dx = target.x - currentPos.x;
        float distX = Mathf.Abs(dx);

        if (distX > stopDistance)
        {
            float stepX = Mathf.Sign(dx) * moveSpeed * Time.deltaTime;
            if (Mathf.Abs(stepX) > distX) stepX = Mathf.Sign(dx) * distX;
            transform.position = new Vector3(currentPos.x + stepX, currentPos.y, currentPos.z);
            return;
        }

        // X轴到达后移动Y轴
        transform.position = new Vector3(target.x, currentPos.y, currentPos.z);
        
        float dy = target.y - currentPos.y;
        float distY = Mathf.Abs(dy);

        if (distY > stopDistance)
        {
            float stepY = Mathf.Sign(dy) * moveSpeed * Time.deltaTime;
            if (Mathf.Abs(stepY) > distY) stepY = Mathf.Sign(dy) * distY;
            transform.position = new Vector3(target.x, currentPos.y + stepY, currentPos.z);
            return;
        }

        // Y轴也到达，完成移动
        transform.position = new Vector3(target.x, target.y, currentPos.z);
        CompleteMovement();
    }

    void CompleteMovement()
    {
        isMoving = false;
        targetPosition = null;
        OnArrived?.Invoke();
    }

    /// <summary>移动到世界坐标</summary>
    public void MoveTo(Vector2 worldPos)
    {
        if (!respondToEvents) return;

        targetPosition = worldPos;
        isMoving = true;
    }

    /// <summary>移动到X坐标</summary>
    public void MoveToX(float x)
    {
        if (!respondToEvents) return;

        Vector3 currentPos = transform.position;
        targetPosition = new Vector2(x, currentPos.y);
        isMoving = true;
    }

    /// <summary>移动到Y坐标（保持X和Z不变）</summary>
    public void MoveToY(float y)
    {
        if (!respondToEvents) return;

        Vector3 currentPos = transform.position;
        targetPosition = new Vector2(currentPos.x, y);
        isMoving = true;
    }

    /// <summary>移动到指定X和Y坐标</summary>
    public void MoveToXY(float x, float y)
    {
        if (!respondToEvents) return;

        targetPosition = new Vector2(x, y);
        isMoving = true;
    }

    //取消当前移动
    public void Cancel()
    {
        isMoving = false;
        targetPosition = null;
    }

    //启用事件响应
    public void EnableResponse()
    {
        respondToEvents = true;
    }

    //禁用事件响应
    public void DisableResponse()
    {
        respondToEvents = false;
    }

    //获取当前目标位置
    public Vector2? GetTargetPosition()
    {
        return targetPosition;
    }
}