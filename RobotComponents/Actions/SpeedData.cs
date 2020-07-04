﻿// This file is part of RobotComponents. RobotComponents is licensed 
// under the terms of GNU General Public License as published by the 
// Free Software Foundation. For more information and the LICENSE file, 
// see <https://github.com/RobotComponents/RobotComponents>.

// System Libs
using System;
using System.Linq;
// RobotComponents Libs
using RobotComponents.Definitions;

namespace RobotComponents.Actions
{
    /// <summary>
    /// SpeedData class. SpeedData is used to specify the velocity at which both the robot and the external axes move.
    /// Speed data defines the velocity at which the tool center point moves, the reorientation speed of the tool, and 
    /// at which linear or rotating external axes move. When several different types of movement are combined, one of 
    /// the velocities often limits all movements.The velocity of the other movements will be reduced in such a way 
    /// that all movements will finish executing at the same time.
    /// </summary>
    public class SpeedData : Action
    {
        #region fields
        private string _name; // SpeeData variable name
        private double _v_tcp; // Tool center point speed
        private double _v_ori; // Re-orientation speed
        private double _v_leax; // External linear axis speed
        private double _v_reax; // External rotational axis speed
        private bool _predefined; // ABB predefinied data (e.g. v5, v10, v20, v30)?
        private readonly bool _exactPredefinedValue; // field that indicates if the exact predefined value was selected

        private static readonly string[] _validPredefinedNames = new string[] { "v5", "v10", "v20", "v30", "v40", "v50", "v60", "v80", "v100", "v150", "v200", "v300", "v400", "v500", "v600", "v800", "v1000", "v1500", "v2000", "v2500", "v3000", "v4000", "v5000", "v6000", "v7000" };
        private static readonly double[] _validPredefinedValues = new double[] { 5, 10, 20, 30, 40, 50, 60, 80, 100, 150, 200, 300, 400, 500, 600, 800, 1000, 1500, 2000, 2500, 3000, 4000, 5000, 6000, 7000 };

        #endregion

        #region constructors
        /// <summary>
        /// Defines an empty SpeedData object. 
        /// </summary>
        public SpeedData()
        {
        }

        /// <summary>
        /// Constructor for creating a predefined SpeedData. ABB defined already a number of speed data in the system module.
        /// </summary>
        /// <param name="v_tcp"> The velocity of the tool center point (TCP) in mm/s. </param>
        public SpeedData(double v_tcp)
        {
            // Get nearest predefined speeddata value
            double tcp = _validPredefinedValues.Aggregate((x, y) => Math.Abs(x - v_tcp) < Math.Abs(y - v_tcp) ? x : y);

            // Check if the exact predefined value is used or the nearest one
            if (v_tcp - tcp == 0)
            {
                _exactPredefinedValue = true;
            }
            else
            {
                _exactPredefinedValue = false;
            }


            // Set other fields
            _name = "v" + tcp.ToString();
            _v_tcp = tcp;
            _v_ori = 500;
            _v_leax = 5000;
            _v_reax = 1000;
            _predefined = true;
        }

        /// <summary>
        /// Constructor for creating a predefined SpeedData. ABB defined already a number of speed data in the system module.
        /// </summary>
        /// <param name="v_tcp"> The velocity of the tool center point (TCP) in mm/s. </param>
        public SpeedData(int v_tcp)
        {
            // Get nearest predefined speeddata value
            double tcp = _validPredefinedValues.Aggregate((x, y) => Math.Abs(x - v_tcp) < Math.Abs(y - v_tcp) ? x : y);

            // Check if the exact predefined value is used or the nearest one
            if (v_tcp - tcp == 0)
            {
                _exactPredefinedValue = true;
            }
            else
            {
                _exactPredefinedValue = false;
            }

            // Set other fields
            _name = "v" + tcp.ToString();
            _v_tcp = tcp;
            _v_ori = 500;
            _v_leax = 5000;
            _v_reax = 1000;
            _predefined = true;
        }

        /// <summary>
        /// Constructor for creating a custom SpeedData. 
        /// </summary>
        /// <param name="name"> The SpeedData variable name. </param>
        /// <param name="v_tcp"> The velocity of the tool center point (TCP) in mm/s. </param>
        /// <param name="v_ori"> The reorientation velocity of the TCP expressed in degrees/s. </param>
        /// <param name="v_leax"> The velocity of linear external axes in mm/s. </param>
        /// <param name="v_reax"> The velocity of rotating external axes in degrees/s. </param>
        public SpeedData(string name, double v_tcp, double v_ori = 500, double v_leax = 5000, double v_reax = 1000)
        {
            _name = name;
            _v_tcp = v_tcp;
            _v_ori = v_ori;
            _v_leax = v_leax;
            _v_reax = v_reax;
            _predefined = false;
            _exactPredefinedValue = false;
        }

        /// <summary>
        /// Creates a new speeddata by duplicating an existing speeddata. 
        /// This creates a deep copy of the existing speeddata. 
        /// </summary>
        /// <param name="speeddata"> The speeddata that should be duplicated. </param>
        public SpeedData(SpeedData speeddata)
        {
            _name = speeddata.Name;
            _v_tcp = speeddata.V_TCP;
            _v_ori = speeddata.V_ORI;
            _v_leax = speeddata.V_LEAX;
            _v_reax = speeddata.V_REAX;
            _predefined = speeddata.PreDefinied;
            _exactPredefinedValue = speeddata.ExactPredefinedValue;
        }

