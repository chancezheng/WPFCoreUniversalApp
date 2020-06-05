using DesktopUniversalCustomControl.CustomComponent;
using DesktopUniversalFrame.Common;
using DesktopUniversalFrame.Entity;
using DesktopUniversalFrame.Model.Indentity;
using DesktopUniversalFrame.ViewModel.Login;
using MySql.Data.MySqlClient;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace DesktopUniversalFrame.ViewModel.Login
{
    /// <summary>
    /// 登陆ViewModel
    /// </summary>
    public class UserViewModel : NotifyPropertyChanged
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

        private DelegateCommand<Window> _loadedWindowCommand;
        private DelegateCommand<UserControl> _userControlLoaded;
        private DelegateCommand<Hyperlink> _giteeHyperlinkCommand;
        private DelegateCommand<Hyperlink> _githubHyperlinkCommand;
        private DelegateCommand<ToggleButton> _registerBackCommand;


        /// <summary>
        /// 窗口加载
        /// </summary>
        public DelegateCommand<Window> LoadedWindowCommand
        {
            get { return _loadedWindowCommand; }
            set { _loadedWindowCommand = value; }
        }

        /// <summary>
        /// UserControl加载
        /// </summary>
        public DelegateCommand<UserControl> UserControlLoaded
        {
            get { return _userControlLoaded; }
            set { _userControlLoaded = value; }
        }

        /// <summary>
        /// Gitee
        /// </summary>
        public DelegateCommand<Hyperlink> GiteeHyperlinkCommand
        {
            get { return _giteeHyperlinkCommand; }
            set { _giteeHyperlinkCommand = value; }
        }

        /// <summary>
        /// github
        /// </summary>
        public DelegateCommand<Hyperlink> GithubHyperlinkCommand
        {
            get { return _githubHyperlinkCommand; }
            set { _githubHyperlinkCommand = value; }
        }


        private DelegateCommand<ImageButton> _registerCommand;
        private DelegateCommand<ImageButton> _loginCommand;
        private DelegateCommand<ToggleButton> _forgotPasswordCommand;
        private DelegateCommand<ImageButton> _modifyPasswordCommand;


        /// <summary>
        /// 注册
        /// </summary>
        public DelegateCommand<ImageButton> RegisterCommand
        {
            get { return _registerCommand; }
            set { _registerCommand = value; }
        }

        /// <summary>
        /// 登陆
        /// </summary>
        public DelegateCommand<ImageButton> LoginCommand
        {
            get { return _loginCommand; }
            set { _loginCommand = value; }
        }

        /// <summary>
        /// 忘记密码
        /// </summary>
        public DelegateCommand<ToggleButton> ForgotPasswordCommand
        {
            get { return _forgotPasswordCommand; }
            set { _forgotPasswordCommand = value; }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        public DelegateCommand<ImageButton> ModifyPasswordCommand
        {
            get { return _modifyPasswordCommand; }
            set { _modifyPasswordCommand = value; }
        }

        /// <summary>
        /// 注册→与←返回
        /// </summary>

        public DelegateCommand<ToggleButton> RegisterBackCommand
        {
            get => _registerBackCommand;
            set => SetProperty(ref _registerBackCommand, value);
        }

        #endregion

        #region Data

        private string _registerName;
        private string _registerPassword;
        private bool _isRegisterBtnEnable = true;
        private string _registerStateInfo;

        private bool _isModifyBtnEnable = true;
        private string _modifyStateInfo;

        private string _userName;
        private string _userPassword;
        private string _loginInfo;
        private bool _isChecked = true;
        private bool _isLoginBtnEnable = true;
        private UserOperationType _userOperationType = UserOperationType.Login;
        private string _returnMain = "注册→";


        /// <summary>
        /// 注册用户名
        /// </summary>
        public string RegisterName
        {
            get => _registerName;
            set => SetProperty(ref _registerName, value);
        }

        //注册密码
        public string RegisterPassword
        {
            get => _registerPassword;
            set => SetProperty(ref _registerPassword, value);
        }

        /// <summary>
        /// Register Button IsEnable?
        /// </summary>
        public bool IsRegisterBtnEnable
        {
            get => _isRegisterBtnEnable;
            set => SetProperty(ref _isRegisterBtnEnable, value);
        }

        /// <summary>
        /// 注册状态信息
        /// </summary>
        public string RegisterStateInfo
        {
            get => _registerStateInfo;
            set => SetProperty(ref _registerStateInfo, value);
        }

        /// <summary>
        /// Modify Button IsEnable?
        /// </summary>
        public bool IsModifyBtnEnable
        {
            get => _isModifyBtnEnable;
            set => SetProperty(ref _isModifyBtnEnable, value);
        }

        /// <summary>
        /// 修改状态信息
        /// </summary>
        public string ModifyStateInfo
        {
            get => _modifyStateInfo;
            set => SetProperty(ref _modifyStateInfo, value);
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string UserPassword
        {
            get => _userPassword;
            set => SetProperty(ref _userPassword, value);
        }

        /// <summary>
        /// 登陆时状态信息
        /// </summary>
        public string LoginInfo
        {
            get => _loginInfo;
            set => SetProperty(ref _loginInfo, value);
        }

        /// <summary>
        /// Remember IsChecked?
        /// </summary>
        public bool IsChecked
        {
            get => _isChecked;
            set => SetProperty(ref _isChecked, value);
        }

        /// <summary>
        /// Login Button IsEnable?
        /// </summary>
        public bool IsLoginBtnEnable
        {
            get => _isLoginBtnEnable;
            set => SetProperty(ref _isLoginBtnEnable, value);
        }

        /// <summary>
        /// 用户操作
        /// </summary>
        public UserOperationType UserOperationType
        {
            get => _userOperationType;
            set => SetProperty(ref _userOperationType, value);
        }

        /// <summary>
        /// 主界面Return
        /// </summary>
        public string ReturnMain
        {
            get => _returnMain;
            set => SetProperty(ref _returnMain, value);
        }


        #endregion

        private Storyboard BeginStoryboard, EndStoryboard;
        private ConfigrationOperation ConfigrationOperation;
        private string exeConfigFilePath = AppDomain.CurrentDomain.BaseDirectory + "DesktopUniversalFrame.dll.config";

        public UserViewModel()
        {
            ConfigrationOperation = new ConfigrationOperation(exeConfigFilePath);

            LoadedWindowCommand = new DelegateCommand<Window>(WindowLoaded);
            UserControlLoaded = new DelegateCommand<UserControl>(UserCtlLoaded);
            RegisterCommand = new DelegateCommand<ImageButton>(Register);
            LoginCommand = new DelegateCommand<ImageButton>(Login);
            RegisterBackCommand = new DelegateCommand<ToggleButton>(RegisterBack);
            ForgotPasswordCommand = new DelegateCommand<ToggleButton>(ForgotPassword);
            ModifyPasswordCommand = new DelegateCommand<ImageButton>(ModifyPassword);
            GiteeHyperlinkCommand = new DelegateCommand<Hyperlink>(GitHyperink);
            GithubHyperlinkCommand = new DelegateCommand<Hyperlink>(GitHyperink);
        }

        //窗体加载
        private void WindowLoaded(Window win)
        {
            //BeginStoryboard = win.Resources["Start"] as Storyboard;
            //BeginStoryboard.Begin();
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

        UserControl LoginAnimation3DCtl;
        private void UserCtlLoaded(UserControl userControl)
        {
            LoginAnimation3DCtl = userControl;
            AnimationView.JoinRotation3DAnimation(LoginAnimation3DCtl, 0D);
        }

        //注册
        private async void Register(ImageButton btn)
        {           
            IsRegisterBtnEnable = false;
            RegisterStateInfo = "正在提交注册信息...";

            await Task.Run(() =>
            {
                if(string.IsNullOrEmpty(RegisterName) || string.IsNullOrEmpty(RegisterPassword))
                {
                    RegisterStateInfo = "请先输入用户名和密码";
                    IsRegisterBtnEnable = true;
                    return;
                }

                string commandText = "select username from userinfo where username = @name";
                object result = SqlHelper.ExcuteScalar(commandText, new MySqlParameter("@name", RegisterName));

                if (result != null)
                {
                    RegisterStateInfo = "用户已存在!";
                    IsRegisterBtnEnable = true;
                    return;
                }

                bool isInserted = ORMHelper.InsertData<UserInfoModel>(new UserInfoModel
                {
                    userName = RegisterName,
                    Password = RegisterPassword,
                });

                Thread.Sleep(2000);
                if (isInserted)
                {
                    UserName = RegisterName;
                    UserPassword = RegisterPassword;
                    RegisterStateInfo = "注册成功!";
                  
                    UserOperationType = UserOperationType.Login;
                    ReturnMain = "注册→";

                    Application.Current.Dispatcher.BeginInvoke(() => AnimationView.JoinRotation3DAnimation(LoginAnimation3DCtl, 0D));                  
                }
                else
                    RegisterStateInfo = "注册失败!";
                IsRegisterBtnEnable = true;
            });          
        }

        //登陆
        private async void Login(ImageButton imageButton)
        {
            if (IsChecked)
            {
                ConfigrationOperation.SetUserConfiguration("UserName", UserName);
                ConfigrationOperation.SetUserConfiguration("Password", UserPassword);
            }
            IsLoginBtnEnable = false;
            LoginInfo = "正在校验身份...";

            await Task.Run(() =>
            {
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
                IsLoginBtnEnable = true;
            });
        }

        //注册与返回
        private void RegisterBack(ToggleButton regBtn)
        {          
            if (ReturnMain == "注册→")
            {              
                RegisterName = string.Empty;
                RegisterPassword = string.Empty;
                RegisterStateInfo = string.Empty;

                UserOperationType = UserOperationType.Register;
                ReturnMain = "←返回";

                AnimationView.JoinRotation3DAnimation(LoginAnimation3DCtl, 180D);
            }
            else if (ReturnMain == "←返回")
            {
                UserOperationType = UserOperationType.Login;
                ReturnMain = "注册→";

                AnimationView.JoinRotation3DAnimation(LoginAnimation3DCtl, 0D, 1D);
            }
            else { }
        }

        //忘记密码
        private void ForgotPassword(ToggleButton fpBtn)
        {
            if (ReturnMain == "注册→")
            {
                ReturnMain = "←返回";
                UserOperationType = UserOperationType.ForgotPassword;
                AnimationView.JoinRotation3DAnimation(LoginAnimation3DCtl, 90D, 1D);
            }
        }

        //修改密码
        private async void ModifyPassword(ImageButton obj)
        {
            IsModifyBtnEnable = false;
            ModifyStateInfo = "正在修改密码...";

            await Task.Run(() =>
            {
                string commandText = $"update userinfo set password='{UserPassword}' where username=@name";
                int result = SqlHelper.ExcuteModify(commandText, new MySqlParameter("@name", UserName));
                if(result <= 0)
                {
                    Thread.Sleep(2000);
                    ModifyStateInfo = "用户名输入错误";
                }
                else
                {
                    Thread.Sleep(2000);
                    ModifyStateInfo = "修改成功";

                    Application.Current.Dispatcher.BeginInvoke(() => AnimationView.JoinRotation3DAnimation(LoginAnimation3DCtl, 0D));
                }
                IsModifyBtnEnable = true;
            });
        }

        /// <summary>
        /// 外网访问
        /// </summary>
        /// <param name="hyperlink"></param>
        private void GitHyperink(Hyperlink hyperlink)
        {
            Process.Start(new ProcessStartInfo("explorer", hyperlink.NavigateUri.AbsoluteUri));

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

            //if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            //{
            //    string url = url.Replace("&", "^&");
            //    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
            //}
        }
    }
}
