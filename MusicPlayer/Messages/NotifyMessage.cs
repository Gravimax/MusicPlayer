using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
    public class NotifyMessage
    {
        public NotifyMessage(string title, string message)
        {
            this.Title = title;
            this.Message = message;
        }

        public readonly string Title;
        public readonly string Message;
    }
}
