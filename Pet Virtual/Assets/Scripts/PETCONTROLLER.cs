using System;
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
            if(pet_name_text != null) { pet_name_text.text = this.nome; }
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
        //esta com fome
        if (this.fome >= 70){ if (hungry_obj != null) { hungry_obj.SetActive(true);} }
        else{ if (hungry_obj != null) { hungry_obj.SetActive(false); } }
        //estou doente
        if (this.saude <= 40) { if (sick_obj != null) { sick_obj.SetActive(true); } }
        else { if (sick_obj != null) { sick_obj.SetActive(false); } }
        //estou cansado
        if (this.currentStam <= this.stamina/4) { if (tired_obj != null) { tired_obj.SetActive(true); } }
        else { if (tired_obj != null) { tired_obj.SetActive(false); } }
        //estou com sono
        if (this.currentLife <= this.heath / 4) { if (sleepy_obj != null) { sleepy_obj.SetActive(true); } }
        else { if (sleepy_obj != null) { sleepy_obj.SetActive(false); } }
    }
    private void Update()
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
    /*setters*/

    public void Petpet()
    {
        if(paciencia < 100){paciencia++;}
        if (afeto < 100){afeto++;}
    }
    
    public void Disciplinate() {
        if (felicidade > 0)
        {
            felicidade -= 3;
        }
        if (disciplina < 100)
        {
            disciplina += 2;
        }
    }

    public void Praise() { 
        if(disciplina > 0){disciplina -= 3;}
        if (felicidade < 100){felicidade += 2;}
    }

    public string Stats_pet() {
        string s = $"Health: {this.currentLife}/{this.heath}\n" +
                   $"Stamina: {this.currentStam}/{this.stamina}\n" +
                   $"ATK: {this.atk}\n" +
                   $"SPEED: {this.spd}\n";
        if(afeto >= 0 && afeto < 50) { s += $"{this.nome} esta indiferente"; }
        else if(afeto >= 50 && afeto < 100) { s += $"{this.nome} gosta de voce"; }
        else { s += $"{this.nome} te ama"; }
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
            }else if (line.Equals("[moves]"))
            {
                inPET = false;
                inMoves = true;
            }
            else if(line != string.Empty)
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
    public int Get_spd()=> this.spd;
    public int Get_using_move()=> this.using_move;
    public List<MOVES> Get_learnedMoves() => this.learnedMoves;
    //in game
    public int Get_currentLife()=> this.currentLife;
    public int Get_currentStam()=> this.currentStam;
    //care status
    public int Get_paciencia()=> this.paciencia;
    public int Get_disciplina()=> this.disciplina;
    public int Get_felicidade()=> this.felicidade;
    public int Get_fome()=> this.fome;
    public int Get_toilet()=> this.toilet;
    public int Get_saude()=> this.saude;
    public int Get_afeto()=> this.afeto;
    public int Get_higiene()=> this.higiene;
    public PET_STATE Get_petstate() { return this.pet_state; }
}
