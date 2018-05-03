using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Omi.Education.Library.SignalR.Connection.HubModel;

namespace Omi.Education.Library.SignalR.Connection {

    public class OmiHubConnector : IOmiHubConnector, IDisposable {
        private HubConnection connection;
        public string baseUri = "";
        public event EventHandler<ErrorEventArgs> CommunicationError;
        public event EventHandler<ReceiveEventArgs> ReceiveMessage;
        public string HubToken;
        public List<string> GroupList { get; set; }

        public OmiHubConnector (string uri) {
            baseUri = uri;
            HubToken = "";
            GroupList = new List<string> ();
        }
        private async Task<HubConnection> ConnectionAsync () {
            int reConnectionCount = 0;
            while (true) {
                var HubConnection = new HubConnectionBuilder ()
                    .WithUrl (baseUri)
                    .WithConsoleLogger ()
                    .Build ();

                try {
                    HubConnection.On<string> ("Reply", (Result) => {
                        ReplyContent result = PublicMethod.JsonDeSerialize<ReplyContent> (Result);
                        switch (result.ReplyMethodName) {
                            case ReplyMethodName.Connection:
                                HubToken = result.Content;
                                break;
                            case ReplyMethodName.Subscribe:
                                GroupList.Add (result.Content);
                                break;
                            case ReplyMethodName.DeSubscribe:
                                GroupList.Remove (result.Content);
                                break;
                            default:
                                break;
                        }
                    });
                    HubConnection.On<string> ("Send", (content) => {
                        if (ReceiveMessage != null) {
                            ReceiveMessage.Invoke (this, new ReceiveEventArgs (PublicMethod.JsonDeSerialize<SendContent> (content)));
                        }
                    });
                    await HubConnection.StartAsync ();
                    await awaitHubtoken ();
                    return HubConnection;
                } catch (Exception e) {
                    reConnectionCount++;
                    if (reConnectionCount >= 3) {
                        CommunicationError (this, new ErrorEventArgs (true, e.ToString ()));
                        return HubConnection;
                    }
                    await Task.Delay (5000);
                }
            }
        }

        private async Task awaitHubtoken () {
            int awaitHubCount = 0;
            while (String.IsNullOrEmpty (HubToken)) {
                awaitHubCount++;
                if (awaitHubCount > 3) {
                    throw new Exception ("HubToken等待逾時");
                }
                await Task.Delay (1000);
            }
        }

        public async Task Register () {
            connection = await ConnectionAsync ();
        }

        public Task Send (string messageContent) {
            try {
                connection.InvokeAsync ("Send", messageContent);
            } catch (Exception e) {
                CommunicationError (this, new ErrorEventArgs (true, e.ToString ()));
            }
            return Task.CompletedTask;
        }

        public Task Subscribe (string groupName) {
            try {
                connection.InvokeAsync ("Subscribe", groupName);
            } catch (Exception e) {
                CommunicationError (this, new ErrorEventArgs (true, e.ToString ()));
            }
            return Task.CompletedTask;
        }

        public Task DeSubscribe (string groupName) {
            try {
                connection.InvokeAsync ("DeSubscribe", groupName);
            } catch (Exception e) {
                CommunicationError (this, new ErrorEventArgs (true, e.ToString ()));
            }
            return Task.CompletedTask;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose (bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    connection.DisposeAsync ();
                    GC.SuppressFinalize (this);
                }
                disposedValue = true;
            }
        }
        public void Dispose () {
            Dispose (true);
        }
        #endregion
    }
}