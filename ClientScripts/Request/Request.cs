using Common;
using UnityEngine;
using ExitGames.Client.Photon;

public abstract class Request : MonoBehaviour
{
    public OperationCode opCode;
    public abstract void OnSendRequest();
    public abstract void OnOperationResponse(OperationResponse operationResponse);

    public virtual void Start()
    {
        PhotonEngine.instance.AddRequest(this);
    }

    public void OnDestroy()
    {
        PhotonEngine.instance.RemoveRequest(this);
    }
}
