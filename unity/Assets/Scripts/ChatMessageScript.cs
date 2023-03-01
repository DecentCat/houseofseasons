using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ChatMessageScript : MonoBehaviour
{
    [SerializeField] GameObject username;
    [SerializeField] GameObject message;

    public void SetUsernameAndMessage(string username, string message)
    {
        SetUsername(username);
        SetMessage(message);
    }

    public void SetUsername(string username)
    {
        this.username.GetComponent<TextMeshProUGUI>().text = username;
    }

    public void SetMessage(string message)
    {
        this.message.GetComponent<TextMeshProUGUI>().text = message;
    }
}
