using UnityEngine;

public class MoveEnemy : MonoBehaviour
{

    private GameRaceController _GameRaceController;

    [Header ("Atributos do inimigo")]
    public float _enemySpeed = -0.0015f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _GameRaceController = FindFirstObjectByType(typeof(GameRaceController)) as GameRaceController;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position += new Vector3(0, _GameRaceController._enemySpeed, 0);

        if (transform.position.y <= -10.13f)
        {
            Destroy(this.gameObject);
        }
    }
}
