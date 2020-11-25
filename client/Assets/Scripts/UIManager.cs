using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;
    public InputField usernameField;

    private void Awake() {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Client already exists. Destroying...");
            Destroy(this);
        }
    }
    
    //public void ConnectToServer {
    //    startMenu.SetAction(false);
    //    usernameField.interactable = false;
    //    Client.instance.ConnectToServer();
    //}

}
