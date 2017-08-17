using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using ExitGames.Client.Photon;
using System.IO;
using System.Xml.Serialization;

public class SyncPlayerRequest : Request 
{
    private Player player;

    public override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }

    public override void OnSendRequest()
    {
        PhotonEngine.Peer.OpCustom((byte)opCode, null, true);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        string usernameListString = (string)DictionaryHelper.GetValue<byte, object>(operationResponse.Parameters, (byte)ParameterCode.UsaernameList);

        using(StringReader reader = new StringReader(usernameListString))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
            List<string> usernameList = serializer.Deserialize(reader) as List<string>;
            player.OnSyncPlayerResponse(usernameList);
        }
    }
}
