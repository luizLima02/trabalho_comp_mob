using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using NUnit.Framework.Constraints;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;
using Vector3 = UnityEngine.Vector3;

public enum PET_STATE
{
    AWAKE,
    SLEEP,
    ANGRY,
    HAPPY,
    CHILL
}
public class PETCONTROLLER : MonoBehaviour
{
    //canvas
    [Header("TEXT AREA")]
    [SerializeField]
    private TMP_Text pet_name_text;
    //scriptable
    private Pet pet;
    private string escolhido = "";
    [Header("SPRITE AREA")]
    [SerializeField]
    SpriteRenderer Body;
    [SerializeField]
    SpriteRenderer Hat;
    [Header("OVERLAY AREA")]
    [SerializeField]
    private GameObject sleep_obj_overlay;
    [Header("ESTADOS AREA")]
    //icones de estado
    [SerializeField]
    private GameObject tired_obj;
    [SerializeField]
    private GameObject hungry_obj;
    [SerializeField]
    private GameObject sleepy_obj;
    [SerializeField]
    private GameObject sick_obj;
    //info pet
    private int hat_id;
    private string nome;
    //status
    private int heath;
    private int stamina;
    private int atk;
    private int spd;
    private int using_move;
    private List<MOVES> learnedMoves;
    //in game
    private int currentLife;
    private int currentStam;
    //care status
    private int paciencia;
    private int disciplina;
    private int felicidade;
    private int fome;
    private int toilet;
    private int saude;
    private int afeto;
    private int higiene;
    //estado do PET
    private PET_STATE pet_state;
    //running variavles
    private float tempoParaMudarDirecao = 0;
    private float contadorTempo = 0;
    private float speed_move = 0.5f;
    private Vector3 move_dir = Vector3.left;
    private Vector3 limiteMinimo = new Vector3(-1.7f, 0.7f);
    private Vector3 limiteMaximo = new Vector3(1.7f, 3f);
    /**/
    bool awake_rodando = false;
    bool sleep_rodando = false;

    public float Range(double minimum, double maximum)
    {
        Random random = new();
        return (float)(random.NextDouble() * (maximum - minimum) + minimum);
    }

