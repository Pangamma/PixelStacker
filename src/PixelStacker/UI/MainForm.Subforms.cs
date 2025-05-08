using PixelStacker.Extensions;
using PixelStacker.Logic.Collections.ColorMapper;
using PixelStacker.Logic.Utilities;
using PixelStacker.UI.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PixelStacker.UI
{
    public partial class MainForm
    {
        private MaterialSelectWindow MaterialOptions { get; set; } = null;
        private ColorReducerForm ColorReducerForm { get; set; } = null;
        public MaterialPickerForm MaterialPickerForm { get; private set; }

        private void gridOptionsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            using var form = new GridSettingsForm();
            form.Options = this.Options;
            this.snapManager.RegisterChild(form);
            form.ShowDialog(this);
        }

        private void selectMaterialsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.MaterialOptions == null)
            {
                this.MaterialOptions = new MaterialSelectWindow(this.Options);

                this.MaterialOptions.OnColorPaletteRecompileRequested = (token) =>
                {
                    ProgressX.Report(40, Resources.Text.Progress_CompilingColorMap);
                    this.InvokeEx(c =>
                    {
                        var combos = c.Palette.ToValidCombinationList(this.Options);
                        c.ColorMapper.SetSeedData(combos, this.Palette, this.Options.IsSideView);
                    });
                    ProgressX.Report(100, Resources.Text.Progress_CompiledColorMap);
                };
            }

            this.MaterialOptions.ShowDialog(this);
        }

        private void contributorsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var form = new Credits();
            form.ShowDialog(this);
        }

        private void sizingToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var form = new SizeForm(this.Options);
            form.ShowDialog(this);
        }

        private void preprocessingToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (this.ColorReducerForm == null || this.ColorReducerForm.IsDisposed)
            {
                this.ColorReducerForm = new ColorReducerForm(this, this.Options);
                this.snapManager.RegisterChild(this.ColorReducerForm);
            }

            if (!this.ColorReducerForm.Visible)
            {
                this.ColorReducerForm.Show(this);
            }
        }

        private void colorMatchAlgorithmToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            string title = "Select an algorithm";
            IColorMapper[] mappers = new IColorMapper[] {
                new KdTreeMapper(),
                new AverageColorKdTreeMapper(),
                new SrgbKdTreeMapper(),
                new CielabColorMapper()
            };

            Dictionary<string, IColorMapper> items = mappers.ToDictionary(x => x.AlgorithmTitle, v => v);


            Form prompt = new Form()
            {
                Width = 300,
                Height = 400,
                Text = title,
                MaximizeBox = false,
                MinimizeBox = false,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen
            };

            ListBox listBox = new ListBox() { Dock = DockStyle.Fill };
            listBox.Items.AddRange(items.Keys.Cast<object>().ToArray());
            listBox.SelectedItem = this.ColorMapper.AlgorithmTitle;
            listBox.SelectedValueChanged += (object sender, System.EventArgs e) =>
            {
                if (listBox.SelectedItem != null)
                {
                    string displayText = listBox.GetItemText(listBox.SelectedItem);

                    if (displayText != null && items.TryGetValue(displayText, out IColorMapper value))
                    {
                        this.ColorMapper = value;
                        renderToolStripMenuItem_Click(sender, null);
                    }
                }
            };

            Button okButton = new Button()
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Dock = DockStyle.Bottom,
                Anchor = AnchorStyles.Right,
                Width = 80,
                Height = 30
            };

            okButton.Click += (sender, e) => {
                string displayText = listBox.GetItemText(listBox.SelectedItem);
                if (displayText != null && items.TryGetValue(displayText, out IColorMapper value))
                {
                    this.ColorMapper = value;
                    renderToolStripMenuItem_Click(sender, null);
                }
                prompt.Close();
            };

            prompt.AcceptButton = okButton;

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                Padding = new Padding(10),
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // ListBox takes most space
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));       // Button fits content
            layout.Controls.Add(listBox, 0, 0);
            layout.Controls.Add(okButton, 0, 1);
            prompt.Controls.Add(layout);

            prompt.Show(this);
        }
    }
}
