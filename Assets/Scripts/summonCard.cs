using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class summonCard : MonoBehaviour
    , IPointerClickHandler
{

    public void OnPointerClick(PointerEventData eventData)
    {
        int index = gameObject.transform.GetSiblingIndex();

        if (canISummon())
        {
            GameObject.Find("BattleManager").GetComponent<battle>().summon_card(index);
            Destroy(gameObject);
        }
        else
            GameObject.Find("hand").GetComponent<hand>().makeYouBig();
    }

    bool canISummon()
    {
        return GameObject.Find("BattleManager").GetComponent<battle>().currentPhase == battle.phase.player && GameObject.Find("hand").GetComponent<hand>().handState == hand.state.big;
    }
}
