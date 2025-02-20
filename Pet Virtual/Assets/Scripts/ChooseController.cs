using UnityEngine;
using TMPro;
using System.IO;
using NUnit.Framework;
using UnityEngine.InputSystem;

public class ChooseController : MonoBehaviour
{
    [Header("Overlay")]
    [SerializeField]
    private GameObject choose_overlay;
    [SerializeField]
    private GameObject name_overlay;
    [Header("text fields")]
    [SerializeField]
    private TMP_Text choose_overlay_text;
    [SerializeField]
    private TMP_InputField pet_name_field;
    
    private string choosenPet;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Camera.main.aspect = (float)Screen.width / (float)Screen.height;
        if (choose_overlay != null)
            choose_overlay.SetActive(false);
        if (name_overlay != null)
            name_overlay.SetActive(false);
    }

    public void choose_oxl()
    {
        choosenPet = "OXL";
        choose_overlay_text.text = "Are your sure of your choice? OXL";
        if (choose_overlay != null)
            choose_overlay.SetActive(true);
    }

    public void choose_cat()
    {
        choosenPet = "CAT";
        choose_overlay_text.text = "Are your sure of your choice? CAT";
        if (choose_overlay != null)
            choose_overlay.SetActive(true);
    }

    public void choose_dog()
    {
        choosenPet = "DOG";
        choose_overlay_text.text = "Are your sure of your choice? DOG";
        if (choose_overlay != null)
            choose_overlay.SetActive(true);
    }

    public void cyes()
    {
        if (choose_overlay != null)
            choose_overlay.SetActive(false);
        if (name_overlay != null)
            name_overlay.SetActive(true);
    }

    public void confirm()
    {
        if (choosenPet.Equals("") || pet_name_field == null) 
        {//error 
            return;
        }
        if(pet_name_field.text == string.Empty) {  return; }
        //confirma a selecao
        //pega o usuario do arquivo LOGGED_USER.txt
        string usuario = getUser();
        //carrego o choosenPet
        Pet pet_escolhido = Resources.Load<Pet>($"PETS/{choosenPet}");
        PetGerado petG;
        if (pet_escolhido != null)
        {
            //gero o choosenPet
            petG = pet_escolhido.GeneratePet(pet_name_field.text);
        }
        else
        {
            Debug.LogError("Falha ao carregar o ScriptableObject!");
            return;
        }
        //cria arquivo usr e CURRENT_USER
        cria_usr(usuario, petG);
        if (SceneController.Instance != null)
        {
            SceneController.Instance.LoadSpecificScene("Home Scene");
        }
        else
        {
            SceneController.Instance.LoadSpecificScene("Login Scene");
        }
    }

    void cria_usr(string usuario, PetGerado petG)
    {
        string path = $"Assets/BACKEND/USUARIOS/{usuario}.usr";
        StreamWriter writerusuario = new StreamWriter(path, true);
        writerusuario.WriteLine("[user]");
        writerusuario.WriteLine("Coins:10");
        writerusuario.WriteLine("[hats]");
        writerusuario.WriteLine("{}");
        //escreve o pet
        writerusuario.WriteLine("[pets]");
        writerusuario.WriteLine($"Escolhido:{choosenPet}");
        writerusuario.WriteLine($"Nome:{petG.nome}");
        writerusuario.WriteLine($"Health:{petG.health}");
        writerusuario.WriteLine($"Stamina:{petG.stamina}");
        writerusuario.WriteLine($"Ataque:{petG.atk}");
        writerusuario.WriteLine($"Speed:{petG.spd}");
        writerusuario.WriteLine($"UsingMove:{petG.using_move}");
        //escreve os moves
        writerusuario.WriteLine("[moves]");
        foreach (MOVES m in petG.learnedMoves)
        {
            writerusuario.WriteLine(m.ToString());
        }
        writerusuario.WriteLine("[ingame]");
        writerusuario.WriteLine("{}");
        writerusuario.Close();
        //escrevo no CURRENT_PET
        string pathPet = "Assets/BACKEND/USUARIOS/CURRENT_PET.txt";
        StreamWriter writerPet = new StreamWriter(pathPet, false);
        writerPet.WriteLine("[pet]");
        writerPet.WriteLine($"escolhido:{choosenPet}");
        writerPet.WriteLine($"nome:{petG.nome}");
        writerPet.WriteLine($"health:{petG.health}");
        writerPet.WriteLine($"stamina:{petG.stamina}");
        writerPet.WriteLine($"ataque:{petG.atk}");
        writerPet.WriteLine($"speed:{petG.spd}");
        writerPet.WriteLine($"usingMove:{petG.using_move}");
        //in game
        writerPet.WriteLine($"currentLife:{petG.health}");
        writerPet.WriteLine($"currentStam:{petG.stamina}");
        //care
        writerPet.WriteLine($"paciencia:{100}");
        writerPet.WriteLine($"disciplina:{50}");
        writerPet.WriteLine($"felicidade:{50}");
        writerPet.WriteLine($"fome:{0}");
        writerPet.WriteLine($"toilet:{0}");
        writerPet.WriteLine($"saude:{100}");
        writerPet.WriteLine($"afeto:{0}");
        writerPet.WriteLine($"higiene:{100}");
        writerPet.WriteLine($"hat:{0}");
        writerPet.WriteLine($"state:{0}");
        //moves
        writerPet.WriteLine("[moves]");
        foreach (MOVES m in petG.learnedMoves)
        {
            writerPet.WriteLine(m.ToString());
        }
        writerPet.Close();
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

    public void cancel_choose()
    {
        if (choose_overlay != null)
            choose_overlay.SetActive(false);
        if (name_overlay != null)
            name_overlay.SetActive(false);
        choosenPet = "";
    }
}
