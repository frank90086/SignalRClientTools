using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Omi.Education.Library.SignalR.Connection.HubModel;

namespace Omi.Education.Library.SignalR.Connection
{
    public interface IOmiHubConnector
    {
       event EventHandler<ErrorEventArgs> CommunicationError;
       event EventHandler<ReceiveEventArgs> ReceiveMessage;
       Task Register();
       Task Send(string messageContent);
       Task Subscribe(string groupName);
       Task DeSubscribe(string groupName);
       
    }
}