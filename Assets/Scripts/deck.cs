using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deck : MonoBehaviour
{
    public List<int> decklist;
    public int slot = 8;
    public List<int> bosslist;
    public List<int> additionalDecklist;
    public int how_many_draw;

    // Start is called before the first frame update
    void Start()
    {
        decklist = new List<int>();
        bosslist = new List<int>();
        additionalDecklist = new List<int>();
        initialdeck();
        initialboss();
        initialadd();
        how_many_draw = 0;
        
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        decklist.Sort();
    }

    public void Intodeck(int a)
    {
        decklist.Add(a);
    }

    public void Getoutfromdeck(int a)
    {
        decklist.Remove(a);
    }

    void initialdeck()
    {
        int personalCard = -1;

        switch (GameObject.Find("ChosenCharacter").GetComponent<selectCharacter>().chara)
        {
            case selectCharacter.Character.Rem:
                personalCard = 10;
                break;
            case selectCharacter.Character.Gae:
                personalCard = 14;
                break;
            case selectCharacter.Character.Violet:
                personalCard = 15;
                break;
        }
        Intodeck(personalCard);
        Intodeck(20);
        Intodeck(21);
        Intodeck(22);
        Intodeck(23);
        Intodeck(24);
        Intodeck(25);
        Intodeck(26);
        Intodeck(27);
        Intodeck(28);
    }
    void initialboss()
    {
        bosslist.Add(1);
        while(bosslist.Count < 5)
        {
            int rand = Random.Range(2,6);
            if(!bosslist.Contains(rand))
                bosslist.Add(rand);
        }
        bosslist.Add(6);
    }
    void initialadd()
    {
        for (int i = 50; i < 75; i++)
        {
            for(int j = 0; j < 5; j++)
                additionalDecklist.Add(i);
        }
        additionalDecklist.Remove(51);
        additionalDecklist.Remove(51);
        additionalDecklist.Remove(53);
        additionalDecklist.Remove(53);
        additionalDecklist.Remove(53);
        additionalDecklist.Remove(56);
        additionalDecklist.Remove(56);
        additionalDecklist.Remove(59);
        additionalDecklist.Remove(59);
        additionalDecklist.Remove(59);


        additionalDecklist.Remove(60);
        additionalDecklist.Remove(60);
        additionalDecklist.Remove(60);
        additionalDecklist.Remove(63);
        additionalDecklist.Remove(63);
        additionalDecklist.Remove(66);
        additionalDecklist.Remove(66);
        additionalDecklist.Remove(69);
        additionalDecklist.Remove(69);
        additionalDecklist.Remove(69);


        additionalDecklist.Remove(70);
        additionalDecklist.Remove(71);
        additionalDecklist.Remove(72);
        additionalDecklist.Remove(73);
        additionalDecklist.Remove(74);
        additionalDecklist.Remove(74);
        additionalDecklist.Add(75);

        shuffle(additionalDecklist);
        
    }
    void shuffle(List<int> list)
    {
        int shuffling = list.Count;
        int temp;
        int rand;
        while(shuffling > 1)
        {
            temp = list[shuffling-1];
            rand = Random.Range(0, shuffling);
            list[shuffling-1] = list[rand];
            list[rand] = temp;
                
            shuffling--;
        }
    }
}
