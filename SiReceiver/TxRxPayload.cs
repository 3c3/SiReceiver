using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiReceiver
{
    public class TxRxPayload
    {
        public byte rssi;
        public byte pwrLevel;
        public int longtitude;
        public int latitude;
        public ushort packetId;

        public TxRxPayload(byte[] payloadBuff)
        {
            if (payloadBuff.Length != 12) throw new ArgumentException("Buffer is not correct size");

            rssi = payloadBuff[0];
            pwrLevel = payloadBuff[1];
            packetId = (ushort)(payloadBuff[2] | (payloadBuff[3] << 8));
            longtitude = (payloadBuff[4]) | (payloadBuff[5] << 8) | (payloadBuff[6] << 16) | (payloadBuff[7] << 24);
            latitude = (payloadBuff[8]) | (payloadBuff[9] << 8) | (payloadBuff[10] << 16) | (payloadBuff[11] << 24);
            
        }

        public override string ToString()
        {
            return String.Format("Id: {0}, RSSI: {1}, Pwr: {2}, Lon: {3}, Lat: {4}", packetId, rssi, pwrLevel, longtitude, latitude);
        }
    }
}
