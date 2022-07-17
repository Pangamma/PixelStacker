using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelStacker.Logic.CanvasEditor.History
{
    /// <summary>
    /// Throw block changes in. Get history records and render instructions out.
    /// </summary>
    public class SuperHistory
    {
        #region History
        public List<HistoryRecord> HistoryPast { get; set; } = new List<HistoryRecord>();
        public List<HistoryRecord> HistoryFuture { get; set; } = new List<HistoryRecord>();
        public bool IsUndoEnabled => HistoryPast.Count > 0;
        public bool IsRedoEnabled => HistoryFuture.Count > 0;


        /// <summary>
        /// Applies the change to the underlying canvas data and then returns a queue of items that can be used
        /// to render changes in the buffer queue.
        /// </summary>
        /// <param name="record"></param>
        /// <param name="isForward">If true, it means you are DOING it. False = UNDO</param>
        private List<RenderRecord> ApplyChangeToCanvasData(HistoryRecord records, bool isForward, RenderRecord.RenderRecordType renderType = RenderRecord.RenderRecordType.ALL)
        {
            this.Canvas.IsCustomized = true;
            List<RenderRecord> output = new List<RenderRecord>();
            foreach (var record in records.ChangeRecords)
            {
                var colorInt = isForward ? record.PaletteIDAfter : record.PaletteIDBefore;
                if (this.Canvas.MaterialPalette[colorInt] == null) return output;
                output.Add(new RenderRecord()
                {
                    ChangedPixels = record.ChangedPixels,
                    PaletteID = colorInt,
                    RenderMode = renderType
                });

                foreach (var xy in record.ChangedPixels)
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    this.Canvas.CanvasData.SetDirectly(xy.X, xy.Y, colorInt);
#pragma warning restore CS0618 // Type or member is obsolete
                }
            }

            return RenderRecord.SplitRecordsIintoSMallerChunks(output);
        }

        /// <summary>
        /// A change is a record of a completed action. This function adds history to undo/redo.
        /// of undo/redo
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        private List<RenderRecord> AddHistory(HistoryRecord record)
        {
            this.HistoryFuture.Clear();
            var renderRecords = ApplyChangeToCanvasData(record, true);
            this.HistoryPast.Add(record);

            while (this.HistoryPast.Count > 0 && this.HistoryPast.Count > Constants.MAX_HISTORY_SIZE)
            {
                this.HistoryPast.RemoveAt(0);
            }

            return renderRecords;
        }

        /// <summary>
        /// Will re-render as well.
        /// </summary>
        public void RedoChange()
        {
            // Check if we can
            if (!this.IsRedoEnabled) return;

            var record = this.HistoryFuture.Last();
            this.HistoryFuture.RemoveAt(this.HistoryFuture.Count - 1);

            var renderRecords = ApplyChangeToCanvasData(record, true, RenderRecord.RenderRecordType.ALL);
            this.HistoryPast.Add(record);
            lock (padlock)
            {
                this.RenderQueue.Enqueue(renderRecords);
            }
        }

        public void UndoChange()
        {
            // Check if we can
            if (!this.IsUndoEnabled) return;

            var record = this.HistoryPast.Last();
            this.HistoryPast.RemoveAt(this.HistoryPast.Count - 1);

            var renderRecords = ApplyChangeToCanvasData(record, false, RenderRecord.RenderRecordType.ALL);
            this.HistoryFuture.Add(record);
            lock (padlock)
            {
                this.RenderQueue.Enqueue(renderRecords);
            }
        }
        #endregion History

        #region HistoryBuffer
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


        /// <summary>
        /// Purely maps a point to the most recent history change FROM (oldest) TO (latest).
        /// </summary>
        private Dictionary<PxPoint, BufferedHistoryNode> Coordinates = new Dictionary<PxPoint, BufferedHistoryNode>();

        /// <summary>
        /// Purely contains the most recent and up-to-date palette ID for the pixels in the map.
        /// Useful for quickly rendering the most recent changes onto the canvas.
        /// </summary>
        private Dictionary<PxPoint, int> VisualBuffer = new Dictionary<PxPoint, int>();

        public int RenderCount { get { lock (padlock) { return VisualBuffer.Count; } } }
        public int CoordinatesCount { get { lock (padlock) { return VisualBuffer.Count; } } }
        #endregion HistoryBuffer


        private RenderedCanvas Canvas { get; }
        private object padlock = new object();
        private Queue<List<RenderRecord>> RenderQueue = new Queue<List<RenderRecord>>();

        public SuperHistory(RenderedCanvas canvas)
        {
            this.Canvas = canvas;
        }

        public bool HasChunksToRender()
        {
            this.FlushVisualBufferToRenderQueue();
            lock (padlock)
            {
                return this.RenderQueue.Count > 0;
            }
        }

        public bool TryGetRenderChunks(out List<RenderRecord> result)
        {
            // flush buffer to a record. Throw the record into the render queue.
            this.FlushVisualBufferToRenderQueue();
            lock (padlock)
            {
                var f = RenderQueue.TryDequeue(out result);
                return f;
            }
        }


        /// <summary>
        /// Call this method to mark the end of a tool operation on the canvas.
        /// This absolutely needs a better name, but I don't want to spend 15m
        /// trying to decide on a good name right now. lol.
        /// </summary>
        public void FlushHistoryBufferAndFlushVisualBufferThenRenderIt()
        {
            this.FlushVisualBufferToRenderQueue();
            var historyRecord = this.FlushCurrentHistoryBuffer();
            this.AddHistory(historyRecord);
            var toRender = historyRecord.ToRenderRecords(true, RenderRecord.RenderRecordType.SHADOWS_ONLY);
            lock (padlock)
            {
                RenderQueue.Enqueue(toRender);
            }
        }

        /// <summary>
        /// Pops the full stack of changes off the stack. This is the entire change.
        /// Use it for redo/undo operations.
        /// </summary>
        /// <param name="purge"></param>
        /// <returns></returns>
        private HistoryRecord FlushCurrentHistoryBuffer()
        {
            lock (padlock)
            {
                var r = new HistoryRecord();
                if (Coordinates.Count == 0) return r;

                Dictionary<PxPoint, BufferedHistoryNode> tmp = Coordinates;
                Coordinates = new Dictionary<PxPoint, BufferedHistoryNode>();

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




        /// <summary>
        /// Clears current VisualBuffer, and transfers contents into the 
        /// render queue
        /// </summary>
        /// <param name="purge"></param>
        /// <returns></returns>
        public void FlushVisualBufferToRenderQueue()
        {
            lock (padlock)
            {
                if (VisualBuffer.Count == 0) return;
                var tmp = VisualBuffer;
                VisualBuffer = new Dictionary<PxPoint, int>();

                var r = tmp.GroupBy(x => x.Value)
                    .Select(x => new RenderRecord()
                    {
                        PaletteID = x.Key,
                        ChangedPixels = x.Select(x => x.Key).ToList(),
                        RenderMode = RenderRecord.RenderRecordType.BLOCKS_ONLY
                    });

                //this.RenderQueue.Enqueue(rSplit);
                this.RenderQueue.Enqueue(RenderRecord.SplitRecordsIintoSMallerChunks(r));
            }
        }


        ///// <summary>
        ///// A change is a record of a completed action. This function adds history to undo/redo.
        ///// of undo/redo
        ///// </summary>
        ///// <param name="record"></param>
        ///// <returns></returns>
        //public List<RenderRecord> AddHistory(HistoryRecord record)
        //{
        //    var items = this.History.AddHistory(record);

        //    return items;
        //    //    this.HistoryFuture.Clear();
        //    //    var renderRecords = ApplyChangeToData(record, true);
        //    //    this.HistoryPast.Add(record);

        //    //    while (this.HistoryPast.Count > 0 && this.HistoryPast.Count > Constants.MAX_HISTORY_SIZE)
        //    //    {
        //    //        this.HistoryPast.RemoveAt(0);
        //    //    }

        //    //    this.OnHistoryChange?.Invoke(record, true);
        //    //    return renderRecords;
        //}

        ///// <summary>
        ///// Will re-render as well.
        ///// </summary>
        //public List<RenderRecord> RedoChange()
        //{
        //    var items = this.History.RedoChange();

        //    return items;
        //    // Check if we can
        //    //if (!this.IsRedoEnabled) return new List<RenderRecord>();

        //    //var record = this.HistoryFuture.Last();
        //    //this.HistoryFuture.RemoveAt(this.HistoryFuture.Count - 1);

        //    //var renderRecords = ApplyChangeToData(record, true);
        //    //this.HistoryPast.Add(record);
        //    //this.OnHistoryChange?.Invoke(record, true);
        //    //return renderRecords;
        //}


        ///// <summary>
        ///// If undo is enabled, this method moves the history record around in the stack.
        ///// It will then also apply this change to the data itself in the canvas.
        ///// </summary>
        ///// <returns></returns>
        //public List<RenderRecord> UndoChange()
        //{
        //    // Possible issue: Undo/Redo while buffer is not empty??
        //    // Snapshot current buffer. Maybe clear it.
        //    List<RenderRecord> items = this.History.UndoChange();

        //    // Now add render items to the list of things to render.

        //    return items;
        //}

        public void AppendToVisualBuffer(int PaletteIDBefore, int PaletteIDAfter, PxPoint loc)
        {
            lock (padlock)
            {
                VisualBuffer[loc] = PaletteIDAfter;
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

        public void AppendToVisualBuffer(int PaletteIDBefore, int PaletteIDAfter, List<PxPoint> locs)
        {
            lock (padlock)
            {
                foreach (var loc in locs)
                {
                    VisualBuffer[loc] = PaletteIDAfter;
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


        public class HistoryChunk
        {
            public DateTime Time { get; set; }
            public List<RenderRecord> Records { get; set; }
            public bool IsForShadows { get; set; } = false;
        }
    }
}
