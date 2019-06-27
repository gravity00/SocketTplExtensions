using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace System.Net.Sockets
{
    public static partial class SocketTplExtensions
    {
        /// <summary>
        /// Sends data asynchronously to a specific remote host.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <param name="socketFlags"></param>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public static Task<int> SendToAsync(this Socket socket, byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint endpoint)
        {
            var tcs = new TaskCompletionSource<int>(socket);
            socket.BeginSendTo(buffer, offset, size, socketFlags, endpoint, BeginSendToCallback, tcs);
            return tcs.Task;
        }

        private static readonly AsyncCallback BeginSendToCallback = ar =>
        {
            var tcs = (TaskCompletionSource<int>) ar.AsyncState;
            try
            {
                tcs.TrySetResult(((Socket) tcs.Task.AsyncState).EndSendTo(ar));
            }
            catch (Exception e)
            {
                tcs.TrySetException(e);
            }
        };
    }
}
