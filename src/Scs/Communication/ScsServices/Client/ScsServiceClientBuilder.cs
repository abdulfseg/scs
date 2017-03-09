using System;
using System.Net.Security;
using Hik.Communication.Scs.Communication.EndPoints;
using System.Security.Cryptography.X509Certificates;
namespace Hik.Communication.ScsServices.Client
{
    /// <summary>
    /// This class is used to build service clients to remotely invoke methods of a SCS service.
    /// </summary>
    public class ScsServiceClientBuilder
    {
        /// <summary>
        /// Creates a client to connect to a SCS service.
        /// </summary>
        /// <typeparam name="T">Type of service interface for remote method call</typeparam>
        /// <param name="endpoint">EndPoint of the server</param>
        /// <param name="clientObject">Client-side object that handles remote method calls from server to client.
        /// May be null if client has no methods to be invoked by server</param>
        /// <returns>Created client object to connect to the server</returns>
        public static IScsServiceClient<T> CreateClient<T>(ScsEndPoint endpoint,TimeSpan pingTimeout=default(TimeSpan), object clientObject = null) where T : class
        {
            return new ScsServiceClient<T>(endpoint.CreateClient(pingTimeout.Seconds<=0?30000:pingTimeout.Seconds), clientObject);
        }

        /// <summary>
        /// Creates a client to connect to a SCS service.
        /// </summary>
        /// <typeparam name="T">Type of service interface for remote method call</typeparam>
        /// <param name="endpointAddress">EndPoint address of the server</param>
        /// <param name="clientObject">Client-side object that handles remote method calls from server to client.
        /// May be null if client has no methods to be invoked by server</param>
        /// <returns>Created client object to connect to the server</returns>
        public static IScsServiceClient<T> CreateClient<T>(string endpointAddress,TimeSpan pingTimeout=default(TimeSpan), object clientObject = null) where T : class
        {
            return CreateClient<T>(ScsEndPoint.CreateEndPoint(endpointAddress),pingTimeout, clientObject);
        }

        /// <summary>
        /// SSL
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serverCert"></param>
        /// <param name="clientCert"></param>
        /// <param name="nombreServerCert"></param>
        /// <param name="endpoint"></param>
        /// <param name="pingTimeout"></param>
        /// <param name="remoteCertificateFalidatonCallback"></param>
        /// <param name="localCertificateSelectionCallback"></param>
        /// <param name="clientObject"></param>
        /// <returns></returns>
        public static IScsServiceClient<T> CreateSecureClient<T>( X509Certificate2 clientCert, string nombreServerCert, ScsEndPoint endpoint,TimeSpan pingTimeout=default(TimeSpan),Func<object, X509Certificate, X509Chain, SslPolicyErrors,bool> remoteCertificateFalidatonCallback=null,Func<object, string, X509CertificateCollection, X509Certificate, string[],X509Certificate> localCertificateSelectionCallback=null, object clientObject = null) where T : class
        {
            return new ScsServiceClient<T>(endpoint.CreateSecureClient(clientCert, nombreServerCert,pingTimeout.Seconds<=0?30000:pingTimeout.Seconds,remoteCertificateFalidatonCallback,localCertificateSelectionCallback), clientObject);
        }

        /// <summary>
        /// SSL
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clientCert"></param>
        /// <param name="nombreServerCert"></param>
        /// <param name="endpointAddress"></param>
        /// <param name="pingTimeout"></param>
        /// <param name="remoteCertificateFalidatonCallback"></param>
        /// <param name="localCertificateSelectionCallback"></param>
        /// <param name="clientObject"></param>
        /// <returns></returns>
        public static IScsServiceClient<T> CreateSecureClient<T>( X509Certificate2 clientCert, string nombreServerCert, string endpointAddress,TimeSpan pingTimeout=default(TimeSpan),Func<object, X509Certificate, X509Chain, SslPolicyErrors,bool> remoteCertificateFalidatonCallback=null,Func<object, string, X509CertificateCollection, X509Certificate, string[],X509Certificate> localCertificateSelectionCallback=null, object clientObject = null) where T : class
        {
            return CreateSecureClient<T>( clientCert, nombreServerCert, ScsEndPoint.CreateEndPoint(endpointAddress),pingTimeout,remoteCertificateFalidatonCallback,localCertificateSelectionCallback, clientObject);
        }
    }
}