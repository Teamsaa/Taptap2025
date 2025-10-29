using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���뽥�������ű�
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
    /// ���붯��
    /// </summary>
    public void FadeIn()
    {
        canvasGroup.DOFade(1, .1f);
    }

    /// <summary>
    /// ��������-�첽�ع�
    /// </summary>
    /// <param name="obj"></param>
    public void FadeOut(AsyncOperation obj)
    {
        canvasGroup.DOFade(0, .1f);
    }

    /// <summary>
    /// ��������
    /// </summary>
    public void FadeOut()
    {   
        Debug.Log("���ý�������");
        canvasGroup.DOFade(0, .1f);
    }
}
