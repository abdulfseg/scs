using System.Security.Authentication;

namespace Hik.Communication.Scs {
    /// <summary>
    /// 
    /// </summary>
    public static class ConnectionProperties {
        /// <summary>
        /// override the default sslProtocol, 
        /// If not specified or None - SslProtocols.Tls12 will be used
        /// </summary>
        public static SslProtocols SslProtocol { get; set; }
    }
}