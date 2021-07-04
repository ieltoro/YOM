using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ClientGameSetup : MonoBehaviour
{
    YOMNetworkManager manager;

    [Tooltip("0 = Waiting \n 1 = Vote \n 2 = MinigameVote")]
    [SerializeField] GameObject[] canvas;
    PlayerScript ps;
    public bool dead;
    bool voted;

    private void Start()
    {
        manager = FindObjectOfType<YOMNetworkManager>();
        ps = ClientSaveGame.csg.localPlayer.GetComponent<PlayerScript>();
        balance = ps.votesBalance;
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

    public void EnableVote(bool a)
    {
        canvas[0].SetActive(false);
        if (!voted)
            canvas[1].SetActive(a);
    }
    public void EnableShop(bool a)
    {
        print("Shop " + a);
        canvas[0].SetActive(false);
    }
    #region Vote

    [Header("Vote")]
    public InputField voteinput;
    public Text balanceText;
    int balance;
    public void UpdateBalance(int amount)
    {
        balance = amount;
    }
    public void PressedVoteSubmit()
    {
        int i = int.Parse(voteinput.text);
        if (i > balance)
        {
            print("Cant use more balance then you have");
            return;
        }
        voted = true;
        canvas[1].SetActive(false);
        ps.VotesCasted(i);
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
