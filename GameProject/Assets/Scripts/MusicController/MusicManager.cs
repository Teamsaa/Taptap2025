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
    /// 播放指定的音乐
    /// </summary>
    public void PlayMusic()
    {
        Debug.Log("开始播放音乐");
        audioSource.Play();
        
    }

    /// <summary>
    /// 暂停当前播放的音乐
    /// </summary>
    public void PauseMusic()
    {
        // 如果当前正在播放音乐，暂停
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    /// <summary>
    /// 继续播放当前暂停的音乐
    /// </summary>
    public void ResumeMusic()
    {
        // 如果当前音乐处于暂停状态，继续播放
        if (!audioSource.isPlaying)
        {
            audioSource.UnPause();
        }
    }

    /// <summary>
    /// 设置音乐的音量
    /// </summary>
    /// <param name="volume">音量值，范围 0 到 1</param>
    public void SetVolume(float volume)
    {
        // 使用 Mathf.Clamp 确保音量值在 0 到 1 之间
        audioSource.volume = Mathf.Clamp(volume, 0f, 1f);
    }


    /// <summary>
    /// 停止播放音乐
    /// </summary>
    public void StopMusic()
    {
        // 停止 AudioSource 的播放
        audioSource.Stop();
    }

}
