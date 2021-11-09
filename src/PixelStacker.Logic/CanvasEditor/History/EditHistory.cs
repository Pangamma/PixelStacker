using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic.CanvasEditor.History
{
    public class EditHistory
    {
        public List<ChangeRecord> HistoryPast { get; set; } = new List<ChangeRecord>();
        public List<ChangeRecord> HistoryFuture { get; set; } = new List<ChangeRecord>();
        public Queue<RenderRecord> BufferedRenderQueue { get; set; } = new Queue<RenderRecord>();
        public bool IsUndoEnabled => HistoryPast.Count > 0;
        public bool IsRedoEnabled => HistoryFuture.Count > 0;

        private RenderedCanvas Canvas = null;

        public EditHistory (RenderedCanvas canvas)
        {
            this.Canvas = canvas;
        }

        public void Clear()
        {
            this.HistoryFuture.Clear();
            this.HistoryPast.Clear();
            this.BufferedRenderQueue.Clear();
            //this.SetMenuButtonStates();
        }

        /// <summary>
        /// Applies the change to the underlying canvas data and then queues the change record to the buffer queue.
        /// </summary>
        /// <param name="record"></param>
        /// <param name="isForward">If true, it means you are DOING it. False = UNDO</param>
        private void ApplyChange(ChangeRecord record, bool isForward)
        {
            var colorInt = isForward ? record.PaletteIDAfter : record.PaletteIDBefore;
            if (this.Canvas.MaterialPalette[colorInt] == null) return;
            foreach (var xy in record.ChangedPixels)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                this.Canvas.CanvasData.SetDirectly(xy.X, xy.Y, colorInt);
#pragma warning restore CS0618 // Type or member is obsolete
            }

            this.BufferedRenderQueue.Enqueue(new RenderRecord() { ChangedPixels = record.ChangedPixels, PaletteID = colorInt });
        }


        public void AddChange(ChangeRecord record)
        {
            this.HistoryFuture.Clear();
            ApplyChange(record, true);
            this.HistoryPast.Add(record);

            while (this.HistoryPast.Count > 0 && this.HistoryPast.Count > Constants.MAX_HISTORY_SIZE)
            {
                this.HistoryPast.RemoveAt(0);
            }
        }

        public void RedoChange()
        {
            // Check if we can
            if (!this.IsRedoEnabled) return;

            var record = this.HistoryFuture.Last();
            this.HistoryFuture.RemoveAt(this.HistoryFuture.Count - 1);

            ApplyChange(record, true);
            this.HistoryPast.Add(record);
        }

        public void UndoChange()
        {
            // Check if we can
            if (!this.IsUndoEnabled) return;

            var record = this.HistoryPast.Last();
            this.HistoryPast.RemoveAt(this.HistoryPast.Count - 1);

            ApplyChange(record, false);
            this.HistoryFuture.Add(record);
        }
    }
}
