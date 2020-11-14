// This file is part of RobotComponents. RobotComponents is licensed 
// under the terms of GNU General Public License as published by the 
// Free Software Foundation. For more information and the LICENSE file, 
// see <https://github.com/RobotComponents/RobotComponents>.

// System Libs
using System.Collections.Generic;
// ABB Libs
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;

namespace RobotComponents.Controllers
{
    public class Controller
    {
        #region fields
        private ABB.Robotics.Controllers.Controller _controller;
        private ABB.Robotics.Controllers.UserInfo _userInfo = UserInfo.DefaultUser;
        private string _userName = UserInfo.DefaultUser.Name;
        private string _password = UserInfo.DefaultUser.Password;
        #endregion

        #region constructors
        public Controller()
        {
            _controller = null;
        }

        public Controller(ControllerInfo controllerInfo)
        {
            _controller = ABB.Robotics.Controllers.Controller.Connect(controllerInfo, ConnectionType.Standalone);
        }
        #endregion

        #region static methods
        public static ControllerInfo[] GetControllers()
        {
            NetworkScanner scanner = new NetworkScanner();
            scanner.Scan();

            ControllerInfo[] controllers = scanner.GetControllers();

            return controllers;
        }
        #endregion

        #region methods
        public override string ToString()
        {
            if (_controller == null)
            {
                return "Controller (-)";
            }
            else
            {
                return "Controller (" + _controller.Name + ")";
            }
        }

        public ABB.Robotics.Controllers.Controller GetController()
        {
            return _controller;
        }

        public bool LogOn()
        {
            try
            {
                _controller.Logon(_userInfo);
                return true;
            }

            catch
            {
                return false;
            }
        }

        public bool LogOff()
        {
            try
            {
                _controller.Logoff();
                return true;
            }

            catch
            {
                return false;
            }
        }

        public bool Dispose()
        {
            try
            {
                if (_controller.Connected == true)
                {
                    _controller.Logoff();
                }

                _controller.Dispose();
                _controller = null;

                return true;
            }

            catch
            {
                return false;
            }
        }

        public void SetUserInfo(string name, string password = "")
        {
            _userName = name;
            _password = password;
            
            _userInfo = new UserInfo(_userName, _password);
        }

        public void SetDefaultUser()
        {
            _userInfo = UserInfo.DefaultUser;

            _userName = _userInfo.Name;
            _password = _userInfo.Password;
        }
        #endregion

        #region properties
        public bool IsValid
        {
            get
            {
                if (_controller == null) { return false; }
                return true;
            }
        }

        public string UserName
        {
            get { return _userName; }
        }

        public UserInfo UserInfo
        {
            get { return _userInfo; }
        }
        #endregion
    }
}
