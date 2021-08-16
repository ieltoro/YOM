using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerMyTown : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;


    [SerializeField] float speed;
    [SerializeField] float perspectiveZoomSpeed = 0.5f;
    [SerializeField] float minX = -200;
    [SerializeField] float maxX = 200;
    [SerializeField] float minZ = -500;
    [SerializeField] float maxZ = 310;
    [SerializeField] float minY = -30;
    [SerializeField] float maxY = 200;
    [SerializeField] float maxFov = 90, minFov = 35;
    [SerializeField] Camera cam;

    private void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                print(hit.collider.tag);
                if (hit.collider.tag == "BuildZone")
                {
                    hit.collider.transform.GetComponent<Lotbuild>().PressedThisLot();
                }
                if(hit.collider.tag == "House")
                {
                    hit.collider.transform.GetComponent<HouseManager>().PressedHouse();
                }
            }
        }
        #region Move Camera

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            speed = cam.fieldOfView / 28000;
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

            transform.Translate(-touchDeltaPosition.x * speed, 0, -touchDeltaPosition.y * speed, Space.World);

            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, minX, maxX),
                Mathf.Clamp(transform.position.y, minY, maxY),
                Mathf.Clamp(transform.position.z, minZ, maxZ));
        }

        if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
          
            cam.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minFov, maxFov);
        }
        #endregion
    }
}
