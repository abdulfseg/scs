using System;
using System.Net.Security;
using Hik.Communication.Scs.Communication.EndPoints;
using System.Security.Cryptography.X509Certificates;
namespace Hik.Communication.Scs.Client
{
    /// <summary>
    /// This class is used to create SCS Clients to connect to a SCS server.
    /// </summary>
    public static class ScsClientFactory
    {
        /// <summary>
        /// Creates a new client to connect to a server using an end point.
        /// </summary>
        /// <param name="endpoint">End point of the server to connect it</param>
        /// <returns>Created TCP client</returns>
        public static IScsClient CreateClient(ScsEndPoint endpoint,TimeSpan pingTimeout)
        {
            return endpoint.CreateClient(pingTimeout.Seconds<=0?30000:pingTimeout.Seconds);
        }

        /// <summary>
        /// Creates a new client to connect to a server using an end point.
        /// </summary>
        /// <param name="endpointAddress">End point address of the server to connect it</param>
        /// <returns>Created TCP client</returns>
        public static IScsClient CreateClient(string endpointAddress,TimeSpan pingTimeout)
        {
            return CreateClient(ScsEndPoint.CreateEndPoint(endpointAddress),pingTimeout);
        }


        /// <summary>
        /// SSL
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="serverCert"></param>
        /// <param name="clientCert"></param>
        /// <param name="nombreServerCert"></param>
        /// <param name="pingTimeout"></param>
        /// <param name="remoteCertificateFalidatonCallback"></param>
        /// <param name="localCertificateSelectionCallback"></param>
        /// <returns></returns>
        public static IScsClient CreateSecureClient(ScsEndPoint endpoint, X509Certificate2Collection clientCert, string nombreServerCert, TimeSpan pingTimeout, Func<object, X509Certificate, X509Chain, SslPolicyErrors, bool> remoteCertificateFalidatonCallback = null, Func<object, string, X509CertificateCollection, X509Certificate, string[], X509Certificate> localCertificateSelectionCallback = null) {
            return endpoint.CreateSecureClient( clientCert, nombreServerCert,pingTimeout.Seconds<=0?30000:pingTimeout.Seconds,remoteCertificateFalidatonCallback,localCertificateSelectionCallback);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="clientCert"></param>
        /// <param name="nombreServerCert"></param>
        /// <param name="pingTimeout"></param>
        /// <param name="remoteCertificateFalidatonCallback"></param>
        /// <param name="localCertificateSelectionCallback"></param>
        /// <returns></returns>
        public static IScsClient CreateSecureClient(ScsEndPoint endpoint, X509Certificate2 clientCert, string nombreServerCert,TimeSpan pingTimeout,Func<object, X509Certificate, X509Chain, SslPolicyErrors,bool> remoteCertificateFalidatonCallback=null,Func<object, string, X509CertificateCollection, X509Certificate, string[],X509Certificate> localCertificateSelectionCallback=null)
        {
            return endpoint.CreateSecureClient( clientCert, nombreServerCert,pingTimeout.Seconds<=0?30000:pingTimeout.Seconds,remoteCertificateFalidatonCallback,localCertificateSelectionCallback);
        }

        /// <summary>
        /// SSL
        /// </summary>
        /// <param name="endpointAddress"></param>
        /// <returns></returns>
        public static IScsClient CreateSecureClient(string endpointAddress,TimeSpan pingTimeout)
        {
            return CreateClient(ScsEndPoint.CreateEndPoint(endpointAddress),pingTimeout);
        }
    }
}