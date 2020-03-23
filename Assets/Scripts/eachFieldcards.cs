using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.IO;

public class eachFieldcards : MonoBehaviour
    , IPointerClickHandler
    , IPointerEnterHandler
    , IPointerExitHandler
{
    public int thisfield;
    public bool isThischecked;
    public bool dreaming;
    public bool sunset;

    public fieldData FieldData;
    public carddata Carddata;
    void Start()
    {
        isThischecked = false;
        dreaming = false;
        sunset = false;
        Carddata = new carddata();
        CoverYourField();
    }

    void Update()
    {
        if(GameObject.Find("EmptyField").GetComponent<fieldcards>().didyouchecklevel)
        {
            thisfield = GameObject.Find("EmptyField").GetComponent<fieldcards>().fields[itsnumber()];
            FieldData = GameObject.Find("EmptyField").GetComponent<fieldcards>().fieldDatas[itsnumber()];
            if(thisfield == 0)
                openYourField();
            else if(thisfield == 14 || thisfield == 15)
                thisfield = 20 + GameObject.Find("Face").GetComponent<whoandwhere>().thefloor;
        }
        if(GameObject.Find("Face").GetComponent<whoandwhere>().selectingTime == false)
        {
            GameObject.Find("SelectOne").transform.SetAsFirstSibling();
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isThischecked == true || sunset == true)
            ShowYourField();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        CoverYourField();
    }

    void ShowYourField()
    {
        Text text = GameObject.Find("WhatField").transform.GetChild(0).GetComponent<Text>();
        text.text = FieldData.name + "\n" + FieldData.text;
    }
    void CoverYourField()
    {
        Text text = GameObject.Find("WhatField").transform.GetChild(0).GetComponent<Text>();
        text.text = "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int playerpoint = GameObject.Find("EmptyField").GetComponent<fieldcards>().playerHere;
        if(isThischikai(playerpoint, itsnumber()))
        {
            moveHere();
            if(isThischecked == false)
                openYourField();
            ShowYourField();
            }
    }

    void openYourField()
    {
        Texture fieldimage = GameObject.Find("EmptyField").GetComponent<fieldcards>().fieldImages[thisfield];
        RawImage thisimage = this.GetComponent<RawImage>();
        thisimage.texture = fieldimage;
        isThischecked = true;
        activateField();
    }

    bool isThischikai(int from, int to)
    {
        int distance = from - to;
        if ((from % 4 != 3 || distance != -1) && (from % 4 != 0 || distance != 1))
        {
            if (distance == 1 || distance == -1 || distance == 4 || distance == -4)
                return true;
            return false;
        }
        else
            return false;
    }

    void moveHere()
    {
        int here = 0;
        here = itsnumber();
        GameObject.Find("EmptyField").GetComponent<fieldcards>().changePos(here);
        GameObject.Find("EmptyField").GetComponent<fieldcards>().characterPosChange();
    }

    int itsnumber()
    {
        int here = 0;
        if(this.name.Length == 6)
            here = int.Parse(this.name.Substring(5,1));
        else if(this.name.Length == 7)
            here = int.Parse(this.name.Substring(5,2));
        return here;
    }

    void activateField()
    {
        switch(thisfield)
        {
            case 0: //시작
            case 1:
            case 2: break;//텅빈
            case 3:
            case 4:
            case 5://보물상자
                abandonCard();
                selectCard();
                break;
            case 6://모닥불
                healed(GameObject.Find("Face").GetComponent<whoandwhere>().Carddata.hp);
                abandonCard();
                break;
            case 7://영사실
                damaged(GameObject.Find("Face").GetComponent<whoandwhere>().currentHP/2);
                break;
            case 8:
            case 9://황야
                selectCard();
                break;
            case 10://가시덤불
                damaged(1);
                break;
            case 11://샘물
                healed(2);
                break;
            case 12://진실의 방
                roomOfTruth();
                break;
            case 13://보스
                boss();
                break;
            case 21://꿈속의 바다
                oceanOfDream();
                break;
            case 22://별빛 정원
                starryGarden();
                break;
            case 23://노을색 들판
                sunsetField();
                break;
            case 24://어두운 숲
                darkForest();
                break;
            case 25://깊고 어두운 환상
                break;
            case 26://검은 방
                break;
            default:
                break; 
        }
    }

    void selectCard()
    {
        GameObject.Find("Face").GetComponent<whoandwhere>().selectingTime = true;
        
        GameObject.Find("Face").GetComponent<whoandwhere>().first = setNameToSelect(0);
        GameObject.Find("Face").GetComponent<whoandwhere>().second = setNameToSelect(1);
        GameObject.Find("Face").GetComponent<whoandwhere>().third = setNameToSelect(2);

        GameObject.Find("SelectOne").transform.SetAsLastSibling();
    }
    int setNameToSelect(int num)
    {
        int st = GameObject.Find("Deck").GetComponent<deck>().additionalDecklist[num];
        LoadCardDataFromjson(st);
        Text text = GameObject.Find("SelectOne").transform.GetChild(num).GetChild(0).GetComponent<Text>();
        text.text = Carddata.name;
        return st;
    }
    void LoadCardDataFromjson(int number)
    {
        string path = Path.Combine(Application.dataPath, "card_" + number);
        string jsonData = File.ReadAllText(path);
        Carddata = JsonUtility.FromJson<carddata>(jsonData);
    }

    void abandonCard()
    {
        GameObject.Find("Face").GetComponent<whoandwhere>().selectingTime = true;
        for(int i = 0; i<9; i++)
            setNameToAbandon(i);
        GameObject.Find("TrashOne").transform.SetAsLastSibling();
    }
    int setNameToAbandon(int num)
    {
        int st = GameObject.Find("Deck").GetComponent<deck>().decklist[num];
        LoadCardDataFromjson(st);
        Text text = GameObject.Find("TrashOne").transform.GetChild(num).GetChild(0).GetComponent<Text>();
        text.text = Carddata.name;
        return st;
    }

    void damaged(int a)
    {
        if(dreaming == false && GameObject.Find("Face").GetComponent<whoandwhere>().thefloor == 1)
            GameObject.Find("Face").GetComponent<whoandwhere>().damage(a);
    }

    void healed(int a)
    {
        GameObject.Find("Face").GetComponent<whoandwhere>().heal(a);
    }

    void roomOfTruth()
    {
        int currentFloor = GameObject.Find("Face").GetComponent<whoandwhere>().thefloor;
        int bossNumber = GameObject.Find("Deck").GetComponent<deck>().bosslist[currentFloor-1];
        string additionalText = "\n현재 보스는 ";
        switch(bossNumber)
        {
            case 1: additionalText += "가이드 요정 입니다."; break;
            case 2: additionalText += "유령 입니다."; break;
            case 3: additionalText += "과학자 입니다."; break;
            case 4: additionalText += "안개 입니다."; break;
            case 5: additionalText += "해골 입니다."; break;
            case 6: additionalText += "마왕 입니다."; break;
        }
        FieldData.text += additionalText;
    }
    
    void boss()
    {
        //SceneManager.LoadScene(3);
    }

    void oceanOfDream()
    {
        dreaming = true;
    }
    void starryGarden()
    {
        abandonCard();
        GameObject.Find("Deck").GetComponent<deck>().Intodeck(Random.Range(50,76));
    }
    void sunsetField()
    {
        int playerpoint = GameObject.Find("EmptyField").GetComponent<fieldcards>().playerHere;
        for (int i = 0; i < 16; i++)
            if (isThischikai(playerpoint, i))
                GameObject.Find("EmptyField").transform.GetChild(i).GetComponent<eachFieldcards>().sunset = true;
    }
    void darkForest()
    {

    }
}