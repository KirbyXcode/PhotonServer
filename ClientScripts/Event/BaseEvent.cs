using Common;
using UnityEngine;
using ExitGames.Client.Photon;

public abstract class BaseEvent : MonoBehaviour 
{
    public EventCode eventCode;
    public abstract void OnEvent(EventData eventData);

    public virtual void Start()
    {
        PhotonEngine.instance.AddEvent(this);
    }

    public void OnDestroy()
    {
        PhotonEngine.instance.RemoveEvent(this);
    }
}
