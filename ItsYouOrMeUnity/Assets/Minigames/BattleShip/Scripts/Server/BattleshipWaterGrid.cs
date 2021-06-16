using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleshipWaterGrid : MonoBehaviour
{
    [SerializeField] GameObject tilePref;
    [SerializeField] MultiCamera cam;
    [SerializeField] BattleshipServer server;
    public List<GameObject> tiles;
    int x = 1;
    int y;

    public void SetSize(int amount)
    {
        
        if (amount / x < x)
        {
            y = amount / x;

            print("x Axis = " + x + "y axis = " + y + " of total amount " + (y+x));
        }
        else
        {
            x++;
            SetSize(amount);
            return;
        }
       
        for (int yy = 0; yy < y; yy++)
        {
            for (int xx = 0; xx < x; xx++)
            {
                GameObject tempTile = Instantiate(tilePref, this.transform);
                cam.targets.Add(tempTile.transform);
                tiles.Add(tempTile);
                tempTile.transform.position = new Vector3(xx * 5, 0.1f, yy * 5);
            }
        }
        server.StartBattle(x,y);
    }
}
