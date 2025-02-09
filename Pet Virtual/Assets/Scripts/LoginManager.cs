using System;
using UnityEngine;
using TMPro;

public class LoginManager : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField userinputField;
    [SerializeField]
    private TMP_InputField passwordInputField;
    [SerializeField]
    private TMP_Text system_error;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Login()
    {
        bool userFieldFilled = false;
        bool passwordFieldFilled = false;

        if (userinputField != null) {
            if (userinputField.text != string.Empty) {
                userFieldFilled = true;
                Debug.Log("User: "+ userinputField.text);
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
        if(userFieldFilled && passwordFieldFilled)
        {
            /*Realiza o login*/
            if (system_error != null) { system_error.text = ""; }
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

    public void switch_to_Cad()
    {
        Debug.Log("Cadastro Button");
    }
}
