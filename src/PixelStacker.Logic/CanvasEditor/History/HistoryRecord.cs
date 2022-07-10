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

        public List<RenderRecord> ToRenderRecords(bool isForward, RenderRecord.RenderRecordType renderMode = RenderRecord.RenderRecordType.ALL)
        {
            MaterialPalette mp = MaterialPalette.FromResx();
            List<RenderRecord> output = new List<RenderRecord>();
            foreach (var record in ChangeRecords)
            {
                var colorInt = isForward ? record.PaletteIDAfter : record.PaletteIDBefore;
#if FAIL_FAST
                if (mp[colorInt] == null) throw new KeyNotFoundException();
#else
                if (mp[colorInt] == null) return output;
#endif
                output.Add(new RenderRecord()
                {
                    ChangedPixels = record.ChangedPixels,
                    PaletteID = colorInt,
                    RenderMode = renderMode
                });
            }

            return RenderRecord.SplitRecordsIintoSMallerChunks(output);
        }
    }
}
