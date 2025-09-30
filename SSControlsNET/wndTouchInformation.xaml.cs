using SSCommonNET;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SSControlsNET
{
	/// <summary>
	/// Interaction logic for wndTouchInformation.xaml
	/// </summary>
	public partial class wndTouchInformation : Window
	{
		private bool _isLoaded;
		private System.Windows.Point mousePoint;
		private Rectangle? _prevExtRect;

		public wndTouchInformation(string title, string message)
		{
			this.InitializeComponent();
			this.Title = title;
			this.txtMessage.Text = message;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				if (this._isLoaded)
					return;
				this.InitProc();
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

		private void InitProc()
		{
		}

		public override void OnApplyTemplate()
		{
			try
			{
				if (this.GetTemplateChild("btnClose") is Button templateChild1)
					templateChild1.Click += new RoutedEventHandler(this.btnMainClose_Click);
				if (this.GetTemplateChild("btnMainClose") is Button templateChild2)
					templateChild2.Click += new RoutedEventHandler(this.btnMainClose_Click);
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("## {0}\r\n{1} ##", (object)ex.StackTrace, (object)ex.Message));
			}
			base.OnApplyTemplate();
		}

		private void btnMainClose_Click(object sender, RoutedEventArgs e) => this.Close();

		protected override void OnKeyDown(KeyEventArgs e)
		{
			try
			{
				if (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.RightAlt || e.SystemKey == Key.F4)
					e.Handled = true;
				else
					base.OnKeyDown(e);
			}
			catch (Exception ex)
			{
			}
		}

		private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			try
			{
				System.Windows.Point position = e.GetPosition((IInputElement)this);
				this.mousePoint = new System.Windows.Point(position.X, position.Y);
				((UIElement)sender).CaptureMouse();
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		private void Grid_MouseMove(object sender, MouseEventArgs e)
		{
			try
			{
				if (this._prevExtRect.HasValue || !((UIElement)sender).IsMouseCaptured || e.LeftButton != MouseButtonState.Pressed)
					return;
				System.Windows.Point position = e.GetPosition((IInputElement)this);
				this.Left -= this.mousePoint.X - position.X;
				this.Top -= this.mousePoint.Y - position.Y;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			try
			{
				Console.WriteLine("## Grid_MouseLeftButtonUp : 환경설정 ##");
				((UIElement)sender).ReleaseMouseCapture();
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		private void Window_Closing(object sender, CancelEventArgs e)
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

		private void DoFinal()
		{
			try
			{
				GC.Collect();
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public void SetMessage(string message)
		{
			try
			{
				this.txtMessage.Text = message;
				CommonUtils.WaitTime(10);
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}
	}
}
