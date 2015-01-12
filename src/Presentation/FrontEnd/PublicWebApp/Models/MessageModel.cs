using System.Collections.ObjectModel;

namespace VirtoCommerce.Web.Models
{

    public enum MessageType
    {
        Success = 0,
        Note,
        Notice,
        Error
    }
    public class MessageModel
    {
        public MessageModel(string text)
        {
            Text = text;
        }
        public MessageModel(string text, MessageType type)
            : this(text)
        {
            Type = type;
        }

        public string Text { get; set; }
        public MessageType Type { get; set; }
    }

    public class MessagesModel : Collection<MessageModel>
    {
        
    }
}