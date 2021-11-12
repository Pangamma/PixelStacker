namespace PixelStacker.UI.Forms
{
    partial class Credits
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
            this.lblIntroduction = new System.Windows.Forms.Label();
            this.customPanel1 = new PixelStacker.UI.Controls.CustomPanel();
            this.tableCredits = new PixelStacker.UI.Controls.CustomTableLayoutPanel();
            this.customPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblIntroduction
            // 
            this.lblIntroduction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblIntroduction.Location = new System.Drawing.Point(12, 9);
            this.lblIntroduction.Name = "lblIntroduction";
            this.lblIntroduction.Size = new System.Drawing.Size(746, 44);
            this.lblIntroduction.TabIndex = 1;
            this.lblIntroduction.Text = "This program is made possible thanks to the helpful efforts of the contributors b" +
    "elow. ";
            // 
            // customPanel1
            // 
            this.customPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.customPanel1.AutoScroll = true;
            this.customPanel1.Controls.Add(this.tableCredits);
            this.customPanel1.Location = new System.Drawing.Point(12, 56);
            this.customPanel1.Name = "customPanel1";
            this.customPanel1.OnCommandKey = null;
            this.customPanel1.Size = new System.Drawing.Size(746, 382);
            this.customPanel1.TabIndex = 2;
            // 
            // tableCredits
            // 
            this.tableCredits.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableCredits.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableCredits.ColumnCount = 2;
            this.tableCredits.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableCredits.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableCredits.Location = new System.Drawing.Point(3, 3);
            this.tableCredits.Name = "tableCredits";
            this.tableCredits.OnCommandKey = null;
            this.tableCredits.RowCount = 2;
            this.tableCredits.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableCredits.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableCredits.Size = new System.Drawing.Size(743, 376);
            this.tableCredits.TabIndex = 0;
            // 
            // Credits
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 450);
            this.Controls.Add(this.customPanel1);
            this.Controls.Add(this.lblIntroduction);
            this.Name = "Credits";
            this.Text = "Credits";
            this.Load += new System.EventHandler(this.Credits_Load);
            this.customPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblIntroduction;
        private Controls.CustomPanel customPanel1;
        private Controls.CustomTableLayoutPanel tableCredits;
    }
}