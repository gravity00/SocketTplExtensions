using System.Collections.Generic;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace System.Net.Sockets
{
    public static partial class SocketTplExtensions
    {
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
    }
}
