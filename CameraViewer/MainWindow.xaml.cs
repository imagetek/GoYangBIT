using DirectShowLib;
using Emgu.CV;
using SSData.Utils;
using System.Windows;
using System.Windows.Controls;

namespace CameraViewer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Camera Capture Variables
		private VideoCapture _capture; //Camera
		private bool _captureInProgress = false; //Variable to track camera state
		VideoDevices[] WebCams; //List containing all the camera available
		int selectedWebCameraIndex = 0;
		private readonly Mat _frame = new();
		#endregion

		bool showError = false;
		public MainWindow()
		{
			InitializeComponent();
			PopulateCameras();
		}

		private void PopulateCameras()
		{
			FindCameras();
			UpdateButtonVisibility();

			_capture = new VideoCapture(0);
			_capture.ImageGrabbed += ProcessFrame;
			//if (VidDevices.Items.Count > 0)
			//{
			//	VidDevices.SelectedIndex = 0; //Set the selected device the default
			//	startBtn.IsEnabled = true; //Enable the start
			//}
		}

		private void UpdateButtonVisibility()
		{
			if (WebCams?.Length > 0)
			{
				selectedCamera.Text = WebCams[0].CameraName;
				selectedCamera.Visibility = Visibility.Visible;
				populateCameraBtn.Visibility = Visibility.Collapsed;
				buttonCanvas.Visibility = Visibility.Visible;
				startBtn.IsEnabled = true;
				stopBtn.IsEnabled = true;
			}
			else
			{
				selectedCamera.Visibility = Visibility.Collapsed;
				populateCameraBtn.Visibility = Visibility.Visible;
				buttonCanvas.Visibility = Visibility.Collapsed;
				startBtn.IsEnabled = false;
				stopBtn.IsEnabled = false;
			}
		}

		private static void ShowErrorMessage()
		{
			MessageBox.Show("Could not find any camera!", "Camera not found.", MessageBoxButton.OK);
		}

		private void FindCameras()
		{
			DsDevice[] _SystemCamereas = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
			WebCams = new VideoDevices[_SystemCamereas.Length];

			for (int i = 0; i < _SystemCamereas.Length; i++)
			{
				WebCams[i] = new VideoDevices(i, _SystemCamereas[i].Name, _SystemCamereas[i].ClassID);
			}
		}

		private void StartCaptureButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice).Length == 0)
				{
					WebCams = [];
					UpdateButtonVisibility();
					ShowErrorMessage();
					return;
				}
				if (selectedWebCameraIndex < WebCams.Length)
				{
					//_capture = new VideoCapture(selectedWebCameraIndex); //edgar old capture when several cameras available
					//_capture = new VideoCapture(0);
					//_capture.ImageGrabbed += ProcessFrame;
					_capture.Start();
					_captureInProgress = true;
					UpdateView();
				}

			}
			catch (Exception ex)
			{
				LogWriterService.WriteToLog(ex);
			}
		}

		private void StopCaptureButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				StopCapture();
			}
			catch (Exception ex)
			{
				LogWriterService.WriteToLog(ex);
			}

		}

		private void StopCapture()
		{
			_capture.Stop();
			//_capture.ImageGrabbed -= ProcessFrame;
			_captureInProgress = false;
			UpdateView();
		}

		private void VidDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (sender is ComboBox selection)
			{
				var cameraIndex = selection.SelectedIndex;
				selectedWebCameraIndex = cameraIndex;
			}
		}

		private void UpdateView()
		{
			Dispatcher.BeginInvoke(new Action(() =>
			{
				mediaElem.Visibility = _captureInProgress ? Visibility.Visible : Visibility.Collapsed;
			}));
		}

		private void ProcessFrame(object? sender, EventArgs arg)
		{
			if (_capture != null && _capture.Ptr != IntPtr.Zero)
			{
				_capture.Retrieve(_frame);
				Dispatcher.BeginInvoke(new Action(() =>
				{
					videoBorder.Width = _frame.Width;
					videoBorder.Height = _frame.Height;
					mediaElem.Source = _frame?.ToBitmapSource();
				}));
			}
		}

		private void populateCameraBtn_Click(object sender, RoutedEventArgs e)
		{
			PopulateCameras();
			if (WebCams?.Length == 0)
			{
				ShowErrorMessage();
			}
		}

		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			StopCapture();
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			PopulateCameras();
		}
	}

	public class VideoDevices
	{
		public VideoDevices(int i, string name, Guid classID)
		{
			Index = i;
			CameraName = name;
			CameraClassID = classID;
		}

		public int Index { get; set; }
		public string CameraName { get; set; }
		public Guid CameraClassID { get; set; }
	}

}