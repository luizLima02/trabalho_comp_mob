using UnityEngine;
using UnityEngine.SceneManagement;

public class MovePlayerCar : MonoBehaviour
{

    /*[Header("Atributos de movimento do carro")]
    public float _deslocamentoHorizontal = 0.5f;*/   // DEBUG MODE


    private Vector3 lastTouchPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKey("right"))     // DEBUG MODE
        {
            transform.position += new Vector3(_deslocamentoHorizontal, 0, 0);
        }
        else if (Input.GetKey("left"))
        {
            transform.position += new Vector3(-_deslocamentoHorizontal, 0, 0);
        }*/

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    lastTouchPosition = touchPosition; // Guarda a posição inicial do toque
                    break;

                case TouchPhase.Moved:
                    // Calcula o deslocamento do toque e move o carro apenas no eixo X
                    float deltaX = touchPosition.x - lastTouchPosition.x;
                    transform.position += new Vector3(deltaX, 0, 0);
                    lastTouchPosition = touchPosition; // Atualiza a última posição do toque
                    break;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
