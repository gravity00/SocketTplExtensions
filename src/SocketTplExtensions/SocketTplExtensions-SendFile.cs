using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace System.Net.Sockets
{
    public static partial class SocketTplExtensions
    {
#if !NETSTANDARD1_3

        /// <summary>
        /// Sends the file <paramref name="filename" /> to a connected <see cref="Socket" /> object using
        /// the <see cref="TransmitFileOptions.UseDefaultWorkerThread" /> flag.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static Task SendFileAsync(this Socket socket, string filename)
        {
            NotNull(socket, nameof(socket));

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
            NotNull(socket, nameof(socket));

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

#endif
    }
}
