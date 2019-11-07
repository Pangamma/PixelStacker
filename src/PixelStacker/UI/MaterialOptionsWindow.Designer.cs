namespace PixelStacker.UI
{
    partial class MaterialOptionsWindow
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MaterialOptionsWindow));
            this.cbxEnableLayer2 = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.cbxIsSideView = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbxEnableLayer2
            // 
            this.cbxEnableLayer2.AutoSize = true;
            this.cbxEnableLayer2.Location = new System.Drawing.Point(12, 12);
            this.cbxEnableLayer2.Name = "cbxEnableLayer2";
            this.cbxEnableLayer2.Size = new System.Drawing.Size(122, 17);
            this.cbxEnableLayer2.TabIndex = 1;
            this.cbxEnableLayer2.Text = "Enable second layer";
            this.cbxEnableLayer2.UseVisualStyleBackColor = true;
            this.cbxEnableLayer2.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.tableLayoutPanel);
            this.panel1.Location = new System.Drawing.Point(12, 69);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(911, 410);
            this.panel1.TabIndex = 3;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoScroll = true;
            this.tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.Size = new System.Drawing.Size(911, 410);
            this.tableLayoutPanel.TabIndex = 3;
            // 
            // cbxIsSideView
            // 
            this.cbxIsSideView.AutoSize = true;
            this.cbxIsSideView.Location = new System.Drawing.Point(161, 12);
            this.cbxIsSideView.Name = "cbxIsSideView";
            this.cbxIsSideView.Size = new System.Drawing.Size(73, 17);
            this.cbxIsSideView.TabIndex = 4;
            this.cbxIsSideView.Text = "Side View";
            this.cbxIsSideView.UseVisualStyleBackColor = true;
            this.cbxIsSideView.CheckedChanged += new System.EventHandler(this.cbxIsSideView_CheckedChanged);
            // 
            // MaterialOptionsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(935, 491);
            this.Controls.Add(this.cbxIsSideView);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cbxEnableLayer2);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MaterialOptionsWindow";
            this.Text = "Material Options";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PixelArtOptionsWindow_FormClosing);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox cbxEnableLayer2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.CheckBox cbxIsSideView;
    }
}