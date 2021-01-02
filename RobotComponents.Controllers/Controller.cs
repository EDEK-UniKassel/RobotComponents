// This file is part of RobotComponents. RobotComponents is licensed 
// under the terms of GNU General Public License as published by the 
// Free Software Foundation. For more information and the LICENSE file, 
// see <https://github.com/RobotComponents/RobotComponents>.

// System Libs
using System;
using System.IO;
using System.Collections.Generic;
// ABB Libs
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;
using ABB.Robotics.Controllers.RapidDomain;
using ABB.Robotics.Controllers.IOSystemDomain;

namespace RobotComponents.Controllers
{
    public class Controller
    {
        #region fields
        private ABB.Robotics.Controllers.Controller _controller;
        private ABB.Robotics.Controllers.UserInfo _userInfo = UserInfo.DefaultUser;
        private string _userName = UserInfo.DefaultUser.Name;
        private string _password = UserInfo.DefaultUser.Password;
        private readonly List<string> _logger = new List<string>();
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

        private static string CurrentTime()
        {
            DateTime localDate = DateTime.Now;
            return localDate.ToString();
        }
        #endregion

        #region methods
        public override string ToString()
        {
            if (_controller == null)
            {
                return "Controller (-)";
            }
            else if (_controller.IsVirtual == true)
            {
                return "Virtual controller (" + _controller.Name + ")";
            }
            else
            {
                return "Physical controller (" + _controller.Name + ")";
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
                _logger.Add(System.String.Format("{0}: Log on with username {1} succeeded.", CurrentTime(), _userName));
                return true;
            }

            catch
            {
                _logger.Add(System.String.Format("{0}: Log on with username {1} failed.", CurrentTime(), _userName));
                return false;
            }
        }

        public bool LogOff()
        {
            try
            {
                _controller.Logoff();
                _logger.Add(System.String.Format("{0}: Log off succeeded.", CurrentTime()));
                return true;
            }

            catch
            {
                _logger.Add(System.String.Format("{0}: Log off failed.", CurrentTime()));
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
                _logger.Add(System.String.Format("{0}: Failed to dispose the controller object.", CurrentTime()));
                return false;
            }
        }

        public void SetUserInfo(string name, string password = "")
        {
            _userName = name;
            _password = password;
            
            _userInfo = new UserInfo(_userName, _password);

            _logger.Add(System.String.Format("{0}: Username set to {1}", CurrentTime(), _userName));
        }

        public void SetDefaultUser()
        {
            _userInfo = UserInfo.DefaultUser;

            _userName = _userInfo.Name;
            _password = _userInfo.Password;

            _logger.Add(System.String.Format("{0}: User Info set to DefaultUser.", CurrentTime()));
        }

        public SignalCollection GetAnalogOutputs()
        {
            return _controller.IOSystem.GetSignals(filter: IOFilterTypes.Output | IOFilterTypes.Analog);
        }

        public SignalCollection GetDigitalOutputs()
        {
            return _controller.IOSystem.GetSignals(filter: IOFilterTypes.Output | IOFilterTypes.Digital);
        }

        public SignalCollection GetAnalogInputs()
        {
            return _controller.IOSystem.GetSignals(filter: IOFilterTypes.Input | IOFilterTypes.Analog);
        }

        public SignalCollection GetDigitalInputs()
        {
            return _controller.IOSystem.GetSignals(filter: IOFilterTypes.Input | IOFilterTypes.Digital);
        }

        public Signal PickSignal(string name)
        {
            return _controller.IOSystem.GetSignal(name);
        }

        public bool UploadModules(List<string> modules)
        {
            return false; // Returns true on succes
        }

        public bool UploadModule(string module)
        {
            StopProgram();

            string userDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "RobotComponents", "temp");

            if (!Directory.Exists(userDirectory))
            {
                Directory.CreateDirectory(userDirectory);
            }

            // TODO

            return false; // Returns true on sucess
        }
        public bool ResetProgramPointer()
        {
            // TODO

            return false; // Returns true on sucess
        }

        public bool RunProgram()
        {
            if (_controller.OperatingMode != ControllerOperatingMode.Auto)
            {
                _logger.Add(System.String.Format("{0}: Could not start the program. The controller is not set in automatic mode.", CurrentTime()));
                return false;
            }

            else if (_controller.State != ControllerState.MotorsOn)
            {
                _logger.Add(System.String.Format("{0}: Could not start the program. The motors are not on.", CurrentTime()));
                return false;
            }

            else
            {
                using (Mastership master = Mastership.Request(_controller))
                {
                    _controller.Rapid.Start(RegainMode.Continue, ExecutionMode.Continuous, ExecutionCycle.Once, StartCheck.CallChain);
                    master.Release();
                }

                _logger.Add(System.String.Format("{0}: Program started.", CurrentTime()));
                return true;
            }
        }

        public bool StopProgram()
        {
            if (_controller.OperatingMode != ControllerOperatingMode.Auto)
            {
                _logger.Add(System.String.Format("{0}: Could not stop the program. The controller is not set in automatic mode.", CurrentTime()));
                return false;
            }

            else
            {
                using (Mastership master = Mastership.Request(_controller))
                {
                    _controller.Rapid.Stop(StopMode.Instruction);
                    master.Release();
                }

                _logger.Add(System.String.Format("{0}: Program stopped.", CurrentTime()));
                return false;
            }
        }

        public void ResetLogger()
        {
            _logger.Clear();
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

        public string Name
        {
            get { return _controller.Name; }
        }

        public string UserName
        {
            get { return _userName; }
        }

        public UserInfo UserInfo
        {
            get { return _userInfo; }
        }

        public List<string> Logger
        {
            get { return _logger; }
        }
        #endregion
    }
}
