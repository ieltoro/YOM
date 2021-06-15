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
    List<GameObject> ships;
    int pConnected;


    void Start()
    {
        CalculateGrid();
        //manager = FindObjectOfType<YOMNetworkManager>();
        //sc = FindObjectOfType<ServerCalls>();
        //sc.ConnectedToMiniGame(SceneManager.GetActiveScene().name);
    }
    public void ConnectedToMiniGame(GameObject p)
    {
        manager.SpawnNewPlayer(p, 3);
        pConnected++;
        connectedText.text = pConnected.ToString() + "/" + GameSaveHolder.gsh.playersAliveAndConnected;
    }
    public void AllConnectedAndPressedStart()
    {
        CalculateGrid();
    }
    #endregion

    #region Play

    void CalculateGrid()
    {
        int i = players.Count * 8;
        grid.SetSize(4*8);
    }
    public void TimeisOutSetup()
    {
        sc.TimesOutToSetUpShips();
    }
    public void AssignBattleshipPositions(int ship1, float rot1, int ship2, float rot2, BattleshipPlayer ownerName)
    {
        GameObject temp = Instantiate(shipPrefab);
        ownerName.ship.Add(temp);
        temp.GetComponent<Battleship>().SetInfo(ownerName);
        ships.Add(temp);
    }
    public void AllHaveassigned()
    {
        timer.StopPlaceShipTimer();
        SetupBattlefield();
    }
    void SetupBattlefield()
    {

        sc.TimeToBomb();
    }

    public void BombedATile(int tile)
    {
        grid.tiles[tile].GetComponent<BattleshipTile>().bombed = true;
    }

    #endregion

    public void ReturnFromGame()
    {
        GameSaveHolder.gsh.resultsLastGame.Add(GameSaveHolder.gsh.players[0]);
        ServerCalls.sc.ReturnFromMiniGame();
        SceneManager.LoadScene("Game");
    }
}
