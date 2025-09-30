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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SSControlsNET
{
	/// <summary>
	/// Interaction logic for ssSpinnerPlayer.xaml
	/// </summary>
	public partial class ssSpinnerPlayer : UserControl, IComponentConnector
	{
		public static DependencyProperty BallBrushProperty = DependencyProperty.Register(nameof(BallBrush), typeof(Brush), typeof(ssSpinnerPlayer), (PropertyMetadata)new UIPropertyMetadata((object)Brushes.Blue, new System.Windows.PropertyChangedCallback(ssSpinnerPlayer.PropertyChangedCallback)));
		public static DependencyProperty BallsProperty = DependencyProperty.Register(nameof(Balls), typeof(int), typeof(ssSpinnerPlayer), (PropertyMetadata)new UIPropertyMetadata((object)8, new System.Windows.PropertyChangedCallback(ssSpinnerPlayer.PropertyChangedCallback), new CoerceValueCallback(ssSpinnerPlayer.CoerceBallsValue)));
		public static DependencyProperty BallSizeProperty = DependencyProperty.Register(nameof(BallSize), typeof(double), typeof(ssSpinnerPlayer), (PropertyMetadata)new UIPropertyMetadata((object)20.0, new System.Windows.PropertyChangedCallback(ssSpinnerPlayer.PropertyChangedCallback), new CoerceValueCallback(ssSpinnerPlayer.CoerceBallSizeValue)));

		public ssSpinnerPlayer()
		{
			InitializeComponent();
		}
		private void Spinner_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.Transform.CenterX = this.ActualWidth / 2.0;
			this.Transform.CenterY = this.ActualHeight / 2.0;
			this.Refresh();
		}

		public Brush BallBrush
		{
			get => (Brush)this.GetValue(ssSpinnerPlayer.BallBrushProperty);
			set => this.SetValue(ssSpinnerPlayer.BallBrushProperty, (object)value);
		}

		public int Balls
		{
			get => (int)this.GetValue(ssSpinnerPlayer.BallsProperty);
			set => this.SetValue(ssSpinnerPlayer.BallsProperty, (object)value);
		}

		private static object CoerceBallsValue(DependencyObject d, object baseValue)
		{
			return (object)Math.Min(100, Math.Max(1, Convert.ToInt32(baseValue)));
		}

		public double BallSize
		{
			get => (double)this.GetValue(ssSpinnerPlayer.BallSizeProperty);
			set => this.SetValue(ssSpinnerPlayer.BallSizeProperty, (object)value);
		}

		private static object CoerceBallSizeValue(DependencyObject d, object baseValue)
		{
			return (object)Math.Min(100.0, Math.Max(1.0, Convert.ToDouble(baseValue)));
		}

		private static void PropertyChangedCallback(
		  DependencyObject d,
		  DependencyPropertyChangedEventArgs e)
		{
			((ssSpinnerPlayer)d).Refresh();
		}

		private void Refresh()
		{
			int balls = this.Balls;
			double ballSize = this.BallSize;
			this.canvas.Children.Clear();
			double val1 = this.ActualWidth / 2.0;
			double val2 = this.ActualHeight / 2.0;
			double num1 = Math.Min(val1, val2) - ballSize / 2.0;
			double num2 = Convert.ToDouble(balls);
			for (int index = 1; index <= balls; ++index)
			{
				double num3 = Convert.ToDouble(index);
				double length1 = val1 + Math.Cos(num3 / num2 * 2.0 * Math.PI) * num1 - ballSize / 2.0;
				double length2 = val2 + Math.Sin(num3 / num2 * 2.0 * Math.PI) * num1 - ballSize / 2.0;
				Ellipse ellipse = new Ellipse();
				ellipse.Fill = this.BallBrush;
				ellipse.Opacity = num3 / num2;
				ellipse.Height = ballSize;
				ellipse.Width = ballSize;
				Ellipse element = ellipse;
				Canvas.SetLeft((UIElement)element, length1);
				Canvas.SetTop((UIElement)element, length2);
				this.canvas.Children.Add((UIElement)element);
			}
		}
	}
}
