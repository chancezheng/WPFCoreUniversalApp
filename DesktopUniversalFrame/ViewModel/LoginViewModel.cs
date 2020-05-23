using Chance.DesktopCustomControl.CustomComponent;
using Chance.DesktopCustomControl.CustomView.MsgDlg;
using DesktopUniversalFrame.Common;
using DesktopUniversalFrame.Entity;
using MySql.Data.MySqlClient;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace DesktopUniversalFrame.ViewModel
{
    public class LoginViewModel : NotifyPropertyChanged
    {
        #region Command

        /// <summary>
        /// 拖动窗口
        /// </summary>
        public DelegateCommand<Window> MoveWindowCommand
        {
            get => new DelegateCommand<Window>(win =>
                {
                    if (win != null)
                    {
                        win.MouseMove += (sender, e) =>
                        {
                            var point = e.GetPosition(win);
                            if (e.LeftButton == MouseButtonState.Pressed && point.Y <= 50)
                            {
                                win.DragMove();
                                e.Handled = true;
                            }
                        };
                    }
                });
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        public DelegateCommand<Window> CloseWindowCommand
        {
            get => new DelegateCommand<Window>(win =>
              {
                  if (win != null)
                  {
                      EndStoryboard.Begin();
                  }
              });
        }

        /// <summary>
        /// 窗口加载
        /// </summary>
        private DelegateCommand<Window> _loadedWindowCommand;
        public DelegateCommand<Window> LoadedWindowCommand
        {
            get { return _loadedWindowCommand; }
            set { _loadedWindowCommand = value; }
        }

        /// <summary>
        /// Gitee
        /// </summary>
        private DelegateCommand<Hyperlink> _giteeHyperlinkCommand;
        public DelegateCommand<Hyperlink> GiteeHyperlinkCommand
        {
            get { return _giteeHyperlinkCommand; }
            set { _giteeHyperlinkCommand = value; }
        }

        /// <summary>
        /// github
        /// </summary>
        private DelegateCommand<Hyperlink> _githubHyperlinkCommand;
        public DelegateCommand<Hyperlink> GithubHyperlinkCommand
        {
            get { return _githubHyperlinkCommand; }
            set { _githubHyperlinkCommand = value; }
        }

        /// <summary>
        /// 登陆
        /// </summary>
        private DelegateCommand<ImageButton> _loginCommand;
        public DelegateCommand<ImageButton> LoginCommand
        {
            get { return _loginCommand; }
            set { _loginCommand = value; }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        private string _userName;
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        /// <summary>
        /// 密码
        /// </summary>
        private string _userPassword;
        public string UserPassword
        {
            get => _userPassword;
            set => SetProperty(ref _userPassword, value);
        }

        /// <summary>
        /// 登陆时状态信息
        /// </summary>
        private string _loginInfo;
        public string LoginInfo
        {
            get => _loginInfo;
            set => SetProperty(ref _loginInfo, value);
        }

        /// <summary>
        /// Login Button IsEnable?
        /// </summary>
        private bool _isBtnEnable = true;
        public bool IsBtnEnable
        {
            get => _isBtnEnable;
            set => SetProperty(ref _isBtnEnable, value);
        }

        /// <summary>
        /// Remember IsChecked?
        /// </summary>
        private bool _isChecked = true;
        public bool IsChecked
        {
            get => _isChecked;
            set => SetProperty(ref _isChecked, value);
        }

        #endregion


        private ConfigrationOperation ConfigrationOperation;
        private Storyboard BeginStoryboard, EndStoryboard;

        public LoginViewModel()
        {
            LoadedWindowCommand = new DelegateCommand<Window>(WindowLoaded);
            GiteeHyperlinkCommand = new DelegateCommand<Hyperlink>(GitHyperink);
            GithubHyperlinkCommand = new DelegateCommand<Hyperlink>(GitHyperink);
            LoginCommand = new DelegateCommand<ImageButton>(Login);

            ConfigrationOperation = new ConfigrationOperation(AppDomain.CurrentDomain.BaseDirectory + "DesktopUniversalFrame.dll.config");
        }

        private void WindowLoaded(Window win)
        {
            BeginStoryboard = win.Resources["Start"] as Storyboard;
            BeginStoryboard.Begin();
            EndStoryboard = win.Resources["End"] as Storyboard;
            EndStoryboard.Completed += (sender, e) =>
            {
                win.Close();
            };

            if (IsChecked)
            {
                UserName = ConfigrationOperation.GetUserConfiguration("UserName");
                UserPassword = ConfigrationOperation.GetUserConfiguration("Password");
            }
        }

        //登陆
        private async void Login(ImageButton imageButton)
        {
            await Task.Run(() =>
            {
                if (IsChecked)
                {
                    ConfigrationOperation.SetUserConfiguration("UserName", UserName);
                    ConfigrationOperation.SetUserConfiguration("Password", UserPassword);
                }
                IsBtnEnable = false;
                LoginInfo = "正在校验身份...";
                string commandText = "select password from userinfo where username = @name";
                string result = SqlHelper.ExcuteScalar(commandText, new MySqlParameter("@name", UserName)) as string;
                if (UserPassword.Equals(result))
                {
                    Thread.Sleep(2000);
                    LoginInfo = "登陆成功...";                  
                }
                else
                {
                    Thread.Sleep(2000);
                    LoginInfo = "登陆失败...";
                }
                IsBtnEnable = true;
            });
        }


        /// <summary>
        /// 外网访问
        /// </summary>
        /// <param name="hyperlink"></param>
        private void GitHyperink(Hyperlink hyperlink)
        {
            Process.Start(new ProcessStartInfo("explorer",hyperlink.NavigateUri.AbsoluteUri));

            ////命令行启动
            //string url = hyperlink.NavigateUri.AbsoluteUri;
            //Process p = new Process();           
            //p.StartInfo.FileName = "cmd.exe";
            //p.StartInfo.UseShellExecute = false;
            //p.StartInfo.RedirectStandardInput = true;
            //p.StartInfo.RedirectStandardOutput = false;
            //p.StartInfo.RedirectStandardError = true;
            //p.StartInfo.CreateNoWindow = true;
            //p.Start();
            //p.StandardInput.WriteLine(new StringBuilder("start " + url + "&exit"));
            //p.StandardInput.AutoFlush = true;
            //p.WaitForExit();
        }
    }
}
