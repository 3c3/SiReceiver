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
            int read = port.Read(serialBuffer, buffIdx, port.BytesToRead);
            buffIdx += read;
            CheckBuffer();
        }

        private void CheckBuffer()
        {
            int i = 0;
            int dataLen;
            for(; i < buffIdx - 2; i++)
            {
                dataLen = GetDataLen(i);
                if(dataLen != -1)
                {
                    if (dataLen + 2 + i >= buffIdx) return; // още данни трябват
                    ProcessData(i + 3, dataLen - 1);
                    i += dataLen + 3;
                }
            }

            int j = 0;
            for (; i < buffIdx; i++) serialBuffer[j++] = serialBuffer[i];
            buffIdx = j;
        }

        private int GetDataLen(int idx)
        {
            if (serialBuffer[idx] != 0xDE) return -1;
            if (serialBuffer[idx + 1] != 0xAD) return -1;
            return serialBuffer[idx + 2];
        }

        private void ProcessData(int dataStart, int dataLen)
        {
            byte msgType = serialBuffer[dataStart];

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
                    byte[] buffer = new byte[dataLen];

                    Buffer.BlockCopy(serialBuffer, dataStart + 1, buffer, 0, dataLen);
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
