using OpenCvSharp;
using OpenCvSharp.Extensions;
using SSCommonNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
using System.Windows.Threading;

namespace SSControlsNET
{
	/// <summary>
	/// Interaction logic for ssWebcamPlayer.xaml
	/// </summary>
	public partial class ssWebcamPlayer : UserControl
	{
		private bool _isLoaded;
		private BackgroundWorker _bwApps;
		private int RotateGBN;
		private bool IsPlaying;
		private VideoCapture _vcap;
		private WriteableBitmap _wb;
		private bool recodeYN;
		private VideoWriter cvVWriter;

		public ssWebcamPlayer()
		{
			InitializeComponent();
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				int num = this._isLoaded ? 1 : 0;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
			finally
			{
				this._isLoaded = true;
			}
		}

		public void InitProc(int camIdx, int nRotate)
		{
			try
			{
				this.InitWebCam(camIdx);
				this.RotateGBN = nRotate;
				if (this._bwApps == null)
				{
					this._bwApps = new BackgroundWorker();
					this._bwApps.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this._bwApps_RunWorkerCompleted);
					this._bwApps.WorkerReportsProgress = false;
					this._bwApps.WorkerSupportsCancellation = true;
					this._bwApps.ProgressChanged += new ProgressChangedEventHandler(this._bwApps_ProgressChanged);
					this._bwApps.DoWork += new DoWorkEventHandler(this._bwApps_DoWork);
				}
				this.Run();
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public bool Run()
		{
			try
			{
				if (this._bwApps != null && !this.IsPlaying)
					this._bwApps.RunWorkerAsync((object)1000);
				return true;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return false;
			}
		}

		public bool IsPlay()
		{
			try
			{
				return this.IsPlaying;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return false;
			}
		}

		public void Stop()
		{
			try
			{
				if (this._bwApps == null)
					return;
				this._bwApps.CancelAsync();
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		private void _bwApps_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				if (!(sender is BackgroundWorker backgroundWorker))
					return;
				while (true)
				{
					Mat frame;
					do
					{
						if (backgroundWorker.CancellationPending)
						{
							e.Cancel = true;
							this.IsPlaying = false;
							return;
						}
						this.IsPlaying = true;
						frame = new Mat();
						Mat mat = new Mat();
						if (this.recodeYN)
						{
							if (this._vcap.Read(frame))
								this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate)(() =>
								{
									if (this.cvVWriter != null)
										this.cvVWriter.Write((InputArray)frame);
									Bitmap bitmap = frame.Flip(FlipMode.Y).ToBitmap();
									if (bitmap == null)
										return;
									this.img.Source = (ImageSource)bitmap.ConvertBitmapToBitmapSource();
								}));
						}
					}
					while (!this._vcap.Read(frame));
					this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate)(() =>
					{
						Bitmap bitmap = frame.Flip(FlipMode.Y).ToBitmap();
						if (bitmap == null)
							return;
						this.img.Source = (ImageSource)bitmap.ConvertBitmapToBitmapSource();
					}));
				}
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		private void _bwApps_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			try
			{
				Console.WriteLine("BackgroundWorkers 진행도 {0}", (object)e.ProgressPercentage);
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		private void _bwApps_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			try
			{
				if (e.Error == null)
					return;
				Console.WriteLine("### 오류 : {0} ###", (object)e.Error.Message);
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		private void InitWebCam(int camaraIdx)
		{
			try
			{
				this._vcap = VideoCapture.FromCamera(camaraIdx, VideoCaptureAPIs.DSHOW);
				this._vcap.FrameWidth = 640;
				this._vcap.FrameHeight = 480;
				this._wb = new WriteableBitmap(this._vcap.FrameWidth, this._vcap.FrameHeight, 96.0, 96.0, PixelFormats.Rgb24, (BitmapPalette)null);
				this.img.Source = (ImageSource)this._wb;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public void DoFinal()
		{
			try
			{
				if (this._bwApps != null)
				{
					this._bwApps.CancelAsync();
					this._bwApps.Dispose();
					this._bwApps = (BackgroundWorker)null;
				}
				if (this._wb != null)
				{
					this._wb.Freeze();
					this._wb = (WriteableBitmap)null;
				}
				if (this._vcap != null)
				{
					this._vcap.Dispose();
					this._vcap = (VideoCapture)null;
				}
				GC.Collect();
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			try
			{
				this.DoFinal();
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public ImageSource GetSnapshot()
		{
			try
			{
				try
				{
					CommonUtils.WaitTime(500);
				}
				catch (Exception ex)
				{
					TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				}
				return this.img.Source != null ? this.img.Source.Clone() : (ImageSource)null;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
				return (ImageSource)null;
			}
		}

		private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
		{
		}

		public void StartRecod(string fileName)
		{
			try
			{
				if (this.recodeYN)
					return;
				this.recodeYN = true;
				this.cvVWriter = new VideoWriter(fileName, FourCC.FromString("X264"), 20.0, new OpenCvSharp.Size(640, 480));
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public void StopRecod()
		{
			try
			{
				this.recodeYN = false;
				if (this.cvVWriter == null)
					return;
				this.cvVWriter.Release();
				this.cvVWriter.Dispose();
				this.cvVWriter = (VideoWriter)null;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}
	}
}
