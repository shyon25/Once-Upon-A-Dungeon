using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class whoandwhere : MonoBehaviour
{
    public int thefloor;

    public carddata Carddata;
    public Texture face_rem;
    public Texture face_gae;
    public Texture face_violet;

    public int number;
    public int currentHP;

    public int first, second, third; //for selection

    public bool selectingTime;

    void Start()
    {
        whoami();
        currentHP = Carddata.hp;
        setMyHp();
        setFloor(1);
        selectingTime = false;
    }

    void Update()
    {
        setFloor(thefloor);
        setMyHp();
    }

    
    void whoami()
    {
        switch (GameObject.Find("ChosenCharacter").GetComponent<selectCharacter>().chara)
        {
            case selectCharacter.Character.Rem:
                number = 0;
                this.GetComponent<RawImage>().texture = face_rem;
                break;
            case selectCharacter.Character.Gae:
                this.GetComponent<RawImage>().texture = face_gae;
                number = 4;
                break;
            case selectCharacter.Character.Violet:
                this.GetComponent<RawImage>().texture = face_violet;
                number = 5;
                break;
        }
        LoadCardDataFromjson();
    }

    void setMyHp()
    {
        Text HP = this.transform.GetChild(0).GetComponent<Text>();
        HP.text = currentHP + " / " + Carddata.hp;
    }

    void setFloor(int floor)
    {
        thefloor = floor;
        Text floorText = GameObject.Find("floor").transform.GetChild(0).GetComponent<Text>();
        floorText.text = thefloor + "층";
    }
    
    void LoadCardDataFromjson()
    {
        string path = Path.Combine(Application.dataPath, "card_" + number);
        string jsonData = File.ReadAllText(path);
        Carddata = JsonUtility.FromJson<carddata>(jsonData);
    }

    public void damage(int deal)
    {
        currentHP -= deal;
        if (currentHP < 0)
            currentHP = 0;
    }
    public void heal(int deal)
    {
        currentHP += deal;
        if (currentHP > Carddata.hp)
            currentHP = Carddata.hp;
    }
    public void upFloor()
    {
        thefloor += 1;
    }
    public void downFloor()
    {
        thefloor -= 1;
        if (thefloor < 1)
            thefloor = 1;
    }
}
