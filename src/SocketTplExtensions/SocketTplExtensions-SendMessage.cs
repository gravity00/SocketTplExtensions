using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace System.Net.Sockets
{
    public static partial class SocketTplExtensions
    {
        /// <summary>
        /// Sends the message encoded to a connected <see cref="Socket"/>.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="message"></param>
        /// <param name="encoding"></param>
        /// <param name="socketFlags"></param>
        /// <returns></returns>
        public static Task<int> SendMessageAsync(this Socket socket, string message, Encoding encoding, SocketFlags socketFlags)
        {
            NotNull(socket, nameof(socket));
            NotNull(message, nameof(message));
            NotNull(encoding, nameof(encoding));

            if (message.Length == 0)
                return socket.SendAsync(EmptyBuffer, 0, 0, socketFlags);

            var buffer = encoding.GetBytes(message);
            return socket.SendAsync(buffer, 0, buffer.Length, socketFlags);
        }

        /// <summary>
        /// Sends the message encoded to a connected <see cref="Socket"/>.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="message"></param>
        /// <param name="encoding"></param>
        /// <param name="buffer"></param>
        /// <param name="bufferIndex"></param>
        /// <param name="socketFlags"></param>
        /// <returns></returns>
        public static Task<int> SendMessageAsync(this Socket socket, string message, 
            Encoding encoding, byte[] buffer, int bufferIndex, SocketFlags socketFlags)
        {
            NotNull(socket, nameof(socket));
            NotNull(message, nameof(message));
            NotNull(encoding, nameof(encoding));
            NotNull(buffer, nameof(buffer));

            if (message.Length == 0)
                return socket.SendAsync(EmptyBuffer, 0, 0, socketFlags);

            var encodedBytes = encoding.GetBytes(message, 0, message.Length, buffer, bufferIndex);
            return socket.SendAsync(buffer, bufferIndex, bufferIndex + encodedBytes, socketFlags);
        }

        /// <summary>
        /// Sends the message encoded to a connected <see cref="Socket"/>.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="chars"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <param name="encoding"></param>
        /// <param name="socketFlags"></param>
        /// <returns></returns>
        public static Task<int> SendMessageAsync(this Socket socket, char[] chars, int index, int count, 
            Encoding encoding, SocketFlags socketFlags)
        {
            NotNull(socket, nameof(socket));
            NotNull(chars, nameof(chars));
            NotNull(encoding, nameof(encoding));

            if (chars.Length == 0)
                return socket.SendAsync(EmptyBuffer, 0, 0, socketFlags);

            var buffer = encoding.GetBytes(chars, index, count);
            return socket.SendAsync(buffer, 0, buffer.Length, socketFlags);
        }

        /// <summary>
        /// Sends the message encoded to a connected <see cref="Socket"/>.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="chars"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <param name="encoding"></param>
        /// <param name="buffer"></param>
        /// <param name="bufferIndex"></param>
        /// <param name="socketFlags"></param>
        /// <returns></returns>
        public static Task<int> SendMessageAsync(this Socket socket, char[] chars, int index, int count, 
            Encoding encoding, byte[] buffer, int bufferIndex, SocketFlags socketFlags)
        {
            NotNull(socket, nameof(socket));
            NotNull(chars, nameof(chars));
            NotNull(encoding, nameof(encoding));
            NotNull(buffer, nameof(buffer));

            if (chars.Length == 0)
                return socket.SendAsync(EmptyBuffer, 0, 0, socketFlags);

            var encodedBytes = encoding.GetBytes(chars, index, count, buffer, index);
            return socket.SendAsync(buffer, bufferIndex, bufferIndex + encodedBytes, socketFlags);
        }
    }
}
