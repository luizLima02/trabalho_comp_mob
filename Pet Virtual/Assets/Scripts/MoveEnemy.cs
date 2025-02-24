using UnityEngine;

public class MoveEnemy : MonoBehaviour
{

    private GameRaceController _GameRaceController;

    //[Header ("Atributos do inimigo")]
    //public float _enemySpeed = -5f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _GameRaceController = FindFirstObjectByType(typeof(GameRaceController)) as GameRaceController;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(_GameRaceController._enemySpeed);
        float speed = _GameRaceController._enemySpeed * Time.deltaTime;//_enemySpeed;
        transform.position += new Vector3(0, speed, 0);

        if (transform.position.y <= -10.13f)
        {
            Destroy(this.gameObject);
        }
    }
}
