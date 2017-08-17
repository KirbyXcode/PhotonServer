using System;
using System.Collections.Generic;
using Common;
using Photon.SocketServer;

namespace yukiServer.Handler
{
    public abstract class BaseHandler
    {
        public OperationCode opCode;

        public abstract void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer);
    }
}
