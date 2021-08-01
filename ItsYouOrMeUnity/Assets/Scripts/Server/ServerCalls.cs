using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class ServerCalls : NetworkBehaviour
{
    #region Setup and stuff

    public static ServerCalls sc;
    ClientGameSetup cgs;

    private void Awake()
    {
        if (ServerCalls.sc == null)
        {
            ServerCalls.sc = this;
        }
        else
        {
            if (ServerCalls.sc != this)
            {
                Destroy(this);
            }
            //if (ClientGameSetup.cgs != this)
            //{
            //    Destroy(ClientGameSetup.cgs.gameObject);
            //    ClientGameSetup.cgs = this;
            //}
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void ChangeScene(string load)
    {
        RPC_StartingGame(load);
    }
    [ClientRpc]
    void RPC_StartingGame(string game)
    {
        SceneManager.LoadScene(game + " Phone");
    }
  
    public void SendInfo(int value)
    {
        RPC_SendInfo(value);
    }
    [ClientRpc]
    void RPC_SendInfo(int i)
    {
        if(cgs == null)
            cgs = FindObjectOfType<ClientGameSetup>();
                       
        if (i == 2) // Waiting
        {
            FindObjectOfType<YOMNetworkManager>().playing = true;
            cgs.ChangeUi(0);
        }
        if (i == 3) // VoteRound
        {
            cgs.ChangeUi(1);
        }
    }
   
    public void SendMiniGames(string i1, string i2)
    {
        print("1");
        RPC_RecieveMiniGames(i1, i2);
    }
    [ClientRpc]
    void RPC_RecieveMiniGames(string i1, string i2)
    {
        print("2");
        FindObjectOfType<ClientGameSetup>().StartMinigameVote(i1, i2);
    }
     
    public void ReturnFromMiniGame(int mode)
    {
        RPC_ReturnFromMiniGame(mode);
    }
    [ClientRpc]
    void RPC_ReturnFromMiniGame(int mode)
    {
        if(mode == 0)
        {
            SceneManager.LoadScene("Game Phone");
        }
        
    }
 
    #endregion
    #region Minigames
    #region Battleship

    public void StartBattleship(int x, int y)
    {
        RPC_StartBattleship(x,y);
    }
    [ClientRpc]
    void RPC_StartBattleship(int x, int y)
    {
        FindObjectOfType<BattleshipClient>().SetupGameboard(x,y);
    }
    public void TimesOutToSetUpShips()
    {
        RPC_TimesOutToSetUpShips();
    }
    [ClientRpc]
    void RPC_TimesOutToSetUpShips()
    {
        FindObjectOfType<BattleshipClient>().TimeIsOut();
    }
    public void TimeToBomb()
    {
        RPC_TimeToBomb();
    }
    [ClientRpc]
    void RPC_TimeToBomb()
    {
        FindObjectOfType<BattleshipClient>().BombTime();
    }

    #endregion
    #endregion

}

