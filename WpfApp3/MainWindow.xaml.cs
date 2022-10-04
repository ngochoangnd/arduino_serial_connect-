using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using System.Windows.Controls.Primitives;

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        static public SerialPort SerialPort1 = new SerialPort();
        static public SerialPort SerialPort2 = new SerialPort();
        private readonly Encoding _stringEncoder = Encoding.GetEncoding("ISO-8859-1");


        // tim cac cong COM kha dung_________________________________________________________
        //____________________________________________________________________________________
        void getAvailablePorts()
        {
            com1.Items.Clear();
            com2.Items.Clear();
            String[] ports = SerialPort.GetPortNames();
            foreach (String port in ports) {
                com1.Items.Add(port);
                com2.Items.Add(port);
            }
        }
        // Nhan du lieu tu cong com________________________________________________________________
        //_________________________________________________________________________________________
        



        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {

            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();

            indata += "\n";
            Show.Text = indata;
 //           SetText(indata.ToString());
            if (indata != "")
            {
                SerialPort2.Write("1");
            }
        }



        delegate void SetTextCallback(string text);
        private void SetText(string text)
        {
            if (this.Data.Dispatcher.CheckAccess())
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Dispatcher.Invoke(d, new object[] { text });
      
            }
            else
            {
                this.Data.AppendText(text);
            }
        }






        //____ham main o day goi cac ham ra____________________________________



        public MainWindow()
        {
           InitializeComponent();
            Data.Text = "No Action did!!";
           getAvailablePorts();
           SerialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
           


//_________________________________________________________________________________________________________
//__________________________________________________________________________________________________________
            //SerialPort2 = new SerialPort();
            //SerialPort2.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
             if(Data.Text=="Connected" || Data.Text== "Refreshed!!")
            {
                Data.Text = "ngắt kết nối đi đã bạn ơi!";
            }
            else if (com1.Text == "" || com2.Text == "")
            {
                Data.Text = "Chưa chọn cổng com kìa BRO!!!!";
    
            }
            else 
            {
                try
                {
                    //_____dinh cau hinh cho com1___________________
                    string Baud = "9600";
                    SerialPort1.PortName = com1.Text;
                    SerialPort1.BaudRate = Convert.ToInt32(Baud);
                    SerialPort1.RtsEnable = true;
                    SerialPort1.DtrEnable = true;

                    SerialPort1.Open();
                    //textBox1.Enabled = true;
                    //____dinh cau hinh cho com2______________________
                    string Baud2 = "9600";
                    SerialPort2.PortName = com2.Text;
                    SerialPort2.BaudRate = Convert.ToInt32(Baud2);
                    SerialPort2.RtsEnable = true;
                    SerialPort2.DtrEnable = true;
                    SerialPort2.Open();
                    Data.Text = "Connected";

                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("Lỗi Kết Nối!!");
                }
            }
        }

        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {
            SerialPort1.Close();
            SerialPort2.Close();
            Data.Text = "Disconnected";

        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            getAvailablePorts();
            Data.Text = "Refreshed!!";
        }
    }
}
