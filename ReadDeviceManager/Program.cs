using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HidLibrary;

namespace ReadDeviceManager
{
    class Program
    {

        static private HidDevice[] _deviceList;
        //static private HidDevice _selectedDevice;
        public delegate void ReadHandlerDelegate(HidReport report);
        private static List<string> devList = new List<string>();
        private static string searchDevice = "touch";

        static SerialPort _serialPort;
        private static string PortName = "";

        private static bool sPortState = false;
        static void Main(string[] args)
        {
            ReadConfiguration();
            ConnectSPort();
            //

            Thread workerThread = new Thread(new ThreadStart(Loop));
            workerThread.Start();
            Console.ReadLine();

        }


        private static void Loop()
        {
            Thread.Sleep(2000);
            while (true)
            {
                Console.Clear();
                RefreshDevices();
                var aa = devList.Find(x => x.Contains(searchDevice));
                //if(aa.Length< "")
                //Console.WriteLine(aa.Length);
                if(aa == null && !sPortState)
                {
                    _serialPort.WriteLine("close");
                    sPortState = true;
                    Console.WriteLine("unplug touchFrame");
                } 
                else if(aa == null && sPortState)
                {
                    _serialPort.WriteLine("open");
                    sPortState = false;
                    Console.WriteLine("plug touchFrame");
                }
                else
                {
                    Console.WriteLine("everythings ok ");
                }

                Thread.Sleep(2000);
            }
        }
        private static void RefreshDevices()
        {
            _deviceList = HidDevices.Enumerate().ToArray();
            //_deviceList = HidDevices.Enumerate(0x536, 0x207, 0x1c7).ToArray();
            //Devices.DisplayMember = "Description";
            //Devices.DataSource = _deviceList;
            devList.Clear();
            foreach (var item in _deviceList)
            {
                devList.Add(item.Description);
            }
            if (_deviceList.Length > 0) _selectedDevice = _deviceList[0];

            //foreach (var item in devList)
            //{
            //    Console.Write(item + "\n");
            //}

        }



        private static void ConnectSPort()
        {
            _serialPort = new SerialPort(PortName, 9600);
            try
            {
                _serialPort.Open();
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
            }

        }



        private static void ReadConfiguration()
        {
            string[] lines = File.ReadAllLines(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\config.txt"));
            string[] strArrays = lines;
            for (int i = 0; i < (int)strArrays.Length; i++)
            {
                string line = strArrays[i];
                string[] args = line.Split(new char[] { ':' });
                string str = args[0];
                switch (str)
                {
                    case "port":
                        {
                            PortName = args[1];
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }
    }
}
