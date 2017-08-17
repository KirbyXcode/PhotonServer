using System;
using System.Collections.Generic;
using Photon.SocketServer;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net.Config;
using System.IO;
using Common;
using yukiServer.Handler;
using yukiServer.Threads;

namespace yukiServer
{
    //所有的Server端 主类都集成自ApplicationBase
    public class yukiServer: ApplicationBase
    {
        public static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public static yukiServer Instance
        {
            get;
            private set;
        }

        public Dictionary<OperationCode, BaseHandler> handlerDict = new Dictionary<OperationCode, BaseHandler>();

        public List<ClientPeer> peerList = new List<ClientPeer>(); //通过这个集合可以访问到所有客户端的peer，从而向任何一个客户端发送数据

        private SyncPositionThread syncPositionThread = new SyncPositionThread();

        //当一个客户端请求链接的时候
        //使用PeerBase，表示和一个客户端的链接
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            log.Info("Connected with a client successfully!");
            ClientPeer peer = new ClientPeer(initRequest);
            peerList.Add(peer);
            return peer;
        }

        //Server端初始化
        protected override void Setup()
        {
            Instance = this;
            //日志文件初始化
            log4net.GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(Path.Combine(ApplicationRootPath, "bin_Win64"), "log");
            FileInfo configFileInfo = new FileInfo(Path.Combine(this.BinaryPath, "log4net.config"));
            if(configFileInfo.Exists)
            {
                //让Photon知道使用的是哪个日志插件
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
                //让log4net这个插件读取配置文件
                XmlConfigurator.ConfigureAndWatch(configFileInfo);
            }
            log.Info("Setup Completed!");

            InitHandler();
            //启动玩家位置同步线程
            syncPositionThread.Run();
        }

        private void InitHandler()
        {
            BaseHandler loginHandler = new LoginHandler();
            handlerDict.Add(OperationCode.Login, loginHandler);
            //handlerDict.Add(loginHandler.opCode, loginHandler);

            BaseHandler defaultHandler = new DefaultHandler();
            handlerDict.Add(OperationCode.Default, defaultHandler);

            BaseHandler registerHandler = new RegisterHandler();
            handlerDict.Add(OperationCode.Register, registerHandler);

            BaseHandler syncPositionHandler = new SyncPositionHandler();
            handlerDict.Add(OperationCode.SyncPosition, syncPositionHandler);

            BaseHandler syncPlayerHandler = new SyncPlayerHandler();
            handlerDict.Add(OperationCode.SyncPlayer, syncPlayerHandler);
        }

        //Server端关闭的时候
        protected override void TearDown()
        {
            //停止玩家位置同步线程
            syncPositionThread.Stop();
            log.Info("Server shut down!");
        }
    }
}
