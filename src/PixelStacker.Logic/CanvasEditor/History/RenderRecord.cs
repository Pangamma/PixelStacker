using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using System.Collections.Generic;
using System.Linq;

namespace PixelStacker.Logic.CanvasEditor.History
{
    public class RenderRecord
    {
        public enum RenderRecordType
        {
            SHADOWS_ONLY,
            BLOCKS_ONLY,
            ALL
        }

        public List<PxPoint> ChangedPixels { get; set; } = new List<PxPoint>();
        public int PaletteID { get; set; } = 0;

        /// <summary>
        /// If TRUE, only shadows will be rendered. Unless it needs to render the pic tile as well.
        /// If FALSE, only pic tiles will be rendered.
        /// </summary>
        public RenderRecordType RenderMode { get; set; } = RenderRecordType.BLOCKS_ONLY;


        public static List<RenderRecord> SplitRecordsIintoSMallerChunks(IEnumerable<RenderRecord> r)
        {
            var rSplit = new List<RenderRecord>();
            
            foreach (var ri in r)
            {
                int pages = (ri.ChangedPixels.Count / Constants.MaxPageSizeForRenders) + 1;
                for (int i = 0; i < pages; i++)
                {
                    rSplit.Add(new RenderRecord()
                    {
                        RenderMode = ri.RenderMode,
                        PaletteID = ri.PaletteID,
                        ChangedPixels = ri.ChangedPixels.Skip(Constants.MaxPageSizeForRenders * i).Take(Constants.MaxPageSizeForRenders).ToList()
                    });
                }
            }

            return rSplit;
        }
    }
}
