using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Fishbot
{
	/// <summary>
	/// Logika interakcji dla klasy MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		IntPtr handler() { return new WindowInteropHelper(this).Handle; }
		EButton.Button btn = new EButton.Button();

		int defaultProcessId;
		int currentProcessId;
		int processId = 0;

		public MainWindow()
		{
			InitializeComponent();
			defaultProcessId = User.GetForegroundWindow();
		}

		void FishBot()
		{
			currentProcessId = User.GetForegroundWindow();
			User.ShowWindow(processId, 1);
			User.SetForegroundWindow(processId);

			Thread.Sleep(2000);
			btn.PressKey((short)EButton.Button.BT7.F3);
			Thread.Sleep(1000);
			btn.PressKey((short)EButton.Button.BT7.SPACE);

			bool foundText = false;
			int attempt = 0;

			while (!foundText) // max 10 prób lub zatrzymanie
			{
				Thread.Sleep(1000); // czekaj 1 sekundę między próbami

				Bitmap screenshot = User.CaptureWindow(processId);
				screenshot.Save("screenshot.png", ImageFormat.Png);
				string text = OCR.RecognizeText(screenshot);
				Console.WriteLine(text);

				if (text.Contains("Kliknij spację jeszcze"))
				{
					foundText = true;

					// Znajdź liczbę razy, które trzeba wcisnąć spację
					int startIndex = text.IndexOf("Kliknij spację jeszcze") + "Kliknij spację jeszcze".Length;
					int endIndex = text.IndexOf(" razy", startIndex);
					if (int.TryParse(text.Substring(startIndex, endIndex - startIndex).Trim(), out int count))
					{
						for (int i = 0; i < count; i++)
						{
							btn.PressKey((short)EButton.Button.BT7.SPACE);
							Thread.Sleep(100);
						}
					}
				}

				attempt++;
				foundText = false;
			}

			// Przywróć poprzednie okno
			if (processId != currentProcessId)
			{
				User.SetForegroundWindow(currentProcessId);
			}
			else
			{
				User.SetForegroundWindow(defaultProcessId);
			}
		}


		private void FindButton(object sender, RoutedEventArgs e)
		{
			//FIND ALL PROCESSES
			for (int window = User.GetWindow(User.GetDesktopWindow(), 5); window != 0; window = User.GetWindow(window, 2))
			{
				if (window == handler().ToInt32())
				{
					window = User.GetWindow(window, 2);
				}
				if (User.IsWindowVisible(window) != 0)
				{
					StringBuilder stringBuilder = new StringBuilder(50);
					User.GetWindowText(window, stringBuilder, stringBuilder.Capacity);
					string text = stringBuilder.ToString();
					if (text.Length > 0)
					{
						//ADD PROCESS TO LIST
						processlist.Items.Add(new MyProcess(text, window));
					}
				}
			}

			//DISPLAY PROCESS NAME NOT ID
			processlist.DisplayMemberPath = "ProcessName";
		}

		private void Processlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			processId = (((ListBox)sender).SelectedItem as MyProcess).ProcessId;
		}

		private void StartButton(object sender, RoutedEventArgs e)
		{
			if (processId == 0)
			{
				return;
			}
			Task.Run(() => FishBot());
        }

		private void StopButton(object sender, RoutedEventArgs e)
		{
		}
	}
}
