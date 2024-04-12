using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    public GameObject spawnEffectPrefab;

    [SerializeField]
    private int spawnLimit = 10;
    private int spawnCount = 0;

    private LayerMask enemyLayer;

    void Start()
    {
        enemyLayer = LayerMask.GetMask("Enemies");
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        Debug.Log("Coroutine started");
        while (spawnCount < spawnLimit)
        {
            yield return new WaitUntil(() => AreAllEnemiesDefeated());

            List<Coroutine> spawnCoroutines = new List<Coroutine>();
            foreach (Transform spawnPoint in spawnPoints)
            {
                Coroutine spawnCoroutine = StartCoroutine(SpawnWithEffect(spawnPoint));
                spawnCoroutines.Add(spawnCoroutine);
            }

            foreach (Coroutine cor in spawnCoroutines)
            {
                yield return cor;
            }

            spawnCount++;
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator SpawnWithEffect(Transform spawnPoint)
    {
        GameObject effectInstance = Instantiate(spawnEffectPrefab, spawnPoint.position, Quaternion.identity);
        Animator effectAnimator = effectInstance.GetComponent<Animator>();
        AnimatorStateInfo stateInfo = effectAnimator.GetCurrentAnimatorStateInfo(0);

        yield return new WaitForSeconds(stateInfo.length / 2); // Chờ hiệu ứng hoàn tất

        Destroy(effectInstance);

        int index = Random.Range(0, enemyPrefabs.Length);
        Debug.Log("Spawning enemy at: " + spawnPoint.position);
        Instantiate(enemyPrefabs[index], spawnPoint.position, Quaternion.identity);
    }

    bool AreAllEnemiesDefeated()
    {
        return GameObject.FindGameObjectsWithTag("Enemies").Length == 0;
    }
}
