using System;
using System.Collections.Generic;
using System.Threading;
using Common;
using System.IO;
using System.Xml.Serialization;
using Photon.SocketServer;

namespace yukiServer.Threads
{
    public class SyncPositionThread
    {
        private Thread thread;

        public void Run()
        {
            thread = new Thread(UpdatePosition);
            thread.IsBackground = true;
            thread.Start();
        }

        public void Stop()
        {
            thread.Abort();
        }

        private void UpdatePosition()
        {
            Thread.Sleep(5000); //等待5秒
            
            while(true)
            {
                Thread.Sleep(50); //等待0.05秒
                //进行位置同步
                SyncPosition();
            }
        }

        private void SyncPosition()
        {
            List<PlayerData> playerDataList = new List<PlayerData>();
            foreach (ClientPeer peer in yukiServer.Instance.peerList)
            {
                if (!string.IsNullOrEmpty(peer.username))
                {
                    PlayerData playerData = new PlayerData();
                    playerData.Username = peer.username;
                    playerData.Pos = new Vector3Data() { x = peer.x, y = peer.y, z = peer.z };
                    playerDataList.Add(playerData);
                }
            }

            StringWriter sw = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(typeof(List<PlayerData>));
            serializer.Serialize(sw, playerDataList);
            sw.Close();
            string playerDataListString = sw.ToString();
            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data.Add((byte)ParameterCode.PlayerDataList, playerDataListString);

            foreach (ClientPeer peer in yukiServer.Instance.peerList)
            {
                if (!string.IsNullOrEmpty(peer.username))
                {
                    EventData eventData = new EventData((byte)EventCode.SyncPosition);
                    eventData.Parameters = data;
                    peer.SendEvent(eventData, new SendParameters());
                }
            }
        }
    }
}
