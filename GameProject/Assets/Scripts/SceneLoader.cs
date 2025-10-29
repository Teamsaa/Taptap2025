using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 渐入渐出动画脚本
/// </summary>
public class SceneLoader : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        canvasGroup = GetComponentInChildren<CanvasGroup>();
    }

    /// <summary>
    /// 渐入动画
    /// </summary>
    public void FadeIn()
    {
        canvasGroup.DOFade(1, .1f);
    }

    /// <summary>
    /// 渐出动画-异步重构
    /// </summary>
    /// <param name="obj"></param>
    public void FadeOut(AsyncOperation obj)
    {
        canvasGroup.DOFade(0, .1f);
    }

    /// <summary>
    /// 渐出动画
    /// </summary>
    public void FadeOut()
    {   
        Debug.Log("调用渐出动画");
        canvasGroup.DOFade(0, .1f);
    }
}
