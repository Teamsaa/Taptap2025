using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DialogueData")]
public class DialogData : ScriptableObject
{
    [SerializeField] private List<string> data = new List<string>();
}
