using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetShootingPlayer : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    Gyroscope m_Gyro;
    Quaternion changerot, targetRot, crot;
    Vector3 aim;
    [SerializeField] float speed, lerpSpeed;
    [SerializeField] Image crosshair;
    float xSize, ySize;
    float width, height;
    Vector3 transf;
    // fixa 0,0,0,0 vid start


    void Start()
    {
        //Instantiate(prefab, this.transform);
        m_Gyro = Input.gyro;
        m_Gyro.enabled = true;
        print(Screen.width + "  -   " + Screen.height);
        xSize = Screen.width * 10f;
        ySize = Screen.height * 10f;
        StartCoroutine(StartCD());
        print("-Screen.width / 2     " + -Screen.width / 2);
    }

    IEnumerator StartCD()
    {
        yield return new WaitForSeconds(1);
        changerot = ReCenter();
        crosshair.enabled = true;
    }

    void Update()
    {
        //Vector3 v = m_Gyro.rotationRate;
        //v = v * 2000 * Time.deltaTime;
        //prefab.transform.localEulerAngles = v;

        crot = GetGyro();
        Vector3 pos = new Vector3(crot.z * xSize, crot.x * ySize, 0);
        print(pos);
        transf = pos;
        crosshair.rectTransform.position = transf;
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
            return q * new Quaternion(0, 0, 1, 0);
        }
    }
    private Quaternion ReCenter()
    {
        Quaternion g = Input.gyro.attitude;
        Quaternion q = new Quaternion(-g.x, -g.y, -g.z, -g.w);
        return new Quaternion(-g.x, -g.y, -g.z, -g.w);
    }
}
