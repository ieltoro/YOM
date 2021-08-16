using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using System;

public class AutManager : MonoBehaviour
{
    public static AutManager aut;
   
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;
    public FirebaseFirestore db;

    [Header("Login")]
    [SerializeField] InputField loginName;
    [SerializeField] InputField loginPassord;

    [Header("Register")]
    [SerializeField] InputField registerName;
    [SerializeField] InputField registerPassword1, registerPassword2, registerEmail;
    [SerializeField] ClientLobby lobby;
   
    [Header("Setup")]
    bool[] _skins = new bool[20];
    public Dictionary<string, object> databasetwo;
    #region Setup

    private void Awake()
    {
        if (AutManager.aut == null)
        {
            AutManager.aut = this;
        }
        else
        {
            if (AutManager.aut != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("noppsi to firebase " + dependencyStatus);
            }
        });
    }
    private void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;
        print("connected");
        //lobby.ConnectedToFirebase();
    }
    #endregion

    #region Login

    PlayerBalance balance;
    PlayerSkins skins;
    PlayerInfo info;
    PlayerStats stats;
    MytownCasino casino;
    MytownFarm farm;
    MytownGarage garage;
    MytownHouse house;
    MytownShop shop;

    public void LoginPressed()
    {
        auth.SignInWithEmailAndPasswordAsync(loginName.text, loginPassord.text).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            user = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                user.DisplayName, user.UserId);
            GetUserData();
        });
    }
    void GetUserData()
    {
        Query playerquerry = db.Collection("users").Document(user.UserId).Collection("player");
        playerquerry.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot playerquerrySnapshot = task.Result;
            foreach (DocumentSnapshot documentSnapshot in playerquerrySnapshot.Documents)
            {
                if (String.Format("{0}", documentSnapshot.Id) == "balance")
                {
                    balance = documentSnapshot.ConvertTo<PlayerBalance>();
                }
                if (String.Format("{0}", documentSnapshot.Id) == "skins")
                {
                    skins = documentSnapshot.ConvertTo<PlayerSkins>();
                }
                if (String.Format("{0}", documentSnapshot.Id) == "info")
                {
                    info = documentSnapshot.ConvertTo<PlayerInfo>();
                }
                if (String.Format("{0}", documentSnapshot.Id) == "stats")
                {
                    stats = documentSnapshot.ConvertTo<PlayerStats>();
                }
            };
            lobby.ChangeUi(3);
        });

    }

    public void AnonymSigninPressed()
    {
        lobby.ChangeUi(4);
        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }

            user = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                user.DisplayName, user.UserId);
            lobby.ChangeUi(3);
        });
    }
    #endregion

    #region Register
    public void RegisterPressed()
    {
        if (registerPassword1.text != registerPassword2.text)
        {
            print("Missmatch passwords");
            return;
        }
        auth.CreateUserWithEmailAndPasswordAsync(registerEmail.text, registerPassword1.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            user = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                user.DisplayName, user.UserId);
            SetupData();
        });
        
    }
    public void SetupData()
    {
        DocumentReference reff = db.Collection("users").Document(user.UserId);
        PlayerBalance pb = new PlayerBalance
        {

            coins = 100,
            gold = 0,
            packs = 1,
        };
        reff.Collection("player").Document("balance").SetAsync(pb).ContinueWithOnMainThread(task =>
        {
            PlayerSkins ps = new PlayerSkins
            {
                equiped = 0,
                unlocked = _skins
            };
            reff.Collection("player").Document("skins").SetAsync(ps).ContinueWithOnMainThread(task =>
            {
                PlayerInfo pi = new PlayerInfo
                {
                    userid = user.UserId,
                    nickname = registerName.text,
                    regdate = WorldTimer.sharedInstance.GetCurrentDateNow(),
                };
                reff.Collection("player").Document("info").SetAsync(pi).ContinueWithOnMainThread(task =>
                {
                    PlayerStats ps = new PlayerStats
                    {
                        level = 1,
                        wins = 0
                    };
                    reff.Collection("player").Document("stats").SetAsync(ps).ContinueWithOnMainThread(task =>
                    {
                        MytownHouse mh = new MytownHouse
                        {
                            level = 1,
                            upgrading = false,
                            upgradingDone = ""
                        };
                        reff.Collection("mytown").Document("house").SetAsync(mh).ContinueWithOnMainThread(task =>
                        {
                            MytownGarage mg = new MytownGarage
                            {
                                level = 0,
                                zonePos = 0,
                                upgrading = false,
                                upgradingDone = ""
                            };
                            reff.Collection("mytown").Document("garage").SetAsync(mg).ContinueWithOnMainThread(task =>
                            {
                                MytownFarm mf = new MytownFarm
                                {
                                    level = 0,
                                    zonePos = 0,
                                    upgrading = false,
                                    upgradingDone = ""
                                };
                                reff.Collection("mytown").Document("farm").SetAsync(mf).ContinueWithOnMainThread(task =>
                                {
                                    MytownShop ms = new MytownShop
                                    {
                                        level = 0,
                                        zonePos = 0,
                                        upgrading = false,
                                        upgradingDone = ""
                                    };
                                    reff.Collection("mytown").Document("shop").SetAsync(ms).ContinueWithOnMainThread(task =>
                                    {
                                        MytownCasino mc = new MytownCasino
                                        {
                                            level = 0,
                                            zonePos = 0,
                                            upgrading = false,
                                            upgradingDone = ""
                                        };
                                        reff.Collection("mytown").Document("casino").SetAsync(mc);
                                    });
                                });
                            });
                        });
                    });
                });
            });


            DoneUpload();
        });
    }
    void DoneUpload()
    {
        print("Done");
        ClientSaveGame.csg.PlayerSignedIn(registerName.text, user.UserId);
        lobby.ChangeUi(3);
    }
    #endregion

    #region Data
    public void GetMyTownData()
    {
        Query mytownQuerry = db.Collection("users").Document(user.UserId).Collection("mytown");
        mytownQuerry.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot mytownQuerrySnapshot = task.Result;
            foreach (DocumentSnapshot documentSnapshot in mytownQuerrySnapshot.Documents)
            {
                if (String.Format("{0}", documentSnapshot.Id) == "casino")
                {
                    casino = documentSnapshot.ConvertTo<MytownCasino>();
                }
                if (String.Format("{0}", documentSnapshot.Id) == "farm")
                {
                    farm = documentSnapshot.ConvertTo<MytownFarm>();
                }
                if (String.Format("{0}", documentSnapshot.Id) == "garage")
                {
                    garage = documentSnapshot.ConvertTo<MytownGarage>();
                }
                if (String.Format("{0}", documentSnapshot.Id) == "house")
                {
                    house = documentSnapshot.ConvertTo<MytownHouse>();
                }
                if (String.Format("{0}", documentSnapshot.Id) == "shop")
                {
                    shop = documentSnapshot.ConvertTo<MytownShop>();
                }
            };
            ClientSaveGame.csg.SetPlayerData(balance, info, skins, stats);
            ClientSaveGame.csg.SetMyTownData(casino, farm, garage, house, shop);

            FindObjectOfType<MyTownManager>().GetArea();
        });
    }
    public void ChangeBalance(int amount)
    {
        DocumentReference reff = db.Collection("users").Document(user.UserId).Collection("player").Document("balance");
        Dictionary<string, object> updates = new Dictionary<string, object>
{
        { "coins",  amount}
};

        reff.UpdateAsync(updates).ContinueWithOnMainThread(task => {
            Debug.Log("Updated the coins ");
        });
        // You can also update a single field with: cityRef.UpdateAsync("Capital", false);
    }

    #region Buildings

    public void UpdateMyTownHouse()
    {
        DocumentReference reff = db.Collection("users").Document(user.UserId).Collection("mytown").Document("house");
        MytownHouse upload = ClientSaveGame.csg.townHouse;

        reff.SetAsync(upload).ContinueWithOnMainThread(task => {
            Debug.Log("Updated the house ");
        });
    }
    public void UpdateMyTownGarage()
    {
        DocumentReference reff = db.Collection("users").Document(user.UserId).Collection("mytown").Document("garage");
        MytownGarage upload = ClientSaveGame.csg.townGarage;

        reff.SetAsync(upload).ContinueWithOnMainThread(task => {
            Debug.Log("Updated the garage ");
        });
    }
    public void UpdateMyTownFarm()
    {
        DocumentReference reff = db.Collection("users").Document(user.UserId).Collection("mytown").Document("farm");
        MytownFarm upload = ClientSaveGame.csg.townFarm;

        reff.SetAsync(upload).ContinueWithOnMainThread(task => {
            Debug.Log("Updated the farm ");
        });
    }
    public void UpdateMyTownShop()
    {
        DocumentReference reff = db.Collection("users").Document(user.UserId).Collection("mytown").Document("shop");
        MytownShop upload = ClientSaveGame.csg.townShop;

        reff.SetAsync(upload).ContinueWithOnMainThread(task => {
            Debug.Log("Updated the shop ");
        });
    }
    public void UpdateMyTownCasino()
    {
        DocumentReference reff = db.Collection("users").Document(user.UserId).Collection("mytown").Document("casino");
        MytownCasino upload = ClientSaveGame.csg.townCasino;

        reff.SetAsync(upload).ContinueWithOnMainThread(task => {
            Debug.Log("Updated the casino ");
        });
    }
    #endregion
    #endregion
}

