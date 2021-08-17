using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleshipClient : MonoBehaviour
{
    #region Setup
    [SerializeField] GameObject[] ui;
    [HideInInspector] public BattleshipPlayer player;
    [SerializeField] GameObject tilesPrefab;
    [SerializeField] FlexibleGridLayout gridScript;
    [SerializeField] Image[] battleship;
    [HideInInspector] public List<BattleshipTileSelected> tiles;
    [HideInInspector] public bool horizontal;
    [HideInInspector] public int currentRot = 3;
    BattleshipTileSelected currentTile;
    [SerializeField] Text infoText;
    int currentShip;
    int tileNr;
    int[] choosen = new int[2];

    void Start()
    {
        print("are you ledaer?" + ClientSaveGame.csg.localPlayer.GetComponent<PlayerScript>().leader);
        if (ClientSaveGame.csg.localPlayer.GetComponent<PlayerScript>().leader)
        {
            ui[1].SetActive(true);
        }
    }
    public void SetupGameboard(int xSize, int ySize)
    {
        gridScript.rows = ySize;
        gridScript.columns = xSize;
        for(int i = (xSize * ySize); i > 0; i--)
        {
            GameObject g = Instantiate(tilesPrefab, gridScript.transform);
            g.GetComponent<BattleshipTileSelected>().SetInfo(this);
            
        }
        foreach(BattleshipTileSelected t in tiles)
        {
            t.CheckNeighbours();
        }
        StartCoroutine(SetSizeBoat());
    }
    IEnumerator SetSizeBoat()
    {
        yield return new WaitForSeconds(0.2f);
        float size = gridScript.cellSize.x;
        battleship[0].rectTransform.sizeDelta = new Vector2(size, size);
        battleship[0].transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
        battleship[1].rectTransform.sizeDelta = new Vector2(size, size);
        battleship[1].transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
        yield return new WaitForSeconds(2f);
        ui[0].SetActive(false);
    }
    public int TileNrGrab()
    {
        tileNr++;
        return tileNr -1;
    }
    public void StartGame()
    {
        player.StartGame();
        ui[1].SetActive(false);
    }
    #endregion
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
        battleship[currentShip].transform.parent = tile.transform;
        battleship[currentShip].rectTransform.localPosition = new Vector2(0, 0);
        //battleship[currentShip].rectTransform.localPosition = pos;
        battleship[currentShip].rectTransform.localEulerAngles = new Vector3(0, 0, rotation);
    }
    public void RotateShip()
    {
        battleship[currentShip].rectTransform.localEulerAngles = new Vector3(0, 0, currentTile.RotateShip());
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
            currentRot = 3;
            print(choosen[0]);
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
            ChangeUi(0);
            print(choosen[0] + "  :  " + choosen[1]);
            ChangeFace();
            player.AssignShipPosition(choosen[0], battleship[0].rectTransform.localEulerAngles.z, choosen[1], battleship[1].rectTransform.localEulerAngles.z);
        }
    }
    public void RandomBattleships()
    {
        if(currentShip == 0)
        {
            currentRot = Random.Range(0, 4);
            int t = Random.Range(0, tiles.Count);
            tiles[t].PressedThisTile();
            ConfirmPos();
            currentRot = Random.Range(0, 4);
            int r = Random.Range(0, tiles.Count);
            if(!tiles[r].free)
            {
                int a = Random.Range(0, 2);
                if(a == 0)
                {
                    r += 2;
                    if (r >= tiles.Count)
                        r = 0;
                }
                else
                {
                    r -= 2;
                    if (r == 0)
                        r = tiles.Count - 1;
                }

            }
            tiles[r].PressedThisTile();
            ConfirmPos();
        }
        if(currentShip == 1)
        {
            currentRot = Random.Range(0, 4);
            int r = Random.Range(0, tiles.Count);
            if (!tiles[r].free)
            {
                int a = Random.Range(0, 2);
                if (a == 0)
                {
                    r += 2;
                    if (r >= tiles.Count)
                        r = 0;
                }
                else
                {
                    r -= 2;
                    if (r == 0)
                        r = tiles.Count - 1;
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
    void ChangeFace()
    {
        battleship[0].gameObject.SetActive(false);
        battleship[1].gameObject.SetActive(false);
        foreach (BattleshipTileSelected g in tiles)
        {
            g.bombTime = true;
        }
    }

    #endregion
    #region BombOthers

    [Header("BombFace")]
    [SerializeField] Text playerBombingName;

    public void BombTime()
    {
        ChangeUi(2);
    }
    public void BombedThisTile(int tile)
    {
        print(tile);
        ChangeUi(0);
        player.BombedThisTile(tile);
    }

    #endregion
}
