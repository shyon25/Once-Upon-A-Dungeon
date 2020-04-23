using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

public class fieldcards : MonoBehaviour
{
    public fieldData FieldData;
    public int number;
    public bool youcanMove;
    public bool didyouchecklevel;
    public int playerHere;
    public List<int> fields = new List<int>();
    public List<fieldData> fieldDatas = new List<fieldData>();

    public List<Texture> fieldImages = new List<Texture>(26);

    public bool secondChance;
    public bool dreaming;
    public bool starry;

    void Start()
    {
        youcanMove = true;
        didyouchecklevel = false;
        secondChance = false;
        dreaming = false;
        starry = false;
    }
    
    void Update()
    {
        if(didyouchecklevel == false)
            shufflefields();
    }

    public void changePos(int a)
    {
        playerHere = a;
    }

    void shufflefields()
    {
        int random;
        while (fields.Count < 16)
        {
            random = Random.Range(0, 16);
            if (!fields.Contains(random))
                fields.Add(random);
        }
        initiateFields();
    }

    void initiateFields()
    {
        for(int i = 0; i<16; i++)
        {
            number = fields[i];
            if(number == 0)
                playerHere = i;
            else if(number == 14 || number == 15)
                number = 20 + GameObject.Find("Deck").GetComponent<whoandwhere>().thefloor;
            LoadCardDataFromjson();
            fieldData temp = new fieldData();
            temp.name = FieldData.name;
            temp.text = FieldData.text;
            fieldDatas.Add(temp);
        }
        didyouchecklevel = true;
        characterPosChange();
    }

    public void characterPosChange()
    {
        RectTransform player = GameObject.Find("MiniCharacter").GetComponent<RectTransform>();
        GameObject field = GameObject.Find("Field"+playerHere);
        player.SetParent(field.transform);
        player.anchoredPosition = new Vector2(0,0);
    }

    [ContextMenu("To json file")]
    void SaveCardDataTojson()
    {
        string jsonData = JsonUtility.ToJson(FieldData);
        string path = Path.Combine(Application.dataPath, "field_" + number);
        File.WriteAllText(path, jsonData);
    }

    [ContextMenu("From json data")]
    void LoadCardDataFromjson()
    {
        string path = Path.Combine(Application.dataPath, "field_" + number);
        string jsonData = File.ReadAllText(path);
        FieldData = JsonUtility.FromJson<fieldData>(jsonData);
    }

}
[System.Serializable]
public class fieldData
{
    public string name;
    public string text;
}
