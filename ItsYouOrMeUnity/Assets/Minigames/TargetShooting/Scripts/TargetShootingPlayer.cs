using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetShootingPlayer : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    Gyroscope m_Gyro;
    Quaternion changerot, targetRot, crot;
    [SerializeField] float speed, lerpSpeed;
    [SerializeField] Image crosshair;
    float xSize, ySize;


    // fixa 0,0,0,0 vid start


    void Start()
    {
        //Instantiate(prefab, this.transform);
        m_Gyro = Input.gyro;
        m_Gyro.enabled = true;

        xSize = Screen.width * 10f;
        ySize = Screen.height * 10f;
        StartCoroutine(StartCD());
    }

    IEnumerator StartCD()
    {
        yield return new WaitForSeconds(1);
        changerot = ReCenter();
        crosshair.enabled = true;
    }

    void Update()
    {
        //print(m_Gyro.rotationRate + " :Rot, " + m_Gyro.attitude + " :att");
        crot = GetGyro();
        print(crot + "    -  Calculated");
        print(Input.gyro.attitude);
        crosshair.rectTransform.localPosition = new Vector3(crot.z * xSize, crot.x * ySize, 0);
    }

    private Quaternion GetGyro()
    {
        Quaternion r = Input.gyro.attitude;
        if (r.z < 0)
        {
            Quaternion q = new Quaternion( 0 , ( r.y + changerot.y ) , ( r.z - changerot.z ) , ( r.w - changerot.w ));
            return q * new Quaternion(0, 0, 1, 0);
        }
        else
        {
            Quaternion q = new Quaternion( 0 , ( r.y - changerot.y ) , ( r.z + changerot.z ) , ( r.w + changerot.w ));
            return new Quaternion(0f, 0, 0, 0.5f) * q * new Quaternion(0, 0, 1, 0);
        }
    }
    private Quaternion ReCenter()
    {
        Quaternion g = Input.gyro.attitude;
        Quaternion q = new Quaternion(-g.x, -g.y, -g.z, -g.w);
        print(g + "    Old : New    " + q);
        return new Quaternion(-g.x, -g.y, -g.z, -g.w);
    }
}
