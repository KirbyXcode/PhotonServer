using Common;
using UnityEngine;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using System.Xml.Serialization;
using System.IO;

public class SyncPositionEvent : BaseEvent
{
    private Player player;

    public override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }

    public override void OnEvent(EventData eventData)
    {
        string playerDataListString = (string)DictionaryHelper.GetValue<byte, object>(eventData.Parameters, (byte)ParameterCode.PlayerDataList);
        using(StringReader reader = new StringReader(playerDataListString))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<PlayerData>));
            List<PlayerData> playerDataList = (List<PlayerData>)serializer.Deserialize(reader);
            player.OnSyncPositionEvent(playerDataList);
        }
    }
}
