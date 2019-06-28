using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace System.Net.Sockets
{
    public static partial class SocketTplExtensions
    {
#if !NETSTANDARD

        /// <summary>
        /// Begins an asynchronous request for a remote host connection.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static Task ConnectAsync(this Socket socket, string host, int port)
        {
            NotNull(socket, nameof(socket));

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
            NotNull(socket, nameof(socket));

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
            NotNull(socket, nameof(socket));

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
            NotNull(socket, nameof(socket));

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

#endif
    }
}
