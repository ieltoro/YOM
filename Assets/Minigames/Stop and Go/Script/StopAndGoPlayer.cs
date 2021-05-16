using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class StopAndGoPlayer : NetworkBehaviour
{
    public bool moving, redLight, canmove;
    public float speed = 0.75f;
    StopAndGoManager rlm;
    public Transform startPos;

    private void Awake()
    {
        if(hasAuthority)
        {
            rlc = FindObjectOfType<StopAndGoClient>();
            rlc.myPlayer = this;
        }
        if(isServer)
        {
            rlm = FindObjectOfType<StopAndGoManager>();
            rlm.players.Add(this);
            if (rlm.leader == null)
            {
                rlm.leader = this.gameObject;
                YouAreTheLeader();
            }
        }
        if(!isServer && !hasAuthority)
        {
            Destroy(gameObject);
        }
    }

    #region Client

    StopAndGoClient rlc;

    [TargetRpc]
    void YouAreTheLeader()
    {
        rlc.Leader();
    }
    public void PressedStartRed()
    {
        PlayersReady();
    }
    public void PlayerPressing(bool d)
    {
        PressingDown(d);
    }

    #endregion


    #region Server
    public void SetSpawn(Transform pos)
    {
        transform.position = pos.position;
        transform.rotation = pos.rotation;
        startPos = pos;
    }
    [Command]
    void PlayersReady()
    {
        rlm.StartRedLight();
    }
    public void EnableMove()
    {
        canmove = true;
    }
    private void Update()
    {
        if (!isServer)
            return;
        if (moving && canmove)
        {
            if (redLight)
            {
                BackToSpawn();
                return;
            }
            transform.position = transform.position + new Vector3(0, 0, speed * Time.deltaTime);

        }
    }
    [Command]
    void PressingDown(bool pd)
    {
        moving = true;
    }
    public void BackToSpawn()
    {
        canmove = false;
        transform.position = startPos.position;
        transform.rotation = startPos.rotation;
        StartCoroutine(RespawnCD());
    }
    IEnumerator RespawnCD()
    {
        yield return new WaitForSeconds(2);
        canmove = true;
    }

    #endregion
}
