using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private MusicManager musicManager;

    private void Start()
    {
        musicManager = GetComponent<MusicManager>();
        musicManager.PlayMusic();
    }

}
