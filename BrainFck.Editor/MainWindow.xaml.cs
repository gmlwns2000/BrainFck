using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace BrainFck.Editor
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        Thread thread;
        event EventHandler<string> RunCompleted;
        string Status
        {
            set
            {
                Tb_Status.Dispatcher.Invoke(() =>
                {
                    Tb_Status.Text = value;
                });
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            RunCompleted += MainWindow_RunCompleted;
        }

        private void MainWindow_RunCompleted(object sender, string e)
        {
            Status = "Run completed";

            MessageBox.Show(e);
        }

        private void Bt_Run_Click(object sender, RoutedEventArgs e)
        {
            Console.Clear();

            Status = "Start compile";

            if(thread != null)
            {
                thread.Abort();
            }

            string program = Tb_Code.Text;

            thread = new Thread(() =>
            {
                Compiler c = new Compiler(program);
                string completed = "";
                try
                {
                    completed = c.Run();
                }
                catch (Exception ex)
                {
                    completed = ex.ToString();
                    MessageBox.Show(ex.ToString());

                    Status = "Error occured";
                }
                RunCompleted?.Invoke(c, completed);
            });
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
