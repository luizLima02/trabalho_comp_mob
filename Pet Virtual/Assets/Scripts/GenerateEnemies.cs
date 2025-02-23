using UnityEngine;

public class GenerateEnemies : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemyPrefabs;

    private float       _timer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= 1.0f)
        {
            _timer = 0;

            int randomIndex = Random.Range(0, _enemyPrefabs.Length);
            GameObject randomEnemy = _enemyPrefabs[randomIndex];
            Instantiate(randomEnemy, new Vector3(Random.Range(-1.16f, 1.16f), transform.position.y, 0), Quaternion.Euler(0, 0, 0));
        }
    }
}
