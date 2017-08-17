using System;
using System.Collections.Generic;
using Photon.SocketServer;
using Common;
using System.Xml.Serialization;
using System.IO;

namespace yukiServer.Handler
{
    public class SyncPlayerHandler: BaseHandler
    {
        public SyncPlayerHandler()
        {
            opCode = OperationCode.SyncPlayer;
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            //取得所有已经登录（在线玩家）的用户名
            List<string> usernameList = new List<string>();
            foreach (ClientPeer clientPeer in yukiServer.Instance.peerList)
            {
                if(!string.IsNullOrEmpty(clientPeer.username) && clientPeer != peer)
                    usernameList.Add(clientPeer.username);
            }

            StringWriter sw = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
            serializer.Serialize(sw, usernameList);
            sw.Close();
            string usernameListString = sw.ToString();
            //yukiServer.log.Info(usernameListString);

            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data.Add((byte)ParameterCode.UsaernameList, usernameListString);
            OperationResponse opResponse = new OperationResponse(operationRequest.OperationCode);
            opResponse.Parameters = data;
            peer.SendOperationResponse(opResponse, sendParameters);

            //告诉其他客户端有新的客户端加入
            foreach (ClientPeer clientPeer in yukiServer.Instance.peerList)
            {
                if(!string.IsNullOrEmpty(clientPeer.username) && clientPeer != peer)
                {
                    EventData eventData = new EventData((byte)EventCode.NewPlayer);
                    Dictionary<byte, object> data2 = new Dictionary<byte, object>();
                    data2.Add((byte)ParameterCode.Username, peer.username);
                    eventData.Parameters = data2;
                    clientPeer.SendEvent(eventData, sendParameters);
                }
            }
        }
    }
}
