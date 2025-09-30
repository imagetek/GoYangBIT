using SSCommonNET;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace SSControlsNET
{
	/// <summary>
	/// Interaction logic for wndProgress.xaml
	/// </summary>
	public partial class wndProgress : Window
	{
		private System.Windows.Point mousePoint;
		private Rectangle? _prevExtRect;

		public wndProgress(string title, string message, bool _useProg)
		{
			this.InitializeComponent();
			this.Title = title;
			this.txtMessage.Text = message;
			if (_useProg)
				this.pgrBar.Visibility = Visibility.Visible;
			else
				this.pgrBar.Visibility = Visibility.Hidden;
		}

		private void wndBaseDialogStyle_Loaded(object sender, RoutedEventArgs e)
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

		public void SetPrograss(string msg, double value)
		{
			try
			{
				if (!msg.Equals(""))
					this.txtMessage.Text = msg;
				this.pgrBar.Value = value;
				CommonUtils.WaitTime(10);
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public void SetTitle(string title)
		{
			try
			{
				if (title.Equals(""))
					return;
				this.Title = title;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}
	}
}
