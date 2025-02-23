using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{

    // Propriedades do Chão
    [Header("Configuração do chão")]
    public float        _chaoDestruido;
    public float        _chaoTamanho;
    public float        _chaoVelocidade;
    public GameObject   _chaoPrefab;

    [Header("Configuração do obstáculo")]
    public float        _ObstaculoTempo;
    public GameObject   _ObstaculoPrefab;
    public float        _ObstaculoVelocidade;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine("SpawnObstaculo");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnObstaculo()
    {

        yield return new WaitForSeconds(_ObstaculoTempo);

        GameObject ObjetoObstaculoTemp = Instantiate(_ObstaculoPrefab);
        StartCoroutine("SpawnObstaculo");

    }
}
