using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFDemo7._20.Model;
using WPFDemo7._20.Common;
using System.Windows;
using System.Reflection;

namespace WPFDemo7._20.ViewModel
{
    class MainViewModel : NotifyBase
    {
        public UserModel UserInfo { get; set; }
        private String _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; this.DoNotify(); }
        }
        // FrameworkElement:为 WPF元素提供 WPF 框架级属性集、事件集和方法集。
        //此类表示附带的 WPF 框架级实现，它是基于由 UIElement 定义的 WPF 核心级 API 构建的。
        private FrameworkElement _mainContent;

        public FrameworkElement MainContent
        {
            get { return _mainContent; }
            set { _mainContent = value; this.DoNotify(); }
        }
        public CommandBase NavChangedCommand { get; set; }
        public MainViewModel()
            {
              UserInfo = new UserModel();
              this.NavChangedCommand = new CommandBase();
              this.NavChangedCommand.DoExecute = new Action<object>(DoNavChanged);
              this.NavChangedCommand.DoCanExecute = new Func<object, bool>((x)=>true);
              DoNavChanged("FirstPageView");
            }
        public void DoNavChanged(Object obj)
        {
            Type type = Type.GetType("WPFDemo7._20.View."+obj.ToString());
            // ConstructorInfo:发现类构造函数的属性，并提供对构造函数元数据的访问权限
            ConstructorInfo cti = type.GetConstructor(System.Type.EmptyTypes);
            this.MainContent =(FrameworkElement)cti.Invoke(null);
        }
    }
}
