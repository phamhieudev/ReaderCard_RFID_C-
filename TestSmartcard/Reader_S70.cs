using System;
using System.Collections.Generic;
using System.Text;

namespace TestSmartcard
{
    /// <summary>
    /// 读写类型枚举,0: 读0 - 31块; 1:读32-38块; 2:读2块
    /// </summary>
    public enum ReadWriteTypeModeEnum
    {
        Read0To31 = 0,
        Read32TO38 = 1,
        ReadAll = 2
    }
    /// <summary>
    /// 密钥选择 a 密钥,b密钥
    /// </summary>
    public enum KeyModeEnum
    {
        KeyA = 0x60,
        KeyB = 0x61
    }
    /// <summary>
    /// 使用密钥类型,密码组或原始密钥
    /// </summary>
    public enum UseKeyModeEnum
    {
        Key = 0,
        KeyGroup = 1
    }

    /// <summary>
    /// 读卡器打开指定Led颜色
    /// </summary>
    public enum LightColor
    {
        /// <summary>
        /// 关闭所有等
        /// </summary>
        CloseLED = 0x00,
        /// <summary>
        /// 红灯
        /// </summary>
        RedLED = 0x01,
        /// <summary>
        /// 红灯
        /// </summary>
        GreenLED = 0x02,
        /// <summary>
        /// 灯全亮
        /// </summary>
        AllLED = 0x03
    }

