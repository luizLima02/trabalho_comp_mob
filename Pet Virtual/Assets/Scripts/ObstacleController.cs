using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Rigidbody2D ObstaculoRB;

    private GameController  _GameController;
    private CameraShaker    _CameraShaker;

    void Start()
    {
        ObstaculoRB = GetComponent<Rigidbody2D>();
        //ObstaculoRB.linearVelocity = new Vector2(-250f, 0);

        _GameController = FindFirstObjectByType(typeof(GameController)) as GameController;

        _CameraShaker = FindFirstObjectByType(typeof(CameraShaker)) as CameraShaker;

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
            _GameController.PerderVida(1);
            if (_GameController._vidasPlayer <=0)
            {
                Debug.Log("Fim do Jogo");
                _GameController._txtVidas.text = "0";
            }
            Debug.Log("Tocou no Obstaculo");
            _CameraShaker.ShakeIt();
            
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
        Debug.Log("Obstaculo foi destruído");
    }
}
