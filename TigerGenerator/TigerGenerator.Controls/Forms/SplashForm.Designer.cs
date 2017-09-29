namespace TigerGenerator.Controls.Forms
{
    partial class SplashForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashForm));
            this.pbStatus = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            this.lEmail = new DevExpress.XtraEditors.LabelControl();
            this.lInfo = new DevExpress.XtraEditors.LabelControl();
            this.pTiger = new DevExpress.XtraEditors.PictureEdit();
            this.pDevExpress = new DevExpress.XtraEditors.PictureEdit();
            ((System.ComponentModel.ISupportInitialize)(this.pbStatus.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pTiger.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pDevExpress.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pbStatus
            // 
            this.pbStatus.EditValue = 0;
            this.pbStatus.Location = new System.Drawing.Point(23, 231);
            this.pbStatus.Name = "pbStatus";
            this.pbStatus.Size = new System.Drawing.Size(404, 12);
            this.pbStatus.TabIndex = 5;
            // 
            // lEmail
            // 
            this.lEmail.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.lEmail.Location = new System.Drawing.Point(23, 286);
            this.lEmail.Name = "lEmail";
            this.lEmail.Size = new System.Drawing.Size(143, 13);
            this.lEmail.TabIndex = 6;
            this.lEmail.Text = "bogdanbenetskyy@gmail.com";
            // 
            // lInfo
            // 
            this.lInfo.Location = new System.Drawing.Point(23, 206);
            this.lInfo.Name = "lInfo";
            this.lInfo.Size = new System.Drawing.Size(50, 13);
            this.lInfo.TabIndex = 7;
            this.lInfo.Text = "Starting...";
            // 
            // pTiger
            // 
            this.pTiger.Cursor = System.Windows.Forms.Cursors.Default;
            this.pTiger.EditValue = global::TigerGenerator.Controls.Properties.Resources.TigerLogo;
            this.pTiger.Location = new System.Drawing.Point(12, 12);
            this.pTiger.Name = "pTiger";
            this.pTiger.Properties.AllowFocused = false;
            this.pTiger.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.pTiger.Properties.Appearance.Options.UseBackColor = true;
            this.pTiger.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pTiger.Properties.ShowMenu = false;
            this.pTiger.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
            this.pTiger.Properties.ZoomAccelerationFactor = 1D;
            this.pTiger.Size = new System.Drawing.Size(426, 180);
            this.pTiger.TabIndex = 9;
            // 
            // pDevExpress
            // 
            this.pDevExpress.Cursor = System.Windows.Forms.Cursors.Default;
            this.pDevExpress.EditValue = ((object)(resources.GetObject("pDevExpress.EditValue")));
            this.pDevExpress.Location = new System.Drawing.Point(278, 266);
            this.pDevExpress.Name = "pDevExpress";
            this.pDevExpress.Properties.AllowFocused = false;
            this.pDevExpress.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.pDevExpress.Properties.Appearance.Options.UseBackColor = true;
            this.pDevExpress.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pDevExpress.Properties.ShowMenu = false;
            this.pDevExpress.Properties.ZoomAccelerationFactor = 1D;
            this.pDevExpress.Size = new System.Drawing.Size(160, 48);
            this.pDevExpress.TabIndex = 8;
            // 
            // SplashForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 320);
            this.Controls.Add(this.pTiger);
            this.Controls.Add(this.pDevExpress);
            this.Controls.Add(this.lInfo);
            this.Controls.Add(this.lEmail);
            this.Controls.Add(this.pbStatus);
            this.Name = "SplashForm";
            this.Text = "Form1";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pbStatus.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pTiger.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pDevExpress.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.MarqueeProgressBarControl pbStatus;
        private DevExpress.XtraEditors.LabelControl lEmail;
        private DevExpress.XtraEditors.LabelControl lInfo;
        private DevExpress.XtraEditors.PictureEdit pDevExpress;
        private DevExpress.XtraEditors.PictureEdit pTiger;
    }
}