        /// <summary>
        /// Duplicates a SpeedData object
        /// </summary>
        /// <returns> Returns a deep copy of the SpeedData object. </returns>
        public SpeedData Duplicate()
        {
            return new SpeedData(this);
        }

        /// <summary>
        /// A method to duplicate the SpeedData object to an Action object. 
        /// </summary>
        /// <returns> Returns a deep copy of the SpeedData object as an Action object. </returns>
        public override Action DuplicateAction()
        {
            return new SpeedData(this) as Action;
        }
        #endregion

        #region method
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns> A string that represents the current object. </returns>
        public override string ToString()
        {
            if (!this.IsValid)
            {
                return "Invalid Speed Data";
            }
            else if (this.PreDefinied == true)
            {
                return "Predefined Speed Data ("+ _name + ")";
            }
            else
            {
                return "Custom Speed Data (" + _name + ")";
            }
        }

        /// <summary>
        /// Used to create variable definition code of this action. 
        /// </summary>
        /// <param name="robot"> Defines the Robot were the code is generated for. </param>
        /// <returns> Returns the RAPID code line as a string. </returns>
        public override string ToRAPIDDeclaration(Robot robot)
        {
            if (_predefined == false)
            {
                return "VAR speeddata " + _name + " := [" + _v_tcp + ", " + _v_ori + ", " + _v_leax + ", " + _v_reax + "];";
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Used to create action instruction code line. 
        /// </summary>
        /// <param name="robot"> Defines the Robot were the code is generated for. </param>
        /// <returns> Returns the RAPID code line as a string. </returns>
        public override string ToRAPIDInstruction(Robot robot)
        {
            return string.Empty;
        }

        /// <summary>
        /// Used to create variable definitions in the RAPID Code. It is typically called inside the CreateRAPIDCode() method of the RAPIDGenerator class.
        /// </summary>
        /// <param name="RAPIDGenerator"> Defines the RAPIDGenerator. </param>
        public override void ToRAPIDDeclaration(RAPIDGenerator RAPIDGenerator)
        {
            if (_predefined == false)
            {
                // Only adds speedData Variable if not already in RAPID Code
                if (!RAPIDGenerator.SpeedDatas.ContainsKey(this.Name))
                {
                    RAPIDGenerator.SpeedDatas.Add(this.Name, this);
                    RAPIDGenerator.StringBuilder.Append(Environment.NewLine + "\t" + this.ToRAPIDDeclaration(RAPIDGenerator.Robot));
                }
            }
        }

        /// <summary>
        /// Used to create action instructions in the RAPID Code. It is typically called inside the CreateRAPIDCode() method of the RAPIDGenerator class.
        /// </summary>
        /// <param name="RAPIDGenerator"> Defines the RAPIDGenerator. </param>
        public override void ToRAPIDInstruction(RAPIDGenerator RAPIDGenerator)
        {
        }
        #endregion

        #region properties
        /// <summary>
        /// A boolean that indicates if the SpeedData is valid. 
        /// </summary>
        public override bool IsValid
        {
            get
            {
                if (Name == "") { return false; }
                if (Name == null) { return false; }
                if (V_TCP <= 0) { return false; }
                if (V_ORI <= 0) { return false; }
                if (V_LEAX <= 0) { return false; }
                if (V_REAX <= 0) { return false; }
                return true;
            }
        }

        /// <summary>
        /// The SpeedData variable name. 
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// The velocity of the tool center point (TCP) in mm/s.
        /// If a stationary tool or coordinated external axes are used, the velocity is specified relative to the work object.
        /// </summary>
        public double V_TCP
        {
            get { return _v_tcp; }
            set { _v_tcp = value; }
        }

        /// <summary>
        /// The reorientation velocity of the TCP expressed in degrees/s. 
        /// If a stationary tool or coordinated external axes are used, the velocity is specified relative to the work object.
        /// </summary>
        public double V_ORI
        {
            get { return _v_ori; }
            set { _v_ori = value; }
        }

        /// <summary>
        /// The velocity of linear external axes in mm/s.
        /// </summary>
        public double V_LEAX
        {
            get { return _v_leax; }
            set { _v_leax = value; }
        }

        /// <summary>
        /// The velocity of rotating external axes in degrees/s.
        /// </summary>
        public double V_REAX
        {
            get { return _v_reax; }
            set { _v_reax = value; }
        }

        /// <summary>
        /// A boolean that indicates if the speeddata is predefined by ABB. 
        /// </summary>
        public bool PreDefinied
        {
            get { return _predefined; }
            set { _predefined = value; }
        }

        /// <summary>
        /// Indicates if the exact predefined speeddata value is used.
        /// If false the nearest predefined speedata or a custom zonedata is used.
        /// </summary>
        public bool ExactPredefinedValue
        {
            get { return _exactPredefinedValue; }
        }

        /// <summary>
        /// Defines an array with valid predefined speeddata variable names
        /// </summary>
        public static string[] ValidPredefinedNames 
        { 
            get { return _validPredefinedNames; }
        }

        /// <summary>
        /// Defines an array with valid predefined speeddata values
        /// </summary>
        public static double[] ValidPredefinedValues 
        { 
            get { return _validPredefinedValues; }
        }
        #endregion

    }
}