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
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnSelectNone = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbxEnableLayer2
            // 
            this.cbxEnableLayer2.AutoSize = true;
            this.cbxEnableLayer2.Location = new System.Drawing.Point(16, 15);
            this.cbxEnableLayer2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbxEnableLayer2.Name = "cbxEnableLayer2";
            this.cbxEnableLayer2.Size = new System.Drawing.Size(159, 21);
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
            this.panel1.Location = new System.Drawing.Point(16, 46);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1215, 544);
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
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.Size = new System.Drawing.Size(1215, 544);
            this.tableLayoutPanel.TabIndex = 3;
            // 
            // cbxIsSideView
            // 
            this.cbxIsSideView.AutoSize = true;
            this.cbxIsSideView.Location = new System.Drawing.Point(215, 15);
            this.cbxIsSideView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbxIsSideView.Name = "cbxIsSideView";
            this.cbxIsSideView.Size = new System.Drawing.Size(91, 21);
            this.cbxIsSideView.TabIndex = 4;
            this.cbxIsSideView.Text = "Side View";
            this.cbxIsSideView.UseVisualStyleBackColor = true;
            this.cbxIsSideView.CheckedChanged += new System.EventHandler(this.cbxIsSideView_CheckedChanged);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(354, 10);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(97, 29);
            this.btnSelectAll.TabIndex = 5;
            this.btnSelectAll.Text = "Select all";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnSelectNone
            // 
            this.btnSelectNone.Location = new System.Drawing.Point(468, 10);
            this.btnSelectNone.Name = "btnSelectNone";
            this.btnSelectNone.Size = new System.Drawing.Size(97, 29);
            this.btnSelectNone.TabIndex = 6;
            this.btnSelectNone.Text = "Select none";
            this.btnSelectNone.UseVisualStyleBackColor = true;
            this.btnSelectNone.Click += new System.EventHandler(this.btnSelectNone_Click);
            // 
            // MaterialOptionsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1247, 604);
            this.Controls.Add(this.btnSelectNone);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.cbxIsSideView);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cbxEnableLayer2);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnSelectNone;
    }
}