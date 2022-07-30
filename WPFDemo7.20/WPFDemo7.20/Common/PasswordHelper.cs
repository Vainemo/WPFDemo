using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPFDemo7._20.Common
{
    /*
     *  依赖属性/附加属性
     *   依赖属性是一种可以自己没有值，并能通过使用Binding从数据源获得值（依赖在别人身上）的属性。
     * 拥有依赖属性的对象称为"依赖对象"。 继承树上可以看出，WPF的所有UI控件都是依赖对象。
     */
    /// <summary>
    /// 附加类
    /// </summary>
    public class PasswordHelper
    {
        /*    1.声明依赖属性，通常是static readonly的，通过DependencyProperty类Register方法
         *    2.声明附加属性，通过DependencyProperty类RegisterAttached方法
         *   两者区别：
         *    1.一般默认依赖属性使用CLR属性进行包装,附加属性使用Get,Set方法进行包装.
         *   
         *    2.注册依赖属性时,会传入一个属性元数据,但内部定义了一个默认的属性元数据(defaultMetadata ),当依赖属性注册完毕后，
         *   则重写了属性元数据(OverrideMetadata),而注册附加属性时,则直接传入参数.这个参数则直接作为了依赖属性的默认元数据
         *   
         *   3.附加属性被应用到的类并非定义附加属性的那个类，依赖属性被应用到定义附加属性的那个类。
         */
        public static readonly DependencyProperty PassworePropert = DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordHelper), new FrameworkPropertyMetadata("",new PropertyChangedCallback(OnPropertyChanged)));
        public static string GetPassword(DependencyObject d)
        {
            return d.GetValue(PassworePropert).ToString();
        }
        public static void SetPassword(DependencyObject d,string value)
        {
            d.SetValue(PassworePropert, value);
        }
        public static readonly DependencyProperty AttachProperty = DependencyProperty.RegisterAttached("Attach", typeof(bool), typeof(PasswordHelper), new FrameworkPropertyMetadata(default(bool), new PropertyChangedCallback(OnAttached)));

        static bool isUpdating=false;
        public static bool GetAttach(DependencyObject d)
        {
            return (bool)d.GetValue(AttachProperty);
        }
        public static void SetAttach(DependencyObject d, bool value)
        {
            d.SetValue(AttachProperty, value);
        }
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = d as PasswordBox;
            passwordBox.PasswordChanged -= Password_PasswordChanged;
            if (!isUpdating)
            {
                passwordBox.Password = e.NewValue?.ToString();
            }
            passwordBox.PasswordChanged += Password_PasswordChanged;
        }
        private static void OnAttached(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = d as PasswordBox;
            passwordBox.PasswordChanged += Password_PasswordChanged;
        }

        private static void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            isUpdating = true;
            SetPassword(passwordBox,passwordBox.Password);
            isUpdating = false;
        }
    }
}
