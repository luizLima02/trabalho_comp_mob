using UnityEngine;
using UnityEngine.UI;

public class GameRaceController : MonoBehaviour
{

    [Header("Atributos do background")]
    public float    _backgroundSpeed = -8f;
    public float    _instantiateInterval = 10f;

    [Header("Atributos do inimigo")]
    public float    _enemySpeed = -30f;

    [Header("Configuração de UI")]
    public Text     _txtMetros;

    [Header("Controle de Distância")]
    public int      _metrosPercorridos = 0;

    [Header("Atributos de movimento do carro")]
    public float    _deslocamentoHorizontal = 3f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("DistanciaPercorrida", 0f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DistanciaPercorrida()
    {
        _metrosPercorridos++;
        _txtMetros.text = _metrosPercorridos.ToString() + " M";

        if ((_metrosPercorridos % 100) == 0)
        {
            _backgroundSpeed -= 0.1f;
            _enemySpeed -= 0.1f;
        }
    }

    public void VoltarAoMenu()
    {
        if (SceneController.Instance != null)
        {
            SceneController.Instance.LoadPreviousScene();
        }
    }
}
