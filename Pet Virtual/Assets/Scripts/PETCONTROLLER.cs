using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using NUnit.Framework.Constraints;
using TMPro;
using UnityEngine;

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

    void Awake()
    {
        pet_state = PET_STATE.AWAKE;
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
        if (this.fome <= 80){ if (hungry_obj != null) { hungry_obj.SetActive(true);} }
        else{ if (hungry_obj != null) { hungry_obj.SetActive(false); } }
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
