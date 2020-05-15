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

    public board Board;
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
    public bool aiming = false;
    public int attacker;

    public GameObject card;
    public Texture cardZone;

    // Start is called before the first frame update
    void Start()
    {
        deck = new List<int>(GameObject.Find("Deck").GetComponent<deck>().decklist);
        shuffle(deck);
        addBossUniqueCard();
        bossdeck = new List<int>(GameObject.Find("Deck").GetComponent<deck>().bossDecklist);
        shuffle(bossdeck);

        handlist = new List<int>();
        grave = new List<int>();
        bossgrave = new List<int>();
        bossfield = new List<int>();
        field = new List<int>();
        Carddata = new carddata();
        LoadCardDataFromjson(GameObject.Find("Deck").GetComponent<whoandwhere>().number);
        Board = new board();

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
        attacker = -1;

    }

    public void showYourName(int n)
    {
        if (n != -1)
        {
            LoadCardDataFromjson(n);
            Text effect = GameObject.Find("speedwagonZone").transform.GetChild(1).transform.GetChild(0).GetComponent<Text>();
            effect.text = "비용: " + Carddata.cost + " " + Carddata.name + "\n" + "Atk : " + Carddata.atk + ", Hp : " + Carddata.hp + "\n" + Carddata.effect;
        }
    }

    void CharactersName()
    {
        Text EText = GameObject.Find("EnemyCharacterZone").transform.GetChild(0).GetComponent<Text>();
        Text MText = GameObject.Find("MyCharacterZone").transform.GetChild(0).GetComponent<Text>();

        int currentBoss = GameObject.Find("Deck").GetComponent<deck>().bosslist[GameObject.Find("Deck").GetComponent<whoandwhere>().thefloor-1];
        LoadCardDataFromjson(currentBoss + 28);
        EText.text = Carddata.name + " " + Carddata.atk + " / " + Carddata.hp;
        Board.current_E = new carddata(Carddata);
        Board.max_E = new carddata(Carddata);
        int currentP = GameObject.Find("Deck").GetComponent<whoandwhere>().number;
        LoadCardDataFromjson(currentP);
        MText.text = Carddata.name + " " + Carddata.atk + " / " + Carddata.hp;
        Board.current_P = new carddata(Carddata);
        Board.max_P = new carddata(Carddata);
        Board.current_P.hp = GameObject.Find("Deck").GetComponent<whoandwhere>().currentHP;
    }

    // Update is called once per frame
    void Update()
    {
        setHand();
        setBossField();
        setField();
        updateLife();
    }

    void updateLife()
    {
        Text text = GameObject.Find("BossLife").GetComponent<Text>();
        text.text = Board.current_E.hp.ToString() + " / " + Board.max_E.hp.ToString();
        text = GameObject.Find("MyLife").GetComponent<Text>();
        text.text = Board.current_P.hp.ToString() + " / " + Board.max_P.hp.ToString();
    }

    void setBossField()
    {
        for (int i = 0; i < Mathf.Min(bossfield.Count-how_many_boss_right, slot); i++)
        {
            LoadCardDataFromjson(bossfield[i + how_many_boss_right]);
            RawImage cardImage = GameObject.Find("EnemyCardZone").transform.GetChild(i).GetComponent<RawImage>();
            cardImage.texture = card.GetComponent<RawImage>().texture;
            Text cardText = GameObject.Find("EnemyCardZone").transform.GetChild(i).transform.GetChild(0).GetComponent<Text>();
            cardText.text = Carddata.name + " / " + Carddata.cost + " / " + Board.current_EF[i+how_many_boss_right].atk + " / " + Board.current_EF[i+how_many_boss_right].hp;
            //yield return new WaitForSeconds(0.01f);
        }
        for (int i = bossfield.Count-how_many_boss_right; i < slot; i++)
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
        for (int i = 0; i < Mathf.Min(field.Count-how_many_my_right,slot); i++)
        {
            LoadCardDataFromjson(field[i + how_many_my_right]);
            RawImage cardImage = GameObject.Find("MyCardZone").transform.GetChild(i).GetComponent<RawImage>();
            cardImage.texture = card.GetComponent<RawImage>().texture;
            Text cardText = GameObject.Find("MyCardZone").transform.GetChild(i).transform.GetChild(0).GetComponent<Text>();
            cardText.text = Carddata.name + " / " + Carddata.cost + " / " + Board.current_PF[i+how_many_my_right].atk + " / " + Board.current_PF[i+how_many_my_right].hp;
            //yield return new WaitForSeconds(0.01f);
        }
        for (int i = field.Count-how_many_my_right; i < slot; i++)
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
            recover();
        }
    }
    void recover()
    {
        Board.action_E = true;
        Board.action_P = true;
        for (int i = 0; i < Board.action_PF.Count; i++)
        {
            Board.action_PF[i] = true;
            Board.current_PF[i].hp = Board.max_PF[i].hp;
        }
        for (int i = 0; i < Board.action_EF.Count; i++)
        {
            Board.action_EF[i] = true;
            Board.current_EF[i].hp = Board.max_EF[i].hp;
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
            summon_Enemy();
            yield return new WaitForSeconds(0.1f);
        }
    }
    void summon_Enemy()
    {
        bossfield.Add(bossdeck[0]);
        LoadCardDataFromjson(bossdeck[0]);
        carddata Temp = new carddata(Carddata);
        Board.current_EF.Add(Temp);
        Board.max_EF.Add(Temp);
        Board.action_EF.Add(true);
        bossdeck.RemoveAt(0);
        enemyField++;
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
            enemyDone = false;
            StartCoroutine("EnemyAttackTime");
            currentPhase = phase.end;
        }
    }

    bool enemyDone = false;

    IEnumerator EnemyAttackTime()
    {
        while (enemyDone == false)
        {
            enemyAttack();
            yield return new WaitForSeconds(1f);
        }
        currentPhase = phase.start;
        recover();
    }
    void enemyAttack()
    {
        int i = 0;
        for (i = 0; i < bossfield.Count; i++)
        {
            if(Board.current_EF[i].type == Type.SPELL)
            {
                activateEnemySpell(i);
            }
        }

        for (i = 0; i < bossfield.Count; i++)
        {
            if (i > slot)
                how_many_boss_right += 1;
            if(Board.action_EF[i] == true && Board.current_EF[i].type == Type.MERCENARY)
            {
                whoisMyRival(i+7);
                break;
            }
            
        }
        if(i >= bossfield.Count)
        {
            whoisMyRival(6);
            enemyDone = true;
        }

    }
    void whoisMyRival(int attack)
    {
        int rival = 0;
        
        for (int i = 0; i < field.Count; i++)
        {
            if(Board.current_PF[i].type == Type.MERCENARY);
            {
                rival = i+1;
                break;
            }
        }
        if (field.Count == 0)
        {
            rival = 0;
        }
        fight(attack, rival);
    }

    void activateEnemySpell(int i)
    {
        bossfield.RemoveAt(i + how_many_boss_right);
        Board.action_EF.RemoveAt(i + how_many_boss_right);
        Board.current_EF.RemoveAt(i + how_many_boss_right);
        Board.max_EF.RemoveAt(i + how_many_boss_right);
    }

    void attacklog(int attack, int defense)
    {
        string attacker = "";
        string defenser = "";
        switch (attack)
        {
            case 0:
                LoadCardDataFromjson(GameObject.Find("Deck").GetComponent<whoandwhere>().number);
                attacker = Carddata.name;
                break;
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                LoadCardDataFromjson(field[attack - 1]);
                attacker = Carddata.name;
                break;
            case 6:
                LoadCardDataFromjson(GameObject.Find("Deck").GetComponent<deck>().bosslist[GameObject.Find("Deck").GetComponent<whoandwhere>().thefloor-1] + 28);
                attacker = Carddata.name;
                break;
            case 7:
            case 8:
            case 9:
            case 10:
            case 11:
                LoadCardDataFromjson(bossfield[attack - 7]);
                attacker = Carddata.name;
                break;
        }
        switch (defense)
        {
            case 0:
                LoadCardDataFromjson(GameObject.Find("Deck").GetComponent<whoandwhere>().number);
                defenser = Carddata.name;
                break;
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                LoadCardDataFromjson(field[defense - 1]);
                defenser = Carddata.name;
                break;
            case 6:
                LoadCardDataFromjson(GameObject.Find("Deck").GetComponent<deck>().bosslist[GameObject.Find("Deck").GetComponent<whoandwhere>().thefloor - 1] + 28);
                defenser = Carddata.name;
                break;
            case 7:
            case 8:
            case 9:
            case 10:
            case 11:
                LoadCardDataFromjson(bossfield[defense - 7]);
                defenser = Carddata.name;
                break;
        }

        Debug.Log(attacker + "가 " + defenser + "를 공격.");
    }

    public void fight(int attack, int defense)
    {
        attacklog(attack, defense);
        
        //내 캐릭터 = 0, 상대 캐릭터 = 6
        //내 필드 = 1~5, 상대 필드 = 7~11
        DataCall(attack);
        carddata A = new carddata(Carddata);
        DataCall(defense);
        carddata D = new carddata(Carddata);
        before_effect_A(A, D, attack, defense);
        before_effect_D(A, D, attack, defense);

        switch (attack)
        {
            case 0:
                if (defense == 6)
                {
                    int dice = Random.Range(1, 7);
                    Debug.Log("Dice = " + dice);
                    if (Board.current_P.atk >= dice)
                        Board.current_E.hp -= 1;
                    if (Board.current_E.hp <= 0)
                        emergencyCall();
                }
                else
                {
                    Board.current_EF[defense - 7 + how_many_boss_right].hp -= Board.current_P.atk;
                    if (Board.current_EF[defense - 7 + how_many_boss_right].hp <= 0)
                        destroyit(defense);
                }
                Board.action_P = false;
                break;
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                if (defense == 6)
                {
                    int dice = Random.Range(1, 7);
                    Debug.Log("Dice = " + dice);
                    if (Board.current_PF[attack - 1 + how_many_my_right].atk >= dice)
                        Board.current_E.hp -= 1;
                    if (Board.current_E.hp <= 0)
                        emergencyCall();
                }
                else
                {
                    Board.current_EF[defense - 7 + how_many_boss_right].hp -= Board.current_PF[attack - 1 + how_many_my_right].atk;
                    if (Board.current_EF[defense - 7 + how_many_boss_right].hp <= 0)
                        destroyit(defense);
                }
                Board.action_PF[attack-1+how_many_my_right] = false;
                break;
            case 6:
                if (defense == 0)
                {
                    int dice = Random.Range(1, 7);
                    Debug.Log("Dice = " + dice);
                    if (Board.current_E.atk >= dice)
                        Board.current_P.hp -= Board.current_E.atk;

                    if (Board.current_P.hp <= 0)
                    {
                        Debug.Log("GameOver");
                    }
                }
                else
                {
                    Board.current_PF[defense - 1 + how_many_my_right].hp -= Board.current_E.atk;
                    if (Board.current_PF[defense - 1 + how_many_my_right].hp <= 0)
                        destroyit(defense);
                }
                Board.action_E = false;
                break;
            case 7:
            case 8:
            case 9:
            case 10:
            case 11:
                if (defense == 0)
                {
                    int dice = Random.Range(1, 7);
                    Debug.Log("Dice = " + dice);
                    if (Board.current_EF[attack - 7 + how_many_boss_right].atk >= dice)
                        Board.current_P.hp -= 1;

                    if (Board.current_P.hp <= 0)
                    {
                        Debug.Log("GameOver");
                    }
                }
                else
                {
                    Board.current_PF[defense - 1 + how_many_my_right].hp -= Board.current_EF[attack - 7 + how_many_boss_right].atk;
                    if (Board.current_PF[defense - 1 + how_many_my_right].hp <= 0)
                        destroyit(defense);
                }
                Board.action_EF[attack-7 + how_many_boss_right] = false;
                break;
        }

        if (D.hp > 0)
            after_effect_D(A, D, attack, defense);
        if (A.hp > 0)
            after_effect_A(A, D, attack, defense);
    }
    void destroyit(int defensed)
    {
        if (defensed < 6)
        {
            field.RemoveAt(defensed - 1 + how_many_my_right);
            Board.current_PF.RemoveAt(defensed - 1 + how_many_my_right);
            Board.max_PF.RemoveAt(defensed - 1 + how_many_my_right);
            Board.action_PF.RemoveAt(defensed - 1 + how_many_my_right);
            
        }
        else if (defensed > 6)
        {
            bossfield.RemoveAt(defensed - 7 + how_many_boss_right);
            Board.current_EF.RemoveAt(defensed - 7 + how_many_boss_right);
            Board.max_EF.RemoveAt(defensed - 7 + how_many_boss_right);
            Board.action_EF.RemoveAt(defensed - 7 + how_many_boss_right);
            enemyField--;
        }
    }
    void DataCall(int n)
    {
        int yourCard = 0;
        if(n == 0)
        {
            yourCard = GameObject.Find("Deck").GetComponent<whoandwhere>().number;
        }
        else if(n == 6)
        {
            yourCard = GameObject.Find("Deck").GetComponent<deck>().bosslist[GameObject.Find("Deck").GetComponent<whoandwhere>().thefloor];
        }
        else if(n > 0 && n < 6)
        {
            yourCard = field[n - 1 + how_many_my_right];
        }
        else if(n > 6 && n < 12)
        {
            yourCard = bossfield[n - 7 + how_many_boss_right];
        }
        LoadCardDataFromjson(yourCard);
    }

    void before_effect_A(carddata attack, carddata defense, int a, int d)
    {
        switch (a)
        {

        }

    }
    void before_effect_D(carddata attack, carddata defense, int a, int d)
    {

    }
    void after_effect_D(carddata attack, carddata defense, int a, int d)
    {

    }
    void after_effect_A(carddata attack, carddata defense, int a, int d)
    {
        switch (a)
        {
            case 0:
                int myName = GameObject.Find("Deck").GetComponent<whoandwhere>().number;

                switch (myName)
                {
                    case 0:
                        rem();
                        break;
                }

                break;
        }

        void rem()
        {

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

        summon_mine(thecard);
        
        handlist.RemoveAt(num);
    }
    void summon_mine(int thecard)
    {
        field.Add(thecard);
        LoadCardDataFromjson(thecard);
        carddata Temp = new carddata(Carddata);
        Board.current_PF.Add(Temp);
        Board.max_PF.Add(Temp);
        Board.action_PF.Add(true);
    }
}
[System.Serializable]
public class board
{
    public carddata current_E;
    public carddata current_P;
    public List<carddata> current_EF;
    public List<carddata> current_PF;
    public carddata max_E;
    public carddata max_P;
    public List<carddata> max_EF;
    public List<carddata> max_PF;

    public bool action_E;
    public bool action_P;
    public List<bool> action_EF;
    public List<bool> action_PF;

    public board()
    {
        current_EF = new List<carddata>();
        current_PF = new List<carddata>();
        max_EF = new List<carddata>();
        max_PF = new List<carddata>();

        action_E = true;
        action_P = true;
        action_EF = new List<bool>();
        action_PF = new List<bool>();
    }
}
