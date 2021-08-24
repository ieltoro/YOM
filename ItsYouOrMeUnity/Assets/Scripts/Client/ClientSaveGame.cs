using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;

public class ClientSaveGame : MonoBehaviour
{
    public static ClientSaveGame csg;
    private void Awake()
    {
        if (ClientSaveGame.csg == null)
        {
            ClientSaveGame.csg = this;
        }
        else
        {
            if (ClientSaveGame.csg != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    [Header("Player")]
    public string regDate;
    public string playerID;
    public string playerNickname;
    public PlayerBalance pBalance;
    public PlayerInfo pInfo;
    public PlayerSkins pSkins;
    public PlayerStats pStats;

    public void PlayerSignedIn(string _nickname, string _playerID)
    {
        playerID = _playerID;
        playerNickname = _nickname;
    }

    public void SetPlayerData(PlayerBalance _balance, PlayerInfo _info, PlayerSkins _skins, PlayerStats _stats)
    {
        pBalance = _balance;
        pInfo = _info;
        pSkins = _skins;
        pStats = _stats;
    }

    [Header("My town")]
    public MytownHouse townHouse;
    public MytownGarage townGarage;
    public MytownFarm townFarm;
    public MytownShop townShop;
    public MytownCasino townCasino;
    public Economy prices;

    public void SetMyTownData(MytownCasino _casino, MytownFarm _farm, MytownGarage _garage, MytownHouse _house, MytownShop _shop)
    {
        townHouse = _house;
        townGarage = _garage;
        townFarm = _farm;
        townShop = _shop;
        townCasino = _casino;
    }


    [Header("Game")]
    public bool dead;
    public GameObject localPlayer;
    public List<GameObject> players;
}
