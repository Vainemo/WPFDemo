using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFDemo7._20.Common;
using WPFDemo7._20.DataAccess;
using WPFDemo7._20.Model;

namespace WPFDemo7._20.ViewModel
{
    internal class LoginViewModel:NotifyBase
    {

        public LoginModel LoginModel { get; set; }=new LoginModel();
        public CommandBase CloseWindowCommand { get; set; }
        public CommandBase LoginCommand { get; set; }
        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value;this.DoNotify(); }
        }

        public LoginViewModel()
        {
            this.LoginModel = new LoginModel();
            this.LoginModel.UserName = "admin";
            this.LoginModel.Password =string.Empty;
            this.CloseWindowCommand = new CommandBase();
            this.CloseWindowCommand.DoExecute = new Action<object>((x) =>
            {
                (x as Window).Close();
            });
            this.CloseWindowCommand.DoCanExecute = new Func<object, bool>((x) =>
            {
                return true;
            });
            this.LoginCommand = new CommandBase();
            this.LoginCommand.DoExecute = new Action<object>(DoLogin);
            this.LoginCommand.DoCanExecute = new Func<object, bool>((x) =>
            {
                return true;
            });
        }
        private void DoLogin(object x)
        {
            //this.ShowProgress = Visibility.Visible;
            this.ErrorMessage = "";
            if(string.IsNullOrEmpty( LoginModel.UserName))
            {
                this.ErrorMessage = "用请输入用户名！";
                //this.ShowProgress = Visibility.Collapsed;
                return;
            }
            if (string.IsNullOrEmpty(LoginModel.Password))
            {
                this.ErrorMessage = "请输入密码！";
                return;
            }
            if (string.IsNullOrEmpty(LoginModel.ValidationCode))
            {

                this.ErrorMessage = "请输入验证码！";
                return;
            }
            if (LoginModel.ValidationCode.ToLower()!="123")
            {
                this.ErrorMessage = "验证码不正确！";
                return;
            }
            Task.Run(new Action(() =>
            {
                try
                {
                    var user = LoaclDataAccess.Instance().CheckUserInfo(LoginModel.UserName, LoginModel.Password);
                    if (user == null)
                    {
                        throw new Exception("登录失败！用户名或密码错误!");
                    }
                    GlobalValues.UserInfo = user;
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        //退出登录界面
                        (x as Window).DialogResult = true;
                    }));
                   
                }
                catch (Exception ex)
                {
                    this.ErrorMessage = ex.Message;
                }
            }));
        }
    }
}