    void Awake()
    {
        //pet_state = PET_STATE.AWAKE;
        tempoParaMudarDirecao = Range(1f, 5f);
        this.learnedMoves = new List<MOVES>();
        CarregarPet();
        pet = Resources.Load<Pet>($"PETS/{escolhido}");
        if (pet != null)
        {
            Body.sprite = pet.aparencia;
            Hat.sprite = null;
            //seta os valores no canva
            if (pet_name_text != null) { pet_name_text.text = this.nome; }
            if (tired_obj != null) { tired_obj.SetActive(false); }
            if (hungry_obj != null) { hungry_obj.SetActive(false); }
            if (sleepy_obj != null) { sleepy_obj.SetActive(false); }
            if (sick_obj != null) { sick_obj.SetActive(false); }
        }
        else
        {
            Debug.LogError("Falha ao carregar o ScriptableObject!");
        }

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (pet_state == PET_STATE.AWAKE)
        {

            //esta com fome
            if (this.fome >= 70) { if (hungry_obj != null) { hungry_obj.SetActive(true); } }
            else { if (hungry_obj != null) { hungry_obj.SetActive(false); } }
            //estou doente
            if (this.saude <= 40) { if (sick_obj != null) { sick_obj.SetActive(true); } }
            else { if (sick_obj != null) { sick_obj.SetActive(false); } }
            //estou cansado
            if (this.currentStam <= this.stamina / 4) { if (tired_obj != null) { tired_obj.SetActive(true); } }
            else { if (tired_obj != null) { tired_obj.SetActive(false); } }
            //estou com sono
            if (this.currentLife <= this.heath / 4) { if (sleepy_obj != null) { sleepy_obj.SetActive(true); } }
            else { if (sleepy_obj != null) { sleepy_obj.SetActive(false); } }

            //se a stamina zerar, dorme
            if (this.currentStam <= 0)
            {
                StopCoroutine(Pet_Acordado());
                StartCoroutine(Pet_Dormindo());
                this.pet_state = PET_STATE.SLEEP;
                this.currentStam = 0;
                if (sleep_obj_overlay != null)
                {
                    sleep_obj_overlay.SetActive(true);
                }
                Save_pet();
            }
        }
        /*vida do pet*/
        else if (pet_state == PET_STATE.SLEEP)
        {
            if (this.currentStam >= this.stamina)
            {
                this.pet_state = PET_STATE.AWAKE;
                StartCoroutine(Pet_Acordado());
                StopCoroutine(Pet_Dormindo());
                if (sleep_obj_overlay != null)
                {
                    sleep_obj_overlay.SetActive(false);

                }
                Save_pet();
            }

        }
    }
    private void Update()
    {
        if (awake_rodando == false && pet_state == PET_STATE.AWAKE)
        {
            StartCoroutine(Pet_Acordado());
        }
        else if (sleep_rodando == false && pet_state == PET_STATE.SLEEP)
        {
            StartCoroutine(Pet_Dormindo());
        }

        if (pet_state == PET_STATE.AWAKE)
        {
            transform.position = transform.position + (move_dir * Mathf.Lerp(0.1f, this.speed_move, (float)(currentStam) / (float)(stamina)) * Time.deltaTime);
            contadorTempo += Time.deltaTime;
            // Verifica se atingiu algum limite
            if (transform.position.x <= limiteMinimo.x || transform.position.x >= limiteMaximo.x ||
                transform.position.y <= limiteMinimo.y || transform.position.y >= limiteMaximo.y)
            {
                // Recalcula a direção aleatoriamente para dentro da bounding box
                RecalcularDirecao();
                contadorTempo = 0;
                tempoParaMudarDirecao = Range(1f, 5f);
            }
            if (contadorTempo >= tempoParaMudarDirecao)
            {
                // Recalcula a direção aleatoriamente
                RecalcularDirecao();
                // Reseta o contador de tempo
                contadorTempo = 0f;
                // Define um novo tempo aleatório para mudar a direção
                tempoParaMudarDirecao = Range(1f, 5f);
            }
        }

    }
    /*setters*/



    public void Petpet()
    {
        if (paciencia < 100) { paciencia++; }
        if (afeto < 100) { afeto++; }
        Save_pet();
    }

    public void Cansar(int qnt) 
    { 
        this.currentStam -= qnt;
        if (this.currentStam < 0) { this.currentStam = 0; }
        if (this.currentStam > this.stamina) { this.currentStam = this.stamina; }
    }
    public void Disciplinate()
    {
        if (pet_state != PET_STATE.SLEEP)
        {
            if (felicidade > 0)
            {
                felicidade -= 3;
            }
            if (disciplina < 100)
            {
                disciplina += 2;
            }
            Save_pet();
        }
    }

    public void Praise()
    {
        if (pet_state != PET_STATE.SLEEP)
        {
            if (disciplina > 0) { disciplina -= 3; }
            if (felicidade < 100) { felicidade += 2; }
            Save_pet();
        }
    }

    public string Stats_pet()
    {
        string s = $"Health: {this.currentLife}/{this.heath}\n" +
                   $"Stamina: {this.currentStam}/{this.stamina}\n" +
                   $"ATK: {this.atk}\n" +
                   $"SPEED: {this.spd}\n";
        if (afeto >= 0 && afeto < 50) { s += $"{this.nome} esta indiferente\n"; }
        else if (afeto >= 50 && afeto < 100) { s += $"{this.nome} gosta de voce\n"; }
        else { s += $"{this.nome} te ama\n"; }
        if (pet_state == PET_STATE.SLEEP)
        {
            s += $"{this.nome} esta dormindo\n";
            s += $"{this.nome} ira acordar quando recuperar sua stamina\n";
        }
        return s;
    }

