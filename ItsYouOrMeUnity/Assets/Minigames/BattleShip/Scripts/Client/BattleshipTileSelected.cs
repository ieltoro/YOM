using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleshipTileSelected : MonoBehaviour
{
    [SerializeField] int x, y;
    public int order;
    [SerializeField] BattleshipClient client;
    public bool free;
    [SerializeField] BattleshipTileSelected[] friends;
    [SerializeField] float[] zRots;
    BattleshipTileSelected currentConnection;
    int c;
    [SerializeField] bool bombed;

    private void Start()
    {
        order = client.TileNrGrab();
    }

    public void PressedThisTile()
    {
        if(bombed)
        {
            client.BombedThisTile(order);
        }
        else
        {
            c = client.currentRot;
            if (friends[c] != null && friends[c].free)
            {
                client.MoveBattleship(this, new Vector2(x, y), zRots[c]);
                currentConnection = friends[c];
            }
            else
            {
                client.MoveBattleship(this, new Vector2(x, y), RotateShip());
            }
        }
    }
    public float RotateShip()
    {
        c = client.currentRot;
        for (int i = 0; i <= 4; i++)
        {
            c++;
            if (c == 4)
                c = 0;
            if (friends[c] != null && friends[c].free)
            {
                client.currentRot = c;
                currentConnection = friends[c];
                return zRots[c];
            }
        }
        return zRots[c];
    }
    public void LockTiles()
    {
        free = false;
        currentConnection.free = false;
    }
    public void CheckAround(int pos, BattleshipTileSelected f)
    {
        print("Tile connected " + pos);
        friends[pos] = f;
    }
}
