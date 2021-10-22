
namespace PixelStacker.WF.Components
{
    partial class ImageViewer
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
            this.repaintTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // repaintTimer
            // 
            this.repaintTimer.Enabled = true;
            this.repaintTimer.Interval = 15;
            this.repaintTimer.Tick += new System.EventHandler(this.repaintTimer_Tick);
            // 
            // ImageViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::PixelStacker.Resources.UIResources.bg_imagepanel;
            this.Name = "ImageViewer";
            this.Size = new System.Drawing.Size(538, 344);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ImageViewer_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ImageViewer_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ImageViewer_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer repaintTimer;
    }
}