    public class Reader_S70
    {
        /// <summary>
        /// 根据16进制字符串转换为字节数组
        /// </summary>
        public byte[] GetWriteData(string dataStr)
        {
            if (dataStr.Length % 2 != 0)
            {
                throw new Exception("调用GetBlockData方法出错,dataStr长度不正确");
            }
            try
            {
                List<byte> rList = new List<byte>();
                for (int i = 0; i < dataStr.Length; i += 2)
                {
                    byte d = Convert.ToByte(dataStr.Substring(i, 2), 16);
                    rList.Add(d);
                }
                return rList.ToArray();
            }
            catch
            {
                throw new Exception("调用GetBlockData方法出错,将数据转换为dataStr失败,dataStr必须为16进制字符串");
            }

        }
        /// <summary>
        /// 获取密钥
        /// </summary>
        public byte[] GetKeyData(string keyStr)
        {
            byte[] r = GetWriteData(keyStr);
            if (r.Length != 6)
            {
                throw new Exception("密码长度不正确!");
            }
            return r;
        }
        /// <summary>
        /// 根据块字符串获得字节数字,长度不等于16引发一场
        /// </summary>
        public byte[] GetBlockData(string blockStr)
        {
            byte[] r = GetWriteData(blockStr);
            if (r.Length != 16)
            {
                throw new Exception("写入数据长度不正确");
            }
            return r;
        }
        /// <summary>
        /// 将byte[]数据转换为字符串
        /// </summary>
        public string GetStringByData(byte[] data)
        {
            if (data == null) return null;
            StringBuilder sbText = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sbText.Append(data[i].ToString("X2"));
            }
            return sbText.ToString();
        }
        /// <summary>
        /// 打开指定串口
        /// </summary>
        public bool OpenCom(ushort comNo, ushort baud)
        {
            try
            {
                int r = MasterRDImprot.rf_init_com(comNo, baud);
                return r == 0 ? true : false;
            }
            catch
            {
                throw new Exception("Không gọi được hàm!");
            }
        }
        /// <summary>
        /// 关闭已打开的串口
        /// </summary>
        public bool CloseCom()
        {
            try
            {
                int r = MasterRDImprot.rf_ClosePort();
                return r == 0 ? true : false;
            }
            catch
            {
                throw new Exception("调用 rf_ClosePort 函数失败!");
            }
        }
        /// <summary>
        /// 读取卡序列号
        /// </summary>
        public byte[] SelectCard()
        {
            byte[] pData = new byte[10];
            byte pdataLen = 0;
            int r = MasterRDImprot.rf_s70_select(0, pData, ref pdataLen);
            if (r == 0)
            {
                byte[] rData = new byte[pdataLen];
                for (int i = 0; i < pdataLen; i++)
                {
                    rData[i] = pData[i];
                }
                return rData;
            }
            return null;
        }
        /// <summary>
        /// 下载密钥到读卡器
        /// </summary>
        /// <returns></returns>
        public bool DownloadKeyToReader(byte blockNo, string keystr)
        {
            byte[] key = GetKeyData(keystr);
            int r = MasterRDImprot.rf_M1_WriteKeyToEE2(0, blockNo, key);
            if (r == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 核对读卡器中密钥组的密钥
        /// </summary>
        public bool CheckReaderKey(byte keyModel, byte blockNo, byte keyGroupIndex)
        {
            int r = MasterRDImprot.rf_M1_authentication1(0, keyModel, blockNo, keyGroupIndex);
            return r == 0 ? true : false;
        }
        /// <summary>
        /// 使用密钥读卡
        /// </summary>
        public byte[] ReadData(KeyModeEnum km, byte blokNo, string keyStr, out byte[] cardSerialData)
        {
            // byte readModel = (byte)rm;
            byte keyModel = (byte)km;
            byte[] key = GetKeyData(keyStr);
            byte[] pData = new byte[100];
            ulong pdataLen = 0;
            //读取卡序号
            cardSerialData = SelectCard();
            if (cardSerialData == null) return null;
            int r;
            //核对密钥
            r = MasterRDImprot.rf_M1_authentication2(0, keyModel, blokNo, key);

            r = MasterRDImprot.rf_M1_read(0, blokNo, pData, ref pdataLen);
            if (r == 0)
            {
                byte[] rData = new byte[pdataLen];
                for (int i = 0; i < (int)pdataLen; i++)
                {
                    rData[i] = pData[i];
                }
                return rData;
            }
            return null;
        }
        /// <summary>
        /// 使用密钥组读卡
        /// </summary>
        public byte[] ReadData(ReadWriteTypeModeEnum rm, KeyModeEnum km, byte blockNo, byte keyGroupIndex, out byte[] cardSerialData)
        {
            //byte readModel = (byte)rm;
            byte keyModel = (byte)km;
            cardSerialData = SelectCard();
            if (cardSerialData == null) return null;
            if (CheckReaderKey(keyModel, blockNo, keyGroupIndex))
            {
                byte[] pData = new byte[100];
                ulong pDataLen = 0;
                int r = MasterRDImprot.rf_M1_read(0, blockNo, pData, ref pDataLen);
                if (r == 0)
                {
                    byte[] resultData = new byte[pDataLen];
                    for (int i = 0; i < (int)pDataLen; i++)
                    {
                        resultData[i] = pData[i];
                    }
                    return resultData;
                }
            }
            return null;
        }
        /// <summary>
        /// 使用密钥写数据
        /// </summary>
        public bool WriteData(KeyModeEnum km, byte blockNo, string keyStr, string pDataStr)
        {
            //byte readModel = (byte)rm;
            byte keyModel = (byte)km;
            byte[] key = GetKeyData(keyStr);
            byte[] pData = GetBlockData(pDataStr);
            ulong pLen = (ulong)pData.Length;
            SelectCard();
            int r;
            //核对密钥
            r = MasterRDImprot.rf_M1_authentication2(0, keyModel, blockNo, key);

            r = MasterRDImprot.rf_M1_write(0, blockNo, pData);
            return r == 0 ? true : false;
        }
        /// <summary>
        /// 使用密钥组来写数据
        /// </summary>
        public bool WriteData(ReadWriteTypeModeEnum rm, KeyModeEnum km, byte blockNo, byte keyGroupIndex, string pDataStr)
        {
            byte readModel = (byte)rm;
            byte keyModel = (byte)km;
            SelectCard();
            if (CheckReaderKey(keyModel, blockNo, keyGroupIndex))
            {
                byte[] pData = GetWriteData(pDataStr);
                int r = MasterRDImprot.rf_M1_write(0, blockNo, pData);
                return r == 0 ? true : false;
            }
            return false;
        }


        public bool OpenLed(LightColor lc)
        {
            byte color = (byte)lc;
            int r = MasterRDImprot.rf_light(0, color);
            return r == 0 ? true : false;
        }
        public bool Beep(int msec)
        {
            int r = MasterRDImprot.rf_beep(0, msec);
            return r == 0 ? true : false;
        }
    }
}
