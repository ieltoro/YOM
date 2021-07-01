using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ClientGameSetup : MonoBehaviour
{
    YOMNetworkManager manager;

    [Tooltip("0 = Waiting \n 1 = Vote \n 2 = MinigameVote")]
    [SerializeField] GameObject[] canvas;
    public bool dead;


    private void Start()
    {
        manager = FindObjectOfType<YOMNetworkManager>();
    }

    public void ChangeUi(int nr)
    {
        if (ClientSaveGame.csg.dead)
            return;
        foreach (GameObject g in canvas)
        {
            g.SetActive(false);
        }
        canvas[nr].SetActive(true);
    }

    #region Vote

    [Header("Vote")]
    public InputField voteinput;
    public Text balanceText;
    int balance;

    public void VoteStart(int b)
    {
        voteinput.text = "0";
        balance = b;
        balanceText.text = balance.ToString();
        ChangeUi(1);
    }
    public void PressedVoteSubmit()
    {
        int i = int.Parse(voteinput.text);
        if (i > balance)
        {
            print("Cant use more balance then you have");
            return;
        }
        ChangeUi(0);
    }

    #endregion
    #region Minigame
    public string minigame1, minigame2;
    [SerializeField] Text miniText1, miniText2;
    public void StartMinigameVote(string c1, string c2)
    {
        miniText1.text = c1;
        miniText2.text = c2;
        ChangeUi(2);
    }
    public void MinigameVoted(int i)
    {
        ChangeUi(0);
    }

    #endregion
    #region Chat

    [Header("Chat")]
    [SerializeField] InputField chatInput;

    public void PressedSend(PlayerScript reciever)
    {

    }

    #endregion
}
