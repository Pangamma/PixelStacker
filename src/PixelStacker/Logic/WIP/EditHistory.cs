using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic.WIP
{
    public class ChangeRecord
    {
        public List<Point> ChangedPixels { get; set; } = new List<Point>();
        public Color Before { get; set; }
        public Color After { get; set; }
    }

    public class EditHistory
    {
        private MainForm mf;
        public List<ChangeRecord> HistoryPast { get; set; } = new List<ChangeRecord>();
        public List<ChangeRecord> HistoryFuture { get; set; } = new List<ChangeRecord>();
        public bool IsAnyChangePossible { get => this.mf != null && this.mf.LoadedBlueprint != null && this.mf.renderedImagePanel != null; }
        public bool IsUndoEnabled { get => this.IsAnyChangePossible && this.HistoryPast.Any(); }
        public bool IsRedoEnabled { get => this.IsAnyChangePossible && this.HistoryFuture.Any(); }

        public EditHistory(MainForm mf)
        {
            this.mf = mf;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <param name="isForward">If true, it means you are DOING it. False = UNDO</param>
        private async Task ApplyChange(ChangeRecord record, bool isForward)
        {
            // Check if we can
            if (!this.IsAnyChangePossible)
            {
                return;
            }

            var colorInt = isForward ? record.After.ToArgb() : record.Before.ToArgb();
            foreach (var xy in record.ChangedPixels)
            {
                this.mf.LoadedBlueprint.BlocksMap[xy.X, xy.Y] = colorInt;
            }

            await this.mf.renderedImagePanel.ForceReRender();
        }

        public void Clear()
        {
            this.HistoryFuture.Clear();
            this.HistoryPast.Clear();
            this.SetMenuButtonStates();
        }

        public async Task AddChange(ChangeRecord record)
        {
            // Check if we can
            if (!this.IsAnyChangePossible)
            {
                this.SetMenuButtonStates();
                return;
            }

            this.HistoryFuture.Clear();
            await ApplyChange(record, true);
            this.HistoryPast.Add(record);

            while (this.HistoryPast.Count > 0 && this.HistoryPast.Count > Constants.MAX_HISTORY_SIZE)
            {
                this.HistoryPast.RemoveAt(0);
            }

            this.SetMenuButtonStates();
        }

        public async Task RedoChange()
        {
            // Check if we can
            if (!this.IsRedoEnabled)
            {
                this.SetMenuButtonStates();
                return;
            }

            var record = this.HistoryFuture.Last();
            this.HistoryFuture.RemoveAt(this.HistoryFuture.Count - 1);

            await ApplyChange(record, true);
            this.HistoryPast.Add(record);
            this.SetMenuButtonStates();
        }

        public async Task UndoChange()
        {
            // Check if we can
            if (!this.IsUndoEnabled)
            {
                this.SetMenuButtonStates();
                return;
            }

            var record = this.HistoryPast.Last();
            this.HistoryPast.RemoveAt(this.HistoryPast.Count - 1);

            await ApplyChange(record, false);
            this.HistoryFuture.Add(record);
            this.SetMenuButtonStates();
        }

        private void SetMenuButtonStates()
        {
            this.mf.InvokeEx((_mfi) => {
                _mfi.redoToolStripMenuItem.Enabled = this.IsRedoEnabled;
                _mfi.undoToolStripMenuItem.Enabled = this.IsUndoEnabled;
                _mfi.editToolStripMenuItem.Enabled = this.IsRedoEnabled || this.IsUndoEnabled;
            });
        }
    }
}
