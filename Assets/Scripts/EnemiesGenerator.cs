using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesGenerator : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Transform[] generationsPoints;
    public float generationTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        generationsPoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            generationsPoints[i] = transform.GetChild(i);
        }

        StartCoroutine(EnemyAppear());

    }

    IEnumerator EnemyAppear()
    {
        while (true)
        {
            for (int i = 0; i < generationsPoints.Length; i++)
            {
                Transform generationPoint = generationsPoints[i];
                Instantiate(zombiePrefab, generationPoint.position, generationPoint.rotation);
            }
            yield return new WaitForSeconds(generationTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
