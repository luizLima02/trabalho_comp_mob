using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;

public class CadManager : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField userinputField;
    [SerializeField]
    private TMP_InputField passwordInputField;
    [SerializeField]
    private TMP_InputField confpasswordInputField;
    [SerializeField]
    private TMP_InputField emailInputField;

    private bool EULA = false;

    public void set_EULA()
    {
        if (EULA)
        {
           EULA = false;
        }
        else
        {
           EULA = true;
        }
        Debug.Log("EULA: " + EULA.ToString());
    }

    public void Cadastrar_Usuario()
    {
        
        if (userinputField != null && passwordInputField != null && confpasswordInputField != null && emailInputField != null)
        {
            if(userinputField.text != string.Empty &&
               passwordInputField.text != string.Empty &&
               confpasswordInputField.text != string.Empty &&
               emailInputField.text != string.Empty)
            {
                if(passwordInputField.text.Equals(confpasswordInputField.text) == false)
                {
                    //erro password tem que ser iguais
                    Debug.Log("Senhas diferem");
                    return;
                }
                if (inUserName(userinputField.text.ToLower()) == false)
                {
                    //erro usuario já cadastrado
                    Debug.Log("Usuario Ja cadastrado");
                    return;
                }
                if (EULA == false) {
                    //marque a EULA
                    Debug.Log("marque a EULA");
                    return;
                }
                string novoUser = $"[\nusuario:{userinputField.text}\nsenha:{passwordInputField.text}\nemail:{emailInputField.text}\n]";
                writeUser(novoUser);
                Debug.Log("usuario cadastrado");
                if (SceneController.Instance != null)
                {
                    SceneController.Instance.LoadPreviousScene();
                }
            }
            
        }
        else
        {
            Debug.Log("Inicie os Input field");
        }
           
    }

    void writeUser(string user) 
    {
        string path = "Assets/BACKEND/UsuariosDB.txt";
        if (noUsers())
        {
            StreamWriter writerusuario = new StreamWriter(path, false);
            writerusuario.WriteLine(user);
            writerusuario.Close();
        }
        else
        {
            //coloca o novo usuario no fim do arquivo
            StreamWriter writerusuario = new StreamWriter(path, true);
            writerusuario.WriteLine(user);
            writerusuario.Close();
        }
    }

    bool noUsers()
    {
        string path = "Assets/BACKEND/UsuariosDB.txt";
        StreamReader reader = new StreamReader(path);
        //variaveis para ler o arquivo
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            if (line == "[]") { reader.Close(); return true; }
            else { reader.Close(); return false; }
        }
        reader.Close();
        return false;
    }

    /*Retorna se o nome de usuario está livre*/
    bool inUserName(string username)
    {
        //abre o arquivo UsuariosDB.txt
        string path = "Assets/BACKEND/UsuariosDB.txt";
        StreamReader reader = new StreamReader(path);
        //variaveis para ler o arquivo
        string line;
        bool isInsideDictionary = false;
        //le o arquivo e procura o usarname
        while ((line = reader.ReadLine()) != null)
        {
            if(line.Equals("[]")){ reader.Close(); return true; }
            if (line.Contains("[")) { isInsideDictionary = true; }
            else if (line.Contains("]")) { isInsideDictionary = false; }
            else if (isInsideDictionary)
            {
                string[] parts = line.Split(":");
                if (parts.Length == 2) 
                {
                    string campo = parts[0].Trim();
                    if (campo.Equals("usuario"))
                    {
                        string valor = parts[1].Trim();
                        if (valor.Equals(username)){
                            reader.Close();
                            return false;
                        }
                    }
                }
            }
        }
        reader.Close();
        return true;
    }
}
