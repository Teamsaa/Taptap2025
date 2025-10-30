using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Level1 : MonoBehaviour
{
    private bool isPlayingTimeLine = false;
    public PlayableDirector director;
    public PlayerController player;


    public static Level1 Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        director.Play();
    }

    private void Update()
    {
    }

    public void OnTimeLineStart()
    {
        // 禁用玩家控制
        if (player != null)
        {
            player.enabled = false;
        }
        isPlayingTimeLine = true;
    }

    void OnTimelineEnd()
    {
        // 2. 禁用 PlayableDirector
        if (director != null)
        {
            director.gameObject.SetActive(false);
        }
        if (player != null)
        {
            player.enabled = true;
        }
        isPlayingTimeLine = false;
    }

}
