﻿
using System;
using System.Linq;
using Hik.Communication.Scs.Communication.Channels;
using Hik.Communication.Scs.Communication.Channels.Tcp;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Hik.Communication.Scs.Client.Tcp
{

    /// <summary>
    /// This class is used to communicate with server over TCP/IP protocol.
    /// </summary>
    internal class ScsTcpSslClient : ScsClientBase
    {
        /// <summary>
        /// The endpoint address of the server.
        /// </summary>
        private readonly ScsTcpEndPoint _serverEndPoint;

        //private readonly X509Certificate2 _serverCert;
        private readonly X509Certificate2Collection _clientCerts;
        private readonly string _nombreServerCert;
        private readonly Func<object, X509Certificate, X509Chain, SslPolicyErrors, bool> _remoteCertificateFalidatonCallback;
        private readonly Func<object, string, X509CertificateCollection, X509Certificate, string[], X509Certificate> _localCertificateSelectionCallback;


        public ScsTcpSslClient(ScsTcpEndPoint serverEndPoint, X509Certificate2 clientCert, string nombreServerCert, int pingTimeout,
            Func<object, X509Certificate, X509Chain, SslPolicyErrors, bool> remoteCertificateFalidatonCallback,
            Func<object, string, X509CertificateCollection, X509Certificate, string[], X509Certificate> localCertificateSelectionCallback)
            : this(serverEndPoint,new X509Certificate2Collection(clientCert),nombreServerCert,pingTimeout,remoteCertificateFalidatonCallback,localCertificateSelectionCallback) {
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverEndPoint"></param>
        /// <param name="serverCert"></param>
        /// <param name="clientCert"></param>
        /// <param name="nombreServerCert"></param>
        /// <param name="remoteCertificateFalidatonCallback"></param>
        /// <param name="localCertificateSelectionCallback"></param>
        public ScsTcpSslClient(ScsTcpEndPoint serverEndPoint,  X509Certificate2Collection clientCert, string nombreServerCert,int pingTimeout,Func<object, X509Certificate, X509Chain, SslPolicyErrors,bool> remoteCertificateFalidatonCallback,Func<object, string, X509CertificateCollection, X509Certificate, string[],X509Certificate> localCertificateSelectionCallback ):base(pingTimeout)
        {
            _serverEndPoint = serverEndPoint;            
            _clientCerts = clientCert;
            _nombreServerCert = nombreServerCert;
            _remoteCertificateFalidatonCallback = remoteCertificateFalidatonCallback;
            _localCertificateSelectionCallback = localCertificateSelectionCallback;
        }

        

        /// <summary>
        /// Creates a communication channel using ServerIpAddress and ServerPort.
        /// </summary>
        /// <returns>Ready communication channel to communicate</returns>
        protected override ICommunicationChannel CreateCommunicationChannel(params Tuple<SocketOptionLevel,SocketOptionName,object>[] socketOptions) {
            TcpClient client=null;
            //var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);            
            IPAddress addrss;
            try
            {
                client = new TcpClient();
                if (IPAddress.TryParse(_serverEndPoint.IpAddress,out addrss))
                {                            
                    client.Connect(new IPEndPoint(addrss, _serverEndPoint.TcpPort));
                }
                else
                {
                    var endpoint = new DnsEndPoint(_serverEndPoint.IpAddress, _serverEndPoint.TcpPort);                
                
                    client.Connect(endpoint.Host, endpoint.Port);
                }
                var sslStream = new SslStream(client.GetStream(), false, 
                    new RemoteCertificateValidationCallback(_remoteCertificateFalidatonCallback),
                    _localCertificateSelectionCallback == null?null:new LocalCertificateSelectionCallback(_localCertificateSelectionCallback)
                    );
                
                if (_clientCerts.Count<1)
                {
                    throw new Exception("No client certificate found");
                }
                sslStream.AuthenticateAsClient(_nombreServerCert, _clientCerts,ConnectionProperties.SslProtocol==SslProtocols.None? SslProtocols.Tls12:ConnectionProperties.SslProtocol, false);
                return new TcpSslCommunicationChannel( _serverEndPoint, client, sslStream);
            }
            catch (AuthenticationException)
            {
                client?.Close();
                throw;
            }

        }

        //public bool ValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        //{
        //    if (sslPolicyErrors == SslPolicyErrors.RemoteCertificateChainErrors)
        //    {
        //        return _serverCert.GetCertHashString().Equals(certificate.GetCertHashString());
        //    }
        //    else
        //    {
        //        return (sslPolicyErrors == SslPolicyErrors.None);
        //    }
        //}

        //public X509Certificate SelectLocalCertificate(object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
        //{
        //    return _clientCerts;
        //}
    }
}
