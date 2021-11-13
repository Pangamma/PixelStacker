
namespace PixelStacker.UI.Controls
{
    partial class CanvasEditor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CanvasEditor));
            this.timerPaint = new System.Windows.Forms.Timer(this.components);
            this.skiaControl = new PixelStacker.UI.Controls.SkHybridControl();
            this.timerBufferedChangeQueue = new System.Windows.Forms.Timer(this.components);
            this.bgWorkerBufferedChangeQueue = new System.ComponentModel.BackgroundWorker();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsBrushWidth = new System.Windows.Forms.ToolStripLabel();
            this.tbxBrushWidth = new System.Windows.Forms.ToolStripTextBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerPaint
            // 
            this.timerPaint.Enabled = true;
            this.timerPaint.Interval = 20;
            this.timerPaint.Tick += new System.EventHandler(this.timerPaint_Tick);
            // 
            // skiaControl
            // 
            this.skiaControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skiaControl.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("skiaControl.BackgroundImage")));
            this.skiaControl.Location = new System.Drawing.Point(0, 0);
            this.skiaControl.Name = "skiaControl";
            this.skiaControl.Size = new System.Drawing.Size(611, 382);
            this.skiaControl.TabIndex = 0;
            this.skiaControl.PaintSurface += new System.EventHandler<PixelStacker.UI.Controls.GenericSKPaintSurfaceEventArgs>(this.skiaControl_PaintSurface);
            this.skiaControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ImagePanel_Click);
            this.skiaControl.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ImagePanel_DoubleClick);
            this.skiaControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ImagePanel_MouseDown);
            this.skiaControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ImagePanel_MouseMove);
            this.skiaControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ImagePanel_MouseUp);
            // 
            // timerBufferedChangeQueue
            // 
            this.timerBufferedChangeQueue.Enabled = true;
            this.timerBufferedChangeQueue.Interval = 20;
            this.timerBufferedChangeQueue.Tick += new System.EventHandler(this.timerBufferedChangeQueue_Tick);
            // 
            // bgWorkerBufferedChangeQueue
            // 
            this.bgWorkerBufferedChangeQueue.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorkerBufferedChangeQueue_DoWork);
            this.bgWorkerBufferedChangeQueue.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BgWorkerBufferedChangeQueue_RunWorkerCompleted);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsBrushWidth,
            this.tbxBrushWidth});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(611, 27);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsBrushWidth
            // 
            this.tsBrushWidth.Name = "tsBrushWidth";
            this.tsBrushWidth.Size = new System.Drawing.Size(89, 24);
            this.tsBrushWidth.Text = "Brush Width";
            // 
            // tbxBrushWidth
            // 
            this.tbxBrushWidth.AutoSize = false;
            this.tbxBrushWidth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxBrushWidth.Name = "tbxBrushWidth";
            this.tbxBrushWidth.Size = new System.Drawing.Size(40, 27);
            this.tbxBrushWidth.Text = "1";
            this.tbxBrushWidth.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbxBrushWidth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxBrushWidth_KeyPress);
            this.tbxBrushWidth.TextChanged += new System.EventHandler(this.tbxBrushWidth_TextChanged);
            // 
            // CanvasEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.RosyBrown;
            this.BackgroundImage = global::PixelStacker.Resources.UIResources.bg_imagepanel;
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.skiaControl);
            this.Name = "CanvasEditor";
            this.Size = new System.Drawing.Size(611, 382);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ImagePanel_Click);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ImagePanel_DoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ImagePanel_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ImagePanel_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ImagePanel_MouseUp);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timerPaint;
        private Controls.SkHybridControl skiaControl;
        private System.Windows.Forms.Timer timerBufferedChangeQueue;
        private System.ComponentModel.BackgroundWorker bgWorkerBufferedChangeQueue;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel tsBrushWidth;
        private System.Windows.Forms.ToolStripTextBox tbxBrushWidth;
    }
}
