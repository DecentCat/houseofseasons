using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    [SerializeField] private GameObject MessageContainer;
    [SerializeField] private GameObject chatMessagePrefab;

    private int counter = 0;
    private int messageCounter = 0;
    private float messageProbability;

    private Queue<GameObject> activeMessageQueue = new Queue<GameObject>();
    private Queue<Tuple<string, string>> genericMessageQueue = new Queue<Tuple<string, string>>();
    private Queue<Tuple<string, string>> priorityMessageQueue = new Queue<Tuple<string, string>>();

    private List<string> userList = new List<string>();

    private XmlDocument namepoolXml = new XmlDocument();

    void Start()
    {
        InvokeRepeating("RandomMessageEnqueue", 20, 10);

        // load namepool XML
        TextAsset namepool = Resources.Load("namepool") as TextAsset;
        namepoolXml.LoadXml(namepool.text);

    }

    // Update is called once per frame
    void Update()
    {
        if (counter > 200)
        {
            counter = 0;
            PostChatMessage($"user{messageCounter}", "Hello cat :)");
            messageCounter++;
        }
        counter++;
    }

    // Maybe enqueue chat message
    private void RandomMessageEnqueue()
    {

    }

    private Tuple<string, string> GenerateChatMessage(MessageType type, string username = null)
    {
        Tuple<string, string> chatMessage = null;

        if (username == null)
        {
            username = GetRandomUser();
        }

        switch (type)
        {
            case MessageType.Hello:
                chatMessage = IntroduceUser();
                break;
            
            case MessageType.Happy:
                break;

            case MessageType.Sad:
                break;

            default: break;
        }

        return chatMessage;
    }

    private string GetRandomUser()
    {
        return null;
    }

    // generate random username
    private Tuple<string, string> IntroduceUser()
    {
        return new Tuple<string, string>(null, null);
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
    Hello,
    Happy,
    Sad
}
