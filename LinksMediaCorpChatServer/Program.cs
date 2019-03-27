using System;
using System.Net;
using System.Net.Sockets;
using System.Configuration;
using System.Threading;
using System.Collections.Concurrent;
using LinksMediaCorpUtility;
using System.Text;
namespace LinksMediaCorpChatServer
{
    /// <summary>
    /// Program class is used to creat tcp socket and initialed with ip and port
    /// </summary>
    class Program
    {
        private static TcpListener _listener;
        public static ConcurrentDictionary<int, HandleClientRequest> connectByClientId;
        public static ConcurrentDictionary<string, int> connectByClientEmailId;
        static int counter = 0;
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        /// <summary>
        /// star main function
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Server Started");
                StartServer();
                Console.WriteLine("\nPress ENTER to continue...");
                Console.Read();
            }
            catch { }
            finally
            {
                connectByClientId = null;
                connectByClientEmailId = null;
            }

        }
        /// <summary>
        /// Start Server
        /// </summary>
        public static void StartServer()
        {
            int portNo = Convert.ToInt32(ConfigurationManager.AppSettings["PortNo"]);
            string iPAdd = ConfigurationManager.AppSettings["IpAddress"];
            System.Net.IPAddress localIPAddress = System.Net.IPAddress.Parse(iPAdd);
            IPEndPoint ipLocal = new IPEndPoint(localIPAddress, portNo);

            // string  hostname = ConfigurationManager.AppSettings["Hostname"];
            // IPAddress[] IPs = Dns.GetHostAddresses(hostname);
            // IPEndPoint ipLocal = new IPEndPoint(IPs[0], portNo);

            _listener = new TcpListener(ipLocal);

            //_listener = new TcpListener(RoleEnvironment);

            _listener.Start();
            connectByClientId = new ConcurrentDictionary<int, HandleClientRequest>();
            connectByClientEmailId = new ConcurrentDictionary<string, int>();
            WaitForClientConnect();
        }
        /// <summary>
        /// Wait For Client Connect to TCP Server
        /// </summary>
        private static void WaitForClientConnect()
        {
            // Set the event to nonsignaled state.
            allDone.Reset();
            object obj = new object();
            _listener.BeginAcceptTcpClient(new System.AsyncCallback(OnClientConnect), obj);
            // Wait until a connection is made before continuing.
            allDone.WaitOne();

        }
        /// <summary>
        /// On Client Connect callback
        /// </summary>
        /// <param name="asyn"></param>
        private static void OnClientConnect(IAsyncResult asyn)
        {
            // Signal the main thread to continue.
            allDone.Set();
            try
            {
                TcpClient clientSocket = default(TcpClient);
                clientSocket = _listener.EndAcceptTcpClient(asyn);
                ++counter;
                clientSocket.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                clientSocket.Client.NoDelay = true;
                int clientRemoteID = 0;
                clientRemoteID = counter;
                HandleClientRequest clientReq = new HandleClientRequest(clientSocket, clientRemoteID);
                clientReq.StartClient();
                if (!Program.connectByClientId.ContainsKey(clientRemoteID))
                {
                    Console.WriteLine("Start: Join chat room");
                    Program.connectByClientId.TryAdd(clientRemoteID, clientReq);
                }
                WaitForClientConnect();
            }
            catch
            {
                WaitForClientConnect();
            }
        }
    }
}
