using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MessageDate : MonoBehaviour
{
    public string SenderName => senderName;
    public List<string> Keys => Keys;


    private string senderName;
    private List<string> keys = new List<string>();
}
