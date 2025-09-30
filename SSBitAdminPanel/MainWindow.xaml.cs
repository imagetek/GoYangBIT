using SSData;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows;
using SSCommonNET;
using Rectangle = System.Drawing.Rectangle;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace SSBitUI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		static public bool startUpFine = false;
		public static void CaptureScreen(Rect rect)
		{
			try
			{
				//Stopwatch watch = new Stopwatch();
				//watch.Start();

				int W = (int)rect.Width;
				int H = (int)rect.Height;
				int X = (int)rect.X;
				int Y = (int)rect.Y;
				W = (W + 3) / 4 * 4;    // multiple of 4

				using (Bitmap colorBitmap = new Bitmap(W, H, PixelFormat.Format32bppArgb))  // Format32bppArgb Format16bppGrayScale Format4bppIndexed
				{
					using (Graphics gr = Graphics.FromImage(colorBitmap))
					{
						gr.CopyFromScreen(X, Y, 0, 0, colorBitmap.Size);
					}
					colorBitmap.RotateFlip(RotateFlipType.Rotate180FlipY);

					//	colorBitmap.Save("Capture.png", ImageFormat.Png);
					//	System.Diagnostics.Process.Start("Capture.png");

					BitmapData cBd = colorBitmap.LockBits(new Rectangle(0, 0, W, H), ImageLockMode.ReadOnly, colorBitmap.PixelFormat);
					int cStride = cBd.Stride;

					Bitmap grayBitmap = new Bitmap(W, H, PixelFormat.Format8bppIndexed);
					BitmapData gBd = grayBitmap.LockBits(new Rectangle(0, 0, W, H), ImageLockMode.ReadWrite, grayBitmap.PixelFormat);
					int gStride = gBd.Stride;



					unsafe
					{
						for (int y = 0; y < H; y++)
						{
							byte* cPtr = (byte*)cBd.Scan0 + (y * cStride);
							byte* gPtr = (byte*)gBd.Scan0 + (y * gStride);

							for (int x = 0; x < W; x++)
							{
								gPtr[x] = (byte)(cPtr[x * 4] * .114 + cPtr[x * 4 + 1] * .587 + cPtr[x * 4 + 2] * .299);  // Color을 Gray 값으로 변환
																														 // colorbar gPtr[x] = (byte)(x % 256);
							}
						}

						//edgar test
						//SaveCaptureToFile(grayBitmap);
						//edgar end

						colorBitmap.UnlockBits(cBd);
						grayBitmap.UnlockBits(gBd);

						int len = W * H;
						byte[] byteArray = new byte[len];
						byte* bPtr = (byte*)gBd.Scan0;
						System.Runtime.InteropServices.Marshal.Copy((IntPtr)bPtr, byteArray, 0, len);

						SSENV2Manager.TCON이미지쓰기(byteArray, X, Y, W, H);

						//ColorPalette colorPalette = grayBitmap.Palette;
						//for (int i = 0; i < 256; i++)
						//{
						//	colorPalette.Entries[i] = Color.FromArgb(i, i, i);
						//}
						//grayBitmap.Palette = colorPalette;
						//grayBitmap.Save("fast_gray.png", ImageFormat.Png);
						//System.Diagnostics.Process.Start("fast_gray.png");
					}
				}
				//	watch.Stop();
				//	Console.WriteLine($"캡쳐와 큐잉 시간 : {watch.ElapsedMilliseconds} ms");
			}
			catch (Exception ee)
			{
				TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
				System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
			}
		}
	}
}