using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class MessageManage : MonoBehaviour
{
    
    /// <summary>
    /// json化对话key，用于保存玩家对话历史信息
    /// </summary>
    /// <param name="saveFolder">存放信息的文件夹路径</param>
    /// <param name="saveFilePath">存放信息的文件路径</param>
    /// <param name="senderName">当前对话的key，例如第一章对话，第二章对话</param>
    /// <param name="keys">对话系统的kyes</param>
    public void SaveMessage(string saveFolder, string saveFilePath, string senderName, HashSet<string> keys)
    {
        // 判断是否存在该文件夹
        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
        }

        // 读取json文件，写入对话信息
        MessageSaveDate saveData = LoadMessage(saveFilePath);
        MessageDate keysData = saveData.allSenders.Find(d => d.senderName == senderName);
        if (keysData == null)
        {
            keysData = new MessageDate { senderName = senderName };
            saveData.allSenders.Add(keysData);  
        }

        keysData.keys = keys.ToList();
        File.WriteAllText(saveFilePath, JsonUtility.ToJson(saveData, true));
;    }

    /// <summary>
    /// 删除对话信息
    /// </summary>
    /// <param name="saveFilePath">存放信息的文件路径</param>
    /// <param name="senderName">当前对话的key，例如第一章对话，第二章对话</param>
    private void DeleteSenderFromJson(string saveFilePath, string senderName)
    {
        var saveData = LoadMessage(saveFilePath);
        saveData.allSenders.RemoveAll(d => d.senderName == senderName);
        File.WriteAllText(saveFilePath, JsonUtility.ToJson(saveData, true));
    }

    /// <summary>
    /// 加载对话信息
    /// </summary>
    /// <param name="saveFilePath">存放文件路径</param>
    /// <returns></returns>
    public MessageSaveDate LoadMessage(string saveFilePath)
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            return JsonUtility.FromJson<MessageSaveDate>(json);
        }
        return new MessageSaveDate();
    }


}
