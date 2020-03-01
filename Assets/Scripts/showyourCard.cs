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
    Text theText;

    private void Update()
    {
        if (WhatIsMyPlace() > GameObject.Find("Deck").GetComponent<deck>().decklist.Count)
            CleanName();
        else
            CanIShowMyCard();
    }

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
    
    void CanIShowMyCard()
    {
        theText = this.transform.GetChild(0).GetComponent<Text>();
        int mynumber = WhatIsMyPlace() + GameObject.Find("Cardlist Logo").GetComponent<showyourdecklist>().how_many_down;
        number = GameObject.Find("Deck").GetComponent<deck>().decklist[mynumber - 1];
        LoadCardDataFromjson();
        theText.text = Carddata.cost.ToString() + " " + Carddata.name;
    }

    public int WhatIsMyPlace()
    {
        return int.Parse(this.name.Substring(8,1));
    }
    void CleanName()
    {
        Text empty = this.transform.GetChild(0).GetComponent<Text>();
        empty.text = "";
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