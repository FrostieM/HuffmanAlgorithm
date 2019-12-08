using System.Windows;
using System.Windows.Input;
using System.Windows.Forms;
using System.IO;
using System;
using System.Threading;
using System.Text;
using System.Windows.Threading;
using System.Collections.Generic;
using KursovaArhiv.Model;

namespace KursovaArhiv
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		DispatcherTimer timer = new DispatcherTimer();

		public MainWindow()
		{
			InitializeComponent();

			timer.Tick += new EventHandler(timer_Tick);
			timer.Interval = new TimeSpan(0, 0, 1);
		}

		private void SystemMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.DragMove();
		}

		private void ImageExit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (!timer.IsEnabled)
			{
				this.Close();
			}
		}

		private void ImageMinimize_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.WindowState = WindowState.Minimized;
		}

		private void ZipButton_Click(object sender, RoutedEventArgs e)
		{
			ChoosePanel.Visibility = Visibility.Hidden;
			ZipPanelChoose.Visibility = Visibility.Visible;
		}

		private void UnzipButton_Click(object sender, RoutedEventArgs e)
		{
			ChoosePanel.Visibility = Visibility.Hidden;
			UnzipPanelChoose.Visibility = Visibility.Visible;
		}

		private void ChoosePathButton_Click(object sender, RoutedEventArgs e)
		{
			ClickChooseEvent(PathTextBox);
		}

		private void ClickChooseEvent(System.Windows.Controls.TextBox textBox)
		{
			using (var fbd = new FolderBrowserDialog())
			{
				fbd.ShowDialog();

				if (fbd.SelectedPath.Length > 0)
				{
					textBox.Text = fbd.SelectedPath;
				}
			}
		}

		private void ZipPanelChoose_PreviewDragOver(object sender, System.Windows.DragEventArgs e)
		{
			e.Handled = true;
		}

		private void DragDropTextBox_Drop(object sender, System.Windows.DragEventArgs e)
		{
			if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
			{
				string[] files = (string[])e.Data.GetData(System.Windows.DataFormats.FileDrop);
				DragDropTextBox.Text = "";

				for (int i = 0; i < files.Length; ++i)
				{
					DragDropTextBox.AppendText(files[i]);

					if (i != files.Length - 1)
					{
						DragDropTextBox.AppendText("\n");
					}
				}
			}
		}

		private void ZipNextButton_Click(object sender, RoutedEventArgs e)
		{
			if (FileTextBox.Text.Length == 0)
			{
				System.Windows.MessageBox.Show("Введите имя файла");
				return;
			}

			if (PathTextBox.Text.Length == 0 || !Directory.Exists(PathTextBox.Text))
			{
				System.Windows.MessageBox.Show("Ошибка записи пути сохранения");
				return;
			}

			if (DragDropTextBox.Text.Length == 0)
			{
				System.Windows.MessageBox.Show("Выберете файлы для запаковки");
				return;
			}

			var files = DragDropTextBox.Text.Split('\n');

			for (int i = 0; i < files.Length; ++i)
			{
				if (!File.Exists(files[i]) && !Directory.Exists(files[i]))
				{
					System.Windows.MessageBox.Show(string.Format("данного файла или директории ({0}) не существует", files[i]));
					return;
				}
			}

			ZipPanelChoose.Visibility = Visibility.Hidden;
			ExecutePanel.Visibility = Visibility.Visible;

			var list = new List<string>
				{
					PathTextBox.Text,
					FileTextBox.Text
				};
			list.AddRange(DragDropTextBox.Text.Split('\n'));
			new Thread(new ParameterizedThreadStart(StartZip)).Start(list);
			timer.Start();
		}

		private void ZipbackButton_Click(object sender, RoutedEventArgs e)
		{
			ZipPanelChoose.Visibility = Visibility.Hidden;
			ChoosePanel.Visibility = Visibility.Visible;
		}

		private void UnzipPathChooseButton_Click(object sender, RoutedEventArgs e)
		{
			ClickChooseEvent(UnzipPathTextBox);
		}

		private void UnzipFileChooseButton_Click(object sender, RoutedEventArgs e)
		{
			var dlg = new Microsoft.Win32.OpenFileDialog();

			bool? result = dlg.ShowDialog();

			if (result == true && dlg.FileName.Split('.')[1].Equals("hz"))
			{
				UnzipFileTextBox.Text = dlg.FileName;
			}
		}

		private void UnzipNextButton_Click(object sender, RoutedEventArgs e)
		{
			if (UnzipPathTextBox.Text.Length == 0 || !Directory.Exists(UnzipPathTextBox.Text))
			{
				System.Windows.MessageBox.Show("Ошибка записи пути распаковки");
				return;
			}

			if (UnzipFileTextBox.Text.Length == 0 || !File.Exists(UnzipFileTextBox.Text))
			{
				System.Windows.MessageBox.Show("Ошибка чтения файла");
				return;
			}

			UnzipPanelChoose.Visibility = Visibility.Hidden;
			ExecutePanel.Visibility = Visibility.Visible;

			var list = new List<string>
				{
					UnzipPathTextBox.Text,
					UnzipFileTextBox.Text
				};
			
			new Thread(new ParameterizedThreadStart(StartUnzip)).Start(list);
			timer.Start();
		}

		private void UnzipBackButton_Click(object sender, RoutedEventArgs e)
		{
			UnzipPanelChoose.Visibility = Visibility.Hidden;
			ChoosePanel.Visibility = Visibility.Visible;
		}

		private void ExecuteBackButton_Click(object sender, RoutedEventArgs e)
		{
			ExecutePanel.Visibility = Visibility.Hidden;
			ChoosePanel.Visibility = Visibility.Visible;
			ExecuteTextBox.Text = "";
			ExecuteProgressBar.Value = 0;
			ExecuteBackButton.Visibility = Visibility.Hidden;
			ExecuteExitButton.Visibility = Visibility.Hidden;
		}

		private void ExecuteExitButton_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private static void StartZip(object obj)
		{
			var list = obj as List<string>;
			var path = list[0];
			var name = list[1];
			list.RemoveAt(0);
			list.RemoveAt(0);
			var files = list.ToArray();

			new Coder().ZipHuffman(path,name,files);
		}

		private static void StartUnzip(object obj)
		{
			var list = obj as List<string>;
			
			new Coder().UnpackHuffman(list[0], list[1]);
		}

		private void timer_Tick(object sender, EventArgs e)
		{
				if (Classes.ExecuteParams.Start == false)
				{
					ExecuteBackButton.Visibility = Visibility.Visible;
					ExecuteExitButton.Visibility = Visibility.Visible;
					Classes.ExecuteParams.Start = true;
					timer.Stop();
				}

			ExecuteProgressBar.Maximum = Classes.ExecuteParams.Max;
			ExecuteProgressBar.Value = Classes.ExecuteParams.CurrentValue;
			ExecuteTextBox.Text = Classes.ExecuteParams.Str;
	
		}
	}
}
