// ReSharper disable once CheckNamespace
namespace System.Net.Sockets
{
    /// <summary>
    /// Extensions for <see cref="Socket"/>
    /// </summary>
    public static partial class SocketTplExtensions
    {
        private static readonly byte[] EmptyBuffer = new byte[0];

        private static void NotNull(object value, string paramName)
        {
            if (value == null) throw new ArgumentNullException(paramName);
        }
    }
}
