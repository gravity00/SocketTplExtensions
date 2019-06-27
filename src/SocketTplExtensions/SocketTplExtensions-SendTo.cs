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

#if NETSTANDARD1_3

            var args = new SocketAsyncEventArgs
            {
                SocketFlags = socketFlags,
                RemoteEndPoint = endpoint,
                UserToken = tcs
            };
            args.SetBuffer(buffer, offset, size);
            args.Completed += OnSendToCompleted;
            if (!socket.SendToAsync(args))
            {
                tcs.TrySetSendToResult(args);
            }

#else
            
            socket.BeginSendTo(buffer, offset, size, socketFlags, endpoint, BeginSendToCallback, tcs);

#endif

            return tcs.Task;
        }

#if NETSTANDARD1_3

        private static readonly EventHandler<SocketAsyncEventArgs> OnSendToCompleted = (sender, args) =>
        {
            var tcs = (TaskCompletionSource<int>)args.UserToken;
            tcs.TrySetSendToResult(args);
        };

        private static void TrySetSendToResult(this TaskCompletionSource<int> tcs, SocketAsyncEventArgs args)
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

#endif
    }
}
