﻿namespace ParserNII.DataStructures
{
    public class DataFile
    {
        public (byte, byte, byte) ZeroxEE;
        public byte LabelType;
        public int UnixTime; // replace with Time class
        public byte LocomotiveType;
        public short LocomotiveNumber;
        public byte LocomotiveSection;
        public short MinuteByteParametrs;
        public byte CoolingCircuitTemperature;
        public short LeftFuelVolume;
        public short RightFuelVolume;
        public short MiddleFuelVolume;
        public short FuelMass;
        public byte LeftTsDutTemperature;
        public byte RightTsDutTemperature;
        public int Latitude;
        public int Longitude;
        public byte FuelTeemperature;
        public byte FuelDensityCurrent;
        public byte FuelDensityStandard;
        public byte OilCircuitTemperature;
        public byte EnvironmentTemperature;
        public byte UPSTemperature;
        public int TabularNumber;
        // 2 empty bytes
        public short TKCoefficient;
        public short EquipmentAmount;
        public short BIVersion;
        public short LeftDUTOffset;
        public short RightDUTOffset;
        public short CurrentCoefficient;
        public short VoltageCoefficient;
        public byte DieselSpeed;
        // 2 empty bytes
        public byte ColdWaterCircuitTemperature;
        public int MinuteRecordId;
        public byte MRKStatusFlags;
        public byte FuelDensityOnEquip;
        
        // time data

        public short CRC;
    }
}