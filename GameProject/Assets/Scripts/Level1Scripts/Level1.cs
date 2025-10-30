using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Level1 : MonoBehaviour
{
    public bool isPlayingTimeLine = true;
    public PlayableDirector director;
    public PlayableDirector director2;
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
        OnTimeLineStart();
    }

    private void Update()
    {

    }

    public void OnTimeLineStart()
    {
        director.Play();
        // ������ҿ���
        if (player != null)
        {
            player.enabled = false;
        }
        isPlayingTimeLine = true;
        Debug.Log("TimeLineStart:" + isPlayingTimeLine);
    }

    public void OnTimelineEnd()
    {
        // 2. ���� PlayableDirector
        if (director != null)
        {
            director.gameObject.SetActive(false);
        }
        if (player != null)
        {
            player.enabled = true;
        }
        isPlayingTimeLine = false;
        Debug.Log("TimeLineEnd:" + isPlayingTimeLine);
    }

    public void OnTimeLine2Start()
    {
        director2.Play();
        Debug.Log("test2");
        // ������ҿ���
        if (player != null)
        {
            player.enabled = false;
        }
        isPlayingTimeLine = true;
        Debug.Log("TimeLine2Start:" + isPlayingTimeLine);
    }

    public void OnTimeline2End()
    {
        if (director2 != null)
        {
            director2.gameObject.SetActive(false);
        }
        if (player != null)
        {
            player.enabled = true;
        }
        isPlayingTimeLine = false;
        Debug.Log("TimeLine2End:" + isPlayingTimeLine);

        SceneFader.Instance.FadeToScene("Level2");
    }
}
