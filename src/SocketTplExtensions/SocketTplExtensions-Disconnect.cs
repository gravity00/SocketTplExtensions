using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace System.Net.Sockets
{
    public static partial class SocketTplExtensions
    {
#if !NETSTANDARD1_3

        /// <summary>
        /// Begins an asynchronous request to disconnect from a remote endpoint.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="reuseSocket"></param>
        /// <returns></returns>
        public static Task DisconnectAsync(this Socket socket, bool reuseSocket)
        {
            NotNull(socket, nameof(socket));

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

#endif
    }
}
