using Common;
using UnityEngine;
using ExitGames.Client.Photon;

public class NewPlayerEvent : BaseEvent
{
    private Player player;

    public override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }

    public override void OnEvent(EventData eventData)
    {
        string username = (string)DictionaryHelper.GetValue<byte, object>(eventData.Parameters, (byte)ParameterCode.Username);
        player.OnSyncPlayerEvent(username);
    }
}
