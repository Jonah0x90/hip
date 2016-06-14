using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using HybridDSP.Net.HTTP;

namespace ZLSoft.Soa.ServiceBus
{
    public partial class ControlForm : Form
    {
        private HTTPServer _server = null;
        private RequestHandlerFactory _factory = null;

        public ControlForm()
        {
            InitializeComponent();

            _factory = new RequestHandlerFactory(new RequestHandlerFactory.WriteLogDelegate(AppendLogLine));
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (_server == null || !_server.IsRunning)
            {
                int port = int.Parse(tbPort.Text);
                CreateServer(port);

                if (rbDateTime.Checked)
                    _factory.HandlerType = RequestHandlerFactory.RequestHandlerType.DateTime;
                else if (rbGUID.Checked)
                    _factory.HandlerType = RequestHandlerFactory.RequestHandlerType.GUID;

                tbPort.Enabled = false;
                btnStartStop.Text = "Stop";
                rbDateTime.Enabled = false;
                rbGUID.Enabled = false;

                _server.Start();
            }
            else
            {
                _server.Stop();

                tbPort.Enabled = true;
                btnStartStop.Text = "Start";
                rbDateTime.Enabled = true;
                rbGUID.Enabled = true;
            }
        }

        private void CreateServer(int port)
        {
            Debug.Assert(_server == null || !_server.IsRunning);

            _server = new HTTPServer(_factory, port);
            _server.OnServerStart += new HTTPServer.ServerStarted(_server_OnServerStart);
            _server.OnServerStop += new HTTPServer.ServerStopped(_server_OnServerStop);
            _server.OnServerException += new HTTPServer.ServerCaughtException(_server_OnServerException);
        }

        private delegate void AppendLogLineInvoker(string str);
        private void AppendLogLine(string str)
        {
            if (textBox1.InvokeRequired)
                BeginInvoke(new AppendLogLineInvoker(AppendLogLine), new object[] { str });
            else
            {
                using (TextWriter tw = new StringWriter())
                {
                    tw.Write(DateTime.Now.ToString() + ": ");
                    tw.WriteLine(str);
                    textBox1.AppendText(tw.ToString());
                }
            }
        }

        void _server_OnServerStart()
        {
            AppendLogLine("Server started");
        }

        void _server_OnServerStop()
        {
            AppendLogLine("Server stopped");
        }

        void _server_OnServerException(Exception ex)
        {
            AppendLogLine(string.Format("Server caught exception: \"{0}\"", ex.Message));
        }
    }
}