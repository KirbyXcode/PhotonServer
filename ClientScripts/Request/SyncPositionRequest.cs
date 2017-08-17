using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using ExitGames.Client.Photon;

public class SyncPositionRequest : Request
{
    public Vector3 Pos { get; set; }

    public override void OnSendRequest()
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.X, Pos.x);
        data.Add((byte)ParameterCode.Y, Pos.y);
        data.Add((byte)ParameterCode.Z, Pos.z);
        PhotonEngine.Peer.OpCustom((byte)opCode, data, true);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        throw new System.NotImplementedException();
    }
}
