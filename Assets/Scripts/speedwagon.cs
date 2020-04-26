using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class speedwagon : MonoBehaviour
    , IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        int what = MyName();
        GameObject.Find("BattleManager").GetComponent<battle>().showYourName(what);
    }

    int MyName()
    {
        int name = -1;
        if(gameObject.name == "EnemyCharacterZone")
        {
            name = 28 + GameObject.Find("Deck").GetComponent<deck>().bosslist[GameObject.Find("Deck").GetComponent<whoandwhere>().thefloor];
        }
        else if(gameObject.transform.parent.name == "EnemyCardZone")
        {
            int myorder = int.Parse(gameObject.name.Substring(8, 1));
            if (myorder > GameObject.Find("BattleManager").GetComponent<battle>().bossfield.Count)
                name = -1;
            else
                name = GameObject.Find("BattleManager").GetComponent<battle>().bossfield[myorder - 1 + GameObject.Find("BattleManager").GetComponent<battle>().how_many_boss_right];
        }
        else if (gameObject.name == "MyCharacterZone")
        {
            name = GameObject.Find("Deck").GetComponent<whoandwhere>().number;
        }
        else if (gameObject.transform.parent.name == "MyCardZone")
        {
            int myorder = int.Parse(gameObject.name.Substring(8, 1));
            if (myorder > GameObject.Find("BattleManager").GetComponent<battle>().field.Count)
                name = -1;
            else
                name = GameObject.Find("BattleManager").GetComponent<battle>().field[myorder - 1 + GameObject.Find("BattleManager").GetComponent<battle>().how_many_my_right];
        }
        else if(gameObject.transform.parent.name == "hand")
        {
            name = GameObject.Find("BattleManager").GetComponent<battle>().handlist[gameObject.transform.GetSiblingIndex()];
        }
        return name;
    }
}
