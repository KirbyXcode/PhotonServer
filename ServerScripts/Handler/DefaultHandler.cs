using System;
using System.Collections.Generic;
using Common;
using Photon.SocketServer;

namespace yukiServer.Handler
{
    public class DefaultHandler: BaseHandler
    {
        public DefaultHandler()
        {
            opCode = OperationCode.Default;
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            
        }
    }
}
