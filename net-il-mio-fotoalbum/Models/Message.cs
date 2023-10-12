namespace net_il_mio_fotoalbum.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Text { get; set; }

        public Message(string email, string text)
        {
            this.Email = email;
            this.Text = text;
        }

        public Message() { }
    }


}
