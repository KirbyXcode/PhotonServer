using System;
using System.Collections.Generic;
using Photon.SocketServer;
using Common;

namespace yukiServer.Handler
{
    public class LoginHandler: BaseHandler
    {
        public LoginHandler()
        {
            opCode = OperationCode.Login;
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            string username = DictionaryHelper.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.Username) as string;
            string password = DictionaryHelper.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.Password) as string;

            UserManager userManager = new UserManager();
            bool isSuccess = userManager.VerifyAccount(username, password);

            OperationResponse opResponse = new OperationResponse(operationRequest.OperationCode);
            if(isSuccess)
            {
                peer.username = username; //用户账号信息保存到当前客户端接口中(ClientPeer)
                opResponse.ReturnCode = (short)ReturnCode.Success;
            }
            else
            {
                opResponse.ReturnCode = (short)ReturnCode.Failed;
            }
            peer.SendOperationResponse(opResponse, sendParameters);
        }
    }
}
