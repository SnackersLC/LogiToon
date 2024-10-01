namespace ToontownLGP
{
    partial class ToontownLGPWindow
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToontownLGPWindow));
            logitechgp_connection_status_lable = new Label();
            toontown_local_connection_status_lable = new Label();
            current_image_lable = new Label();
            last_data_label = new Label();
            logitechgp_connection_lable = new Label();
            toontown_local_connection_lable = new Label();
            tray_icon = new NotifyIcon(components);
            tray_icon_context_menu = new ContextMenuStrip(components);
            showWindowToolStripMenuItem = new ToolStripMenuItem();
            quitToolStripMenuItem = new ToolStripMenuItem();
            current_picture = new PictureBox();
            last_data_value_label = new Label();
            toontown_population_connection_status_lable = new Label();
            toontown_population_connection_lable = new Label();
            toontown_silly_connection_status_lable = new Label();
            toontown_silly_connection_lable = new Label();
            tray_icon_context_menu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)current_picture).BeginInit();
            SuspendLayout();
            // 
            // logitechgp_connection_status_lable
            // 
            logitechgp_connection_status_lable.AutoSize = true;
            logitechgp_connection_status_lable.Location = new Point(429, 107);
            logitechgp_connection_status_lable.Name = "logitechgp_connection_status_lable";
            logitechgp_connection_status_lable.Size = new Size(173, 32);
            logitechgp_connection_status_lable.TabIndex = 13;
            logitechgp_connection_status_lable.Text = "Not connected";
            // 
            // toontown_local_connection_status_lable
            // 
            toontown_local_connection_status_lable.AutoSize = true;
            toontown_local_connection_status_lable.Location = new Point(501, 11);
            toontown_local_connection_status_lable.Name = "toontown_local_connection_status_lable";
            toontown_local_connection_status_lable.Size = new Size(173, 32);
            toontown_local_connection_status_lable.TabIndex = 12;
            toontown_local_connection_status_lable.Text = "Not connected";
            // 
            // current_image_lable
            // 
            current_image_lable.AutoSize = true;
            current_image_lable.Location = new Point(12, 171);
            current_image_lable.Name = "current_image_lable";
            current_image_lable.Size = new Size(263, 32);
            current_image_lable.TabIndex = 10;
            current_image_lable.Text = "Current image to show:";
            // 
            // last_data_label
            // 
            last_data_label.AutoSize = true;
            last_data_label.Location = new Point(12, 139);
            last_data_label.Name = "last_data_label";
            last_data_label.Size = new Size(210, 32);
            last_data_label.TabIndex = 9;
            last_data_label.Text = "Last received data:";
            // 
            // logitechgp_connection_lable
            // 
            logitechgp_connection_lable.AutoSize = true;
            logitechgp_connection_lable.Location = new Point(12, 107);
            logitechgp_connection_lable.Name = "logitechgp_connection_lable";
            logitechgp_connection_lable.Size = new Size(411, 32);
            logitechgp_connection_lable.TabIndex = 8;
            logitechgp_connection_lable.Text = "Logitech keyboard connection status:";
            logitechgp_connection_lable.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // toontown_local_connection_lable
            // 
            toontown_local_connection_lable.AutoSize = true;
            toontown_local_connection_lable.Font = new Font("Segoe UI", 9F);
            toontown_local_connection_lable.Location = new Point(12, 11);
            toontown_local_connection_lable.Name = "toontown_local_connection_lable";
            toontown_local_connection_lable.Size = new Size(483, 32);
            toontown_local_connection_lable.TabIndex = 7;
            toontown_local_connection_lable.Text = "Toontown Rewritten local connection status:";
            toontown_local_connection_lable.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tray_icon
            // 
            tray_icon.BalloonTipText = "Toontown LGP";
            tray_icon.ContextMenuStrip = tray_icon_context_menu;
            tray_icon.Icon = (Icon)resources.GetObject("tray_icon.Icon");
            tray_icon.Text = "Toontown LGP Connected";
            tray_icon.Visible = true;
            tray_icon.DoubleClick += tray_icon_DoubleClick;
            // 
            // tray_icon_context_menu
            // 
            tray_icon_context_menu.ImageScalingSize = new Size(32, 32);
            tray_icon_context_menu.Items.AddRange(new ToolStripItem[] { showWindowToolStripMenuItem, quitToolStripMenuItem });
            tray_icon_context_menu.Name = "contextMenuStrip1";
            tray_icon_context_menu.Size = new Size(236, 80);
            // 
            // showWindowToolStripMenuItem
            // 
            showWindowToolStripMenuItem.Name = "showWindowToolStripMenuItem";
            showWindowToolStripMenuItem.Size = new Size(235, 38);
            showWindowToolStripMenuItem.Text = "Show window";
            showWindowToolStripMenuItem.Click += tray_icon_DoubleClick;
            // 
            // quitToolStripMenuItem
            // 
            quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            quitToolStripMenuItem.Size = new Size(235, 38);
            quitToolStripMenuItem.Text = "Quit";
            quitToolStripMenuItem.Click += quitToolStripMenuItem_Click;
            // 
            // current_picture
            // 
            current_picture.BackgroundImageLayout = ImageLayout.Zoom;
            current_picture.Location = new Point(12, 206);
            current_picture.Name = "current_picture";
            current_picture.Size = new Size(1600, 430);
            current_picture.TabIndex = 14;
            current_picture.TabStop = false;
            // 
            // last_data_value_label
            // 
            last_data_value_label.AutoSize = true;
            last_data_value_label.Location = new Point(228, 139);
            last_data_value_label.Name = "last_data_value_label";
            last_data_value_label.Size = new Size(115, 32);
            last_data_value_label.TabIndex = 15;
            last_data_value_label.Text = "Unknown";
            // 
            // toontown_population_connection_status_lable
            // 
            toontown_population_connection_status_lable.AutoSize = true;
            toontown_population_connection_status_lable.Location = new Point(568, 43);
            toontown_population_connection_status_lable.Name = "toontown_population_connection_status_lable";
            toontown_population_connection_status_lable.Size = new Size(173, 32);
            toontown_population_connection_status_lable.TabIndex = 17;
            toontown_population_connection_status_lable.Text = "Not connected";
            // 
            // toontown_population_connection_lable
            // 
            toontown_population_connection_lable.AutoSize = true;
            toontown_population_connection_lable.Font = new Font("Segoe UI", 9F);
            toontown_population_connection_lable.Location = new Point(12, 43);
            toontown_population_connection_lable.Name = "toontown_population_connection_lable";
            toontown_population_connection_lable.Size = new Size(550, 32);
            toontown_population_connection_lable.TabIndex = 16;
            toontown_population_connection_lable.Text = "Toontown Rewritten population connection status:";
            toontown_population_connection_lable.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // toontown_silly_connection_status_lable
            // 
            toontown_silly_connection_status_lable.AutoSize = true;
            toontown_silly_connection_status_lable.Location = new Point(562, 75);
            toontown_silly_connection_status_lable.Name = "toontown_silly_connection_status_lable";
            toontown_silly_connection_status_lable.Size = new Size(173, 32);
            toontown_silly_connection_status_lable.TabIndex = 19;
            toontown_silly_connection_status_lable.Text = "Not connected";
            // 
            // toontown_silly_connection_lable
            // 
            toontown_silly_connection_lable.AutoSize = true;
            toontown_silly_connection_lable.Font = new Font("Segoe UI", 9F);
            toontown_silly_connection_lable.Location = new Point(12, 75);
            toontown_silly_connection_lable.Name = "toontown_silly_connection_lable";
            toontown_silly_connection_lable.Size = new Size(544, 32);
            toontown_silly_connection_lable.TabIndex = 18;
            toontown_silly_connection_lable.Text = "Toontown Rewritten silly meter connection status:";
            toontown_silly_connection_lable.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // ToontownLGPWindow
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1629, 642);
            Controls.Add(toontown_silly_connection_status_lable);
            Controls.Add(toontown_silly_connection_lable);
            Controls.Add(toontown_population_connection_status_lable);
            Controls.Add(toontown_population_connection_lable);
            Controls.Add(last_data_value_label);
            Controls.Add(current_picture);
            Controls.Add(logitechgp_connection_status_lable);
            Controls.Add(toontown_local_connection_status_lable);
            Controls.Add(current_image_lable);
            Controls.Add(last_data_label);
            Controls.Add(logitechgp_connection_lable);
            Controls.Add(toontown_local_connection_lable);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ToontownLGPWindow";
            Text = "LogiToon";
            FormClosing += ToontownLGPWindow_FormClosing;
            tray_icon_context_menu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)current_picture).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label logitechgp_connection_status_lable;
        private Label toontown_local_connection_status_lable;
        private Label current_image_lable;
        private Label last_data_label;
        private Label logitechgp_connection_lable;
        private Label toontown_local_connection_lable;
        private NotifyIcon tray_icon;
        private ContextMenuStrip tray_icon_context_menu;
        private ToolStripMenuItem showWindowToolStripMenuItem;
        private ToolStripMenuItem quitToolStripMenuItem;
        private PictureBox current_picture;
        private Label last_data_value_label;
        private Label toontown_population_connection_status_lable;
        private Label toontown_population_connection_lable;
        private Label toontown_silly_connection_status_lable;
        private Label toontown_silly_connection_lable;
    }
}