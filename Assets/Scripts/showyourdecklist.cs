using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class showyourdecklist : MonoBehaviour
{
    public int how_many_down = 0;

    private void Update()
    {
        howmanycards();
    }

    void howmanycards()
    {
        Text manyText;
        manyText = GameObject.Find("HowMany").GetComponent<Text>();
        manyText.text = GameObject.Find("Deck").GetComponent<deck>().decklist.Count.ToString() + "장";
    }

    public void Up()
    {
        if (how_many_down > 0)
            how_many_down -= 1;
    }
    public void Down()
    {
        if (how_many_down < GameObject.Find("Deck").GetComponent<deck>().decklist.Count - GameObject.Find("Deck").GetComponent<deck>().slot)
            how_many_down += 1;
    }
}
