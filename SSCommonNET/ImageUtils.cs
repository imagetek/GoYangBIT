using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace SSCommonNET
{
	public static class ImageUtils
	{
		public static BitmapSource ConvertBitmapToBitmapSource(this Bitmap image)
		{
			Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
			BitmapData bitmapdata = image.LockBits(rect, ImageLockMode.ReadOnly, image.PixelFormat);
			try
			{
				BitmapPalette palette = (BitmapPalette)null;
				if (image.Palette.Entries.Length != 0)
					palette = new BitmapPalette((IList<System.Windows.Media.Color>)((IEnumerable<System.Drawing.Color>)image.Palette.Entries).Select<System.Drawing.Color, System.Windows.Media.Color>((Func<System.Drawing.Color, System.Windows.Media.Color>)(entry => System.Windows.Media.Color.FromArgb(entry.A, entry.R, entry.G, entry.B))).ToList<System.Windows.Media.Color>());
				return BitmapSource.Create(image.Width, image.Height, (double)image.HorizontalResolution, (double)image.VerticalResolution, ImageUtils.SelectPixelFormat(image.PixelFormat), palette, bitmapdata.Scan0, bitmapdata.Stride * image.Height, bitmapdata.Stride);
			}
			finally
			{
				image.UnlockBits(bitmapdata);
			}
		}

		private static System.Windows.Media.PixelFormat SelectPixelFormat(System.Drawing.Imaging.PixelFormat sourceFormat)
		{
			switch (sourceFormat)
			{
				case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
					return PixelFormats.Bgr24;
				case System.Drawing.Imaging.PixelFormat.Format32bppRgb:
					return PixelFormats.Bgr32;
				case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
					return PixelFormats.Bgra32;
				default:
					return new System.Windows.Media.PixelFormat();
			}
		}

		public static Image ConvertImageSourceToImage(ImageSource image)
		{
			try
			{
				MemoryStream memoryStream = new MemoryStream();
				BmpBitmapEncoder bmpBitmapEncoder = new BmpBitmapEncoder();
				bmpBitmapEncoder.Frames.Add(BitmapFrame.Create(image as BitmapSource));
				bmpBitmapEncoder.Save((Stream)memoryStream);
				memoryStream.Flush();
				return Image.FromStream((Stream)memoryStream);
			}
			catch (Exception ex)
			{
				TraceManager.AddLog(string.Format("{0}r\n{1}", (object)ex.StackTrace, (object)ex.Message));
				return (Image)null;
			}
		}

		internal static class NativeMethods
		{
			[DllImport("gdi32.dll")]
			[return: MarshalAs(UnmanagedType.Bool)]
			internal static extern bool DeleteObject(IntPtr hObject);
		}
	}
}
