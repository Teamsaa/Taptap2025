using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Xml.Schema;
using DG.Tweening;

public class MainUIController : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button endButton;
    [SerializeField] private SceneLoader sceneLoader;

    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
        endButton.onClick.AddListener(EndGame);   
        sceneLoader.FadeOut();
    }


    private void StartGame()
    {
        StartCoroutine(LoadScene());
        
    }

    private void EndGame()
    {
        Application.Quit();
    }

    IEnumerator LoadScene()
    {
        // fade in
        sceneLoader.FadeIn();

        yield return new WaitForSeconds(1);
        AsyncOperation async = SceneManager.LoadSceneAsync(1);
        async.completed += sceneLoader.FadeOut;
    }
}
