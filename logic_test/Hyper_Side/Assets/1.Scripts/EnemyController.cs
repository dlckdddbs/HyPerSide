using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public UnitSpawnController unitSpawnController;
    public GameObject[] EnemyUnits;

    int len;

    void Start()
    {
        len = EnemyUnits.Length;
        StartCoroutine(EnemySpawn());
    }

    IEnumerator EnemySpawn()
    {
        while (true)
        {
            GameObject unit = EnemyUnits[Random.Range(0, len)];
            unitSpawnController.UnitSpawning(unit, Random.Range(0, 3), true);

            yield return new WaitForSeconds(10f);
        }
    }
}