    private void RecalcularDirecao()
    {
        float anguloAleatorio = Range(0f, 360f);
        move_dir = new Vector3(Mathf.Cos(anguloAleatorio * Mathf.Deg2Rad), Mathf.Sin(anguloAleatorio * Mathf.Deg2Rad), 0f);

        // Garante que a nova direção é para dentro da bounding box
        if (transform.position.x <= limiteMinimo.x && move_dir.x < 0)
            move_dir.x = -move_dir.x;
        if (transform.position.x >= limiteMaximo.x && move_dir.x > 0)
            move_dir.x = -move_dir.x;
        if (transform.position.y <= limiteMinimo.y && move_dir.y < 0)
            move_dir.y = -move_dir.y;
        if (transform.position.y >= limiteMaximo.y && move_dir.y > 0)
            move_dir.y = -move_dir.y;
    }

    public void Save_pet()
    {
        string pathPet = "Assets/BACKEND/USUARIOS/CURRENT_PET.txt";
        StreamWriter writerPet = new StreamWriter(pathPet, false);
        writerPet.WriteLine("[pet]");
        writerPet.WriteLine($"escolhido:{this.escolhido}");
        writerPet.WriteLine($"nome:{this.nome}");
        writerPet.WriteLine($"health:{this.heath}");
        writerPet.WriteLine($"stamina:{this.stamina}");
        writerPet.WriteLine($"ataque:{this.atk}");
        writerPet.WriteLine($"speed:{this.spd}");
        writerPet.WriteLine($"usingMove:{this.using_move}");
        //in game
        writerPet.WriteLine($"currentLife:{this.currentLife}");
        writerPet.WriteLine($"currentStam:{this.currentStam}");
        //care
        writerPet.WriteLine($"paciencia:{this.paciencia}");
        writerPet.WriteLine($"disciplina:{this.disciplina}");
        writerPet.WriteLine($"felicidade:{this.felicidade}");
        writerPet.WriteLine($"fome:{this.fome}");
        writerPet.WriteLine($"toilet:{this.toilet}");
        writerPet.WriteLine($"saude:{this.saude}");
        writerPet.WriteLine($"afeto:{this.afeto}");
        writerPet.WriteLine($"higiene:{this.higiene}");
        writerPet.WriteLine($"hat:{this.hat_id}");
        writerPet.WriteLine($"state:{this.pet_state}");
        //moves
        writerPet.WriteLine("[moves]");
        foreach (MOVES m in this.learnedMoves)
        {
            writerPet.WriteLine(m.ToString());
        }
        writerPet.Close();
    }

    private void CarregarPet()
    {
        //abre o arquivo CURRENT_PET
        string pathPet = "Assets/BACKEND/USUARIOS/CURRENT_PET.txt";
        StreamReader readPet = new StreamReader(pathPet);
        bool inPET = false;
        bool inMoves = false;
        string line;
        while ((line = readPet.ReadLine()) != null)
        {
            if (line.Equals("[pet]"))
            {
                inPET = true;
                inMoves = false;
            }
            else if (line.Equals("[moves]"))
            {
                inPET = false;
                inMoves = true;
            }
            else if (line != string.Empty)
            {
                if (inPET)
                {
                    string[] parts = line.Split(":");
                    if (parts.Length == 2)
                    {
                        string campo = parts[0].Trim();
                        string valor = parts[1].Trim();
                        setar_campo(campo.ToLower(), valor);
                    }
                }
                if (inMoves)
                {
                    string move = line.Trim();
                    MOVES m = (MOVES)Enum.Parse(typeof(MOVES), move);
                    this.learnedMoves.Add(m);
                }
            }
        }
        readPet.Close();
    }

