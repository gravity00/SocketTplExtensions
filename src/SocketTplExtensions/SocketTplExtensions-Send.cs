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

#if NETSTANDARD1_3

            var args = new SocketAsyncEventArgs
            {
                SocketFlags = socketFlags,
                UserToken = tcs
            };
            args.SetBuffer(buffer, offset, size);
            args.Completed += OnSendCompleted;
            if (!socket.SendAsync(args))
            {
                tcs.TrySetSendResult(args);
            }

#else
            
            socket.BeginSend(buffer, offset, size, socketFlags, BeginSendCallback, tcs);

#endif

            return tcs.Task;
        }

#if !NETSTANDARD

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

#endif

#if NETSTANDARD1_3

        private static readonly EventHandler<SocketAsyncEventArgs> OnSendCompleted = (sender, args) =>
        {
            var tcs = (TaskCompletionSource<int>)args.UserToken;
            tcs.TrySetSendResult(args);
        };

        private static void TrySetSendResult(this TaskCompletionSource<int> tcs, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                tcs.SetResult(args.SendPacketsSendSize);
            }
            else if (args.ConnectByNameError == null)
            {
                tcs.TrySetException(new SocketException());
            }
            else
            {
                tcs.TrySetException(args.ConnectByNameError);
            }
        }

#else

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

#endif
    }
}
