using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class MessageManage : MonoBehaviour
{
    
    /// <summary>
    /// json���Ի�key�����ڱ�����ҶԻ���ʷ��Ϣ
    /// </summary>
    /// <param name="saveFolder">�����Ϣ���ļ���·��</param>
    /// <param name="saveFilePath">�����Ϣ���ļ�·��</param>
    /// <param name="senderName">��ǰ�Ի���key�������һ�¶Ի����ڶ��¶Ի�</param>
    /// <param name="keys">�Ի�ϵͳ��kyes</param>
    public void SaveMessage(string saveFolder, string saveFilePath, string senderName, HashSet<string> keys)
    {
        // �ж��Ƿ���ڸ��ļ���
        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
        }

        // ��ȡjson�ļ���д��Ի���Ϣ
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
    /// ɾ���Ի���Ϣ
    /// </summary>
    /// <param name="saveFilePath">�����Ϣ���ļ�·��</param>
    /// <param name="senderName">��ǰ�Ի���key�������һ�¶Ի����ڶ��¶Ի�</param>
    private void DeleteSenderFromJson(string saveFilePath, string senderName)
    {
        var saveData = LoadMessage(saveFilePath);
        saveData.allSenders.RemoveAll(d => d.senderName == senderName);
        File.WriteAllText(saveFilePath, JsonUtility.ToJson(saveData, true));
    }

    /// <summary>
    /// ���ضԻ���Ϣ
    /// </summary>
    /// <param name="saveFilePath">����ļ�·��</param>
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
