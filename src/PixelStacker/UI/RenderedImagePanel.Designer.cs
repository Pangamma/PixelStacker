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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RenderedImagePanel));
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.replaceColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceMenuItems_1 = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceMenuItems_2 = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceMenuItems_3 = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceMenuItems_4 = new System.Windows.Forms.ToolStripMenuItem();
            this.filterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAddMat0_Filter = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAddMat1_Filter = new System.Windows.Forms.ToolStripMenuItem();
            this.addAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRemoveMat0_Filter = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRemoveMat1_Filter = new System.Windows.Forms.ToolStripMenuItem();
            this.ts_xyz = new System.Windows.Forms.ToolStripMenuItem();
            this.ts_MaterialName = new System.Windows.Forms.ToolStripMenuItem();
            this.averageColorCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rGBAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            resources.ApplyResources(this.contextMenu, "contextMenu");
            // 
            // replaceColorToolStripMenuItem
            // 
            this.replaceColorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.replaceMenuItems_1,
            this.replaceMenuItems_2,
            this.replaceMenuItems_3,
            this.replaceMenuItems_4});
            resources.ApplyResources(this.replaceColorToolStripMenuItem, "replaceColorToolStripMenuItem");
            this.replaceColorToolStripMenuItem.Name = "replaceColorToolStripMenuItem";
            this.replaceColorToolStripMenuItem.Overflow = System.Windows.Forms.ToolStripItemOverflow.AsNeeded;
            // 
            // replaceMenuItems_1
            // 
            this.replaceMenuItems_1.Name = "replaceMenuItems_1";
            resources.ApplyResources(this.replaceMenuItems_1, "replaceMenuItems_1");
            // 
            // replaceMenuItems_2
            // 
            this.replaceMenuItems_2.Name = "replaceMenuItems_2";
            resources.ApplyResources(this.replaceMenuItems_2, "replaceMenuItems_2");
            // 
            // replaceMenuItems_3
            // 
            this.replaceMenuItems_3.Name = "replaceMenuItems_3";
            resources.ApplyResources(this.replaceMenuItems_3, "replaceMenuItems_3");
            // 
            // replaceMenuItems_4
            // 
            this.replaceMenuItems_4.Name = "replaceMenuItems_4";
            resources.ApplyResources(this.replaceMenuItems_4, "replaceMenuItems_4");
            // 
            // filterToolStripMenuItem
            // 
            this.filterToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddMat0_Filter,
            this.btnAddMat1_Filter,
            this.addAllToolStripMenuItem,
            this.removeAllToolStripMenuItem,
            this.btnRemoveMat0_Filter,
            this.btnRemoveMat1_Filter});
            this.filterToolStripMenuItem.Name = "filterToolStripMenuItem";
            resources.ApplyResources(this.filterToolStripMenuItem, "filterToolStripMenuItem");
            // 
            // btnAddMat0_Filter
            // 
            this.btnAddMat0_Filter.Name = "btnAddMat0_Filter";
            resources.ApplyResources(this.btnAddMat0_Filter, "btnAddMat0_Filter");
            this.btnAddMat0_Filter.Click += new System.EventHandler(this.RenderedImagePanel_MaterialFilter_Add_Click);
            // 
            // btnAddMat1_Filter
            // 
            this.btnAddMat1_Filter.Name = "btnAddMat1_Filter";
            resources.ApplyResources(this.btnAddMat1_Filter, "btnAddMat1_Filter");
            this.btnAddMat1_Filter.Click += new System.EventHandler(this.RenderedImagePanel_MaterialFilter_Add_Click);
            // 
            // addAllToolStripMenuItem
            // 
            this.addAllToolStripMenuItem.Name = "addAllToolStripMenuItem";
            resources.ApplyResources(this.addAllToolStripMenuItem, "addAllToolStripMenuItem");
            this.addAllToolStripMenuItem.Click += new System.EventHandler(this.addAllToolStripMenuItem_Click);
            // 
            // removeAllToolStripMenuItem
            // 
            this.removeAllToolStripMenuItem.Name = "removeAllToolStripMenuItem";
            resources.ApplyResources(this.removeAllToolStripMenuItem, "removeAllToolStripMenuItem");
            this.removeAllToolStripMenuItem.Click += new System.EventHandler(this.clearMaterialFilterToolStripMenuItem_Click);
            // 
            // btnRemoveMat0_Filter
            // 
            this.btnRemoveMat0_Filter.Name = "btnRemoveMat0_Filter";
            resources.ApplyResources(this.btnRemoveMat0_Filter, "btnRemoveMat0_Filter");
            this.btnRemoveMat0_Filter.Click += new System.EventHandler(this.RenderedImagePanel_MaterialFilter_Remove_Click);
            // 
            // btnRemoveMat1_Filter
            // 
            this.btnRemoveMat1_Filter.Name = "btnRemoveMat1_Filter";
            resources.ApplyResources(this.btnRemoveMat1_Filter, "btnRemoveMat1_Filter");
            this.btnRemoveMat1_Filter.Click += new System.EventHandler(this.RenderedImagePanel_MaterialFilter_Remove_Click);
            // 
            // ts_xyz
            // 
            resources.ApplyResources(this.ts_xyz, "ts_xyz");
            this.ts_xyz.Name = "ts_xyz";
            // 
            // ts_MaterialName
            // 
            resources.ApplyResources(this.ts_MaterialName, "ts_MaterialName");
            this.ts_MaterialName.Name = "ts_MaterialName";
            // 
            // averageColorCodeToolStripMenuItem
            // 
            resources.ApplyResources(this.averageColorCodeToolStripMenuItem, "averageColorCodeToolStripMenuItem");
            this.averageColorCodeToolStripMenuItem.Name = "averageColorCodeToolStripMenuItem";
            // 
            // rGBAToolStripMenuItem
            // 
            resources.ApplyResources(this.rGBAToolStripMenuItem, "rGBAToolStripMenuItem");
            this.rGBAToolStripMenuItem.Name = "rGBAToolStripMenuItem";
            // 
            // RenderedImagePanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::PixelStacker.Resources.UIResources.bg_imagepanel;
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.DoubleBuffered = true;
            this.Name = "RenderedImagePanel";
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
        private System.Windows.Forms.ToolStripMenuItem btnAddMat0_Filter;
        private System.Windows.Forms.ToolStripMenuItem addAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem btnAddMat1_Filter;
        private System.Windows.Forms.ToolStripMenuItem replaceMenuItems_4;
        private System.Windows.Forms.ToolStripMenuItem btnRemoveMat0_Filter;
        private System.Windows.Forms.ToolStripMenuItem btnRemoveMat1_Filter;
    }
}
