using System;

namespace Omi.Education.Library.SignalR.Connection.HubModel
{
    public class ErrorEventArgs : EventArgs
    {
        public ErrorEventArgs(bool reTry, string message)
        {
            this.ReTry = ReTry;
            this.Message = message;
        }
        public bool ReTry {get; set;}
        public string Message {get; set;}
    }
}

