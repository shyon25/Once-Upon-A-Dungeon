using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class battle : MonoBehaviour
{
    public List<int> deck;
    public List<int> bossdeck;
    public enum phase { start, draw, player, battle, end }
    public phase currentPhase;

    // Start is called before the first frame update
    void Start()
    {
        deck = new List<int>();
        deck = GameObject.Find("Deck").GetComponent<deck>().decklist;
        addBossUniqueCard();
        bossdeck = new List<int>();
        bossdeck = GameObject.Find("Deck").GetComponent<deck>().bossDecklist;
        currentPhase = phase.start;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
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

    public void emergencyCall()
    {
        SceneManager.LoadScene(2);
        GameObject.Find("Deck").GetComponent<whoandwhere>().upFloor();
        GameObject.Find("Deck").GetComponent<whoandwhere>().battleTime = false;
    }
}
