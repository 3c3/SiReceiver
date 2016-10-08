using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace SiReceiver
{
    public partial class Form1 : Form
    {
        private SerialManager serialMan = new SerialManager();

        public Form1()
        {
            InitializeComponent();

            serialMan.PayloadReceived += SerialMan_PayloadReceived;
            serialMan.StatusMessageReceived += SerialMan_StatusMessageReceived;
            serialMan.UnrecognizedPayloadReceived += SerialMan_UnrecognizedPayloadReceived;
        }

        private void SerialMan_UnrecognizedPayloadReceived(byte[] payload)
        {
            Console.Write("Unrecognized payload: ");
            foreach (byte b in payload) Console.Write("{0} ", b);
            Console.WriteLine();
        }

        private void SerialMan_StatusMessageReceived(StatusMessage msg)
        {
            Console.WriteLine("Status: {0}", msg);
        }

        private void SerialMan_PayloadReceived(TxRxPayload payload)
        {
            Console.WriteLine(payload);
        }

        private void portCb_DropDown(object sender, EventArgs e)
        {
            portCb.Items.Clear();
            foreach (string portName in SerialPort.GetPortNames()) portCb.Items.Add(portName);
        }

        private void openCloseBtn_Click(object sender, EventArgs e)
        {
            if(serialMan.IsOpen)
            {
                if (serialMan.ClosePort()) openCloseBtn.Text = "Open";
            }
            else
            {
                if (serialMan.OpenPort(portCb.Text)) openCloseBtn.Text = "Close";
            }
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            serialMan.Start();
        }
    }
}
