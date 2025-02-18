using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PLAYERCONTROLER : MonoBehaviour
{
    //canva
    [Header("TEXT AREA")]
    [SerializeField]
    private TMP_Text coins_text;
    [SerializeField]
    private TMP_Text feedBack_text;
    private InputSystem_Actions inputActions;

    //info player
    private string userName;
    private int coins;
    private List<int> hats_ids;
    [SerializeField]
    private PETCONTROLLER petController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        inputActions = new InputSystem_Actions();
        hats_ids = new List<int>();
        carregar_usuario();
        if(coins_text != null) { coins_text.text = coins.ToString(); }
        if (feedBack_text != null) { feedBack_text.text = ""; }
    }

    private void FixedUpdate()
    {
        if (coins_text != null) {
            if (coins_text.text.Equals(coins.ToString()) == false)
            {
                coins_text.text = coins.ToString();
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void carregar_usuario()
    {
        this.userName = getUser();
        set_Vals();
    }

    public void save_values()
    {
        if (petController != null)
        {
            string pathUSR = $"Assets/BACKEND/USUARIOS/{this.userName}.usr";
            List<string> linhas = new List<string>(File.ReadAllLines(pathUSR));
            string saida = "";
            bool inMoves = false, inHats = false, inGame = false;
            for(int i = 0; i < linhas.Count; i++)
            {
                string line = linhas[i].Trim();
                if (line.Equals("[pets]") || line.Equals("[user]")) { saida += line + "\n"; inMoves = false; inHats = false; inGame = false; }
                else if (line.Equals("[moves]")) { saida += line + "\n"; inMoves = true; inHats = false; inGame = false; }
                else if (line.Equals("[hats]")) { saida += line + "\n"; inMoves = false; inHats = true; inGame = false; }
                else if (line.Equals("[ingame]")) { saida += line + "\n"; inMoves = false; inHats = false; inGame = true; }
                //valores variaveis / possiveis
                else if (inMoves){
                     foreach(MOVES m in petController.Get_learnedMoves())
                     {
                            saida += m.ToString() + "\n";
                     }
                     inMoves = false;
                }
                else if (inHats) 
                {
                    foreach (int m in hats_ids)
                    {
                        saida += m.ToString() + "\n";
                    }
                    if (hats_ids.Count < 1)
                    {
                        saida += "{}\n";
                    }
                }
                else if (inGame) 
                {
                    if (line.Equals("{}"))
                    {
                        saida += $"currentLife:{petController.Get_currentLife()}" +
                                 $"\ncurrentStam:{petController.Get_currentStam()}" +
                                 $"\npaciencia:{petController.Get_paciencia()}" +
                                 $"\ndisciplina:{petController.Get_disciplina()}" +
                                 $"\nfelicidade:{petController.Get_felicidade()}" +
                                 $"\nfome:{petController.Get_fome()}" +
                                 $"\ntoilet:{petController.Get_toilet()}" +
                                 $"\nsaude:{petController.Get_saude()}" +
                                 $"\nafeto:{petController.Get_afeto()}" +
                                 $"\nhigiene:{petController.Get_higiene()}" +
                                 $"\nhat:{petController.Get_hat_id()}" +
                                 $"\nstate:{petController.Get_petstate()}";
                    }
                    else
                    {
                        if(line.Contains("currentLife")){ saida += $"currentLife:{petController.Get_currentLife()}\n"; }
                        else if (line.Contains("currentStam")) { saida += $"currentStam:{petController.Get_currentStam()}\n"; }
                        else if (line.Contains("paciencia")) { saida += $"paciencia:{petController.Get_paciencia()} \n"; }
                        else if (line.Contains("disciplina")) { saida += $"disciplina:{petController.Get_disciplina()} \n";}
                        else if (line.Contains("felicidade")) { saida += $"felicidade:{petController.Get_felicidade()} \n"; }
                        else if (line.Contains("fome")) { saida += $"fome:{petController.Get_fome()} \n"; }
                        else if (line.Contains("toilet")) { saida += $"toilet:{petController.Get_toilet()} \n"; }
                        else if (line.Contains("saude")) { saida += $"saude:{petController.Get_saude()} \n"; }
                        else if (line.Contains("afeto")) { saida += $"afeto:{petController.Get_afeto()} \n"; }
                        else if (line.Contains("higiene")) { saida += $"higiene:{petController.Get_higiene()} \n"; }
                        else if (line.Contains("hat")) { saida += $"hat:{petController.Get_hat_id()} \n"; }
                        else if (line.Contains("state")) { saida += $"state:{petController.Get_petstate()} \n"; }
                    }
                }
                //valores certos
                else if (line.Contains("Coins")) { saida += "Coins:" + this.coins.ToString() + "\n"; }
                else if (line.Contains("Escolhido")) { saida += "Escolhido:" + petController.Get_escolhido() + "\n"; }
                else if (line.Contains("Nome")) { saida += "Nome:" + petController.Get_name() + "\n"; }
                else if (line.Contains("Health")) { saida += "Health:" + petController.Get_health() + "\n"; }
                else if (line.Contains("Stamina")) { saida += "Stamina:" + petController.Get_stamina() + "\n"; }
                else if (line.Contains("Ataque")) { saida += "Ataque:" + petController.Get_atk() + "\n"; }
                else if (line.Contains("Speed")) { saida += "Speed:" + petController.Get_spd() + "\n"; }
                else if (line.Contains("UsingMove")) { saida += "UsingMove:" + petController.Get_using_move() + "\n"; }
            }
            //File.WriteAllLines(pathUSR, linhas);
            StreamWriter writer = new StreamWriter(pathUSR, false);
            writer.WriteLine(saida);
            writer.Close();
        }
        else { return; }
    }

   

    public void deslogar()
    {
        save_values();
        StreamWriter writerPet = new StreamWriter("Assets/BACKEND/USUARIOS/CURRENT_PET.txt", false);
        StreamWriter writerUser = new StreamWriter("Assets/BACKEND/USUARIOS/LOGGED_USER.txt", false);
        writerUser.WriteLine("[]");
        writerPet.WriteLine("[]");
        writerUser.Close();
        writerPet.Close();
        if (SceneController.Instance != null) {
            SceneController.Instance.LoadSpecificScene("Login Scene");
        }
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Interact.started += interagir;
    }
    private void OnDisable()
    {
        inputActions.Disable();
        inputActions.Player.Interact.started -= interagir;
    }

    public void interagir(InputAction.CallbackContext context)
    {
        deslogar();
        //coins++;
        //save_values();
    }

    public void set_Vals()
    {
        string path = $"Assets/BACKEND/USUARIOS/{this.userName}.usr";
        StreamReader reader = new StreamReader(path);
        string line;
        bool inUser = false;
        bool inHats = false;
        while ((line = reader.ReadLine()) != null)
        {
            if (line.Equals("[user]")){ inUser = true; inHats = false; }
            else if(line.Equals("[hats]")){ inUser = false; inHats = true; }
            else if (line.Equals("[pets]")) { reader.Close(); return; }
            else if (line != string.Empty)
            {
                if (inUser) 
                {
                    string[] parts = line.Split(":");
                    if (parts.Length == 2)
                    {
                        string campo = parts[0].Trim();
                        string valor = parts[1].Trim();
                        this.coins = int.Parse(valor);
                    }
                }
                else if (inHats) 
                {
                    if (line.Contains("{}"))
                    {
                        inHats = false;
                        continue;
                    }else{
                        string valor = line.Trim();
                        this.hats_ids.Add(int.Parse(valor));
                    }
                }
            }
        }
        reader.Close();
    }

    public string getUser()
    {
        string path = "Assets/BACKEND/USUARIOS/LOGGED_USER.txt";
        StreamReader reader = new StreamReader(path);
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            if (line.Equals("[]"))
            {
                reader.Close();
                return "None";
            }
            else
            {
                reader.Close();
                return line.Trim();

            }
        }
        reader.Close();
        return "None";
    }
}
