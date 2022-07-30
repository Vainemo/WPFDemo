using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPFDemo7._20.View;

namespace WPFDemo7._20
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //登录窗口关闭之后，打开主窗口
            if (new LoginView().ShowDialog()==true)
            {
                //利用新建对象使线程卡在这里，当主窗口关闭之后，为false
                new MainWindow().ShowDialog();
            }
            //执行关闭语句，退出窗口
            Application.Current.Shutdown();
        }
    }
}
