using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EllaSciFair.Models.Dialog
{
    public class MsgBoxShowEvent
    {
        public MsgBoxShowEvent()
        {
        }
        public string? Message { get; set; }
        public string? Title { get; set; }
        public DialogButtons? Buttons { get; set; }
        public TaskCompletionSource<string>? CompletionSource { get; set; }
    }
}