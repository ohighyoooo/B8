using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject First;
    public GameObject Credit;
    public GameObject BCredit;
    public GameObject BStart;
    public GameObject BCBack;
    public GameObject BExit;

    public void LoadMaingameScene()
    {
        SceneManager.LoadSceneAsync(1);
    }
    // Start is called before the first frame update
    void Start()
    {
        Button CreditButtonComponent = BCredit.GetComponent<Button>();
        CreditButtonComponent.onClick.AddListener(PressCredit);

        Button BackButtonComponent = BCBack.GetComponent<Button>();
        BackButtonComponent.onClick.AddListener(PressBack);

        Button CExitButtonComponent = BExit.GetComponent<Button>();
        CExitButtonComponent.onClick.AddListener(PressExit);

        Button StartButtonComponent = BStart.GetComponent<Button>();
        StartButtonComponent.onClick.AddListener(PressStart);
        Credit.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PressCredit()
    {
        First.SetActive(false);
        Credit.SetActive(true);
    }

    public void PressExit()
    {
        Application.Quit();
    }

    public void PressBack()
    {
        First.SetActive(true);
        Credit.SetActive(false);
    }
    public void PressStart()
    {
        Invoke("LoadMaingameScene", 2f);
    }
}
