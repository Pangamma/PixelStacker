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
            this.replaceMenuItems_1 = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceMenuItems_2 = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceMenuItems_3 = new System.Windows.Forms.ToolStripMenuItem();
            this.filterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnToggleMaterialFilterMenuItem0 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnToggleMaterialFilterMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.addAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ts_xyz = new System.Windows.Forms.ToolStripMenuItem();
            this.ts_MaterialName = new System.Windows.Forms.ToolStripMenuItem();
            this.averageColorCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rGBAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceMenuItems_4 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenu
            // 
            this.contextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.replaceColorToolStripMenuItem,
            this.filterToolStripMenuItem,
            this.ts_xyz,
            this.ts_MaterialName,
            this.averageColorCodeToolStripMenuItem,
            this.rGBAToolStripMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(317, 176);
            // 
            // replaceColorToolStripMenuItem
            // 
            this.replaceColorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.replaceMenuItems_1,
            this.replaceMenuItems_2,
            this.replaceMenuItems_3,
            this.replaceMenuItems_4});
            this.replaceColorToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.replaceColorToolStripMenuItem.Name = "replaceColorToolStripMenuItem";
            this.replaceColorToolStripMenuItem.Overflow = System.Windows.Forms.ToolStripItemOverflow.AsNeeded;
            this.replaceColorToolStripMenuItem.Size = new System.Drawing.Size(316, 24);
            this.replaceColorToolStripMenuItem.Text = "Replace Color (Requires Re-Render)";
            // 
            // replaceMenuItems_1
            // 
            this.replaceMenuItems_1.Name = "replaceMenuItems_1";
            this.replaceMenuItems_1.Size = new System.Drawing.Size(224, 26);
            this.replaceMenuItems_1.Text = "1 - 10";
            // 
            // replaceMenuItems_2
            // 
            this.replaceMenuItems_2.Name = "replaceMenuItems_2";
            this.replaceMenuItems_2.Size = new System.Drawing.Size(224, 26);
            this.replaceMenuItems_2.Text = "11 - 20";
            // 
            // replaceMenuItems_3
            // 
            this.replaceMenuItems_3.Name = "replaceMenuItems_3";
            this.replaceMenuItems_3.Size = new System.Drawing.Size(224, 26);
            this.replaceMenuItems_3.Text = "21 - 30";
            // 
            // filterToolStripMenuItem
            // 
            this.filterToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnToggleMaterialFilterMenuItem0,
            this.btnToggleMaterialFilterMenuItem1,
            this.addAllToolStripMenuItem,
            this.removeAllToolStripMenuItem});
            this.filterToolStripMenuItem.Name = "filterToolStripMenuItem";
            this.filterToolStripMenuItem.Size = new System.Drawing.Size(316, 24);
            this.filterToolStripMenuItem.Text = "Material Filter";
            // 
            // btnToggleMaterialFilterMenuItem0
            // 
            this.btnToggleMaterialFilterMenuItem0.Name = "btnToggleMaterialFilterMenuItem0";
            this.btnToggleMaterialFilterMenuItem0.Size = new System.Drawing.Size(224, 26);
            this.btnToggleMaterialFilterMenuItem0.Text = "Add mat 1";
            this.btnToggleMaterialFilterMenuItem0.Click += new System.EventHandler(this.RenderedImagePanel_MaterialFilter_AddRemove_Click);
            // 
            // btnToggleMaterialFilterMenuItem1
            // 
            this.btnToggleMaterialFilterMenuItem1.Name = "btnToggleMaterialFilterMenuItem1";
            this.btnToggleMaterialFilterMenuItem1.Size = new System.Drawing.Size(224, 26);
            this.btnToggleMaterialFilterMenuItem1.Text = "Add mat 2";
            this.btnToggleMaterialFilterMenuItem1.Click += new System.EventHandler(this.RenderedImagePanel_MaterialFilter_AddRemove_Click);
            // 
            // addAllToolStripMenuItem
            // 
            this.addAllToolStripMenuItem.Name = "addAllToolStripMenuItem";
            this.addAllToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.addAllToolStripMenuItem.Text = "Add all";
            this.addAllToolStripMenuItem.Click += new System.EventHandler(this.addAllToolStripMenuItem_Click);
            // 
            // removeAllToolStripMenuItem
            // 
            this.removeAllToolStripMenuItem.Name = "removeAllToolStripMenuItem";
            this.removeAllToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.removeAllToolStripMenuItem.Text = "Remove all";
            this.removeAllToolStripMenuItem.Click += new System.EventHandler(this.clearMaterialFilterToolStripMenuItem_Click);
            // 
            // ts_xyz
            // 
            this.ts_xyz.Enabled = false;
            this.ts_xyz.Name = "ts_xyz";
            this.ts_xyz.Size = new System.Drawing.Size(316, 24);
            this.ts_xyz.Text = "X: ";
            // 
            // ts_MaterialName
            // 
            this.ts_MaterialName.Enabled = false;
            this.ts_MaterialName.Name = "ts_MaterialName";
            this.ts_MaterialName.Size = new System.Drawing.Size(316, 24);
            this.ts_MaterialName.Text = "Y:";
            // 
            // averageColorCodeToolStripMenuItem
            // 
            this.averageColorCodeToolStripMenuItem.Enabled = false;
            this.averageColorCodeToolStripMenuItem.Name = "averageColorCodeToolStripMenuItem";
            this.averageColorCodeToolStripMenuItem.Size = new System.Drawing.Size(316, 24);
            this.averageColorCodeToolStripMenuItem.Text = "Average ColorCode";
            // 
            // rGBAToolStripMenuItem
            // 
            this.rGBAToolStripMenuItem.Enabled = false;
            this.rGBAToolStripMenuItem.Name = "rGBAToolStripMenuItem";
            this.rGBAToolStripMenuItem.Size = new System.Drawing.Size(316, 24);
            this.rGBAToolStripMenuItem.Text = "RGBA";
            // 
            // replaceMenuItems_4
            // 
            this.replaceMenuItems_4.Name = "replaceMenuItems_4";
            this.replaceMenuItems_4.Size = new System.Drawing.Size(224, 26);
            this.replaceMenuItems_4.Text = "31 - 40";
            // 
            // RenderedImagePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::PixelStacker.Resources.UIResources.bg_imagepanel;
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "RenderedImagePanel";
            this.Size = new System.Drawing.Size(679, 366);
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
        private System.Windows.Forms.ToolStripMenuItem replaceMenuItems_1;
        private System.Windows.Forms.ToolStripMenuItem replaceMenuItems_2;
        private System.Windows.Forms.ToolStripMenuItem replaceMenuItems_3;
        private System.Windows.Forms.ToolStripMenuItem filterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem btnToggleMaterialFilterMenuItem0;
        private System.Windows.Forms.ToolStripMenuItem addAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem btnToggleMaterialFilterMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem replaceMenuItems_4;
    }
}
