using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
public class SceneController : MonoBehaviour
{
    private static SceneController _instance;
    private Stack<string> sceneStack = new Stack<string>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        //inicia na tela de login
        LoadScene("Login Scene");
    }

    public int get_size_pilha()
    {
        return _instance.sceneStack.Count;
    }

    public void LoadScene(string sceneName)
    {
        // Empilha a cena atual antes de carregar a nova cena
        sceneStack.Push(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(sceneName);
    }

    public void LoadPreviousScene()
    {
        if (sceneStack.Count > 0)
        {
            string previousScene = sceneStack.Pop();
            SceneManager.LoadScene(previousScene);
        }
        else
        {
            Debug.LogWarning("Nenhuma cena anterior na pilha.");
        }
    }

    public void LoadSpecificScene(string sceneName)
    {
        sceneStack.Clear();
        SceneManager.LoadScene(sceneName);
    }

    public static SceneController Instance
    {
        get
        {
            if (_instance == null)
            {
                
                if (_instance == null)
                {
                    GameObject singleton = new GameObject();
                    _instance = singleton.AddComponent<SceneController>();
                    singleton.name = typeof(SceneController).ToString() + " (Singleton)";
                    DontDestroyOnLoad(singleton);
                }
            }

            return _instance;
        }
    }

}
