using System;

namespace Omi.Education.Library.SignalR.Connection.HubModel
{
    public class ReceiveEventArgs : EventArgs
    {
        public ReceiveEventArgs(SendContent receiveContent)
        {
            this.ReceiveContent = receiveContent;
            this.ReceiveTime = DateTime.Now;
        }
        public SendContent ReceiveContent {get; set;}
        public DateTime ReceiveTime {get; set;}
    }
}