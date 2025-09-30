using System;
using System.Collections.Generic;
using System.Text;

using IWshRuntimeLibrary;

namespace SSAgent
{
    public class ShortcutManager
    {
        public ShortcutManager()
        {

        }

        public static void CreateShotcut()
        {
            try
            {
                string 시작DIR = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
                string ShortCutFile = System.IO.Path.Combine(시작DIR, @"SSAgent 바로가기.lnk");
                string 실행FILE = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "SSAgent.exe");
                if (System.IO.File.Exists(ShortCutFile) == true) return;
                try
                {
                    // 바로가기 생성
                    WshShell wsh = new WshShell();
                    IWshShortcut wshLink = (IWshShortcut)wsh.CreateShortcut(ShortCutFile);
                    wshLink.TargetPath = 실행FILE;
                    wshLink.WorkingDirectory = System.IO.Directory.GetCurrentDirectory();
                    wshLink.Description = "파주BITViewer Agent";
                    wshLink.IconLocation = 실행FILE;

                    //wshLink.IconLocation = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Siheung.ico");
                    //wshLink.IconLocation = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("SSAgent.Siheung.ico");

                    wshLink.Save();
                }
                catch
                {

                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("### {0}\r\n{1} ###", ee.StackTrace, ee.Message));
            }
        }

        //void btnAdd_Click(object sender, EventArgs e)
        //{   // 시작 프로그램 등록
        //    try
        //    {
        //        // 시작프로그램 등록하는 레지스트리
        //        string runKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        //        RegistryKey strUpKey = Registry.LocalMachine.OpenSubKey(runKey);
        //        if (strUpKey.GetValue("StartupTest") == null)
        //        {
        //            strUpKey.Close();
        //            strUpKey = Registry.LocalMachine.OpenSubKey(runKey, true);
        //            // 시작프로그램 등록명과 exe경로를 레지스트리에 등록
        //            strUpKey.SetValue("StartupTest", Application.ExecutablePath);
        //        }
        //        MessageBox.Show("Add Startup Success");
        //    }
        //    catch
        //    {
        //        MessageBox.Show("Add Startup Fail");
        //    }
        //}

        //void btnRemove_Click(object sender, EventArgs e)
        //{   // 시작프로그램 제거
        //    try
        //    {
        //        string runKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        //        RegistryKey strUpKey = Registry.LocalMachine.OpenSubKey(runKey, true);
        //        // 레지스트리값 제거
        //        strUpKey.DeleteValue("StartupTest");
        //        MessageBox.Show("Remove Startup Success");
        //    }
        //    catch
        //    {
        //        MessageBox.Show("Remove Startup Fail");
        //    }
        //}
        //출처: https://blog.eightbox.net/162 [EIGHTBOX:티스토리]
    }
}