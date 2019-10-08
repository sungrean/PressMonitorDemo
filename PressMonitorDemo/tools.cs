using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PressMonitorDemo
{
    class tools
    {
        public static byte[] ConvertBytes(string sourceStr)
        {
            string[] tmpSrt = sourceStr.Trim().Split(' ');

            byte[] destinationByte = new byte[tmpSrt.Count()];
            for (int i = 0; i < tmpSrt.Count(); i++)
            {
                destinationByte[i] = Convert.ToByte(Convert.ToInt32(tmpSrt[i], 16));
            }
            return destinationByte;
        }
        public static string ConvertString(byte[] sourceBytes)
        {
            string byteStr = string.Empty;
            for (int i = 0; i < sourceBytes.Length; i++)
            {
                byteStr += string.Format("{0:X2}", sourceBytes[i]) + " ";
            }
            return byteStr;
        } 
    }
}
