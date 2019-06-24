using System.Collections.Generic;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace System.Net.Sockets
{
    /// <summary>
    /// Extensions for <see cref="Socket"/>
    /// </summary>
    public static class SocketExtensions
    {
        #region ConnectAsync

        /// <summary>
        /// Begins an asynchronous request for a remote host connection.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static Task ConnectAsync(this Socket socket, string host, int port)
        {
            var tcs = new TaskCompletionSource<bool>(socket);
            socket.BeginConnect(host, port, BeginConnectCallback, tcs);
            return tcs.Task;
        }

        /// <summary>
        /// Begins an asynchronous request for a remote host connection.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="address"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static Task ConnectAsync(this Socket socket, IPAddress address, int port)
        {
            var tcs = new TaskCompletionSource<bool>(socket);
            socket.BeginConnect(address, port, BeginConnectCallback, tcs);
            return tcs.Task;
        }

        /// <summary>
        /// Begins an asynchronous request for a remote host connection.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="addresses"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static Task ConnectAsync(this Socket socket, IPAddress[] addresses, int port)
        {
            var tcs = new TaskCompletionSource<bool>(socket);
            socket.BeginConnect(addresses, port, BeginConnectCallback, tcs);
            return tcs.Task;
        }

        /// <summary>
        /// Begins an asynchronous request for a remote host connection.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public static Task ConnectAsync(this Socket socket, EndPoint endpoint)
        {
            var tcs = new TaskCompletionSource<bool>(socket);
            socket.BeginConnect(endpoint, BeginConnectCallback, tcs);
            return tcs.Task;
        }

        private static readonly AsyncCallback BeginConnectCallback = ar =>
        {
            var tcs = (TaskCompletionSource<bool>) ar.AsyncState;
            try
            {
                ((Socket) tcs.Task.AsyncState).EndConnect(ar);
                tcs.TrySetResult(true);
            }
            catch (Exception e)
            {
                tcs.TrySetException(e);
            }
        };

        #endregion

        #region DisconnectAsync

        /// <summary>
        /// Begins an asynchronous request to disconnect from a remote endpoint.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="reuseSocket"></param>
        /// <returns></returns>
        public static Task DisconnectAsync(this Socket socket, bool reuseSocket)
        {
            var tcs = new TaskCompletionSource<bool>(socket);
            socket.BeginDisconnect(reuseSocket, BeginDisconnectCallback, tcs);
            return tcs.Task;
        }

        private static readonly AsyncCallback BeginDisconnectCallback = ar =>
        {
            var tcs = (TaskCompletionSource<bool>) ar.AsyncState;
            try
            {
                ((Socket) tcs.Task.AsyncState).EndDisconnect(ar);
                tcs.TrySetResult(true);
            }
            catch (Exception e)
            {
                tcs.TrySetException(e);
            }
        };

        #endregion

        #region AcceptAsync

        /// <summary>
        /// Begins an asynchronous operation to accept an incoming connection attempt.
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public static Task<Socket> AcceptAsync(this Socket socket)
        {
            var tcs = new TaskCompletionSource<Socket>(socket);
            socket.BeginAccept(BeginAcceptCallback, tcs);
            return tcs.Task;
        }

        private static readonly AsyncCallback BeginAcceptCallback = ar =>
        {
            var tcs = (TaskCompletionSource<Socket>) ar.AsyncState;
            try
            {
                tcs.TrySetResult(((Socket) tcs.Task.AsyncState).EndAccept(ar));
            }
            catch (Exception e)
            {
                tcs.TrySetException(e);
            }
        };

        #endregion

        #region ReceiveAsync

        /// <summary>
        /// Begins to asynchronously receive data from a connected <see cref="Socket"/>.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <param name="socketFlags"></param>
        /// <returns></returns>
        public static Task<int> ReceiveAsync(this Socket socket, byte[] buffer, int offset, int size, SocketFlags socketFlags)
        {
            var tcs = new TaskCompletionSource<int>(socket);
            socket.BeginReceive(buffer, offset, size, socketFlags, BeginReceiveCallback, tcs);
            return tcs.Task;
        }

        /// <summary>
        /// Begins to asynchronously receive data from a connected <see cref="Socket"/>.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffers"></param>
        /// <param name="socketFlags"></param>
        /// <returns></returns>
        public static Task<int> ReceiveAsync(this Socket socket, IList<ArraySegment<byte>> buffers, SocketFlags socketFlags)
        {
            var tcs = new TaskCompletionSource<int>(socket);
            socket.BeginReceive(buffers, socketFlags, BeginReceiveCallback, tcs);
            return tcs.Task;
        }

        private static readonly AsyncCallback BeginReceiveCallback = ar =>
        {
            var tcs = (TaskCompletionSource<int>) ar.AsyncState;
            try
            {
                tcs.TrySetResult(((Socket) tcs.Task.AsyncState).EndReceive(ar));
            }
            catch (Exception e)
            {
                tcs.TrySetException(e);
            }
        };

        #endregion

        #region SendAsync

        /// <summary>
        /// Sends data asynchronously to a connected <see cref="Socket"/>.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <param name="socketFlags"></param>
        /// <returns></returns>
        public static Task<int> SendAsync(this Socket socket, byte[] buffer, int offset, int size, SocketFlags socketFlags)
        {
            var tcs = new TaskCompletionSource<int>(socket);
            socket.BeginSend(buffer, offset, size, socketFlags, BeginSendCallback, tcs);
            return tcs.Task;
        }

        /// <summary>
        /// Sends data asynchronously to a connected <see cref="Socket"/>.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffers"></param>
        /// <param name="socketFlags"></param>
        /// <returns></returns>
        public static Task<int> SendAsync(this Socket socket, IList<ArraySegment<byte>> buffers, SocketFlags socketFlags)
        {
            var tcs = new TaskCompletionSource<int>(socket);
            socket.BeginSend(buffers, socketFlags, BeginSendCallback, tcs);
            return tcs.Task;
        }

        private static readonly AsyncCallback BeginSendCallback = ar =>
        {
            var tcs = (TaskCompletionSource<int>) ar.AsyncState;
            try
            {
                tcs.TrySetResult(((Socket) tcs.Task.AsyncState).EndSend(ar));
            }
            catch (Exception e)
            {
                tcs.TrySetException(e);
            }
        };

        #endregion

        #region SendFileAsync

        /// <summary>
        /// Sends the file <paramref name="filename" /> to a connected <see cref="Socket" /> object using
        /// the <see cref="TransmitFileOptions.UseDefaultWorkerThread" /> flag.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static Task SendFileAsync(this Socket socket, string filename)
        {
            var tcs = new TaskCompletionSource<bool>(socket);
            socket.BeginSendFile(filename, BeginSendFileCallback, tcs);
            return tcs.Task;
        }

        /// <summary>
        /// Sends a file and buffers of data asynchronously to a connected <see cref="Socket" />.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="filename"></param>
        /// <param name="preBuffer"></param>
        /// <param name="postBuffer"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static Task SendFileAsync(this Socket socket, string filename, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags)
        {
            var tcs = new TaskCompletionSource<bool>(socket);
            socket.BeginSendFile(filename, preBuffer, postBuffer, flags, BeginSendFileCallback, tcs);
            return tcs.Task;
        }

        private static readonly AsyncCallback BeginSendFileCallback = ar =>
        {
            var tcs = (TaskCompletionSource<bool>) ar.AsyncState;
            try
            {
                ((Socket) tcs.Task.AsyncState).EndSendFile(ar);
                tcs.TrySetResult(true);
            }
            catch (Exception e)
            {
                tcs.TrySetException(e);
            }
        };

        #endregion
    }
}
