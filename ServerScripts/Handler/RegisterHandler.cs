using System;
using System.Collections.Generic;
using Photon.SocketServer;
using Common;
using yukiServer.Table;

namespace yukiServer.Handler
{
    public class RegisterHandler: BaseHandler
    {
        public RegisterHandler()
        {
            opCode = OperationCode.Register;
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            string username = DictionaryHelper.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.Username) as string;
            string password = DictionaryHelper.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.Password) as string;

            UserManager userManager = new UserManager();
            User user = userManager.GetByUsername(username);
            OperationResponse opResponse = new OperationResponse(operationRequest.OperationCode);
            if (user == null)
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    opResponse.ReturnCode = (short)ReturnCode.Exception;
                }
                else
                {
                    user = new User() { Username = username, Password = password };
                    userManager.Add(user);
                    opResponse.ReturnCode = (short)ReturnCode.Success;
                }
            }
            else
            {
                opResponse.ReturnCode = (short)ReturnCode.Failed;
            }
            peer.SendOperationResponse(opResponse, sendParameters);
        }
    }
}
