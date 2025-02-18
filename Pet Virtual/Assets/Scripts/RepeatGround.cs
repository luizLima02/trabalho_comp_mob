using UnityEngine;

public class RepeatGround : MonoBehaviour
{

    private GameController _gameController;

    public bool _chaoInstanciado = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gameController = FindFirstObjectByType(typeof(GameController)) as GameController;
    }

    // Update is called once per frame
    void Update()
    {
        if (_chaoInstanciado == false)
        {
            if (transform.position.x <= 0)
            {
                _chaoInstanciado = true;
                GameObject ObjetoTemporarioChao = Instantiate(_gameController._chaoPrefab);
                ObjetoTemporarioChao.transform.position = new Vector3(transform.position.x + _gameController._chaoTamanho + 7000, transform.position.y, 0);

                Debug.Log("O chão foi instanciado");
            }
        }

        if (transform.position.x < _gameController._chaoDestruido) // -5000
        {
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        MoveGround();
    }

    void MoveGround()
    {
        transform.Translate(Vector2.left * _gameController._chaoVelocidade * Time.deltaTime);
    }
}
