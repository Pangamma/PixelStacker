using PixelStacker.Logic.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelStacker.Logic.CanvasEditor.History
{
    /// <summary>
    /// This class helps to minimize rendered changes.
    /// </summary>
    public class BufferedHistory
    {
        class BufferedHistoryNode : ISuperEquatable<BufferedHistoryNode>
        {
            public int BeforeID;
            public int AfterID;

            public bool Equals(BufferedHistoryNode other)
            {
                if (other is null) return false;
                if (other.BeforeID != BeforeID || other.AfterID != AfterID) return false;
                return true;
            }

            public override bool Equals(object other)
            {
                if (other is null) return false;
                if (other is not BufferedHistoryNode) return false;
                if (other.GetHashCode() != this.GetHashCode()) return false;
                return true;
            }

            public override int GetHashCode() => HashCode.Combine(BeforeID, AfterID);
        }

        public object padlock = new { };

        /// <summary>
        /// Purely maps a point to the most recent history change FROM (oldest) TO (latest).
        /// </summary>
        private Dictionary<PxPoint, BufferedHistoryNode> Coordinates = new Dictionary<PxPoint, BufferedHistoryNode>();

        /// <summary>
        /// Purely contains the most recent and up-to-date palette ID for the pixels in the map.
        /// Useful for quickly rendering the most recent changes onto the canvas.
        /// </summary>
        private Dictionary<PxPoint, int> RenderBuffer = new Dictionary<PxPoint, int>();

        public int RenderCount { get { lock (padlock) { return RenderBuffer.Count; } } }
        public int CoordinatesCount { get { lock (padlock) { return RenderBuffer.Count; } } }


        /// <summary>
        /// Pops the full stack of changes off the stack. This is the entire change.
        /// Use it for redo/undo operations.
        /// </summary>
        /// <param name="purge"></param>
        /// <returns></returns>
        public HistoryRecord ToHistoryRecord(bool purge = true)
        {
            lock (padlock)
            {
                var r = new HistoryRecord();
                if (Coordinates.Count == 0) return r;
                
                Dictionary<PxPoint, BufferedHistoryNode> tmp = Coordinates;

                if (purge)
                {
                    Coordinates = new Dictionary<PxPoint, BufferedHistoryNode>();
                }

                var rg = tmp.GroupBy(x => x.Value)
                    .Select(x => new HistoryChangeInstruction()
                    {
                        PaletteIDAfter = x.Key.AfterID,
                        PaletteIDBefore = x.Key.BeforeID,
                        ChangedPixels = x.Select(x => x.Key).ToList()
                    }).ToList();

                r.ChangeRecords = rg;
                return r;
            }
        }

        public List<RenderRecord> ToRenderInstructions(bool purge = true)
        {
            lock (padlock)
            {
                if (RenderBuffer.Count == 0) return new List<RenderRecord>();
                var tmp = RenderBuffer;
                if (purge)
                {
                    RenderBuffer = new Dictionary<PxPoint, int>();
                }

                var r = tmp.GroupBy(x => x.Value)
                    .Select(x => new RenderRecord()
                    {
                        PaletteID = x.Key,
                        ChangedPixels = x.Select(x => x.Key).ToList()
                    }).ToList();
                return r;
            }
        }

        public void AppendVisualChange(HistoryChangeInstruction record) 
            => this.AppendVisualChange(record.PaletteIDBefore, record.PaletteIDAfter, record.ChangedPixels);
        
        public void AppendVisualChange(int PaletteIDBefore, int PaletteIDAfter, PxPoint loc)
        {
            lock (padlock)
            {
                RenderBuffer[loc] = PaletteIDAfter;
                if (Coordinates.TryGetValue(loc, out BufferedHistoryNode node))
                {
                    node.AfterID = PaletteIDAfter;
                }
                else
                {
                    Coordinates[loc] = new BufferedHistoryNode() { BeforeID = PaletteIDBefore, AfterID = PaletteIDAfter };
                }
            }
        }
        
        public void AppendVisualChange(int PaletteIDBefore, int PaletteIDAfter, List<PxPoint> locs)
        {
            lock (padlock)
            {
                foreach (var loc in locs)
                {
                    RenderBuffer[loc] = PaletteIDAfter;
                    if (Coordinates.TryGetValue(loc, out BufferedHistoryNode node))
                    {
                        node.AfterID = PaletteIDAfter;
                    }
                    else
                    {
                        Coordinates[loc] = new BufferedHistoryNode() { BeforeID = PaletteIDBefore, AfterID = PaletteIDAfter };
                    }
                }
            }
        }
    }
}