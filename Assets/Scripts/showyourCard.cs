using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public enum Type { MERCENARY, SPELL, ITEM }

public class showyourCard : MonoBehaviour
{
    public carddata Carddata;

    [ContextMenu("From json data")]
    public void LoadCardDataFromjson(int number)
    {
        string path = Path.Combine(Application.dataPath, "card_" + number);
        string jsonData = File.ReadAllText(path);
        Carddata = JsonUtility.FromJson<carddata>(jsonData);
    }

    // Update is called once per frame
    void Update()
    {
        
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