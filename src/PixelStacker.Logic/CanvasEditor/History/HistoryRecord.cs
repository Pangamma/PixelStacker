using PixelStacker.Logic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic.CanvasEditor.History
{
    public class HistoryRecord
    {
        public List<HistoryChangeInstruction> ChangeRecords { get; set; } = new List<HistoryChangeInstruction>();
    }
}
