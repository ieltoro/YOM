using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyTownManager : MonoBehaviour
{
    WorldTimer timer;
    public int houseLvl;
    public int garageLvl;
    public int casinoLvl;
    public int farmLvl;
    public int storeLvl;
    int coins;
    [SerializeField] Text coinsText;

    [Header("Building")]
    [Tooltip("1 = Garage \n 2 = Farm \n 3 = Shop \n 4 = Casino")]
    [SerializeField] GameObject[] buildingPrefabs;
    [SerializeField] int[] buildPrices;
    [SerializeField] GameObject[] buildButtons;
    [SerializeField] Lotbuild[] buildingZones;
    [SerializeField] GameObject buyZoneCanvas;
    int buildingArea;
    public List<bool> buildings;

    void Start()
    {

        timer = FindObjectOfType<WorldTimer>();
        AutManager.aut.GetMyTownData();
    }

    public void GetArea()
    {
        coins = ClientSaveGame.csg.pBalance.coins;
        coinsText.text = coins.ToString();
     
        #region garage
        if(ClientSaveGame.csg.townGarage.level > 0)
        {
            BuildOnLot(buildingPrefabs[0], ClientSaveGame.csg.townGarage.zonePos);
            buildings[0] = true;
        }
        #endregion
        #region farm
        if (ClientSaveGame.csg.townFarm.level > 0)
        {
            BuildOnLot(buildingPrefabs[1], ClientSaveGame.csg.townFarm.zonePos);
            buildings[1] = true;
        }
        #endregion
        #region shop
        if (ClientSaveGame.csg.townShop.level > 0)
        {
            BuildOnLot(buildingPrefabs[2], ClientSaveGame.csg.townShop.zonePos);
            buildings[2] = true;
        }
        #endregion
        #region casino
        if (ClientSaveGame.csg.townCasino.level > 0)
        {
            BuildOnLot(buildingPrefabs[3], ClientSaveGame.csg.townCasino.zonePos);
            buildings[3] = true;
        }
        #endregion

        // H�mta vilka hus som �r byggda 
    }

    void BuildOnLot(GameObject _buildingPref, int pos)
    {
        GameObject pref = Instantiate(_buildingPref, buildingZones[pos].transform.position, buildingZones[pos].transform.rotation);
    }

    public void PressedBuyZone(int zoneId, bool open)
    {
        buildingArea = zoneId;
        if(buildings.Count > 0)
        {
            for (int i = 0; i < buildings.Count; i++)
            {
                if (buildings[i])
                {
                    buildButtons[i].GetComponent<BuildUIButton>().BuildingAlreadyBuild();
                }
            }
        }
        buyZoneCanvas.SetActive(open);
    }

    public void PressedBuyBuilding(int id)
    {
        print("3");
        if (buildPrices[id] < coins)
        {
            print("MOOOONEY");
            TakeMoney(buildPrices[id]);
            BuildOnLot(buildingPrefabs[id], buildingArea);
            buyZoneCanvas.SetActive(false);
            if(id == 0) // Garage
            {
                ClientSaveGame.csg.townGarage.level = 1;
                ClientSaveGame.csg.townGarage.zonePos = buildingArea;
                AutManager.aut.UpdateMyTownGarage();
            }
            if (id == 1) // farm
            {
                ClientSaveGame.csg.townFarm.level = 1;
                ClientSaveGame.csg.townFarm.zonePos = buildingArea;
                AutManager.aut.UpdateMyTownFarm();
            }
            if (id == 2) // shop
            {
                ClientSaveGame.csg.townShop.level = 1;
                ClientSaveGame.csg.townShop.zonePos = buildingArea;
                AutManager.aut.UpdateMyTownShop();
            }
            if (id == 3) // casino
            {
                ClientSaveGame.csg.townCasino.level = 1;
                ClientSaveGame.csg.townCasino.zonePos = buildingArea;
                AutManager.aut.UpdateMyTownCasino();
            }
        }
        else
        {
            print("Not enough money");
        }
    }

    void TakeMoney(int amount)
    {
        coins -= amount;
        coinsText.text = coins.ToString();
        AutManager.aut.ChangeBalance(coins);
    }

}
