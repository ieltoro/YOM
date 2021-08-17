using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MoonlandingPlayer : NetworkBehaviour
{
    [SerializeField] GameObject spaceshipPrefab;
    [SerializeField] SpaceshipController mySpaceShip;
    [SerializeField] MoonlandingClient client;
    public string playerID;

    private void Start()
    {
        if (hasAuthority)
        {
            print("Is mine");
            client = FindObjectOfType<MoonlandingClient>();
            client.player = this;
            SetOwnerID();
            return;
        }
        if (isServer)
        {
            print("IS Server");
            GameObject temp = Instantiate(spaceshipPrefab);
            mySpaceShip = temp.GetComponent<SpaceshipController>();
            mySpaceShip.owner = this;
            FindObjectOfType<MoonlandingServer>().ConnectedToMiniGame(this);
            return;
        }
        Destroy(gameObject);
    }

    #region Client
    void SetOwnerID()
    {
        playerID = ClientSaveGame.csg.playerID;
        CMD_SetOwnerName(playerID);
    }
    [TargetRpc]
    void RPC_StartFlying(bool answer)
    {
        client.StartFlying(answer);
    }
    public void RecievedInput(Vector2 input)
    {
        CMD_RecievedInput(input);
    }

    #endregion
    #region Server
    public GameObject owner;
    public void StartFlying()
    {
        RPC_StartFlying(true);
        mySpaceShip.enabled = true;
        StartCoroutine(StartCD());
    }
    IEnumerator StartCD()
    {
        yield return new WaitForSeconds(2);
    }
    [Command]
    void CMD_SetOwnerName(string pID)
    {
        playerID = pID;
        foreach (GameObject g in GameSaveHolder.gsh.players)
        {
            if (g.GetComponent<PlayerScript>().id == playerID)
            {
                owner = g;
                return;
            }
        }
    }
    [Command]
    void CMD_RecievedInput(Vector2 input)
    {
        mySpaceShip.InputRecieved(input);
    }

    #endregion
}
