using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;


#region player

[FirestoreData]
public struct PlayerBalance
{
    [FirestoreProperty]
    public int coins { get; set; }

    [FirestoreProperty]
    public int gold { get; set; }

    [FirestoreProperty]
    public int packs { get; set; }
}
[FirestoreData]
public struct PlayerInfo
{
    [FirestoreProperty]
    public string userid { get; set; }
    [FirestoreProperty]
    public string nickname { get; set; }
    [FirestoreProperty]
    public string regdate { get; set; }
}
[FirestoreData]
public struct PlayerStats
{
    [FirestoreProperty]
    public int level { get; set; }

    [FirestoreProperty]
    public int wins { get; set; }
}
[FirestoreData]
public struct PlayerSkins
{
    [FirestoreProperty]
    public int equiped { get; set; }

    [FirestoreProperty]
    public bool[] unlocked { get; set; }
}

#endregion
#region My Town

[FirestoreData]
public struct MytownHouse
{
    [FirestoreProperty]
    public int level { get; set; }
    [FirestoreProperty]
    public bool upgrading { get; set; }
    [FirestoreProperty]
    public string upgradingDone { get; set; }
}

[FirestoreData]
public struct MytownGarage 
{
    [FirestoreProperty]
    public int level { get; set; }
    [FirestoreProperty]
    public int zonePos { get; set; }
    [FirestoreProperty]
    public bool upgrading { get; set; }
    [FirestoreProperty]
    public string upgradingDone { get; set; }
}
[FirestoreData]
public struct MytownFarm
{
    [FirestoreProperty]
    public int level { get; set; }
    [FirestoreProperty]
    public int zonePos { get; set; }
    [FirestoreProperty]
    public bool upgrading { get; set; }
    [FirestoreProperty]
    public string upgradingDone { get; set; }
}
[FirestoreData]
public struct MytownShop
{
    [FirestoreProperty]
    public int level { get; set; }
    [FirestoreProperty]
    public int zonePos { get; set; }
    [FirestoreProperty]
    public bool upgrading { get; set; }
    [FirestoreProperty]
    public string upgradingDone { get; set; }
}
[FirestoreData]
public struct MytownCasino
{
    [FirestoreProperty]
    public int level { get; set; }
    [FirestoreProperty]
    public int zonePos { get; set; }
    [FirestoreProperty]
    public bool upgrading { get; set; }
    [FirestoreProperty]
    public string upgradingDone { get; set; }
}




[FirestoreData]
public class Användare
{
    [FirestoreProperty]
    public string Namn { get; set; }

    [FirestoreProperty]
    public string Adress { get; set; }

    [FirestoreProperty]
    public string Ort { get; set; }
}

[FirestoreData]
public class Info
{
    public string Onskemal { get; set; }
}







#endregion
#region Economy

[FirestoreData]
public struct Economy
{
    [FirestoreProperty]
    public int level { get; set; }
    [FirestoreProperty]
    public bool upgrading { get; set; }
    [FirestoreProperty]
    public string upgradingDone { get; set; }
}

#endregion



