using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] int cos;
    [SerializeField] GameObject[] cosmetic;

    public void ChangeCosmetics(int value)
    {
        cos = value;
        cosmetic[cos].SetActive(true);
    }
}
