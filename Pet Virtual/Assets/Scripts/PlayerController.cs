using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float    speed = 0f;
    public bool     isGrounded = true;
    public float    jumpForce = 650f;

    private Animator anim;
    private Rigidbody2D rig;

    public LayerMask LayerGround;
    public Transform checkGround;
    public string isGroundBool = "eChao";

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        MovimentaPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) // tecla de seta pra cima pressionada
        {
            Jump();
        }
        if (Input.touchCount > 0)
        {
            // Pega o primeiro toque
            Touch touch = Input.touches[0];

            // Verifica se o toque acabou de começar
            if (touch.phase == TouchPhase.Began)
            {
                Jump();
            }
        }
    }

    private void MovimentaPlayer()
    {
        transform.Translate(new Vector3(speed, 0, 0));

    }

    private void FixedUpdate()
    {
        transform.Translate(new Vector3(speed, 0, 0));

        if (Physics2D.OverlapCircle(checkGround.transform.position, 0.2f, LayerGround))
        {
            anim.SetBool(isGroundBool, true);
            isGrounded = true;
        }
        else
        {
            anim.SetBool(isGroundBool, false);
            isGrounded = false;
        }
    }

    public void Jump()
    {
        if (isGrounded) // true
        {
            rig.linearVelocity = Vector2.zero;
            rig.AddForce(new Vector2(0, jumpForce)); // 650
        }
    }
}
