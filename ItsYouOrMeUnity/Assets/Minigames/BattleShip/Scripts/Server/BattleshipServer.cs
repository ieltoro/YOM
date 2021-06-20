using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleshipServer : MonoBehaviour
{
    #region Setup

    [SerializeField] BattleshipTime timer;
    YOMNetworkManager manager;
    ServerCalls sc;
    public List<BattleshipPlayer> players;
    [SerializeField] Text connectedText;
    [SerializeField] BattleshipWaterGrid grid;
    [SerializeField] GameObject shipPrefab;
    public List<GameObject> ships;
    int pConnected;
    int alive;

    void Start()
    {
        manager = FindObjectOfType<YOMNetworkManager>();
        sc = FindObjectOfType<ServerCalls>();
        sc.ConnectedToMiniGame(SceneManager.GetActiveScene().name);
    }
    public void ConnectedToMiniGame(GameObject p)
    {
        manager.SpawnNewPlayer(p, 3);
        pConnected++;
        connectedText.text = pConnected.ToString() + "/" + GameSaveHolder.gsh.playersAliveAndConnected;
    }
    public void AllConnectedAndPressedStart()
    {
        alive = pConnected;
        CalculateGrid();
    }
    #endregion

    #region Play

    void CalculateGrid()
    {
        int i = players.Count * 8;
        grid.SetSize(8*6);

    }
    public void StartBattle(int x, int y)
    {
        sc.StartBattleship(x,y);
        StartCoroutine(StartBattleCD());
    }
    IEnumerator StartBattleCD()
    {
        yield return new WaitForSeconds(1);
        timer.StartPlaceShipTimer();
    }
    int assignedShip;
    public void AssignBattleshipPositions(int ship1, float rot1, int ship2, float rot2, BattleshipPlayer ownerName)
    {
        assignedShip++;
        GameObject temp1 = Instantiate(shipPrefab);
        ownerName.ship.Add(temp1);
        temp1.GetComponent<Battleship>().SetInfo(ownerName);
        ships.Add(temp1);
        temp1.transform.position = grid.tiles[ship1].transform.position;
        temp1.transform.eulerAngles = new Vector3(0, rot1, 0);

        GameObject temp2 = Instantiate(shipPrefab);
        ownerName.ship.Add(temp2);
        temp2.GetComponent<Battleship>().SetInfo(ownerName);
        ships.Add(temp2);
        temp2.transform.position = grid.tiles[ship2].transform.position;
        temp2.transform.eulerAngles = new Vector3(0, rot2, 0);
        if(assignedShip == pConnected)
        {
            AllHaveassigned();
        }

    }

    public void AllHaveassigned()
    {
        timer.StopPlaceShipTimer();
        StartCoroutine(StartBattleRound());
    }
    public void TimeisOutSetup()
    {
        sc.TimesOutToSetUpShips();
    }

    IEnumerator StartBattleRound()
    {
        yield return new WaitForSeconds(1);
        sc.TimeToBomb();
    }

    public void BombedATile(int tile)
    {
        grid.tiles[tile].GetComponent<BattleshipTile>().bombed = true;
    }

    public void PlayerIsOut(GameObject player)
    {
        GameSaveHolder.gsh.resultsLastGame.Add(player);
        alive--;
        if(alive == 0)
        {
            print("All is dead");
        }
    }
    #endregion

    public void ReturnFromGame()
    {
        GameSaveHolder.gsh.resultsLastGame.Add(GameSaveHolder.gsh.players[0]);
        ServerCalls.sc.ReturnFromMiniGame();
        SceneManager.LoadScene("Game");
    }
}
