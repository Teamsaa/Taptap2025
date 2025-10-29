using System.Collections.Generic;
using UnityEngine;

public class InteractablesRegistry : MonoBehaviour
{
    public static InteractablesRegistry Instance;

    readonly HashSet<ClickableItem2D> _alive = new();

    void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void Register(ClickableItem2D item)
    {
        if (item == null) return;
        _alive.Add(item);
        RecomputeFinal();
    }

    public void Unregister(ClickableItem2D item)
    {
        if (item == null) return;
        _alive.Remove(item);
        RecomputeFinal();
    }

    public void NotifyItemBecameInactive(ClickableItem2D item)
    {
        Unregister(item);
    }

    void RecomputeFinal()
    {
        // 清除旧标记
        foreach (var it in _alive) it.SetIsFinal(false);

        // 只剩 1 个
        if (_alive.Count == 1)
        {
            foreach (var it in _alive) { it.SetIsFinal(true); break; }
        }
    }
}
