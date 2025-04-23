namespace PixelStacker.UI.Controls
{
    partial class ImageButtonContainer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageButtonContainer));
            skHybridControl = new SkHybridControl();
            SuspendLayout();
            // 
            // skHybridControl
            // 
            skHybridControl.BackColor = System.Drawing.SystemColors.ControlDark;
            skHybridControl.BackgroundImage = (System.Drawing.Image)resources.GetObject("skHybridControl.BackgroundImage");
            skHybridControl.BoxShadowOnEdges = false;
            skHybridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            skHybridControl.Location = new System.Drawing.Point(0, 0);
            skHybridControl.Name = "skHybridControl";
            skHybridControl.Size = new System.Drawing.Size(625, 396);
            skHybridControl.TabIndex = 0;
            skHybridControl.PaintSurface += skHybridControl_PaintSurface;
            // 
            // ImageButtonContainer
            // 
            Controls.Add(skHybridControl);
            Name = "ImageButtonContainer";
            Size = new System.Drawing.Size(625, 396);
            ResumeLayout(false);
        }

        #endregion

        private SkHybridControl skHybridControl;
    }
}
