using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour 
{
	void Update () 
    {
        if (Input.GetMouseButtonDown(0))
            SendRequest();
	}

    private void SendRequest()
    {
        Dictionary<byte,object> data = new Dictionary<byte,object>();
        data.Add(1, 999);
        data.Add(2, "gdeath");
        PhotonEngine.Peer.OpCustom(1, data, true);
    }
}
