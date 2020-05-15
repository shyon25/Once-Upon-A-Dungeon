using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

public class activateCard : MonoBehaviour
    , IPointerClickHandler
{
    public carddata Carddata;

    void Start()
    {
        Carddata = new carddata();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        battle script = GameObject.Find("BattleManager").GetComponent<battle>();
        if (script.currentPhase == battle.phase.battle)
        {
            int myPosition = -1;
            if (script.aiming == false)
            {
                if (gameObject.transform.parent.name == "MyCardZone")
                {
                    myPosition = int.Parse(gameObject.name.Substring(8, 1));
                    if (CanIAttackYou(myPosition))
                    {
                        script.aiming = true;
                        LoadCardDataFromjson(script.field[myPosition - 1 + script.how_many_my_right]);
                        if (Carddata.type == Type.MERCENARY)
                        {
                            script.attacker = myPosition;
                        }
                        else if (Carddata.type == Type.SPELL)
                        {
                            activateSpell(myPosition);
                            script.aiming = false;
                        }
                        else
                            script.aiming = false;
                    }
                }
                else if (gameObject.transform.name == "MyCharacterZone")
                {
                    myPosition = 0;
                    script.aiming = true;
                    if (CanIAttackYou(myPosition))
                        script.attacker = myPosition;
                }
            }
            else
            {
                if (gameObject.transform.parent.name == "EnemyCardZone")
                {
                    myPosition = int.Parse(gameObject.name.Substring(8, 1)) + 6;
                    script.aiming = false;
                }
                else if (gameObject.transform.name == "EnemyCharacterZone")
                {
                    myPosition = 6;
                    script.aiming = false;
                }
                if (script.aiming == false)
                {
                    if (CanYouAttackedbyMe(myPosition))
                        script.fight(script.attacker, myPosition);
                    else
                        script.aiming = false;
                }
            }
        }
    }

    bool CanIAttackYou(int position)
    {
        battle script = GameObject.Find("BattleManager").GetComponent<battle>();
        bool yes = false;
        if (position-1 < script.field.Count || position == 0)
        {
            switch (position)
            {
                case 0:
                    yes = script.Board.action_P;
                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    LoadCardDataFromjson(script.field[position - 1 + script.how_many_my_right]);
                    yes = script.Board.action_PF[position - 1];
                    break;
            }
        }
        return yes;
    }
    bool CanYouAttackedbyMe(int position)
    {
        battle script = GameObject.Find("BattleManager").GetComponent<battle>();
        bool yes = false;
        if (position-7 < script.bossfield.Count || position == 6)
        {
            switch (position)
            {
                case 6:
                    if(script.bossfield.Count <= 0)
                        yes = true;
                    break;
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                    LoadCardDataFromjson(script.bossfield[position - 7 + script.how_many_boss_right]);
                    if (Carddata.type == Type.MERCENARY)
                        yes = script.Board.action_EF[position - 7];
                    break;
            }
        }
        return yes;
    }

    void activateSpell(int position)
    {
        battle script = GameObject.Find("BattleManager").GetComponent<battle>();
        script.field.RemoveAt(position-1+script.how_many_my_right);
        script.Board.action_PF.RemoveAt(position-1+script.how_many_my_right);
        script.Board.current_PF.RemoveAt(position-1+ script.how_many_my_right);
        script.Board.max_PF.RemoveAt(position - 1 + script.how_many_my_right);
    }

    void LoadCardDataFromjson(int number)
    {
        string path = Path.Combine(Application.dataPath, "card_" + number);
        string jsonData = File.ReadAllText(path);
        Carddata = JsonUtility.FromJson<carddata>(jsonData);
    }
}
