using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToontownLGP
{
    public partial class ToontownLGPWindow : Form
    {
        public enum CONNECTION_STATUS
        {
            NOT_CONNECTED,
            CONNECTING,
            CONNECTION_ISSUE,
            CONNECTED
        }

        private bool _force_close = false;
        private bool _hide_instead_close = false;

        public ToontownLGPWindow()
        {
            InitializeComponent();
        }

        public void SetToontownLocalConnectionStatus(CONNECTION_STATUS status)
        {
            InternalCall((MethodInvoker)delegate
            {
                toontown_local_connection_status_lable.Text = ConvertStatus(status);
            });
        }

        public void SetToontownPopulationConnectionStatus(CONNECTION_STATUS status)
        {
            InternalCall((MethodInvoker)delegate
            {
                toontown_population_connection_status_lable.Text = ConvertStatus(status);
            });
        }

        public void SetToontownSillyConnectionStatus(CONNECTION_STATUS status)
        {
            InternalCall((MethodInvoker)delegate
            {
                toontown_silly_connection_status_lable.Text = ConvertStatus(status);
            });
        }

        public void SetLogitechGPConnectionStatus(CONNECTION_STATUS status)
        {
            InternalCall((MethodInvoker)delegate
            {
                logitechgp_connection_status_lable.Text = ConvertStatus(status);
            });
        }
        public void SetLastReceivedData(String new_data)
        {
            InternalCall((MethodInvoker)delegate
            {
                last_data_value_label.Text = new_data;
            });
        }

        public void SetCurrentPicture(Bitmap new_current_picture)
        {
            InternalCall((MethodInvoker)delegate
            {
                current_picture.BackgroundImage = new_current_picture;
            });
        }

        public void SetHideInsteadClose(bool hide_allowed)
        {
            _hide_instead_close = hide_allowed;
        }

        public void HideWindow()
        {
            tray_icon.Visible = true;
            InternalCall((MethodInvoker)delegate
            {
                this.Hide();
            });
        }

        private String ConvertStatus(CONNECTION_STATUS status)
        {
            switch (status)
            {
                case CONNECTION_STATUS.CONNECTED:
                    return Resources.connection_status_connected;
                case CONNECTION_STATUS.CONNECTING:
                    return Resources.connection_status_connecting;
                case CONNECTION_STATUS.NOT_CONNECTED:
                    return Resources.connection_status_not_connected;
                case CONNECTION_STATUS.CONNECTION_ISSUE:
                    return Resources.connection_status_connection_issue;
            }
            return "";
        }

        private void InternalCall(Delegate method)
        {
            try
            {
                this.Invoke(method);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[WARNING] Window update error: " + ex.ToString());
            }
        }

        private void ToontownLGPWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_force_close) return;

            if (_hide_instead_close)
            {
                e.Cancel = true;
                tray_icon.Visible = true;
                this.Hide();
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _force_close = true;
            this.Close();
        }

        private void tray_icon_DoubleClick(object sender, EventArgs e)
        {
            tray_icon.Visible = false;
            if (!this.Visible)
            {
                this.Show();
            }
        }
    }
}
