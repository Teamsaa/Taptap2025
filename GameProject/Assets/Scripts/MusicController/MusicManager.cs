using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : SingletonMono<MusicManager>
{
    private AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }



    /// <summary>
    /// ����ָ��������
    /// </summary>
    public void PlayMusic()
    {
        Debug.Log("��ʼ��������");
        audioSource.Play();
        
    }

    /// <summary>
    /// ��ͣ��ǰ���ŵ�����
    /// </summary>
    public void PauseMusic()
    {
        // �����ǰ���ڲ������֣���ͣ
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    /// <summary>
    /// �������ŵ�ǰ��ͣ������
    /// </summary>
    public void ResumeMusic()
    {
        // �����ǰ���ִ�����ͣ״̬����������
        if (!audioSource.isPlaying)
        {
            audioSource.UnPause();
        }
    }

    /// <summary>
    /// �������ֵ�����
    /// </summary>
    /// <param name="volume">����ֵ����Χ 0 �� 1</param>
    public void SetVolume(float volume)
    {
        // ʹ�� Mathf.Clamp ȷ������ֵ�� 0 �� 1 ֮��
        audioSource.volume = Mathf.Clamp(volume, 0f, 1f);
    }


    /// <summary>
    /// ֹͣ��������
    /// </summary>
    public void StopMusic()
    {
        // ֹͣ AudioSource �Ĳ���
        audioSource.Stop();
    }

}
