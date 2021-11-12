
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
            this.timerBufferedChangeQueue.Tick += new System.EventHandler(this.timerBufferedChangeQueue_Tick);
            // 
            // CanvasEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.RosyBrown;
            this.BackgroundImage = global::PixelStacker.Resources.UIResources.bg_imagepanel;
            this.Controls.Add(this.skiaControl);
            this.Name = "CanvasEditor";
            this.Size = new System.Drawing.Size(611, 382);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ImagePanel_Click);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ImagePanel_DoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ImagePanel_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ImagePanel_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ImagePanel_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timerPaint;
        private Controls.SkHybridControl skiaControl;
        private System.Windows.Forms.Timer timerBufferedChangeQueue;
    }
}
