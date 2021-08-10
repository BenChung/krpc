﻿using System;
using Expansions.Serenity;
using KRPC.Service.Attributes;
using KRPC.SpaceCenter.ExtensionMethods;
using KRPC.Utils;
using UnityEngine;


namespace KRPC.SpaceCenter.Services.Parts
{
    /// <summary>
    /// A Robotic Piston Part. Obtained by calling <see cref="Part.RoboticRotor"/>
    /// </summary>
    [KRPCClass(Service = "SpaceCenter")]
    public class RoboticRotor : Equatable<RoboticRotor>
    {
        readonly ModuleRoboticServoRotor servo;

        internal static bool Is(Part part)
        {
            return part.InternalPart.HasModule<ModuleRoboticServoRotor>();
        }

        internal RoboticRotor(Part part)
        {
            Part = part;
            var internalPart = part.InternalPart;
            servo = internalPart.Module<ModuleRoboticServoRotor>();

            if (servo == null)
                throw new ArgumentException("Part is not a robotic rotor");
        }

        /// <summary>
        /// Returns true if the objects are equal.
        /// </summary>
        public override bool Equals(RoboticRotor other)
        {
            return
            !ReferenceEquals(other, null) &&
            Part == other.Part &&
            servo.Equals(other.servo);
        }

        /// <summary>
        /// Hash code for the object.
        /// </summary>
        public override int GetHashCode()
        {
            int hash = Part.GetHashCode() ^ servo.GetHashCode();

            return hash;
        }

        /// <summary>
        /// The KSP Robotic Servo Hinge object.
        /// </summary>
        public ModuleRoboticServoRotor InternalRotor
        {
            get { return servo; }
        }

        /// <summary>
        /// The part object for this robotic hinge.
        /// </summary>
        [KRPCProperty]
        public Part Part { get; private set; }

        /// <summary>
        ///Target Angle for Robotic Hinge
        /// </summary>
        [KRPCProperty]
        public float TargetPosition { get { return servo.rpmLimit; } set { servo.rpmLimit = value; servo.OnPreModifyServo(); } }

        /// <summary>
        ///Current Angle for Robotic Hinge
        /// </summary>
        [KRPCProperty]
        public float CurrentPosition { get { return servo.currentRPM; } }

       

        /// <summary>
        /// Lock Movement
        /// </summary>
        [KRPCProperty]
        public bool RotationLocked
        {
            get { return servo.servoIsLocked; }
            set
            {
                if (value == true) servo.EngageServoLock();
                else servo.DisengageServoLock();
            }
        }

        /// <summary>
        /// Engage/Disengage Motor
        /// </summary>
        [KRPCProperty]
        public bool MotorEngaged
        {
            get { return servo.servoMotorIsEngaged; }
            set
            {
                if (value == true) servo.EngageMotor();
                else servo.DisengageMotor();
            }
        }

        /// <summary>
        ///Torque Limit Percentage
        /// </summary>
        [KRPCProperty]
        public float TorqueLimit { get { return servo.servoMotorLimit; } set { servo.servoMotorLimit = value; } }

        /// <summary>
        /// Returns Hinge to Build Angle Position
        /// </summary>
        [KRPCMethod]
        public void Home()
        {
            servo.rpmLimit = servo.launchPosition;
        }

    }
}