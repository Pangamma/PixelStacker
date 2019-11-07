namespace PixelStacker.UI
{
    partial class RenderedImagePanel
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
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.replaceColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ts_xyz = new System.Windows.Forms.ToolStripMenuItem();
            this.ts_MaterialName = new System.Windows.Forms.ToolStripMenuItem();
            this.averageColorCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rGBAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.replaceColorToolStripMenuItem,
            this.ts_xyz,
            this.ts_MaterialName,
            this.averageColorCodeToolStripMenuItem,
            this.rGBAToolStripMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(262, 136);
            // 
            // replaceColorToolStripMenuItem
            // 
            this.replaceColorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.replaceColorToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.replaceColorToolStripMenuItem.Name = "replaceColorToolStripMenuItem";
            this.replaceColorToolStripMenuItem.Overflow = System.Windows.Forms.ToolStripItemOverflow.AsNeeded;
            this.replaceColorToolStripMenuItem.Size = new System.Drawing.Size(261, 22);
            this.replaceColorToolStripMenuItem.Text = "Replace Color (Requires Re-Render)";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Image = global::PixelStacker.Properties.Resources.avatar;
            this.toolStripMenuItem1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(289, 132);
            this.toolStripMenuItem1.Text = "toolStripMenuItem1";
            // 
            // ts_xyz
            // 
            this.ts_xyz.Enabled = false;
            this.ts_xyz.Name = "ts_xyz";
            this.ts_xyz.Size = new System.Drawing.Size(261, 22);
            this.ts_xyz.Text = "X: ";
            // 
            // ts_MaterialName
            // 
            this.ts_MaterialName.Enabled = false;
            this.ts_MaterialName.Name = "ts_MaterialName";
            this.ts_MaterialName.Size = new System.Drawing.Size(261, 22);
            this.ts_MaterialName.Text = "Y:";
            // 
            // averageColorCodeToolStripMenuItem
            // 
            this.averageColorCodeToolStripMenuItem.Enabled = false;
            this.averageColorCodeToolStripMenuItem.Name = "averageColorCodeToolStripMenuItem";
            this.averageColorCodeToolStripMenuItem.Size = new System.Drawing.Size(261, 22);
            this.averageColorCodeToolStripMenuItem.Text = "Average ColorCode";
            // 
            // rGBAToolStripMenuItem
            // 
            this.rGBAToolStripMenuItem.Enabled = false;
            this.rGBAToolStripMenuItem.Name = "rGBAToolStripMenuItem";
            this.rGBAToolStripMenuItem.Size = new System.Drawing.Size(261, 22);
            this.rGBAToolStripMenuItem.Text = "RGBA";
            // 
            // RenderedImagePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::PixelStacker.Properties.Resources.bg_imagepanel;
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.DoubleBuffered = true;
            this.Name = "RenderedImagePanel";
            this.Size = new System.Drawing.Size(509, 297);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RenderedImagePanel_MouseClick);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.RenderedImagePanel_DoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ImagePanel_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ImagePanel_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ImagePanel_MouseUp);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem ts_xyz;
        private System.Windows.Forms.ToolStripMenuItem ts_MaterialName;
        private System.Windows.Forms.ToolStripMenuItem averageColorCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rGBAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceColorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    }
}
