using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace SiReceiver
{
    public enum StatusMessage { InitOk, InitFailed, CrcError, GetFifoError, GetIntError };

    public delegate void StatusMessageReceivedHandler(StatusMessage msg);
    public delegate void PayloadReceivedHandler(TxRxPayload payload);
    public delegate void UnrecognizedPayloadHandler(byte[] payload);

    public class SerialManager
    {
        public event StatusMessageReceivedHandler StatusMessageReceived;
        public event PayloadReceivedHandler PayloadReceived;
        public event UnrecognizedPayloadHandler UnrecognizedPayloadReceived;

        public bool IsOpen
        {
            get { return portOpen; }
        }

        private SerialPort port;
        private bool portOpen;

        private byte[] serialBuffer = new byte[16384];
        private int buffIdx;

        private byte[] payloadBuff = new byte[128];
        private int payloadIdx;
        private byte payloadLen;

        public bool OpenPort(string portName)
        {
            try
            {
                port = new SerialPort(portName, 38400, Parity.None, 8, StopBits.One);
                port.Open();

                port.DataReceived += Port_DataReceived;
                portOpen = true;
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public bool ClosePort()
        {
            if (port == null) return false;
            port.Close();
            portOpen = false;
            return true;
        }

        public void Start()
        {
            byte[] buff = new byte[1];
            buff[0] = 0xFF;
            port?.Write(buff, 0, 1);
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int read = port.Read(serialBuffer, 0, port.BytesToRead);
            for (int i = 0; i < read; i++) AddByte(serialBuffer[i]);
        }

        private void AddByte(byte b)
        {
            if(payloadIdx == 0)
            {
                if (b != 0xDE)
                {
                    payloadIdx = 0;
                    return;
                }
                else payloadBuff[payloadIdx++] = b;
            }
            else if(payloadIdx == 1)
            {
                if (b != 0xAD)
                {
                    payloadIdx = 0;
                    return;
                }
                else payloadBuff[payloadIdx++] = b;
            }
            else if(payloadIdx == 2)
            {
                payloadLen = (byte)(b - 1);
                if(payloadLen > 64)
                {
                    payloadIdx = 0;
                    return;
                }
                payloadBuff[payloadIdx++] = payloadLen;
            }
            else
            {
                payloadBuff[payloadIdx++] = b;
                if(payloadIdx - 4 == payloadLen)
                {
                    ProcessData();
                    payloadIdx = 0;
                }
            }
        }

        
        private void ProcessData()
        {
            byte msgType = payloadBuff[3];

            switch(msgType)
            {
                case 0:
                    StatusMessageReceived?.Invoke(StatusMessage.InitFailed);
                    break;
                case 1:
                    StatusMessageReceived?.Invoke(StatusMessage.InitOk);
                    break;
                case 2:
                    StatusMessageReceived?.Invoke(StatusMessage.CrcError);
                    break;
                case 3:
                    byte[] buffer = new byte[payloadLen];

                    Buffer.BlockCopy(payloadBuff, 4, buffer, 0, payloadLen);
                    try
                    {
                        TxRxPayload trp = new TxRxPayload(buffer);
                        PayloadReceived?.Invoke(trp);
                    }
                    catch(Exception e)
                    {
                        UnrecognizedPayloadReceived?.Invoke(buffer);
                    }
                    break;
                case 4:
                    StatusMessageReceived?.Invoke(StatusMessage.GetFifoError);
                    break;
                case 5:
                    StatusMessageReceived?.Invoke(StatusMessage.GetIntError);
                    break;

            }
        }
    }
}
