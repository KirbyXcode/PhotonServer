using System;
using System.Collections.Generic;
using Photon.SocketServer;
using Common;

namespace yukiServer.Handler
{
    public class SyncPositionHandler: BaseHandler
    {
        public SyncPositionHandler()
        {
            opCode = OperationCode.SyncPosition;
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            //Vector3Data pos = (Vector3Data)DictionaryHelper.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.Position);
            float x = (float)DictionaryHelper.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.X);
            float y = (float)DictionaryHelper.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.Y);
            float z = (float)DictionaryHelper.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.Z);

            //角色位置信息保存到当前客户端接口（ClinetPeer)
            peer.x = x; peer.y = y; peer.z = z;
        }
    }
}
