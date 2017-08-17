using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using ExitGames.Client.Photon;

public class RegisterRequest : Request 
{
    public string Username { get; set; }
    public string Password { get; set; }

    private UIRegister uiRegister;

    public override void Start()
    {
        base.Start();
        uiRegister = GetComponent<UIRegister>();
    }

    public override void OnSendRequest()
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.Username, Username);
        data.Add((byte)ParameterCode.Password, Password);
        PhotonEngine.Peer.OpCustom((byte)opCode, data, true);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        ReturnCode returnCode = (ReturnCode)operationResponse.ReturnCode;
        uiRegister.OnRegisterResponse(returnCode);
    }
}
