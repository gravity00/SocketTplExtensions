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
        public static Task<int> SendMessage(this Socket socket, string message, Encoding encoding, SocketFlags socketFlags)
        {
            NotNull(socket, nameof(socket));
            NotNull(message, nameof(message));

            if (message.Length == 0)
                return socket.SendAsync(EmptyBuffer, 0, 0, socketFlags);

            var buffer = encoding.GetBytes(message);
            return socket.SendAsync(buffer, 0, buffer.Length, socketFlags);
        }

        /// <summary>
        /// Sends the message encoded to a connected <see cref="Socket"/>.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="chars"></param>
        /// <param name="encoding"></param>
        /// <param name="socketFlags"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static Task<int> SendMessage(this Socket socket, char[] chars, int index, int count, 
            Encoding encoding, SocketFlags socketFlags)
        {
            NotNull(socket, nameof(socket));
            NotNull(chars, nameof(chars));

            if (chars.Length == 0)
                return socket.SendAsync(EmptyBuffer, 0, 0, socketFlags);

            var buffer = encoding.GetBytes(chars, index, count);
            return socket.SendAsync(buffer, 0, buffer.Length, socketFlags);
        }
    }
}
