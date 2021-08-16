using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildUIButton : MonoBehaviour
{
    [SerializeField] int buildingID;
    
    public void BuildingAlreadyBuild()
    {
        GetComponent<Button>().interactable = false;
        transform.GetChild(0).gameObject.SetActive(true);
    }
    public void PressedThisBuilding()
    {
        FindObjectOfType<MyTownManager>().PressedBuyBuilding(buildingID);
    }
}
