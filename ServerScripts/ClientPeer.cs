using System;
using System.Collections.Generic;
using Photon.SocketServer;
using Common;
using yukiServer.Handler;

namespace yukiServer
{
    public class ClientPeer:Photon.SocketServer.ClientPeer
    {
        public float x, y, z;
        public string username;

        public ClientPeer(InitRequest initRequest):base(initRequest)
        {

        }

        //处理客户端断开链接的后续工作
        protected override void OnDisconnect(PhotonHostRuntimeInterfaces.DisconnectReason reasonCode, string reasonDetail)
        {
            yukiServer.Instance.peerList.Remove(this);
        }

        //处理客户端的请求
        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            BaseHandler handler = DictionaryHelper.GetValue<OperationCode, BaseHandler>(yukiServer.Instance.handlerDict, (OperationCode)operationRequest.OperationCode);
            if (handler != null)
            {
                handler.OnOperationRequest(operationRequest, sendParameters, this);
            }
            else
            {
                BaseHandler defaultHandler = DictionaryHelper.GetValue<OperationCode, BaseHandler>(yukiServer.Instance.handlerDict, OperationCode.Default);
                defaultHandler.OnOperationRequest(operationRequest, sendParameters, this);
            }

            //switch(operationRequest.OperationCode) //通过OpCode区分请求
            //{
            //    case 1:
            //        yukiServer.log.Info("Received the request from a client");
            //        Dictionary<byte, object> data = operationRequest.Parameters;
            //        object intValue;
            //        data.TryGetValue(1, out intValue);
            //        object stringValue;
            //        data.TryGetValue(2, out stringValue);
            //        yukiServer.log.Info("得到的数据是" + intValue.ToString() + " " + stringValue.ToString());

            //        //向客户端发送反馈
            //        OperationResponse opResponse = new OperationResponse(1);
            //        Dictionary<byte, object> dataResponse = new Dictionary<byte, object>();
            //        dataResponse.Add(1, 100);
            //        dataResponse.Add(2, "wowowo");
            //        opResponse.SetParameters(dataResponse);
            //        SendOperationResponse(opResponse, sendParameters);

            //        //服务器端向客户端发送事件
            //        EventData eventData = new EventData(1);
            //        eventData.Parameters = dataResponse;
            //        SendEvent(eventData, new SendParameters());
            //        break;
            //    case 2:
            //        break;
            //    default:
            //        break;
            //}
        }
    }
}
