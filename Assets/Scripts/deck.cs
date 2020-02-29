using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deck : MonoBehaviour
{
    public List<int> decklist;
    public int slot = 8;

    // Start is called before the first frame update
    void Start()
    {
        decklist = new List<int>();
        Initialdeck();
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

    void Initialdeck()
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
}
