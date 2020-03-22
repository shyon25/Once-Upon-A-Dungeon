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
    public List<int> currentSelection;

    public int first, second, third; //for selection

    public fieldData FieldData;
    public carddata Carddata;
    void Start()
    {
        isThischecked = false;
        currentSelection = new List<int>();
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
        if(isThischecked == true)
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
            case 0: 
            case 1:
            case 2: break;
            case 3:
            case 4:
            case 5: 
                selectCard();
                //abandonCard();
                break;
            case 6:
                healed(GameObject.Find("Face").GetComponent<whoandwhere>().Carddata.hp);
                break;
            case 7:
                damaged(GameObject.Find("Face").GetComponent<whoandwhere>().currentHP/2);
                break;
            case 8:
            case 9:
                selectCard();
                break;
            case 10:
                damaged(1);
                break;
            case 11:
                healed(2);
                break;
            case 12:
                roomOfTruth();
                break;
            case 13:
                boss();
                break;
            case 21:
            case 22:
            case 23:
            case 24:
            case 25:
            case 26:
                break;
            default:
                break; 
        }
    }

    void selectCard()
    {
        int iSelected = 0;
        iSelected = prepareYourSelection();
        GameObject.Find("Deck").GetComponent<deck>().Intodeck(iSelected);
    }
    int prepareYourSelection()
    {
        int final = -1;
        int result = -1;
        GameObject.Find("Face").GetComponent<whoandwhere>().selectingTime = true;

        first = setName(0);
        second = setName(1);
        third = setName(2);

        GameObject.Find("SelectOne").transform.SetAsLastSibling();
        result = selectionCheck();
        switch (result)
        {
            case 1: final = first; break;
            case 2: final = second; break;
            case 3: final = third; break;
        }
        GameObject.Find("Face").GetComponent<whoandwhere>().whatiselect = -1;
        GameObject.Find("SelectOne").transform.SetAsFirstSibling();

        return final;
    }
    int setName(int num)
    {
        int st;
        Text text;
        st = GameObject.Find("Deck").GetComponent<deck>().additionalDecklist[num];
        LoadCardDataFromjson(Carddata, st);
        text = GameObject.Find("selectFirst").transform.GetChild(0).GetComponent<Text>();
        text.text = Carddata.name;
        return st;
    }
    int selectionCheck()
    {
        int result = -1;
        StartCoroutine(letsCheck());
        
        result = GameObject.Find("Face").GetComponent<whoandwhere>().whatiselect;
        StopCoroutine(letsCheck());

        return result;
    }
    IEnumerator letsCheck()
    {
        int result = -1;
        while (result == -1)
        {
            result = GameObject.Find("Face").GetComponent<whoandwhere>().whatiselect;
            yield return new WaitForSeconds(.1f);
        }
    }

    void abandonCard()
    {
        GameObject.Find("Deck").GetComponent<deck>().Getoutfromdeck(28);
    }

    void damaged(int a)
    {
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

    void LoadCardDataFromjson(carddata Carddata, int number)
    {
        string path = Path.Combine(Application.dataPath, "card_" + number);
        string jsonData = File.ReadAllText(path);
        Carddata = JsonUtility.FromJson<carddata>(jsonData);
    }

}