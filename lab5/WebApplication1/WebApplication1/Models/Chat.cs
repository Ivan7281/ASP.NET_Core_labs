namespace WebApplication1.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Message> Messages { get; set; }
    }
    public class Message
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Sender { get; set; }
    }
}
