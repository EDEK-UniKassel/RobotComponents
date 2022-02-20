﻿// This file is part of RobotComponents. RobotComponents is licensed 
// under the terms of GNU General Public License as published by the 
// Free Software Foundation. For more information and the LICENSE file, 
// see <https://github.com/RobotComponents/RobotComponents>.

// System Libs
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
// RobotComponents Libs
using RobotComponents.Definitions;
using RobotComponents.Utils;

namespace RobotComponents.Actions
{
    /// <summary>
    /// Represent the Wait Time instruction.
    /// This action is used to wait a given amount of time between two actions.
    /// </summary>
    [Serializable()]
    public class WaitTime : Action, IInstruction, ISerializable
    {
        #region fields
        private double _duration; // the time expressed in seconds
        private bool _inPosition; // if true the mechanical unit comes to still stand at the sync point
        #endregion

        #region (de)serialization
        /// <summary>
        /// Protected constructor needed for deserialization of the object.  
        /// </summary>
        /// <param name="info"> The SerializationInfo to extract the data from. </param>
        /// <param name="context"> The context of this deserialization. </param>
        protected WaitTime(SerializationInfo info, StreamingContext context)
        {
            int version = (int)info.GetValue("Version", typeof(int)); // <-- use this if the (de)serialization changes
            _duration = (double)info.GetValue("Duration", typeof(double));
            _inPosition = version > 103000 ? (bool)info.GetValue("In Position", typeof(bool)) : false;
        }

        /// <summary>
        /// Populates a SerializationInfo with the data needed to serialize the object.
        /// </summary>
        /// <param name="info"> The SerializationInfo to populate with data. </param>
        /// <param name="context"> The destination for this serialization. </param>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Version", VersionNumbering.CurrentVersionAsInt, typeof(int));
            info.AddValue("Duration", _duration, typeof(double));
            info.AddValue("In Postion", _inPosition, typeof(bool));
        }
        #endregion

        #region constructors
        /// <summary>
        /// Initializes an empty instance of the Override Robot Tool class.
        /// </summary>
        public WaitTime()
        {
        }

        /// <summary>
        /// Initializes an empty instance of the Wait Time class.
        /// </summary>
        /// <param name="duration"> The time, expressed in seconds, that program execution is to wait. </param>
        /// <param name="inPosition"> Specifies whether or not the mechanial units must have come to a standstill before the wait time starts. </param>
        public WaitTime(double duration, bool inPosition = false)
        {
            _duration = duration;
            _inPosition = inPosition;
        }

        /// <summary>
        /// Initializes a new instance of the Wait Time class by duplicating an existing Wait Time instance. 
        /// </summary>
        /// <param name="waitTime"> The Wait Time instance to duplicate. </param>
        public WaitTime(WaitTime waitTime)
        {
            _duration = waitTime.Duration;
            _inPosition = waitTime.InPosition;
        }

        /// <summary>
        /// Returns an exact duplicate of this Wait Time instance.
        /// </summary>
        /// <returns> A deep copy of the Wait Time instance. </returns>
        public WaitTime Duplicate()
        {
            return new WaitTime(this);
        }

        /// <summary>
        /// Returns an exact duplicate of this Wait Time instance as IInstruction.
        /// </summary>
        /// <returns> A deep copy of the Wait Time instance as an IInstruction. </returns>
        public IInstruction DuplicateInstruction()
        {
            return new WaitTime(this);
        }

        /// <summary>
        /// Returns an exact duplicate of this Wait Time instance as an Action. 
        /// </summary>
        /// <returns> A deep copy of the Wait Time instance as an Action. </returns>
        public override Action DuplicateAction()
        {
            return new WaitTime(this);
        }
        #endregion

        #region method
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns> A string that represents the current object. </returns>
        public override string ToString()
        {
            if (!IsValid)
            {
                return "Invalid Wait Time";
            }
            else
            {
                return $"Wait Time ({_duration:0.###} sec.)";
            }
        }

        /// <summary>
        /// Returns the RAPID declaration code line of the this action.
        /// </summary>
        /// <param name="robot"> The Robot were the code is generated for. </param>
        /// <returns> An empty string. </returns>
        public override string ToRAPIDDeclaration(Robot robot)
        {
            return string.Empty;
        }

        /// <summary>
        /// Returns the RAPID instruction code line of the this action. 
        /// </summary>
        /// <param name="robot"> The Robot were the code is generated for. </param>
        /// <returns> The RAPID code line. </returns>
        public override string ToRAPIDInstruction(Robot robot)
        {
            return $"WaitTime {(_inPosition ? "\\InPos, " : "")}{_duration:0.###};";
        }

        /// <summary>
        /// Creates declarations in the RAPID program module inside the RAPID Generator. 
        /// This method is called inside the RAPID generator.
        /// </summary>
        /// <param name="RAPIDGenerator"> The RAPID Generator. </param>
        public override void ToRAPIDDeclaration(RAPIDGenerator RAPIDGenerator)
        {
        }

        /// <summary>
        /// Creates instructions in the RAPID program module inside the RAPID Generator.
        /// This method is called inside the RAPID generator.
        /// </summary>
        /// <param name="RAPIDGenerator"> The RAPID Generator. </param>
        public override void ToRAPIDInstruction(RAPIDGenerator RAPIDGenerator)
        {
            RAPIDGenerator.ProgramInstructions.Add("    " + "    " + ToRAPIDInstruction(RAPIDGenerator.Robot)); 
        }
        #endregion

        #region properties
        /// <summary>
        /// Gets a value indicating whether or not the object is valid.
        /// </summary>
        public override bool IsValid
        {
            get
            {
                if (_duration < 0) { return false; }
                else { return true; }
            }
        }

        /// <summary>
        /// Gets or sets the time, expressed in seconds, that program execution is to wait. 
        /// Min. value 0 seconds. Max. value no limit. Resolution 0.001 seconds.
        /// </summary>
        public double Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the robot and external axes must have come to a standstill 
        /// before this program task starts waiting for other program tasks to reach its meeting point.
        /// </summary>
        public bool InPosition
        {
            get { return _inPosition; }
            set { _inPosition = value; }
        }
        #endregion
    }

}
