using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    public GameObject spawnEffectPrefab;

    public int spawnLimit = 10;
    public int spawnCount = 0;

    private LayerMask enemyLayer;

    public delegate void SpawnCompleted();
    public event SpawnCompleted OnSpawnCompleted;

    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        enemyLayer = LayerMask.GetMask("Enemies");
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (spawnCount < spawnLimit)
        {
            yield return new WaitUntil(AreAllEnemiesDefeated);
            foreach (Transform spawnPoint in spawnPoints)
            {
                StartCoroutine(SpawnWithEffect(spawnPoint));
            }
            spawnCount++;
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitUntil(AreAllEnemiesDefeated);
        OnSpawnCompleted?.Invoke();
    }

    IEnumerator SpawnWithEffect(Transform spawnPoint)
    {
        GameObject effectInstance = Instantiate(spawnEffectPrefab, spawnPoint.position, Quaternion.identity);
        Animator effectAnimator = effectInstance.GetComponent<Animator>();
        AnimatorStateInfo stateInfo = effectAnimator.GetCurrentAnimatorStateInfo(0);

        audioSource.Play();

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
