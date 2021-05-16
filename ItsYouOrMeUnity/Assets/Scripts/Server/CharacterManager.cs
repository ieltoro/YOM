using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] int cos;
    public GameObject lightObj;

    public void ChangeCosmetics(int value)
    {
        cos = value;
    }
}
