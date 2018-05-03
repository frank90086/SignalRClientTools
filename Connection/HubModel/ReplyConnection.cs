namespace Omi.Education.Library.SignalR.Connection.HubModel
{
    public class ReplyContent
    {
        public ReplyStatus ReplyStatus { get; set; }
        public ReplyMethodName ReplyMethodName { get; set; }
        public string Content { get; set; }
    }
}