using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class selectCharacter : MonoBehaviour
{
    public enum Character {Rem, Gae, Violet}
    public Character chara;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void Onclick_Rem()
    {
        SceneManager.LoadScene(2);
        chara = Character.Rem;
        GameObject.Find("ChosenCharacter").GetComponent<selectCharacter>().chara = chara;
    }

    public void Onclick_Gae()
    {
        SceneManager.LoadScene(2);
        chara = Character.Gae;
        GameObject.Find("ChosenCharacter").GetComponent<selectCharacter>().chara = chara;
    }

    public void Onclick_Violet()
    {
        SceneManager.LoadScene(2);
        chara = Character.Violet;
        GameObject.Find("ChosenCharacter").GetComponent<selectCharacter>().chara = chara;
    }
}
