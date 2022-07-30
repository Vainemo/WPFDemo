using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFDemo7._20.Common;

namespace WPFDemo7._20.Model
{
    public class LoginModel:NotifyBase
    {
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                this.DoNotify();
            }
        }

        public string Password 
        {
            get { return _password; }
            set { _password = value; this.DoNotify(); }
        }

        public string ValidationCode
        {
            get { return _validationCode; }
            set { _validationCode = value; this.DoNotify(); }
        }

        private string _password;
        private string _validationCode;

    }
}
