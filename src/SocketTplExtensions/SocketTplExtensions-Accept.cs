using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace System.Net.Sockets
{
    public static partial class SocketTplExtensions
    {
#if !NETSTANDARD

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

#endif
    }
}
