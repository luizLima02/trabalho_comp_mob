using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Rigidbody2D ObstaculoRB;

    private GameController _GameController;

    void Start()
    {
        ObstaculoRB = GetComponent<Rigidbody2D>();
        //ObstaculoRB.linearVelocity = new Vector2(-250f, 0);

        _GameController = FindFirstObjectByType(typeof(GameController)) as GameController;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveObjeto();
    }

    void MoveObjeto()
    {
        transform.Translate(Vector2.left * _GameController._ObstaculoVelocidade * Time.smoothDeltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("Tocou no Obstaculo");
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
        Debug.Log("Obstaculo foi destruído");
    }
}
