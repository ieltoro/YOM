using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleshipClient : MonoBehaviour
{
    [SerializeField] GameObject[] ui;
    [HideInInspector] public BattleshipPlayer player;
    [SerializeField] Image[] battleship;
    [SerializeField] int currentShip;
    [SerializeField] BattleshipTileSelected[] tiles;
    [SerializeField] Text infoText;
    public bool horizontal;
    public int currentRot = 0;
    BattleshipTileSelected currentTile;
    int tileNr;
    int[] choosen = new int[2];
  
    //void Start()
    //{
    //    ClientSaveGame.csg.localPlayer.GetComponent<PlayerScript>().MinigameConnectedTo(0);
    //    if (ClientSaveGame.csg.localPlayer.GetComponent<PlayerScript>().leader)
    //    {
    //        ChangeUi(1);
    //    }
    //}

    public int TileNrGrab()
    {
        tileNr++;
        return tileNr -1;
    }

    #region Assign Battleship Pos

    public void ChangeUi(int u)
    {
        foreach(GameObject g in ui)
        {
            g.SetActive(false);
        }
        ui[u].SetActive(true);
    }
    public void MoveBattleship(BattleshipTileSelected tile, Vector2 pos, float rotation)
    {
        currentTile = tile;
        battleship[currentShip].rectTransform.localPosition = pos;
        battleship[currentShip].rectTransform.localEulerAngles = new Vector3(0, 0, rotation);
    }
    public void ConfirmPos()
    {
        if(currentShip == 0)
        {
            if (currentTile == null)
            {
                InfoDisplay("Place the ship on the tiles");
                return;
            }
                
            currentTile.LockTiles();
            battleship[1].gameObject.SetActive(true);
            choosen[0] = currentTile.order;
            currentShip++;
            return;
        }
        if(currentShip == 1)
        {
            if (!currentTile.free)
            {
                InfoDisplay("Place the ship on the tiles");
                return;
            }
            choosen[1] = currentTile.order;
            currentShip++;
        }
        if(currentShip == 2)
        {
            player.AssignShipPosition(choosen[0], battleship[0].rectTransform.localEulerAngles.z, choosen[1], battleship[1].rectTransform.localEulerAngles.z);
        }
    }
    public void RandomBattleships()
    {
        if(currentShip == 0)
        {
            currentRot = Random.Range(0, 4);
            int t = Random.Range(0, tiles.Length);
            tiles[t].PressedThisTile();
            ConfirmPos();
            currentRot = Random.Range(0, 4);
            int r = Random.Range(0, tiles.Length);
            if(!tiles[r].free)
            {
                int a = Random.Range(0, 2);
                if(a == 0)
                {
                    r += 2;
                    if (r >= tiles.Length)
                        r = 0;
                }
                else
                {
                    r -= 2;
                    if (r == 0)
                        r = tiles.Length - 1;
                }

            }
            tiles[r].PressedThisTile();
            ConfirmPos();
        }
        if(currentShip == 1)
        {
            currentRot = Random.Range(0, 4);
            int r = Random.Range(0, tiles.Length);
            if (!tiles[r].free)
            {
                int a = Random.Range(0, 2);
                if (a == 0)
                {
                    r += 2;
                    if (r >= tiles.Length)
                        r = 0;
                }
                else
                {
                    r -= 2;
                    if (r == 0)
                        r = tiles.Length - 1;
                }

            }
            tiles[r].PressedThisTile();
            ConfirmPos();
        }
    }
    IEnumerator InfoDisplay(string info)
    {
        infoText.text = info;
        infoText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        infoText.gameObject.SetActive(false);
    }
    public void TimeIsOut()
    {
        if (currentShip == 2)
            return;
        if(currentShip < 2)
        {
            RandomBattleships();
        }
    }

    #endregion
    #region BombOthers

    [Header("BombFace")]
    [SerializeField] Text playerBombingName;

    public void BombTime()
    {
        ChangeUi(3);
    }
    public void BombedThisTile(int tile)
    {
        player.BombedThisTile(tile);
        ChangeUi(0);
    }

    #endregion
}
