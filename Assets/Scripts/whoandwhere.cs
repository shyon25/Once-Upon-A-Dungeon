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

    public bool battleTime;

    private static whoandwhere instance = null;

    void Start()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        whoami();
        currentHP = Carddata.hp;
        selectingTime = false;
        setMyHp();
        setFloor(1);
        battleTime = false;

        instance = this;
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        
    }

    
    void whoami()
    {
        switch (GameObject.Find("ChosenCharacter").GetComponent<selectCharacter>().chara)
        {
            case selectCharacter.Character.Rem:
                number = 0;
                GameObject.Find("Face").GetComponent<RawImage>().texture = face_rem;
                break;
            case selectCharacter.Character.Gae:
                GameObject.Find("Face").GetComponent<RawImage>().texture = face_gae;
                number = 4;
                break;
            case selectCharacter.Character.Violet:
                GameObject.Find("Face").GetComponent<RawImage>().texture = face_violet;
                number = 5;
                break;
        }
        LoadCardDataFromjson();
    }

    public void setMyHp()
    {
        Text HP = GameObject.Find("Face").transform.GetChild(0).GetComponent<Text>();
        HP.text = currentHP + " / " + Carddata.hp;
    }

    public void setFloor(int floor)
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
