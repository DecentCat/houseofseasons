using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ChatManager : MonoBehaviour
{
    [SerializeField] private GameObject MessageContainer;
    [SerializeField] private GameObject chatMessagePrefab;

    private Queue<GameObject> activeMessageQueue = new Queue<GameObject>();
    private Queue<Tuple<string, string>> genericMessageQueue = new Queue<Tuple<string, string>>();
    private Queue<Tuple<string, string>> priorityMessageQueue = new Queue<Tuple<string, string>>();

    //private List<string> userList = new List<string>();

    private XmlDocument namepoolXml = new XmlDocument();
    private XmlDocument messagepoolXml = new XmlDocument();

    void Start()
    {
        InvokeRepeating("RandomMessageEnqueue", 20, 10);

        // load namepool and messagepool XML
        TextAsset namepool = Resources.Load("namepool") as TextAsset;
        namepoolXml.LoadXml(namepool.text);
        TextAsset messagepool = Resources.Load("messagepool") as TextAsset;
        messagepoolXml.LoadXml(messagepool.text);
    }

    
    void Update()
    {
        // post message from queue
        if (priorityMessageQueue.Count != 0)
        {
            var message = priorityMessageQueue.Dequeue();
            PostChatMessage(message.Item1, message.Item2);
        }
        else if (genericMessageQueue.Count != 0)
        {
            var message = genericMessageQueue.Dequeue();
            PostChatMessage(message.Item1, message.Item2);
        }
    }

    // Maybe enqueue chat message
    private void RandomMessageEnqueue()
    {
        // enque chat message (maybe)
        if (Random.Range(0, 3) == 0)
        {
            MessageType randType = (MessageType)Random.Range(0, 2);
            var message = GenerateChatMessage(randType);
            genericMessageQueue.Enqueue(message);
        }
    }

    private string GenerateUsername()
    {
        string username = String.Empty;
        // random adjective
        var nodes = namepoolXml.SelectNodes("//adjective");
        username += nodes.Item(Random.Range(0, nodes.Count)).InnerText;

        if (Random.Range(0,2) == 0)
        {
            // random noun
            nodes = namepoolXml.SelectNodes("//noun");
            username += nodes.Item(Random.Range(0, nodes.Count)).InnerText;
        }
        else
        {
            // random name
            nodes = namepoolXml.SelectNodes("//name");
            username += nodes.Item(Random.Range(0, nodes.Count)).InnerText;
        }

        int randPostfix = Random.Range(0, 4);
        if (randPostfix == 0)
        {
            // random postfix
            nodes = namepoolXml.SelectNodes("//postfix");
            username += nodes.Item(Random.Range(0, nodes.Count)).InnerText;
        }
        else if (randPostfix == 1)
        {
            // random year
            username += Random.Range(60, 99).ToString();
        }
        else if (randPostfix == 2)
        {
            // random single number
            username += Random.Range(0, 10).ToString();
        }
        else
        {
            username += Random.Range(0, 999).ToString();
        }

        return username;
    }

    private Tuple<string, string> GenerateChatMessage(MessageType type, string username = null)
    {

        if (username == null)
        {
            username = GenerateUsername();
        }

        string message = string.Empty;
        switch (type)
        {
            case MessageType.Hello:
                var nodes = messagepoolXml.SelectNodes("//hello/message");
                message = nodes.Item(Random.Range(0, nodes.Count)).InnerText;
                break;
            
            case MessageType.Happy:
                nodes = messagepoolXml.SelectNodes("//generic/message");
                message = nodes.Item(Random.Range(0, nodes.Count)).InnerText;
                break;

            case MessageType.Sad:
                nodes = messagepoolXml.SelectNodes("//sad/message");
                message = nodes.Item(Random.Range(0, nodes.Count)).InnerText;
                break;

            default: break;
        }

        return new Tuple<string, string>(username, message);
    }

    // write chat message to the UI
    public void PostChatMessage(string userName, string message)
    {
        GameObject messageGO = Instantiate(chatMessagePrefab, MessageContainer.transform, false) as GameObject;
        //messageGO.transform.SetParent(MessageContainer.transform, false);
        messageGO.transform.SetAsLastSibling();
        messageGO.GetComponent<ChatMessageScript>().SetUsernameAndMessage(userName, message);
        gameObject.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 0);

        activeMessageQueue.Enqueue(messageGO);

        if(activeMessageQueue.Count > 16)
        {
            Destroy(activeMessageQueue.Dequeue());
        }
    }
}

public enum MessageType
{
    Hello = 0,
    Happy = 1,
    Sad = 2
}
