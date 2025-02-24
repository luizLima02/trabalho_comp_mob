using System;
using UnityEngine;
using TMPro;
using System.IO;
using System.Net.NetworkInformation;
using NUnit.Framework;
using System.Xml.Linq;
using Unity.VisualScripting;

public class LoginManager : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField userinputField;
    [SerializeField]
    private TMP_InputField passwordInputField;
    [SerializeField]
    private TMP_Text system_error;

    private void Awake()
    {
        Camera.main.aspect = (float)Screen.width / (float)Screen.height;
        string path = "Assets/BACKEND/USUARIOS/LOGGED_USER.txt";
        StreamReader reader = new StreamReader(path);
        string line;
        string scene_name = "";
        while ((line = reader.ReadLine()) != null)
        {
            if (line.Equals("[]"))
            {
                continue;
            }
            else
            {
                string pathUSR = $"Assets/BACKEND/USUARIOS/{line.Trim()}.usr";
                if (File.Exists(pathUSR))
                {
                    //escreve para "Assets/BACKEND/USUARIOS/CURRENT_PET.txt";
                    Debug.Log("existe");
                    write_current_pet(pathUSR);
                    scene_name = "Home Scene";
                }
                else
                {
                    scene_name = "Choose Scene";
                }
            }
        }
        reader.Close();
        if (scene_name != "")
        {
            SceneController.Instance.LoadSpecificScene(scene_name);
        }

    }

    //da pra usar um inteiro e operacoes binarias nele
    private void write_current_pet(string pathUSR)
    {
        //abre o arquivo usr no modo de leitura
        StreamReader reader = new StreamReader(pathUSR);
        //abre o arquivo "Assets/BACKEND/USUARIOS/CURRENT_PET.txt" no modo de escrita
        string path = "Assets/BACKEND/USUARIOS/CURRENT_PET.txt";
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine("[pet]");
        string moves = "[moves]\n";
        string line;
        bool in_pets = false;
        bool in_moves = false;
        bool in_ingame = false;
        while ((line = reader.ReadLine()) != null)
        {
            if (line.Equals("[user]") || line.Equals("[hats]")) { in_pets = false; in_moves = false; in_ingame = false; }
            else if (line.Equals("[pets]")) { in_pets = true; in_moves = false; in_ingame = false; }
            else if (line.Equals("[moves]")) { in_pets = false; in_moves = true; in_ingame = false; }
            else if (line.Equals("[ingame]")) { in_pets = false; in_moves = false; in_ingame = true; }
            else if (line != string.Empty)
            {
                if (in_pets || in_ingame)
                {
                    writer.WriteLine(line);
                }
                else if (in_moves)
                {
                    moves += line + "\n";
                }
            }
        }
        writer.WriteLine(moves);
        writer.Close();
        reader.Close();
    }

    public void Login()
    {
        bool userFieldFilled = false;
        bool passwordFieldFilled = false;

        if (userinputField != null)
        {
            if (userinputField.text != string.Empty)
            {
                userFieldFilled = true;
                Debug.Log("User: " + userinputField.text);
            }
        }
        if (passwordInputField != null)
        {
            if (passwordInputField.text != string.Empty)
            {
                passwordFieldFilled = true;
                Debug.Log("Password: " + passwordInputField.text);
            }

        }
        if (userFieldFilled && passwordFieldFilled)
        {
            /*Realiza o login*/
            if (system_error != null) { system_error.text = ""; }
            if (verifica_login(userinputField.text, passwordInputField.text))
            {
                string path = $"Assets/BACKEND/USUARIOS/{userinputField.text}.usr";
                //Escreve para LOGGED_USER
                logaUser(userinputField.text);
                if (File.Exists(path))
                {
                    Debug.Log("existe");
                    write_current_pet(path);
                    SceneController.Instance.LoadSpecificScene("Home Scene");
                }
                else
                {
                    SceneController.Instance.LoadSpecificScene("Choose Scene");
                }

            }
            else
            {
                system_error.text = "Erro de Login";
            }

        }/*Mostra mensagem de erro*/
        else if (userFieldFilled == false)
        {
            if (system_error != null) { system_error.text = "Usuario Vazio"; }
        }
        else if (passwordFieldFilled == false)
        {
            if (system_error != null) { system_error.text = "Password Vazio"; }
        }
    }
    private void logaUser(string username)
    {
        string path = "Assets/BACKEND/USUARIOS/LOGGED_USER.txt";
        StreamWriter writerusuario = new StreamWriter(path, false);
        writerusuario.WriteLine(username);
        writerusuario.Close();
    }
    private bool verifica_login(string username, string password)
    {
        //abre o arquivo UsuariosDB
        string path = "Assets/BACKEND/UsuariosDB.txt";
        if (!File.Exists(path)) { File.Create(path); }
        StreamReader reader = new StreamReader(path);
        string line;
        bool isInsideDictionary = false;
        bool usernameCorrect = false;
        bool passwordCorrect = false;
        while ((line = reader.ReadLine()) != null)
        {
            if (line == "[]") { reader.Close(); return true; }
            if (line.Contains("[")) { isInsideDictionary = true; }
            else if (line.Contains("]")) { isInsideDictionary = false; usernameCorrect = false; passwordCorrect = false; }
            else if (isInsideDictionary)
            {
                //procura os campos usuario e senha
                string[] parts = line.Split(":");
                if (parts.Length == 2)
                {
                    string campo = parts[0].Trim();
                    //campo usuario
                    if (campo.Equals("usuario"))
                    {
                        string valor = parts[1].Trim();
                        if (valor.Equals(username))
                        {
                            usernameCorrect = true;
                        }
                        //campo senha
                    }
                    else if (campo.Equals("senha"))
                    {
                        string valor = parts[1].Trim();
                        if (valor.Equals(password))
                        {
                            passwordCorrect = true;
                        }
                    }
                }
                if (usernameCorrect && passwordCorrect)
                {
                    reader.Close();
                    return true;
                }
            }
        }
        reader.Close();
        return false;
    }
    public void switch_to_Cad()
    {
        if (SceneController.Instance != null)
        {
            Debug.Log("Cadastro Button");
            //SceneController.Instance.LoadPreviousScene();
            //Cadast Scene
            Debug.Log(SceneController.Instance.get_size_pilha().ToString());
            SceneController.Instance.LoadScene("Cadast Scene");
        }
    }


}