    //dava para usar um dicionario para as variaveis
    void setar_campo(string campo, string valor)
    {
        //info pet
        if (campo.Equals("escolhido"))
        {
            this.escolhido = valor.ToUpper();
            return;
        }
        if (campo.Equals("nome"))
        {
            this.nome = valor;
            return;
        }
        //status
        if (campo.Equals("health"))
        {
            this.heath = int.Parse(valor);
            return;
        }
        if (campo.Equals("stamina"))
        {
            this.stamina = int.Parse(valor);
            return;
        }
        if (campo.Equals("ataque"))
        {
            this.atk = int.Parse(valor);
            return;
        }
        if (campo.Equals("speed"))
        {
            this.spd = int.Parse(valor);
            return;
        }
        if (campo.Equals("usingmove"))
        {
            this.using_move = int.Parse(valor);
            return;
        }
        //in game
        if (campo.Equals("currentlife"))
        {
            this.currentLife = int.Parse(valor);
            return;
        }
        if (campo.Equals("currentstam"))
        {
            this.currentStam = int.Parse(valor);
            return;
        }
        //care status
        if (campo.Equals("paciencia"))
        {
            this.paciencia = int.Parse(valor);
            return;
        }
        if (campo.Equals("disciplina"))
        {
            this.disciplina = int.Parse(valor);
            return;
        }
        if (campo.Equals("felicidade"))
        {
            this.felicidade = int.Parse(valor);
            return;
        }
        if (campo.Equals("fome"))
        {
            this.fome = int.Parse(valor);
            return;
        }
        if (campo.Equals("toilet"))
        {
            this.toilet = int.Parse(valor);
            return;
        }
        if (campo.Equals("saude"))
        {
            this.saude = int.Parse(valor);
            return;
        }
        if (campo.Equals("afeto"))
        {
            this.afeto = int.Parse(valor);
            return;
        }
        if (campo.Equals("higiene"))
        {
            this.higiene = int.Parse(valor);
            return;
        }
        if (campo.Equals("hat"))
        {
            this.hat_id = int.Parse(valor);
            return;
        }
        if (campo.Equals("state"))
        {
            this.pet_state = (PET_STATE)Enum.Parse(typeof(PET_STATE), valor);
            return;
        }
    }

    //getters
    public int Get_hat_id() => this.hat_id;
    public string Get_escolhido() => this.escolhido;
    public string Get_name() => this.nome;
    public int Get_health() => this.heath;
    public int Get_stamina() => this.stamina;


    public int Get_atk() => this.atk;
    public int Get_spd() => this.spd;
    public int Get_using_move() => this.using_move;
    public List<MOVES> Get_learnedMoves() => this.learnedMoves;
    //in game
    public int Get_currentLife() => this.currentLife;
    public int Get_currentStam() => this.currentStam;
    //care status
    public int Get_paciencia() => this.paciencia;
    public int Get_disciplina() => this.disciplina;
    public int Get_felicidade() => this.felicidade;
    public int Get_fome() => this.fome;
    public int Get_toilet() => this.toilet;
    public int Get_saude() => this.saude;
    public int Get_afeto() => this.afeto;
    public int Get_higiene() => this.higiene;
    public PET_STATE Get_petstate() { return this.pet_state; }


    //co rotines
    IEnumerator Pet_Acordado()
    {
        awake_rodando = true;
        sleep_rodando = false;
        while (pet_state != PET_STATE.SLEEP)
        {
            // Aguarda 1 segundo
            yield return new WaitForSeconds(100f);

            //diminui a stamina

            if (this.currentStam > 0) { this.currentStam--; }
            //aumenta a fome

            if (this.fome < 100) { this.fome++; }
            Debug.Log($"Current Stamina: {this.currentStam}");
            Save_pet();
        }
    }

    IEnumerator Pet_Dormindo()
    {
        awake_rodando = false;
        sleep_rodando = true;
        while (pet_state == PET_STATE.SLEEP)
        {
            // Aguarda 1 segundo
            yield return new WaitForSeconds(50f);

            //diminui a stamina

            if (this.currentStam < this.stamina) { this.currentStam++; }

            Debug.Log($"Current Stamina: {this.currentStam}");
            Save_pet();
        }
    }

    // Este método é chamado quando o aplicativo ganha ou perde foco
    void OnApplicationFocus(bool hasFocus)
    {
        Save_pet();
    }

    // Este método é chamado quando o aplicativo é encerrado
    void OnApplicationQuit()
    {
        Save_pet();
    }
}
