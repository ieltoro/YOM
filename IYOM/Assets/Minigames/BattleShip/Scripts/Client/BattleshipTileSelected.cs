using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleshipTileSelected : MonoBehaviour
{
    public int order;
    [SerializeField] int x, y;
    public BattleshipClient client;
    [SerializeField] BattleshipTileSelected[] friends;
    [SerializeField] float[] zRots;
    BattleshipTileSelected currentConnection;
    public bool free;
    public bool bombTime;
    public bool bombed;
    int c;
    [SerializeField] Transform checker;
    public void SetInfo(BattleshipClient c)
    {
        client = c;
        order = client.TileNrGrab();
        client.tiles.Add(this);
    }
    public void CheckNeighbours()
    {
        StartCoroutine(CheckNeighboursCD());
    }
    IEnumerator CheckNeighboursCD()
    {
        yield return new WaitForSeconds(0.2f);
        float size = new float();
        size = GetComponentInParent<FlexibleGridLayout>().cellSize.x;
        GetComponent<BoxCollider2D>().size = new Vector2(size * 0.8f, size * 0.8f);
        transform.GetChild(4).GetComponent<BoxCollider2D>().size = new Vector2(size * 0.8f, size * 0.8f);

        checker.localPosition = new Vector2(0, -size);
        checker.gameObject.SetActive(true);
        checker.GetComponent<Tilefriend>().Check();
        yield return new WaitForSeconds(0.2f);
        checker.localPosition = new Vector2(-size, 0);
        checker.GetComponent<Tilefriend>().Check();
        yield return new WaitForSeconds(0.2f);
        checker.localPosition = new Vector2(0, +size);
        checker.GetComponent<Tilefriend>().Check();
        yield return new WaitForSeconds(0.2f);
        checker.localPosition = new Vector2(+size, 0);
        checker.GetComponent<Tilefriend>().Check();
        yield return new WaitForSeconds(0.2f);
        checker.gameObject.SetActive(false);
    }
    public void PressedThisTile()
    {
        if(bombTime)
        {
            client.BombedThisTile(order);
            return;
        }
        else
        {
            c = client.currentRot;
            if (friends[c] != null && friends[c].free)
            {
                client.MoveBattleship(this, GetComponent<RectTransform>().rect.position, zRots[c]);
                currentConnection = friends[c];
            }
            else
            {
                client.MoveBattleship(this, GetComponent<RectTransform>().rect.position, RotateShip());
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
        friends[pos] = f;
    }
}
