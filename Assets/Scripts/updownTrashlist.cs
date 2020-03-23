using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class updownTrashlist : MonoBehaviour
{
    public int how_many_down = 0;
    public int slot = 9;
    public carddata Carddata = new carddata();
    public void Up()
    {
        if (how_many_down > 0)
            how_many_down -= 1;
        updateyourlist();
    }
    public void Down()
    {
        if ((how_many_down + 1) * slot < GameObject.Find("Deck").GetComponent<deck>().decklist.Count)
            how_many_down += 1;
        updateyourlist();
    }
    void updateyourlist()
    {
        for (int i = 0; i < slot; i++)
        {
            Text text = GameObject.Find("TrashOne").transform.GetChild(i).GetChild(0).GetComponent<Text>();
            if (canidoit(i))
            {
                int st = GameObject.Find("Deck").GetComponent<deck>().decklist[i + how_many_down*slot];
                LoadCardDataFromjson(st);
                text.text = Carddata.name;
            }
            else
                text.text = "";
        }
    }
    bool canidoit(int name)
    {
        return how_many_down*slot + name < GameObject.Find("Deck").GetComponent<deck>().decklist.Count;
    }
    void LoadCardDataFromjson(int number)
    {
        string path = Path.Combine(Application.dataPath, "card_" + number);
        string jsonData = File.ReadAllText(path);
        Carddata = JsonUtility.FromJson<carddata>(jsonData);
    }
}
