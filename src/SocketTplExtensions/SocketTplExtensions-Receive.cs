using System.Collections.Generic;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace System.Net.Sockets
{
    public static partial class SocketTplExtensions
    {
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

#if !NETSTANDARD

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

#endif

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
    }
}
