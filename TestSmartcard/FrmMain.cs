using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;
using System.Windows.Automation;

namespace TestSmartcard
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            timer1.Start();


        }
        string serialCard = null;
        Reader_S70 readerObj = new Reader_S70();
        ushort baud = 19200;
       
        void ShowMessage(string msgStr, params object[] objs)
        {
            string msg = string.Format(msgStr, objs);
            labelState.Text = msg;
        }
        
        private String getPortCom(string[] comNos)
        {
            for (int i = 0; i < comNos.Length; i++)
            {
                ushort comNoConvert = Convert.ToUInt16(comNos[i].Substring(3));

                if (readerObj.OpenCom(comNoConvert, baud))
                {
                    ShowMessage("Kết nối máy đọc thành công!");
                    break;
                }
                else
                {
                    ShowMessage("Kết nối máy đọc thất bại!");
                }
            }
            return null;
        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
            string[] comNos = SerialPort.GetPortNames();
            getPortCom(comNos); /// kết nối
        }

        private void Read()
        {
            string serialCrad_temp = ReadCardSerialNo();
            int i = 0;
            while (i >= 0)
            {
                Console.WriteLine("Chạy", i);
                if(serialCard == null)
                {
                    serialCard = ReadCardSerialNo();
                    Console.WriteLine("th1");
                    i++;
                    ///// chuyển sang web
                }
                else if(serialCard != null && serialCard != serialCrad_temp)
                {
                    serialCard = ReadCardSerialNo();
                    Console.WriteLine("th2");
                    i++;

                    ///// chuyển sang web
                }
                else if (serialCard != null && serialCard == serialCrad_temp)
                {
                    Console.WriteLine("th3");
                    i++;
                    continue;
                }
            }
            Thread.Sleep(0);

        }

        private string ReadCardSerialNo()
        {
            byte[] cardSerial = readerObj.SelectCard();
            string csh = readerObj.GetStringByData(cardSerial);
            if (csh == null)
            {
                ShowMessage("Không đọc được số sê-ri thẻ");
                txtCardSerialNo.Text = csh;
                return null;
            }
            else
            {
                ShowMessage("Đọc thẻ thành công");
                txtCardSerialNo.Text = csh;
                return csh;
            }
        }

        private void btnReadCardSerialNo_Click(object sender, EventArgs e)
        {
            string serialCrad_temp = ReadCardSerialNo();
                if (serialCard == null)
                {
                Close_Browser();

                serialCard = ReadCardSerialNo();
                    Console.Write(serialCard);
                    Console.WriteLine("th1");
                    if(serialCard != null)
                {
                    ConVertTo_Web(serialCard);

                }
                ///// chuyển sang web
            }
                else if (serialCard != null && serialCard != serialCrad_temp)
                {
                Close_Browser();

                serialCard = ReadCardSerialNo();

                Console.WriteLine("th2");

                if (serialCard != null)
                {
                    ConVertTo_Web(serialCard);

                }

                ///// chuyển sang web
            }
                else if (serialCard != null && serialCard == serialCrad_temp)
                {
                    Console.WriteLine("th3");
                }
            else if (serialCard == null && serialCrad_temp == null)
            {
                Process[] AllProcesses = Process.GetProcesses();
                foreach (var process in AllProcesses)
                {
                    if (process.MainWindowTitle != "")
                    {
                        string s = process.ProcessName.ToLower();
                        if (s == "iexplore" || s == "iexplorer" || s == "chrome" || s == "firefox")
                            process.Kill();
                    }
                }
            }
        }

        private void ConVertTo_Web(String serialCard)
        {
            Process[] AllProcesses = Process.GetProcesses();
            foreach (var process in AllProcesses)
            {
                if (process.MainWindowTitle != "")
                {
                    string s = process.ProcessName.ToLower();
                    if (s == "iexplore" || s == "iexplorer" || s == "chrome" || s == "firefox")
                        process.Kill();
                }
            }

            Process processSt = new Process();
            processSt.StartInfo.FileName = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
            processSt.StartInfo.Arguments = "http://127.0.0.1:5500/index.html?smvl=" + serialCard + " --new-window --kiosk-printing";
            processSt.Start();
            //Process p = new Process();
            //ProcessStartInfo psi = new ProcessStartInfo();
            //psi.Arguments = "--kiosk-printing";
            //p.StartInfo = psi;
            //p.Start();
        }
        
        private void Close_Browser()
        {
            Process[] AllProcesses = Process.GetProcesses();
            foreach (var process in AllProcesses)
            {
                if (process.MainWindowTitle != "")
                {
                    string s = process.ProcessName.ToLower();
                    if (s == "iexplore" || s == "iexplorer" || s == "chrome" || s == "firefox")
                        process.Kill();
                }
            }
        }
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] cardSerial = readerObj.SelectCard();
            string csh = readerObj.GetStringByData(cardSerial);
            if (csh == null)
            {
                ShowMessage("读取卡序号失败");
            }
            else
            {
                ShowMessage("读取卡序号成功");
                txtCardSerialNo.Text = csh;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            btnReadCardSerialNo_Click(sender, e);
            button1_Click(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url1 = "127.0.0.1:5500/index.html";
            Process[] procsChrome = Process.GetProcessesByName("chrome");
            foreach (Process chrome in procsChrome)
            {
                // the chrome process must have a window
                if (chrome.MainWindowHandle == IntPtr.Zero)
                {
                    continue;
                }

                // find the automation element
                AutomationElement elm = AutomationElement.FromHandle(chrome.MainWindowHandle);
                AutomationElement elmUrlBar = elm.FindFirst(TreeScope.Descendants,
                  new PropertyCondition(AutomationElement.NameProperty, "Address and search bar"));

                // if it can be found, get the value from the URL bar
                if (elmUrlBar != null)
                {
                    AutomationPattern[] patterns = elmUrlBar.GetSupportedPatterns();
                    if (patterns.Length > 0)
                    {
                        ValuePattern val = (ValuePattern)elmUrlBar.GetCurrentPattern(patterns[0]);
                        Console.WriteLine("Chrome URL found: " + val.Current.Value);
                        if(val.Current.Value.Equals(url1))
                        {
                            chrome.Kill();
                        }
                    }
                }
            }
        }
    }
}
