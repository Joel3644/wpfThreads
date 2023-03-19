using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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

namespace dervishi.joel._4i.WPFThreads
{
    public partial class MainWindow : Window {
        const int GIRI1 = 50;
        const int GIRI2 = 500;
        const int GIRI3 = 5000;

        int _counter1 = 0;
        int _counter2 = 0;
        int _counter3 = 0;
        int _counter4 = 0;
        static readonly object _locker = new();

        CountdownEvent semaforo;

        public MainWindow() {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            btnGo.IsEnabled = false;

            prbarCounter4.Maximum = (GIRI1 + GIRI2 + GIRI3);

            Thread thread1 = new(incrementa1);
            thread1.Start();

            Thread thread2 = new(incrementa2);
            thread2.Start();

            Thread thread3 = new(incrementa3);
            thread3.Start();

            semaforo = new CountdownEvent(3);

            Thread thread4 = new(() => {
                semaforo.Wait();
                Dispatcher.Invoke(() => {
                    lblCounter1.Text = _counter1.ToString();
                    lblCounter2.Text = _counter2.ToString();
                    lblCounter3.Text = _counter3.ToString();
                    lblCounter4.Text = _counter4.ToString();

                    prbarCounter1.Value = _counter1;
                    prbarCounter2.Value = _counter2;
                    prbarCounter3.Value = _counter3;
                    prbarCounter3.Value = _counter4;
                    btnGo.IsEnabled = true;
                });
            });
            thread4.Start();
        }

        private void incrementa1() {
            for (int x = 0; x < GIRI1; x++) {
                lock (_locker) {
                    _counter1++;
                    _counter4++;
                }

                Dispatcher.Invoke(() => {
                    lblCounter1.Text = _counter1.ToString();
                    lblCounter4.Text = _counter4.ToString();
                    prbarCounter1.Value = x;
                    prbarCounter4.Value = _counter4;
                });
                Thread.Sleep(100);
            }
            semaforo.Signal();
        }

        private void incrementa2() {
            for (int x = 0; x < GIRI2; x++) {
                lock (_locker) {
                    _counter2++;
                    _counter4++;
                }

                Dispatcher.Invoke(() => {
                    lblCounter2.Text = _counter2.ToString();
                    lblCounter4.Text = _counter4.ToString();
                    prbarCounter2.Value = x;
                    prbarCounter4.Value = _counter4;
                });
                Thread.Sleep(10);
            }
            semaforo.Signal();
        }

        private void incrementa3() {
            for (int x = 0; x < GIRI3; x++) {
                lock (_locker) {
                    _counter3++;
                    _counter4++;
                }

                Dispatcher.Invoke(() => {
                    lblCounter3.Text = _counter3.ToString();
                    lblCounter4.Text = _counter4.ToString();
                    prbarCounter3.Value = x;
                    prbarCounter4.Value = _counter4;
                });
                Thread.Sleep(1);
            }
            semaforo.Signal();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e) {
            _counter1 = 0;
            _counter2 = 0;
            _counter3 = 0;
            _counter4 = 0;
            lblCounter1.Text = _counter1.ToString();
            lblCounter2.Text = _counter2.ToString();
            lblCounter3.Text = _counter3.ToString();
            lblCounter4.Text = _counter4.ToString();

            prbarCounter1.Value = _counter1;
            prbarCounter2.Value = _counter2;
            prbarCounter3.Value = _counter3;
            prbarCounter4.Value = _counter4;
        }
    }
}
