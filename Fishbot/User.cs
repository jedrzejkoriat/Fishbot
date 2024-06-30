using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

public class User
{
	[DllImport("user32.dll")]
	public static extern int SetForegroundWindow(int id);

	[DllImport("user32.dll")]
	public static extern int GetWindow(int id, int uCmd);

	[DllImport("user32.dll")]
	public static extern int GetDesktopWindow();

	[DllImport("user32.dll")]
	public static extern int IsWindowVisible(int id);

	[DllImport("user32.dll")]
	public static extern void GetWindowText(int id, StringBuilder sBuilder, int sBuilderCapacity);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool ShowWindow(int hWnd, int nCmdShow);

	[DllImport("user32.dll")]
	public static extern int GetForegroundWindow();

	[DllImport("user32.dll")]
	public static extern IntPtr GetWindowDC(IntPtr hWnd);

	[DllImport("user32.dll")]
	public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

	[DllImport("gdi32.dll")]
	public static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, CopyPixelOperation dwRop);

	[DllImport("user32.dll")]
	public static extern bool GetWindowRect(IntPtr hWnd, ref RECT rect);

	[StructLayout(LayoutKind.Sequential)]
	public struct RECT
	{
		public int left;
		public int top;
		public int right;
		public int bottom;
	}

	public static Bitmap CaptureWindow(int handle)
	{
		IntPtr hdcSrc = GetWindowDC((IntPtr)handle);
		RECT windowRect = new RECT();
		GetWindowRect((IntPtr)handle, ref windowRect);
		int width = windowRect.right - windowRect.left;
		int height = windowRect.bottom - windowRect.top;
		Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
		using (Graphics graphics = Graphics.FromImage(bmp))
		{
			IntPtr hdcDest = graphics.GetHdc();
			BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, CopyPixelOperation.SourceCopy);
			graphics.ReleaseHdc(hdcDest);
		}
		ReleaseDC((IntPtr)handle, hdcSrc);
		return bmp;
	}
}