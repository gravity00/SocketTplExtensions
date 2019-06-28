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
            NotNull(socket, nameof(socket));

            var tcs = new TaskCompletionSource<int>(socket);

#if NETSTANDARD1_3

            var args = new SocketAsyncEventArgs
            {
                SocketFlags = socketFlags,
                UserToken = tcs
            };
            args.SetBuffer(buffer, offset, size);
            args.Completed += OnReceiveCompleted;
            if (!socket.ReceiveAsync(args))
            {
                tcs.TrySetReceiveResult(args);
            }

#else
            
            socket.BeginReceive(buffer, offset, size, socketFlags, BeginReceiveCallback, tcs);

#endif

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
            NotNull(socket, nameof(socket));

            var tcs = new TaskCompletionSource<int>(socket);
            socket.BeginReceive(buffers, socketFlags, BeginReceiveCallback, tcs);
            return tcs.Task;
        }

#endif

#if NETSTANDARD1_3

        private static readonly EventHandler<SocketAsyncEventArgs> OnReceiveCompleted = (sender, args) =>
        {
            var tcs = (TaskCompletionSource<int>) args.UserToken;
            tcs.TrySetReceiveResult(args);
        };

        private static void TrySetReceiveResult(this TaskCompletionSource<int> tcs, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                tcs.SetResult(args.BytesTransferred);
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

#endif
    }
}
