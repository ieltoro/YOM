using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSetup : MonoBehaviour
{
    public static GameSetup gs;
    GameSaveHolder save;
    ServerCalls sc;
    //[SerializeField] PostfxcALLS postfx;
    [SerializeField] Timerhandler time;
    [SerializeField] TransitionSize trans;
    [SerializeField] GameObject background;
    [Tooltip(" 0 = roundstart \n 1 = ChooseMinigame")]
    [SerializeField] GameObject[] UI;
    public static int startPositionIndex;
    public List<Transform> startPositions;

    #region Start
    private void Start()
    {
        if (save == null)
            save = GameSaveHolder.gsh;

        foreach (GameObject g in save.players)
            g.GetComponent<PlayerScript>().SetupGameStart(this);
        votesText.text = "Votes  " + 0 + "/" + save.players.Count;
        if (save.round > 0)
        {
            ReturnFromMiniGame();
            return;
        }
        SetSpawnPosition();
    }
    public void SetSpawnPosition() 
    {
        foreach (GameObject g in save.players)
        {
            g.GetComponent<PlayerScript>().SetSpawn(startPositionIndex);
            startPositionIndex++;
            g.GetComponent<PlayerScript>().SetupGameStart(this);
        }
        StartCoroutine(FirstTimeLaunch());
    } // Called first time launching game
    IEnumerator FirstTimeLaunch()
    {
        stageAnim.SetTrigger("ToStage");
        yield return new WaitForSeconds(1);
        //postfx.ToScene();
        trans.gameObject.SetActive(true);
        trans.TransitionIn();
        background.SetActive(false);

        yield return new WaitForSeconds(7);
        trans.StopTransition();
        //postfx.StopPost();
        sc = ServerCalls.sc;
        save.round++;
        roundNr = save.round;
        roundNrText.text = "R O U N D  " + roundNr.ToString();
        StartCoroutine(StartNewRoundCD());
    }


    public void ReturnFromMiniGame()
    {
        foreach (GameObject g in save.players)
        {
            g.GetComponent<PlayerScript>().SetupGameStart(this);
        }
        roundNr = GameSaveHolder.gsh.round;
        foreach (GameObject g in save.players)
        {
            g.transform.GetChild(0).gameObject.SetActive(true);
        }
        sc = ServerCalls.sc;
        save.round++;
        roundNr = save.round;
        roundNrText.text = "R O U N D  " + roundNr.ToString();
        StartCoroutine(ReturnFromminiGCD());
    }
    IEnumerator ReturnFromminiGCD()
    {
        stageAnim.SetTrigger("Quick");
        //postfx.QuickScene();
        trans.gameObject.SetActive(true);
        trans.TransitionIn();
        background.SetActive(false);
        yield return new WaitForSeconds(2);
        StartCoroutine(StartNewRoundCD());
    }

    IEnumerator StartNewRoundCD()
    {
        UI[0].SetActive(true);
        yield return new WaitForSeconds(1);

        foreach (GameObject g in save.players)
        {
            g.GetComponent<PlayerScript>().TimeToVote();
        }

    }
    #endregion
    #region Vote

    [SerializeField] Text roundNrText, votesText;
    [SerializeField] List<GameObject> lowest, sortorder, finalorder;
    [SerializeField] GameObject secondLowest;
    [SerializeField] GameObject mostVotes;
    int roundNr;
    int votesCasted;
    int amountOut;

    public void PlayerHaveVoted()
    {
        print("New vote recieved");
        votesCasted++;
        votesText.text = "Votes  " + votesCasted + "/" + save.players.Count;
        if (votesCasted == save.players.Count)
        {
            print("All have voted");
            AllVotesCasted();
        }
    }
    public void TimesOutVote()
    {
        time.StartTimer(120 ,0);
    }
    private void AllVotesCasted()
    {
        finalorder.Clear();
        sortorder.Clear();
        time.CancelTimer();
        foreach (GameObject g in save.players)
        {
            if (lowest == null || lowest.Count == 0)
            {
                lowest.Add(g);
            }
            else
            {
                if(mostVotes == null)
                {
                    mostVotes = g;
                }
                else if(mostVotes.GetComponent<PlayerScript>().votesAmount < g.GetComponent<PlayerScript>().votesAmount)
                {
                    mostVotes = g;
                }
                if(g.GetComponent<PlayerScript>().votesAmount < lowest[0].GetComponent<PlayerScript>().votesAmount)
                {
                    secondLowest = lowest[0];
                    lowest[0] = g;
                }
                else if(g.GetComponent<PlayerScript>().votesAmount == lowest[0].GetComponent<PlayerScript>().votesAmount)
                {
                    lowest.Add(g);
                }
                else if (secondLowest == null)
                {
                    secondLowest = g;
                }
                else if(g.GetComponent<PlayerScript>().votesAmount < secondLowest.GetComponent<PlayerScript>().votesAmount)
                {
                    secondLowest = g;
                }
            }
        }
        if(lowest.Count > 1)
        {
            print("We have a draw, more then 1 choosen the lowest nr");
            foreach (GameObject g in save.players)
            {
                if (g.GetComponent<PlayerScript>().hp > 0)
                {
                    sortorder.Add(g);
                }
            }
            foreach(GameObject g in lowest)
            {
                foreach(GameObject f in sortorder)
                {
                    if (g == f)
                        sortorder.Remove(f);
                }
                finalorder.Add(g);
            }
            StartCoroutine(VotesDrawCD());
        }
        if(lowest.Count == 1)
        {
            print("Lowest == 1");
            
            
            foreach (GameObject g in save.players)
            {
                if (g.GetComponent<PlayerScript>().hp > 0 && lowest[0] != g && secondLowest != g)
                {
                    sortorder.Add(g);
                }
            }
            int counter = sortorder.Count;
            for (int i = 0; i < counter; i++)
            {
                int r = Random.Range(0, sortorder.Count);
                finalorder.Add(sortorder[r]);
                sortorder.Remove(sortorder[r]);
            }
                StartCoroutine(VotesResultsCD());
        }
    }
    IEnumerator VotesDrawCD()
    {
        yield return new WaitForSeconds(1);
        foreach(GameObject g in lowest)
        {
            g.GetComponent<PlayerScript>().VotedDraw();
        }
        DrawVoteing(lowest.Count);
    }
    IEnumerator VotesResultsCD()
    {
        yield return new WaitForSeconds(2);

        print("VotesResultsCD");
        stageAnim.SetTrigger("Vote");
      
        yield return new WaitForSeconds(6);
      
        if(finalorder.Count > 0)
        {
            foreach (GameObject g in finalorder)
            {
                g.GetComponent<PlayerScript>().myPlayer.GetComponent<CharacterManager>().lightObj.SetActive(true);
                float r = Random.Range(1.0f, 4.0f);
                yield return new WaitForSeconds(r);
            }
            yield return new WaitForSeconds(2);
            secondLowest.GetComponent<PlayerScript>().myPlayer.GetComponent<CharacterManager>().lightObj.SetActive(true);
        }
        if (finalorder.Count == 0)
        {
            yield return new WaitForSeconds(5);
            secondLowest.GetComponent<PlayerScript>().myPlayer.GetComponent<CharacterManager>().lightObj.SetActive(true);
            lowest[0].GetComponent<PlayerScript>().VotedLeast();

            foreach (GameObject g in save.players)
            {
                if (g.GetComponent<PlayerScript>().hp <= 0)
                {
                    amountOut++;
                }
            }
            if (amountOut == save.players.Count)
            {
                WeHaveAWinner();
                StopAllCoroutines();
            }
        }

        yield return new WaitForSeconds(2);

        foreach (GameObject g in save.players)
        {
            g.GetComponent<PlayerScript>().myPlayer.GetComponent<CharacterManager>().lightObj.SetActive(false);
        }

        yield return new WaitForSeconds(1);

        lowest[0].GetComponent<PlayerScript>().myPlayer.GetComponent<CharacterManager>().lightObj.SetActive(true);
        lowest[0].GetComponent<PlayerScript>().VotedLeast();
        print("You voted least with " + lowest[0].GetComponent<PlayerScript>().votesAmount + " and the most votes used were " + mostVotes.GetComponent<PlayerScript>().votesAmount);

        yield return new WaitForSeconds(3);

        lowest[0].GetComponent<PlayerScript>().myPlayer.GetComponent<CharacterManager>().lightObj.SetActive(false);
        stageAnim.SetTrigger("ToStage");

        yield return new WaitForSeconds(3);
        
        int dead = new int();
        foreach(GameObject g in GameSaveHolder.gsh.players)
        {
            if(g.GetComponent<PlayerScript>().hp == 0)
            {
                dead++;
            }
        }
        if(dead == GameSaveHolder.gsh.players.Count)
        {
            FinalTwo();
        }
        else
        {
            VotePartDone();
        }
        
    }

    private void VotePartDone()
    {
        StartMiniGame();
    }

    #endregion
    #region Duel

    private void DrawVoteing(int amount)
    {
        if (amount == 2)
        {
            mg1 = Random.Range(0, duoMinigames.Length);
            mg2 = Random.Range(0, duoMinigames.Length);
            if (mg2 == mg1)
            {
                int direction = Random.Range(0, 2);
                if (direction == 0)
                {
                    mg2--;
                    if (mg2 < 0)
                        mg2 = minigames.Length - 1;
                }
                if (direction == 1)
                {
                    mg2++;
                    if (mg2 >= minigames.Length)
                        mg2 = 0;
                }
            }

            mgDisplay1.SetMinigame(duoMinigames[mg1]);
            mgDisplay2.SetMinigame(duoMinigames[mg2]);
            UI[0].SetActive(false);
            UI[1].SetActive(true);
            sc.SendMiniGames(duoMinigames[mg1].nameMinigame, duoMinigames[mg2].nameMinigame);
            time.StartTimer(30, 1);
            foreach (GameObject g in finalorder)
            {
                g.GetComponent<PlayerScript>().VoteMiniGameDuel(finalorder.Count, duoMinigames[mg1].nameMinigame, duoMinigames[mg2].nameMinigame);
            }
        }
        if (amount > 2 )
        {

        }

    }
    public void RecievedDuelMiniGameVote(int i)
    {
        miniGChoosen[i]++;
        votedmini++;
        voteCountText[i].text = miniGChoosen[i].ToString();

        if (votedmini == save.playersAlive)
        {
            time.CancelTimer();
            GetAndStartMiniGame();
        }
    }
    #endregion
    #region MiniGame

    public Minigames[] minigames;
    public Minigames[] duoMinigames;
    Minigames playthis;
    [SerializeField] MinigameDisplay mgDisplay1, mgDisplay2;
    [SerializeField] Text[] voteCountText;
    [SerializeField] int[] miniGChoosen;

    int mg1, mg2;
    int votedmini;
    private void StartMiniGame()
    {
        mg1 = Random.Range(0, minigames.Length);
        mg2 = Random.Range(0, minigames.Length);
        if(mg2 == mg1)
        {
            int direction = Random.Range(0, 2);
            if(direction == 0)
            {
                mg2--;
                if (mg2 < 0)
                    mg2 = minigames.Length - 1;
            }
            if (direction == 1)
            {
                mg2++;
                if (mg2 >= minigames.Length)
                    mg2 = 0;
            }
        }

        mgDisplay1.SetMinigame(minigames[mg1]);
        mgDisplay2.SetMinigame(minigames[mg2]);
        UI[0].SetActive(false);
        UI[1].SetActive(true);
        sc.SendMiniGames(minigames[mg1].nameMinigame, minigames[mg2].nameMinigame);
        time.StartTimer(30, 1);
    }
    public void MinigameVotedFor(int i)
    {
        miniGChoosen[i]++;
        votedmini++;
        voteCountText[i].text = miniGChoosen[i].ToString();
        
        if (votedmini == save.playersAlive)
        {
            time.CancelTimer();
            GetAndStartMiniGame();
        }
    }
    public void TimesOutMiniGameVote()
    {
        sc.SendInfo(2);
        GetAndStartMiniGame();
    }
    void GetAndStartMiniGame()
    {
        if(miniGChoosen[0] > miniGChoosen[1])
        {
            playthis = minigames[mg1];
        }
        if (miniGChoosen[0] < miniGChoosen[1])
        {
            playthis = minigames[mg2];
        }
        if(miniGChoosen[0] == miniGChoosen[1])
        {
            int r = Random.Range(0, 2);
            if( r == 0)
                playthis = minigames[mg1];
            else
                playthis = minigames[mg2];
        }
        votedmini = 0;
        for (int i = 0; i < miniGChoosen.Length; i++) 
        {
            miniGChoosen[i] = 0;
        }// RESET Votes

        StartCoroutine(StartMiniGameCD());
    }

    IEnumerator StartMiniGameCD()
    {
        trans.TransitionOut();
        yield return new WaitForSeconds(2);
        foreach (GameObject g in save.players)
        {
            g.transform.GetChild(0).gameObject.SetActive(false);
        }
        SceneManager.LoadScene("Stop and Go");
    }
    #endregion
    #region Final2

    private void FinalTwo()
    {

    }

    #endregion
    #region GameEnded

    private void WeHaveAWinner()
    {

    }

    #endregion
    #region Animations

    [Header("Animations")]
    [SerializeField] Animator stageAnim;
    [SerializeField] Animator roundAnim;
    #endregion
}
