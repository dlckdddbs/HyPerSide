using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawnController : MonoBehaviour
{
    public Transform[] spawnTrans;
    public GameObject area;

    void Awake()
    {
        Card.OnUnitSpawning += UnitSpawning;
        Card.OnCardHolding += area.SetActive;
    }

    public void UnitSpawning(GameObject unit, int wayIndex, bool isEnemy)
    {
        GameObject temp = Instantiate(unit, spawnTrans[wayIndex + (isEnemy ? 3 : 0)].position, Quaternion.Euler(0f, (isEnemy ? -1 : 1) * 90f, 0f));
        temp.GetComponent<Unit>().linetype = (LINETYPE)wayIndex;
        Debug.Log(wayIndex);
    }
}
