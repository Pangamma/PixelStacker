using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.UI.Events
{
    public class CommandKeyEvent : EventArgs
    {
        public bool IsHandled { get; set; }
        public Message Message { get; }
        public Keys Keys { get; }

        public CommandKeyEvent(Message m, Keys k)
        {
            this.Message = m;
            this.Keys = k;
            this.IsHandled = false;
        }
    }
}
