using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


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

    [Header("Configuração do coletável")]
    public float        _coletavelVelocidade;
    public GameObject   _coletavelPrefab;
    public float        _coletavelTempo;

    [Header("Configuração de UI")]
    public int          _pontosPlayer;
    public Text         _txtPontos;
    public int          _vidasPlayer;
    public Text         _txtVidas;
    public Text         _txtMetros;

    [Header("Controle de Distância")]
    public int          _metrosPercorridos = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        StartCoroutine("SpawnObstaculo");
        StartCoroutine("SpawnColetavel");
        InvokeRepeating("DistanciaPercorrida", 0f, 0.2f);

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

        yield return new WaitForSeconds(1.5f);

        StartCoroutine("SpawnColetavel");
    }

    IEnumerator SpawnColetavel()
    {

        int randomStars = Random.Range(1, 5);
        for (int contagem = 1; contagem <= randomStars; contagem++)
        {
            yield return new WaitForSeconds(_coletavelTempo);
            GameObject _objetoSpawn = Instantiate(_coletavelPrefab);
            _objetoSpawn.transform.position = new Vector3(_objetoSpawn.transform.position.x, _objetoSpawn.transform.position.y, 0);
        }

    }

    public void Pontos(int _qtdPontos)
    {
        _pontosPlayer += _qtdPontos;
        _txtPontos.text = _pontosPlayer.ToString();
    }

    public void PerderVida(int _qtdVidas)
    {
        _vidasPlayer -= _qtdVidas;
        _txtVidas.text = _vidasPlayer.ToString();
    }

    void DistanciaPercorrida()
    {
        _metrosPercorridos++;
        _txtMetros.text = _metrosPercorridos.ToString() + " M";

        if ((_metrosPercorridos % 100) == 0)
        {
            _chaoVelocidade += 100f;
            _ObstaculoTempo -= 0.15f;
            _ObstaculoVelocidade += 150f;
        }
    }

    public void VoltarAoMenu()
    {
        if(SceneController.Instance != null)
        {
            SceneController.Instance.LoadPreviousScene();
        }
    }

}
