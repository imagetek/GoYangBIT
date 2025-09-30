using SSCommonNET;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WpfAnimatedGif;

namespace SSControlsNET
{
	/// <summary>
	/// Interaction logic for SSMeidaPlayer.xaml
	/// </summary>
	public partial class SSMeidaPlayer : UserControl, IComponentConnector
	{
		public SSMeidaPlayer()
		{
			InitializeComponent();
		}

		private Window _p;
		private bool _isLoaded;
		private DispatcherTimer _dtIPlayer;
		private int idxNo;
		private List<MediaData> items컨텐츠 = new List<MediaData>();
		private bool 반복YN;

		public void SetParentWindow(Window p) => this._p = p;

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				if (this._isLoaded)
					return;
				this.InitProc();
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
			finally
			{
				this._isLoaded = true;
			}
		}

		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			try
			{
				Console.WriteLine("### Unloaded : SSvplayer ###");
				this.DoFinal();
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
				if (this._dtIPlayer != null)
				{
					this._dtIPlayer.Stop();
					this._dtIPlayer.Tick -= new EventHandler(this._dtIPlayer_Tick);
					this._dtIPlayer = (DispatcherTimer)null;
				}
				GC.Collect();
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public int IMGDisplayTime { get; set; }

		private void InitProc()
		{
			try
			{
				if (this.IMGDisplayTime == 0)
					this.IMGDisplayTime = 10;
				if (this._dtIPlayer != null)
					return;
				this._dtIPlayer = new DispatcherTimer();
				this._dtIPlayer.Interval = TimeSpan.FromSeconds((double)this.IMGDisplayTime);
				this._dtIPlayer.Tick += new EventHandler(this._dtIPlayer_Tick);
				this._dtIPlayer.Tag = (object)0;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public void Set컨텐츠(string FILE_NM, int StretchNo = 1, int PlayTime = 0)
		{
			try
			{
				if (this.items컨텐츠 == null)
					this.items컨텐츠 = new List<MediaData>();
				this.items컨텐츠.Clear();
				Console.WriteLine("컨텐츠 추가 : {0}", (object)FILE_NM);
				FileInfo fileInfo = new FileInfo(FILE_NM);
				MediaData mediaData = new MediaData();
				mediaData.LOCAL_URL = FILE_NM;
				mediaData.FILE_NM = fileInfo.Name;
				mediaData.FILE_EXT = fileInfo.Extension;
				mediaData.STRETCH_GBN = StretchNo;
				if (PlayTime > 0)
					mediaData.Duration = PlayTime;
				this.idxNo = 0;
				this.items컨텐츠.Add(mediaData);
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public void Set컨텐츠목록(List<MediaData> items)
		{
			try
			{
				if (this.items컨텐츠 == null)
					this.items컨텐츠 = new List<MediaData>();
				this.items컨텐츠.Clear();
				this.items컨텐츠.AddRange((IEnumerable<MediaData>)items);
				this.idxNo = 0;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		private void ShowIPlayer()
		{
			try
			{
				DoubleAnimation animation1 = new DoubleAnimation(1.0, 0.0, new Duration(TimeSpan.FromMilliseconds(500.0)), FillBehavior.HoldEnd);
				animation1.Completed += (EventHandler)((sender, e) =>
				{
					if (vplayer.Visibility != Visibility.Visible)
						return;
					this.vplayer.Visibility = Visibility.Hidden;
				});
				this.vplayer.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline)animation1);
				DoubleAnimation animation2 = new DoubleAnimation(1.0, 0.0, new Duration(TimeSpan.FromMilliseconds(500.0)), FillBehavior.HoldEnd);
				animation2.Completed += (EventHandler)((sender, e) =>
				{
					if (this.gplayer.Visibility != Visibility.Visible)
						return;
					this.gplayer.Visibility = Visibility.Hidden;
				});
				this.gplayer.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline)animation2);
				CommonUtils.WaitTime(500);
				if (this.iplayer.Visibility != Visibility.Visible)
					this.iplayer.Visibility = Visibility.Visible;
				DoubleAnimation animation3 = new DoubleAnimation(0.7, 1.0, new Duration(TimeSpan.FromMilliseconds(500.0)), FillBehavior.HoldEnd);
				this.iplayer.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline)animation3);
			}
			catch (Exception ex)
			{
			}
		}

		private void ShowVPlayer()
		{
			try
			{
				DoubleAnimation animation1 = new DoubleAnimation(1.0, 0.0, new Duration(TimeSpan.FromMilliseconds(500.0)), FillBehavior.HoldEnd);
				animation1.Completed += (EventHandler)((sender, e) =>
				{
					if (this.iplayer.Visibility != Visibility.Visible)
						return;
					this.iplayer.Visibility = Visibility.Hidden;
				});
				this.iplayer.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline)animation1);
				DoubleAnimation animation2 = new DoubleAnimation(1.0, 0.0, new Duration(TimeSpan.FromMilliseconds(500.0)), FillBehavior.HoldEnd);
				animation2.Completed += (EventHandler)((sender, e) =>
				{
					if (this.gplayer.Visibility != Visibility.Visible)
						return;
					this.gplayer.Visibility = Visibility.Hidden;
				});
				this.gplayer.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline)animation2);
				CommonUtils.WaitTime(500);
				if (this.vplayer.Visibility != Visibility.Visible)
					this.vplayer.Visibility = Visibility.Visible;
				DoubleAnimation animation3 = new DoubleAnimation(0.8, 1.0, new Duration(TimeSpan.FromMilliseconds(500.0)), FillBehavior.HoldEnd);
				this.vplayer.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline)animation3);
			}
			catch (Exception ex)
			{
			}
		}

		private void ShowGPlayer()
		{
			try
			{
				DoubleAnimation animation1 = new DoubleAnimation(1.0, 0.0, new Duration(TimeSpan.FromMilliseconds(500.0)), FillBehavior.HoldEnd);
				animation1.Completed += (EventHandler)((sender, e) =>
				{
					if (this.iplayer.Visibility != Visibility.Visible)
						return;
					this.iplayer.Visibility = Visibility.Hidden;
				});
				this.iplayer.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline)animation1);
				DoubleAnimation animation2 = new DoubleAnimation(1.0, 0.0, new Duration(TimeSpan.FromMilliseconds(500.0)), FillBehavior.HoldEnd);
				animation2.Completed += (EventHandler)((sender, e) =>
				{
					if (this.vplayer.Visibility != Visibility.Visible)
						return;
					this.vplayer.Visibility = Visibility.Hidden;
				});
				this.vplayer.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline)animation2);
				CommonUtils.WaitTime(500);
				if (this.gplayer.Visibility != Visibility.Visible)
					this.gplayer.Visibility = Visibility.Visible;
				DoubleAnimation animation3 = new DoubleAnimation(0.8, 1.0, new Duration(TimeSpan.FromMilliseconds(500.0)), FillBehavior.HoldEnd);
				this.gplayer.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline)animation3);
			}
			catch (Exception ex)
			{
			}
		}

		private void _dtIPlayer_Tick(object sender, EventArgs e)
		{
			try
			{
				if (this._dtIPlayer != null)
					this._dtIPlayer.Stop();
				this.Next컨텐츠();
				this.Play컨텐츠();
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		private void vplayer_MediaOpened(object sender, RoutedEventArgs e)
		{
			try
			{
				int num = this.vplayer.IsLoaded ? 1 : 0;
			}
			catch (Exception ex)
			{
			}
		}

		private void vplayer_MediaEnded(object sender, RoutedEventArgs e)
		{
			try
			{
				Console.WriteLine("## 영상재생 완료 : {0} ##", (object)e.Source.ToString());
				this.Next컨텐츠();
				this.Play컨텐츠();
			}
			catch (Exception ex)
			{
			}
		}

		private void vplayer_MediaFailed(object sender, ExceptionRoutedEventArgs e)
		{
			try
			{
				Console.WriteLine("## 영상재생 오류 : {0} ##", (object)e.ErrorException.ToString());
				this.Next컨텐츠();
				this.Play컨텐츠();
			}
			catch (Exception ex)
			{
			}
		}

		private void gplayer_On재생완료Event()
		{
			try
			{
				this.Next컨텐츠();
				this.Play컨텐츠();
			}
			catch (Exception ex)
			{
			}
		}

		private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			try
			{
				Size newSize1 = e.NewSize;
				// ISSUE: variable of a boxed type
				double width1 = newSize1.Width;
				newSize1 = e.NewSize;
				// ISSUE: variable of a boxed type
				double height1 = newSize1.Height;
				Console.WriteLine("SSvplayer SizeChange {0}x{1}", (object)width1, (object)height1);
				Size newSize2;
				if (e.WidthChanged)
				{
					Canvas cvMain = this.cvMain;
					newSize2 = e.NewSize;
					double width2 = newSize2.Width;
					cvMain.Width = width2;
					Image iplayer = this.iplayer;
					newSize2 = e.NewSize;
					double width3 = newSize2.Width;
					iplayer.Width = width3;
					MediaElement vplayer = this.vplayer;
					newSize2 = e.NewSize;
					double width4 = newSize2.Width;
					vplayer.Width = width4;
					Image gplayer = this.gplayer;
					newSize2 = e.NewSize;
					double width5 = newSize2.Width;
					gplayer.Width = width5;
				}
				if (!e.HeightChanged)
					return;
				Canvas cvMain1 = this.cvMain;
				newSize2 = e.NewSize;
				double height2 = newSize2.Height;
				cvMain1.Height = height2;
				Image iplayer1 = this.iplayer;
				newSize2 = e.NewSize;
				double height3 = newSize2.Height;
				iplayer1.Height = height3;
				MediaElement vplayer1 = this.vplayer;
				newSize2 = e.NewSize;
				double height4 = newSize2.Height;
				vplayer1.Height = height4;
				Image gplayer1 = this.gplayer;
				newSize2 = e.NewSize;
				double height5 = newSize2.Height;
				gplayer1.Height = height5;
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		private void Display이미지(MediaData item)
		{
			try
			{
				BitmapImage bitmapImage = new BitmapImage(new Uri(item.LOCAL_URL, UriKind.RelativeOrAbsolute));
				this.iplayer.Source = (ImageSource)bitmapImage.Clone();
				this.iplayer.Stretch = item.MediaStretch;
				CommonUtils.WaitTime(50);
				bitmapImage.Freeze();
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		private void Display영상(MediaData item)
		{
			try
			{
				this.vplayer.Source = new Uri(item.LOCAL_URL);
				if (this.vplayer.NaturalDuration.HasTimeSpan)
				{
					TimeSpan timeSpan = this.vplayer.NaturalDuration.TimeSpan;
				}
				this.vplayer.Stretch = item.MediaStretch;
				this.vplayer.Volume = (double)(item.Volume / 100);
				this.vplayer.Play();
			}
			catch (Exception ex)
			{
			}
		}

		private void DisplayGIF이미지(MediaData item)
		{
			try
			{
				this.gplayer.Stretch = item.MediaStretch;
				ImageBehavior.SetAnimatedSource(this.gplayer, (ImageSource)new BitmapImage(new Uri(item.LOCAL_URL, UriKind.RelativeOrAbsolute)));
				ImageBehavior.SetAutoStart(this.gplayer, true);
				ImageBehavior.SetRepeatBehavior(this.gplayer, RepeatBehavior.Forever);
			}
			catch (Exception ex)
			{
			}
		}

		private void Clear영상()
		{
			try
			{
				this.vplayer.Stop();
				if (this.vplayer.Source != (Uri)null)
					this.vplayer.Source = (Uri)null;
				GC.Collect();
			}
			catch (Exception ex)
			{
			}
		}

		public delegate void 재생완료Handler();
		public event 재생완료Handler On재생완료Event;

		private void Next컨텐츠()
		{
			try
			{
				++this.idxNo;
				if (this.idxNo + 1 <= this.items컨텐츠.Count)
					return;
				this.idxNo = 0;
				this.StopPlayer();
				if (this.반복YN || this.On재생완료Event == null)
					return;
				this.On재생완료Event();
			}
			catch (Exception ex)
			{
			}
		}

		private void Play컨텐츠()
		{
			try
			{
				if (this.items컨텐츠 == null || this.items컨텐츠.Count == 0)
					return;
				MediaData mediaData = this.items컨텐츠[this.idxNo];
				switch (mediaData.CONT_GBN)
				{
					case 컨텐츠Type.IMAGE:
						if (!File.Exists(mediaData.LOCAL_URL))
							break;
						this.ShowIPlayer();
						if (this._dtIPlayer == null)
							this.InitProc();
						this.Display이미지(mediaData);
						if (this._dtIPlayer != null && this.items컨텐츠.Count > 0)
						{
							if (mediaData.Duration > 0)
								this._dtIPlayer.Interval = TimeSpan.FromSeconds((double)mediaData.Duration);
							this._dtIPlayer.Start();
						}
						Console.WriteLine(string.Format("[{0}/{1}] 이미지 표시 : {2}", (object)(this.idxNo + 1), (object)this.items컨텐츠.Count, (object)mediaData.LOCAL_URL), (object)"화면", (object)true);
						break;
					case 컨텐츠Type.MOVIE:
						if (!File.Exists(mediaData.LOCAL_URL))
							break;
						this.ShowVPlayer();
						this.Display영상(mediaData);
						Console.WriteLine(string.Format("[{0}/{1}] 동영상 재생 : {2}", (object)(this.idxNo + 1), (object)this.items컨텐츠.Count, (object)mediaData.LOCAL_URL), (object)"화면", (object)true);
						break;
					case 컨텐츠Type.GIF:
						if (!File.Exists(mediaData.LOCAL_URL))
							break;
						this.ShowGPlayer();
						if (this._dtIPlayer == null)
							this.InitProc();
						this.DisplayGIF이미지(mediaData);
						if (this._dtIPlayer != null && this.items컨텐츠.Count > 0)
						{
							if (mediaData.Duration > 0)
								this._dtIPlayer.Interval = TimeSpan.FromSeconds((double)mediaData.Duration);
							this._dtIPlayer.Start();
						}
						Console.WriteLine(string.Format("[{0}/{1}] GIF 재생 : {2}", (object)(this.idxNo + 1), (object)this.items컨텐츠.Count, (object)mediaData.LOCAL_URL), (object)"화면", (object)true);
						break;
				}
			}
			catch (Exception ex)
			{
			}
		}

		public bool IsActivePlayer() => this.items컨텐츠 != null && this.items컨텐츠.Count > 0;

		public void StartPlayer()
		{
			try
			{
				this.Play컨텐츠();
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}

		public void StopPlayer()
		{
			try
			{
				if (this._dtIPlayer != null)
					this._dtIPlayer.Stop();
				this.iplayer.Source = (ImageSource)null;
				ImageBehavior.SetAnimatedSource(this.gplayer, (ImageSource)null);
				this.Clear영상();
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", (object)ex.StackTrace, (object)ex.Message));
			}
		}
	}
}
