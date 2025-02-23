using UnityEngine;

public class CollectibleController : MonoBehaviour
{

    private Rigidbody2D _starsRB2D;
    private GameController _GameController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        _GameController = FindFirstObjectByType(typeof(GameController)) as GameController;
        _starsRB2D = GetComponent<Rigidbody2D>();
       // _starsRB2D.linearVelocity = new Vector2(-6f, 0);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        MoveObjeto();
    }

    void MoveObjeto()
    {
        transform.Translate(Vector2.left * _GameController._coletavelVelocidade * Time.smoothDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _GameController.Pontos(1);
            Debug.Log("Pegou a estrela!");
            Destroy(this.gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
        Debug.Log("A estrela foi destruído");
    }
}
