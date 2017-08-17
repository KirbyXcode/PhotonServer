using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Common;

public class PhotonEngine : MonoBehaviour, IPhotonPeerListener
{
    public static PhotonEngine instance;

    private static PhotonPeer peer;
    public static PhotonPeer Peer
    {
        get
        {
            return peer;
        }
    }

    private Dictionary<OperationCode, Request> requestDict = new Dictionary<OperationCode, Request>();
    private Dictionary<EventCode, BaseEvent> eventDict = new Dictionary<EventCode, BaseEvent>();

    public static string username;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else if(instance != this)
        {
            Destroy(this.gameObject); 
            return;
        }
    }

    void Start()
    {
        //通过Listener接收服务器端的响应
        peer = new PhotonPeer(this, ConnectionProtocol.Udp);
        peer.Connect("127.0.0.1:5055", "yukiGameDemo");
    }

    void Update()
    {
        peer.Service();
    }

    void OnDestroy()
    {
        if (peer != null && peer.PeerState == PeerStateValue.Connected)
            peer.Disconnect();
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        
    }

    //服务器端向客户端发起的事件
    public void OnEvent(EventData eventData)
    {
        EventCode eventCode = (EventCode)eventData.Code;
        BaseEvent e = DictionaryHelper.GetValue<EventCode, BaseEvent>(eventDict, eventCode);
        e.OnEvent(eventData);

        //switch(eventData.Code)
        //{
        //    case 1:
        //        Debug.Log("收到服务器发送了过来的事件");
        //        Dictionary<byte, object> data = eventData.Parameters;
        //        object intValue, stringValue;
        //        data.TryGetValue(1, out intValue);
        //        data.TryGetValue(2, out stringValue);
        //        Debug.Log(intValue.ToString() + " " + stringValue.ToString());
        //        break;
        //    case 2:
        //        break;
        //    default:
        //        break;
        //}
    }

    //服务器端处理完客户端的Request后给客户端响应的回复
    public void OnOperationResponse(OperationResponse operationResponse)
    {
        OperationCode opCode = (OperationCode)operationResponse.OperationCode;
        Request request = null;  
        bool temp = requestDict.TryGetValue(opCode, out request);

        if(temp)
        {
            request.OnOperationResponse(operationResponse);
        }
        else
        {
            Debug.Log("Request is null");
        }

        //switch(operationResponse.OperationCode)
        //{
        //    case 1:
        //        Debug.Log("Received the response from Server");
        //        Dictionary<byte, object> data = operationResponse.Parameters;
        //        object intValue, stringValue;
        //        data.TryGetValue(1, out intValue);
        //        data.TryGetValue(2, out stringValue);
        //        Debug.Log(intValue.ToString() + " " + stringValue.ToString());
        //        break;
        //    case 2:
        //        break;
        //    default:
        //        break;
        //}
    }

    //当Peer与服务器端的状态发生改变的时候
    public void OnStatusChanged(StatusCode statusCode)
    {
        
    }

    public void AddRequest(Request request)
    {
        requestDict.Add(request.opCode, request);
    }

    public void RemoveRequest(Request request)
    {
        requestDict.Remove(request.opCode);
    }

    public void AddEvent(BaseEvent e)
    {
        eventDict.Add(e.eventCode, e);
    }

    public void RemoveEvent(BaseEvent e)
    {
        eventDict.Remove(e.eventCode);
    }
}
