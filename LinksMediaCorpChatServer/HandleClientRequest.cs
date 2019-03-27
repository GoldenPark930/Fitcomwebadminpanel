using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using LinksMediaCorpBusinessLayer;
using LinksMediaCorpUtility;
using System.Globalization;
using LinksMediaCorpEntity;
namespace LinksMediaCorpChatServer
{
    /// <summary>
    /// HandleClientRequest class is used to diffrent user involing chating each other
    /// </summary>
    public class HandleClientRequest
    {
        TcpClient _clientSocket;
        NetworkStream _networkStream = null;
        string senderEmailId = string.Empty;
        public string receiverEmailId = string.Empty;
        int buffersize = 1024 * 4;
        int clientremoteId = 0;
        /// <summary>
        /// Handle Client Request
        /// </summary>
        /// <param name="clientConnected"></param>
        /// <param name="remoteIpAddressID"></param>
        public HandleClientRequest(TcpClient clientConnected, int remoteIpAddressID)
        {
            this._clientSocket = clientConnected;
            this.clientremoteId = remoteIpAddressID;
        }
        /// <summary>
        /// Start Client
        /// </summary>
        public void StartClient()
        {
            _networkStream = _clientSocket.GetStream();
            WaitForRequest();
        }
        /// <summary>
        /// Wait For Request
        /// </summary>
        public void WaitForRequest()
        {
            try
            {
                if (_clientSocket.Connected && _networkStream != null)
                {
                    if (_clientSocket.ReceiveBufferSize > 0)
                    {
                        buffersize = _clientSocket.ReceiveBufferSize;
                    }
                    byte[] buffer = new byte[buffersize];
                    _networkStream.BeginRead(buffer, 0, buffer.Length, ReadCallback, buffer);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Message:WaitForRequest " + ex.Message);
            }
        }
        /// <summary>
        /// IsSenderSent
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private int IsSenderSent(IAsyncResult result)
        {
            int read = 0;
            try
            {
                if (_clientSocket.Available >= 0)
                {
                    NetworkStream stream = _clientSocket.GetStream();
                    read = stream.EndRead(result);
                    if (read == 0)
                    {
                        _clientSocket.Close();
                        _networkStream.Close();
                        _networkStream = null;
                        if (LinksMediaCorpChatServer.Program.connectByClientId.ContainsKey(this.clientremoteId))
                        {
                            HandleClientRequest objHandleClientRequest;
                            if (LinksMediaCorpChatServer.Program.connectByClientId.TryRemove(this.clientremoteId, out objHandleClientRequest))
                            {
                                int removedEmailID = 0;
                                if (!String.IsNullOrEmpty(senderEmailId))
                                {
                                    senderEmailId = senderEmailId.ToLower();
                                    if (LinksMediaCorpChatServer.Program.connectByClientEmailId.TryRemove(senderEmailId, out removedEmailID))
                                    {
                                        removedEmailID = 0;
                                    }
                                }
                            }
                        }
                        senderEmailId = string.Empty;
                        receiverEmailId = string.Empty;
                        return 0;
                    }
                }
                return read;
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// Read Callback
        /// </summary>
        /// <param name="result"></param>
        private void ReadCallback(IAsyncResult result)
        {
            try
            {
                if (_clientSocket.Connected)
                {
                    int read = 0;
                    read = IsSenderSent(result);
                    byte[] buffer = result.AsyncState as Byte[];
                    if (!(read > 0) && buffer != null && buffer.Length > 0)
                    {
                        read = buffer.Length;
                        Console.WriteLine(DateTime.Now.ToString() + " – Data Received count " + read);
                    }
                    string data = Encoding.ASCII.GetString(buffer, 0, read);
                    buffer = null;
                    // string[] address = _clientSocket.Client.RemoteEndPoint.ToString().Split(':');
                    Console.WriteLine(DateTime.Now.ToString() + " – Data Received from Client : " + data);
                    data = string.IsNullOrEmpty(data) ? string.Empty : (data.Length > 0 && data.Contains("$")) ? data.Substring(0, data.LastIndexOf("$")) : data;
                    if (!string.IsNullOrEmpty(data) && data.Length > 0)
                    {
                        string[] senddata = data.Trim().Split('#');
                        string senderId = string.Empty;
                        string receiverId = string.Empty;
                        string message = string.Empty;
                        int operationId = 0;
                        if (senddata != null && senddata.Length > 3)
                        {
                            operationId = Convert.ToInt32(senddata[0]);
                            senderId = Convert.ToString(senddata[1]);
                            receiverId = Convert.ToString(senddata[2]);
                            if (!string.IsNullOrEmpty(receiverId))
                            {
                                receiverId = receiverId.ToLower(CultureInfo.InvariantCulture);
                            }
                            if (!string.IsNullOrEmpty(senderId))
                            {
                                senderId = senderId.ToLower(CultureInfo.InvariantCulture);
                            }
                            message = senddata[3];
                            DoChatOperation(senderId, receiverId, message, operationId, this.clientremoteId);
                            if (operationId == 1005)
                            {
                                // closeConectionStatus = false;
                            }
                            data = string.Empty;
                            senderId = string.Empty;
                            receiverId = string.Empty;
                            message = string.Empty;
                            operationId = 0;
                        }
                        data = string.Empty;
                    }

                }
                WaitForRequest();
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.WouldBlock ||
                    ex.SocketErrorCode == SocketError.IOPending ||
                    ex.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
                {
                    // socket buffer is probably full, wait and try again
                    Thread.Sleep(30);
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Message:ReadCallback " + Ex.Message);
            }
            finally
            {

            }
        }
        /// <summary>
        /// Do ChatOperation
        /// </summary>
        /// <param name="senderId"></param>
        /// <param name="receiverId"></param>
        /// <param name="message"></param>
        /// <param name="operationId"></param>
        /// <param name="chatclientHashcode"></param>
        /// <returns></returns>
        private bool DoChatOperation(string senderId, string receiverId, string message, int operationId, int chatclientHashcode)
        {
            StringBuilder traceLog = null;
            SocketSentChatVM objChatHistoryVM = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start: start DoChatOperation" + senderId + receiverId + message + operationId + chatclientHashcode);
                bool IsOffine = false;
                switch (operationId)
                {
                    case 1001:
                        senderId = senderId.ToLower();
                        receiverId = receiverId.ToLower();
                        senderEmailId = senderId;
                        if (!LinksMediaCorpChatServer.Program.connectByClientEmailId.ContainsKey(senderId))
                        {
                            Console.WriteLine("Start: Join chat room");
                            LinksMediaCorpChatServer.Program.connectByClientEmailId.TryAdd(senderId, chatclientHashcode);
                        }
                        else
                        {
                            int oldTCPClientId = LinksMediaCorpChatServer.Program.connectByClientEmailId[senderId];
                            if (oldTCPClientId > 0)
                            {
                                if (LinksMediaCorpChatServer.Program.connectByClientId.ContainsKey(oldTCPClientId))
                                {
                                    HandleClientRequest objHandleClientRequest;
                                    LinksMediaCorpChatServer.Program.connectByClientId.TryRemove(oldTCPClientId, out objHandleClientRequest);
                                }
                            }
                            Console.WriteLine("Start:  Again Join chat room");
                            LinksMediaCorpChatServer.Program.connectByClientEmailId[senderId] = chatclientHashcode;
                        }
                        break;
                    case 1002:
                        if (!string.IsNullOrEmpty(message))
                        {
                            receiverEmailId = string.Empty;
                            senderEmailId = string.Empty;

                            int receiverHashcode = 0;
                            if (!string.IsNullOrEmpty(receiverId))
                            {
                                receiverId = receiverId.ToLower(CultureInfo.InvariantCulture);
                            }
                            if (!string.IsNullOrEmpty(senderId))
                            {
                                senderId = senderId.ToLower(CultureInfo.InvariantCulture);
                            }
                            senderEmailId = senderId;
                            if (LinksMediaCorpChatServer.Program.connectByClientEmailId.ContainsKey(receiverId))
                            {
                                receiverHashcode = LinksMediaCorpChatServer.Program.connectByClientEmailId[receiverId];
                                receiverEmailId = receiverId.ToLower(CultureInfo.InvariantCulture);
                            }
                            var sentDateTime = DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff");
                            if (receiverHashcode > 0)
                            {
                                //Save to database for History                               
                                string messagetxt = "1003#" + senderId + "#" + receiverId + "#" + message + "#" + sentDateTime;
                                string receiversdetails = string.Empty;
                                receiversdetails = SendMsgToClientId(receiverHashcode, messagetxt, receiverId);
                                //Findout the sender and receiver
                                if (!string.IsNullOrEmpty(receiversdetails))
                                {
                                    var receiverdata = receiversdetails.Split('#');
                                    if (receiverdata != null && receiverdata.Length > 1)
                                    {
                                        if (receiverdata[0] != "NA" && receiverdata[1] != "NA" && string.Compare(senderEmailId, receiverdata[1], StringComparison.OrdinalIgnoreCase) == 0 && string.Compare(receiverEmailId, receiverdata[0], StringComparison.OrdinalIgnoreCase) == 0)
                                        {
                                            IsOffine = false;
                                        }
                                        else
                                        {
                                            IsOffine = true;
                                        }
                                    }
                                }
                                messagetxt = string.Empty;
                            }
                            else
                            {
                                IsOffine = true;
                            }
                            //string ceritifcatePath = Path.Combine(Environment.CurrentDirectory, "Resources\\CertificatesDev.p12");
                            string ceritifcatePath = Path.Combine(Environment.CurrentDirectory, "Resources\\Dist_Certificates.p12");
                            objChatHistoryVM = new SocketSentChatVM()
                            {
                                Message = message,
                                ReceiverEmailId = receiverId,
                                SenderEmailId = senderId,
                                TrasactionDateTime = sentDateTime,
                                IsOffine = IsOffine,
                                CeritifcatePath = ceritifcatePath
                            };
                            //// Save data in background
                            ChatHistoryApiBL.SaveChatHistoryFromSocket(objChatHistoryVM);
                            message = string.Empty;
                            ceritifcatePath = string.Empty;
                        }
                        break;
                    case 1003:
                        if (!string.IsNullOrEmpty(senderId))
                        {
                            senderEmailId = senderId.ToLower(CultureInfo.InvariantCulture);
                        }
                        if (!string.IsNullOrEmpty(receiverId))
                        {
                            receiverEmailId = receiverId.ToLower(CultureInfo.InvariantCulture);
                        }
                        break;

                    case 1004:
                        receiverEmailId = string.Empty;
                        break;
                    case 1005:
                        if (!string.IsNullOrEmpty(senderId))
                        {
                            senderId = senderId.ToLower(CultureInfo.InvariantCulture);
                        }
                        RemoveClientChatUser(chatclientHashcode, senderId);
                        // closeConectionStatus = false;
                        break;

                }
                traceLog.AppendLine("End:DoChatOperation()-" + DateTime.Now.ToLongDateString());
                return true;

            }
            catch (OutOfMemoryException e)
            {
                Console.WriteLine("Out of Memory: {0}", e.Message);
            }
            catch { }
            finally
            {
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                objChatHistoryVM = null;
                traceLog = null;
            }
            return false;
        }
        /// <summary>
        /// Send Msg To ClientId
        /// </summary>
        /// <param name="clientNumber"></param>
        /// <param name="msg"></param>
        public string SendMsgToClientId(int clientNumber, string msg, string receverEmailId)
        {
            StringBuilder traceLog = null;
            string receiverDetail = string.Empty;
            if (LinksMediaCorpChatServer.Program.connectByClientEmailId.ContainsKey(receverEmailId))
            {
                clientNumber = LinksMediaCorpChatServer.Program.connectByClientEmailId[receverEmailId];
            }
            if (Program.connectByClientId.ContainsKey(clientNumber) && !string.IsNullOrEmpty(msg))
            {
                // Chat.Sockets.Socket tcpClient = null;
                try
                {
                    traceLog = new StringBuilder();
                    traceLog.AppendLine("Start:SendMsgToClientId()" + receverEmailId);
                    HandleClientRequest tcpClient = (HandleClientRequest)Program.connectByClientId[clientNumber];
                    if (tcpClient != null && tcpClient._clientSocket.Connected)
                    {

                        byte[] Ack = Encoding.ASCII.GetBytes(msg);
                        tcpClient._networkStream.Write(Ack, 0, Ack.Length);
                        tcpClient._networkStream.Flush();
                        Console.WriteLine("Message sent successfully");
                        receiverDetail = (string.IsNullOrEmpty(tcpClient.senderEmailId) ? "NA" : tcpClient.senderEmailId) + "#" + (string.IsNullOrEmpty(tcpClient.receiverEmailId) ? "NA" : tcpClient.receiverEmailId);
                    }
                }
                catch (OutOfMemoryException e)
                {
                    Console.WriteLine("Out of Memory: {0}", e.Message);
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode == SocketError.WouldBlock ||
                        ex.SocketErrorCode == SocketError.IOPending ||
                        ex.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
                    {
                        // socket buffer is probably full, wait and try again
                        Thread.Sleep(30);
                    }
                }
                catch { }
                finally
                {
                    //  Console.WriteLine("End:SendMsgToClientId-" + DateTime.Now.ToLongDateString());
                    LogManager.LogManagerInstance.WriteTraceLog(traceLog);

                    traceLog = null;
                }
            }
            return receiverDetail;
        }
        /// <summary>
        /// Remove Client Chat User
        /// </summary>
        /// <param name="clientNumber"></param>
        /// <param name="emailId"></param>
        public void RemoveClientChatUser(int clientNumber, string emailId)
        {
            //create a StreamWriter Object
            StringBuilder traceLog = null;
            try
            {
                traceLog = new StringBuilder();
                traceLog.AppendLine("Start:RemoveClientChatUser()" + clientNumber);
                if (clientNumber > 0)
                {
                    HandleClientRequest tcpClient = Program.connectByClientId[clientNumber];
                    if (tcpClient != null)
                    {
                        if (LinksMediaCorpChatServer.Program.connectByClientId.ContainsKey(clientNumber))
                        {
                            HandleClientRequest objHandleClientRequest;
                            LinksMediaCorpChatServer.Program.connectByClientId.TryRemove(clientNumber, out objHandleClientRequest);
                        }
                        if (!string.IsNullOrEmpty(emailId) && LinksMediaCorpChatServer.Program.connectByClientEmailId.ContainsKey(emailId))
                        {
                            int objHandleClientRequest = 0;
                            LinksMediaCorpChatServer.Program.connectByClientEmailId.TryRemove(emailId, out objHandleClientRequest);
                        }
                        Console.WriteLine("Remove Client Chat User successfully");
                    }
                }
            }
            catch (OutOfMemoryException e)
            {
                traceLog.AppendLine("Out of Memory: {0}" + e.Message);
            }
            catch (Exception ex)
            {
                LogManager.LogManagerInstance.WriteErrorLog(ex);
            }
            finally
            {
                traceLog.AppendLine("End:RemoveClientChatUser()-" + DateTime.Now.ToLongDateString());
                LogManager.LogManagerInstance.WriteTraceLog(traceLog);
                traceLog = null;
            }
        }

    }

}
