using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Common;

public class LoginRequest : Request
{
    public string Username { get; set; }
    public string Password { get; set; }

    private UILogin uiLogin;

    public override void Start()
    {
        base.Start();
        uiLogin = GetComponent<UILogin>();
    }

    public override void OnSendRequest()
    {
        Dictionary<byte,object> data = new Dictionary<byte,object>();
        data.Add((byte)ParameterCode.Username, Username);
        data.Add((byte)ParameterCode.Password, Password);
        PhotonEngine.Peer.OpCustom((byte)opCode, data, true);
    } 

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        ReturnCode returnCode = (ReturnCode)operationResponse.ReturnCode;
        uiLogin.OnLoginResponse(returnCode);
    }
}
