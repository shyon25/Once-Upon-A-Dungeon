using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public enum Type { MERCENARY, SPELL, ITEM, CHARACTER }

public class showyourCard : MonoBehaviour
{
    public carddata Carddata;
    public int number;

    [ContextMenu("To json file")]
    void SaveCardDataTojson()
    {
        string jsonData = JsonUtility.ToJson(Carddata);
        string path = Path.Combine(Application.dataPath, "card_" + number);
        File.WriteAllText(path, jsonData);
    }

    [ContextMenu("From json data")]
    void LoadCardDataFromjson()
    {
        string path = Path.Combine(Application.dataPath, "card_" + number);
        string jsonData = File.ReadAllText(path);
        Carddata = JsonUtility.FromJson<carddata>(jsonData);
    }
    
}
[System.Serializable]
public class carddata
{
    public string name;
    public Type type;
    public int atk;
    public int hp;
    public int cost;
    public string effect;
}