using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class battle : MonoBehaviour
{
    public List<int> deck;
    public List<int> bossdeck;
    public List<int> handlist;
    public List<int> grave;
    public List<int> bossgrave;

    public carddata Carddata;
    public List<int> bossfield;
    public List<int> field;

    public enum phase { start, draw, player, battle, end }
    public phase currentPhase;
    public int handlimit;
    public float currentHand;
    public float currentBossField;
    public float currentField;
    public int enemyFieldLimit;
    public int enemyField;
    public int how_many_boss_right;
    public int how_many_my_right;
    public int slot = 5;

    public GameObject card;
    public Texture cardZone;

    // Start is called before the first frame update
    void Start()
    {
        deck = new List<int>(GameObject.Find("Deck").GetComponent<deck>().decklist);
        //deck = ;
        shuffle(deck);
        addBossUniqueCard();
        bossdeck = new List<int>(GameObject.Find("Deck").GetComponent<deck>().bossDecklist);
        //bossdeck = ;
        shuffle(bossdeck);

        handlist = new List<int>();
        grave = new List<int>();
        bossgrave = new List<int>();
        bossfield = new List<int>();
        field = new List<int>();
        Carddata = new carddata();

        handlimit = 5;
        currentPhase = phase.start;
        currentHand = 0;
        currentBossField = 0;
        currentField = 0;
        enemyFieldLimit = GameObject.Find("Deck").GetComponent<whoandwhere>().thefloor + 1;
        enemyField = 0;
        how_many_boss_right = 0;
        how_many_my_right = 0;
        CharactersName();

    }

    void CharactersName()
    {
        Text EText = GameObject.Find("EnemyCharacterZone").transform.GetChild(0).GetComponent<Text>();
        Text MText = GameObject.Find("MyCharacterZone").transform.GetChild(0).GetComponent<Text>();

        int currentBoss = GameObject.Find("Deck").GetComponent<deck>().bosslist[GameObject.Find("Deck").GetComponent<whoandwhere>().thefloor];
        EText.text = bossname(currentBoss);
        int currentP = GameObject.Find("Deck").GetComponent<whoandwhere>().number;
        LoadCardDataFromjson(currentP);
        MText.text = Carddata.name + " " + Carddata.atk + " / " + Carddata.hp;
    }

    string bossname(int num)
    {
        string name = "";
        switch (num)
        {
            case 1: name = "가이드 요정" + " 1 / 1"; break;
            case 2: name = "유령" + " 3 / 10"; break;
            case 3: name = "과학자" +  " 4 / 7"; break;
            case 4: name = "안개" + " 3 / 15"; break;
            case 5: name = "해골" + " 1 / 1"; break;
            case 6: name = "마왕" + " 4 / 20"; break;
        }
        return name;
    }

    // Update is called once per frame
    void Update()
    {
        setHand();
        setBossField();
        setField();
    }
    void setBossField()
    {
        for (int i = 0; i < Mathf.Min(bossfield.Count, slot); i++)
        {
            LoadCardDataFromjson(bossfield[i + how_many_boss_right]);
            RawImage cardImage = GameObject.Find("EnemyCardZone").transform.GetChild(i).GetComponent<RawImage>();
            cardImage.texture = card.GetComponent<RawImage>().texture;
            Text cardText = GameObject.Find("EnemyCardZone").transform.GetChild(i).transform.GetChild(0).GetComponent<Text>();
            cardText.text = Carddata.name + " / " + Carddata.cost + " / " + Carddata.atk + " / " + Carddata.hp;
            //yield return new WaitForSeconds(0.01f);
        }
        for (int i = bossfield.Count; i < slot; i++)
        {
            RawImage cardImage = GameObject.Find("EnemyCardZone").transform.GetChild(i).GetComponent<RawImage>();
            cardImage.texture = cardZone;
            Text cardText = GameObject.Find("EnemyCardZone").transform.GetChild(i).transform.GetChild(0).GetComponent<Text>();
            cardText.text = "";
            //yield return new WaitForSeconds(0.01f);
        }
    }

    void setField()
    {
        for (int i = 0; i < Mathf.Min(field.Count,slot); i++)
        {
            LoadCardDataFromjson(field[i + how_many_my_right]);
            RawImage cardImage = GameObject.Find("MyCardZone").transform.GetChild(i).GetComponent<RawImage>();
            cardImage.texture = card.GetComponent<RawImage>().texture;
            Text cardText = GameObject.Find("MyCardZone").transform.GetChild(i).transform.GetChild(0).GetComponent<Text>();
            cardText.text = Carddata.name + " / " + Carddata.cost + " / " + Carddata.atk + " / " + Carddata.hp;
            //yield return new WaitForSeconds(0.01f);
        }
        for (int i = field.Count; i < slot; i++)
        {
            RawImage cardImage = GameObject.Find("MyCardZone").transform.GetChild(i).GetComponent<RawImage>();
            cardImage.texture = cardZone;
            Text cardText = GameObject.Find("MyCardZone").transform.GetChild(i).transform.GetChild(0).GetComponent<Text>();
            cardText.text = "";
            //yield return new WaitForSeconds(0.01f);
        }
    }

    void setHand()
    {
        makeCard();
        destroyCard();
        nameCard();
        joolsue();
    }
    void makeCard()
    {
        if (currentHand < handlist.Count)
        {
            GameObject instantObject;
            instantObject = Instantiate(card);
            instantObject.transform.SetParent(GameObject.Find("hand").transform);
            currentHand++;
        }
    }
    void destroyCard()
    {
        if (currentHand > handlist.Count)
        {
            currentHand--;
        }
    }
    void nameCard()
    {
        for (int i = 0; i < currentHand; i++)
        {
            LoadCardDataFromjson(handlist[i]);
            Text text = GameObject.Find("hand").transform.GetChild(i).transform.GetChild(0).GetComponent<Text>();
            text.text = Carddata.name;
        }
    }
    void joolsue()
    {
        GameObject nowHand;
        for (int i = 0; i < currentHand; i++)
        {
            float scale = 1 / currentHand;
            if (currentHand < 5)
                scale = 0.2f;
            nowHand = GameObject.Find("hand").transform.GetChild(i).gameObject;
            nowHand.GetComponent<RectTransform>().anchorMin = new Vector2(i * scale, 0f);
            nowHand.GetComponent<RectTransform>().anchorMax = new Vector2((i + 1) * scale, 1f);
            nowHand.GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);
            nowHand.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
            nowHand.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
        }
    }

    public void letsStart()
    {
        if(currentPhase == phase.end)
        {
            currentPhase = phase.start;
        }
    }

    public void letsDraw()
    {
        if (currentPhase == phase.start)
        {
            currentPhase = phase.draw;
            StartCoroutine("drawit");
        }
    }
    IEnumerator drawit()
    {
        while (handlist.Count < handlimit && deck.Count != 0)
        {
            handlist.Add(deck[0]);
            deck.RemoveAt(0);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void letsPlayer()
    {
        if (currentPhase == phase.draw)
        {
            currentPhase = phase.player;
            StartCoroutine("enemyReady");
        }
    }
    IEnumerator enemyReady()
    {
        while (enemyField < enemyFieldLimit && bossdeck.Count != 0)
        {
            bossfield.Add(bossdeck[0]);
            bossdeck.RemoveAt(0);
            enemyField++;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void letsBattle()
    {
        if (currentPhase == phase.player)
        {
            currentPhase = phase.battle;
        }
    }

    public void letsEnd()
    {
        if (currentPhase == phase.battle)
        {
            currentPhase = phase.end;
        }
    }

    //addcard
    void addBossUniqueCard()
    {
        int theFloor = GameObject.Find("Deck").GetComponent<whoandwhere>().thefloor;
        int boss = GameObject.Find("Deck").GetComponent<deck>().bosslist[theFloor-1];
        for (int i = 0; i<3; i++)
            GameObject.Find("Deck").GetComponent<deck>().bossDecklist.Add(bossCard(boss)+i);
    }
    int bossCard(int boss)
    {
        return 29 + boss * 3;
    }
    void shuffle(List<int> list)
    {
        int shuffling = list.Count;
        int temp;
        int rand;
        while (shuffling > 1)
        {
            temp = list[shuffling - 1];
            rand = Random.Range(0, shuffling);
            list[shuffling - 1] = list[rand];
            list[rand] = temp;

            shuffling--;
        }
    }
    //test
    public void emergencyCall()
    {
        SceneManager.LoadScene(2);
        GameObject.Find("Deck").GetComponent<whoandwhere>().upFloor();
        GameObject.Find("Deck").GetComponent<whoandwhere>().battleTime = false;
    }
    void LoadCardDataFromjson(int number)
    {
        string path = Path.Combine(Application.dataPath, "card_" + number);
        string jsonData = File.ReadAllText(path);
        Carddata = JsonUtility.FromJson<carddata>(jsonData);
    }
    //field button
    public void bossright()
    {
        if(bossfield.Count - how_many_boss_right > slot)
            how_many_boss_right++;
    }
    public void bossleft()
    {
        if (how_many_boss_right > 0)
            how_many_boss_right--;
    }
    public void myright()
    {
        if (field.Count - how_many_my_right > slot)
            how_many_my_right++;
    }
    public void myleft()
    {
        if (how_many_my_right > 0)
            how_many_my_right--;
    }

    //battle
    public void summon_card(int num)
    {
        int thecard = handlist[num];

        field.Add(thecard);
        handlist.RemoveAt(num);
    }
}
