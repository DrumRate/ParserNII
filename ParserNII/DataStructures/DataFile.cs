﻿using System.Collections.Generic;
using ParserNII.Attributes;

namespace ParserNII.DataStructures
{
    public class DataFile
    {
        public Dictionary<string, DataElement> data = new Dictionary<string, DataElement>();
        //public (byte, byte, byte) ZeroxEE;

        //public byte LabelType;

        //public uint UnixTime; // replace with Time class

        //public byte LocomotiveType;

        //public ushort LocomotiveNumber;

        //public byte LocomotiveSection;

        //public FirstMinuteByteParams MinuteByteParametrs;
  
        //[ParamName("Температура контура охлаждения")]
        //public sbyte CoolingCircuitTemperature;

        //[ParamName("Объем топлива левый")]
        //public ushort LeftFuelVolume;

        //[ParamName("Объем топлива правый")]
        //public ushort RightFuelVolume;

        //[ParamName("Объем топлива средний")]
        //public ushort MiddleFuelVolume;

        //[ParamName("Масса топлива")]
        //public ushort FuelMass;

        //[ParamName("Температура ТС ДУТ левая")]
        //public sbyte LeftTsDutTemperature;

        //[ParamName("Температура ТС ДУТ правая")]
        //public sbyte RightTsDutTemperature;

        //[ParamName("Широта")]
        //public int Latitude;

        //[ParamName("Долгота")]
        //public int Longitude;

        //[ParamName("Температура топлива")]
        //public sbyte FuelTemperature;

        //[ParamName("Плотность топлива текущая")]
        //public short FuelDensityCurrent;

        //[ParamName("Плотность топлива при 20 С")]
        //public short FuelDensityStandard;

        //[ParamName("Температура контура масла")]
        //public sbyte OilCircuitTemperature;

        //[ParamName("Температура окружающего воздуха")]
        //public sbyte EnvironmentTemperature;

        //[ParamName("Напряжение ИБП")]
        //public double UPSVoltage;

        //[ParamName("Табельный номер")]
        //public int TabularNumber;


        //// 2 empty bytes
        //[ParamName("Коэффициент по ТК")]
        //public short TKCoefficient;

        //[ParamName("Объем экипировки")]
        //public short EquipmentAmount;

        //[ParamName("Версия БИ")]
        //public ushort BIVersion;

        //[ParamName("Смещение ДУТ левого")]
        //public double LeftDUTOffset;

        //[ParamName("Смещение ДУТ правого")]
        //public double RightDUTOffset;

        //[ParamName("Коэффициент по току")]
        //public short CurrentCoefficient;

        //[ParamName("Коэффициент по напряжению")]
        //public short VoltageCoefficient;

        //[ParamName("Коэффициент по оборотам дизеля")]
        //public byte DieselSpeed;

        //// 2 empty bytes
        //[ParamName("Температура воды холодного контура")]
        //public sbyte ColdWaterCircuitTemperature;

        ////[ParamName("ID минутной записи")]
        //public int MinuteRecordId;

        ////[ParamName("Флаги состояния МРК")]
        //public byte MRKStatusFlags;

        //[ParamName("Плотность топлива при экипировке")]
        //public short FuelDensityOnEquip;
        
        // time data
        public readonly SecondBlock[] SecondsBlock = new SecondBlock[20];
        //public short CRC;
    }
}