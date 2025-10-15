using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    [Header("对话文本")]
    [SerializeField] private DialogData dialogData;

    private string SenderName;
    private string SaveFilePath;
    private string SaveFloder;
}
