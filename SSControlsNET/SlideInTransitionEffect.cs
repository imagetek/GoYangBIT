using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace SSControlsNET
{
	public class SlideInTransitionEffect : ShaderEffect
	{
		public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty(nameof(Input), typeof(SlideInTransitionEffect), 0);
		public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register(nameof(Progress), typeof(double), typeof(SlideInTransitionEffect), (PropertyMetadata)new UIPropertyMetadata((object)30.0, ShaderEffect.PixelShaderConstantCallback(0)));
		public static readonly DependencyProperty SlideAmountProperty = DependencyProperty.Register(nameof(SlideAmount), typeof(Point), typeof(SlideInTransitionEffect), (PropertyMetadata)new UIPropertyMetadata((object)new Point(1.0, 0.0), ShaderEffect.PixelShaderConstantCallback(1)));
		public static readonly DependencyProperty Texture2Property = ShaderEffect.RegisterPixelShaderSamplerProperty(nameof(Texture2), typeof(SlideInTransitionEffect), 1);

		public SlideInTransitionEffect()
		{
			try
			{
				this.PixelShader = new PixelShader()
				{
					UriSource = new Uri("pack://application:,,,/SSControlsNET;component/Effects/transition_slidein.ps", UriKind.RelativeOrAbsolute)
				};
				this.UpdateShaderValue(InputProperty);
				this.UpdateShaderValue(ProgressProperty);
				this.UpdateShaderValue(SlideAmountProperty);
				this.UpdateShaderValue(Texture2Property);
			}
			catch (Exception ex)
			{
				Trace.WriteLine((object)ex);
			}
		}

		public Brush Input
		{
			get => (Brush)this.GetValue(InputProperty);
			set => this.SetValue(InputProperty, (object)value);
		}

		public double Progress
		{
			get => (double)this.GetValue(ProgressProperty);
			set => this.SetValue(ProgressProperty, (object)value);
		}

		public Point SlideAmount
		{
			get => (Point)this.GetValue(SlideAmountProperty);
			set => this.SetValue(SlideAmountProperty, (object)value);
		}

		public Brush Texture2
		{
			get => (Brush)this.GetValue(Texture2Property);
			set => this.SetValue(Texture2Property, (object)value);
		}
	}

	public enum SceneDirection
	{
		SD_ToUp,
		SD_ToDown,
		SD_ToLeft,
		SD_ToRight,
		SD_LeftToUp,
		SD_UpToLeft,
		SD_RightToUp,
		SD_UpToRight,
	}
}
