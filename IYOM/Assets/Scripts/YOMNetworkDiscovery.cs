using System.Net;
using Mirror;
using Mirror.Discovery;
using UnityEngine;

public class DiscoveryRequest : NetworkMessage
{

}

public class DiscoveryResponse : NetworkMessage
{
    public string Ip4;
}

public class YOMNetworkDiscovery : NetworkDiscoveryBase<DiscoveryRequest, DiscoveryResponse>
{

    #region Server

    public void StartServer()
    {
        AdvertiseServer();
    }

    protected override void ProcessClientRequest(DiscoveryRequest request, IPEndPoint endpoint)
    {
        base.ProcessClientRequest(request, endpoint);
    }


    /// Process the request from a client

    /// Override if you wish to provide more information to the clients
    /// such as the name of the host player

    /// <param name="request">Request coming from client</param>
    /// <param name="endpoint">Address of the client that sent the request</param>
    /// <returns>A message containing information about this server</returns>
    protected override DiscoveryResponse ProcessRequest(DiscoveryRequest request, IPEndPoint endpoint) 
    {
        return new DiscoveryResponse();
    }

    #endregion

    #region Client

    /// Create a message that will be broadcasted on the network to discover servers
    
    /// Override if you wish to include additional data in the discovery message
    /// such as desired game mode, language, difficulty, etc... </remarks>
    /// <returns>An instance of ServerRequest with data to be broadcasted</returns>
    protected override DiscoveryRequest GetRequest()
    {
        return new DiscoveryRequest();
    }

    /// Process the answer from a server
   
    /// A client receives a reply from a server, this method processes the
    /// reply and raises an event
    /// <param name="response">Response that came from the server</param>
    /// <param name="endpoint">Address of the server that replied</param>
    protected override void ProcessResponse(DiscoveryResponse response, IPEndPoint endpoint) 
    {
        print("Recieved Endpoint from server " + endpoint.Address);
        FindObjectOfType<ClientLobby>().FoundServer(endpoint.Address.ToString());
    }

    #endregion
}
