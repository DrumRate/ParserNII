using System;
using System.IO;

namespace ParserNII.DataStructures
{
    public class DatFileParser
    {
        public DataFile Parse(Stream stream)
        {
            DataFile result = new DataFile();
            byte[] buffer;

            // 3 bytes
            buffer = new byte[3];
            stream.Read(buffer, 0, buffer.Length);
            result.ZeroxEE = (buffer[0], buffer[1], buffer[2]);

            // byte
            result.LabelType = (byte)stream.ReadByte();

            // int
            buffer = new byte[4];
            stream.Read(buffer, 0, buffer.Length);
            result.UnixTime = BitConverter.ToInt32(buffer, 0);

            // byte
            result.LocomotiveType = (byte)stream.ReadByte();

            // short
            buffer = new byte[2];
            stream.Read(buffer, 0, buffer.Length);
            result.LocomotiveNumber = BitConverter.ToInt16(buffer, 0);

            // byte
            result.LocomotiveSection = (byte)stream.ReadByte();

            // short (enum)
            buffer = new byte[2];
            stream.Read(buffer, 0, buffer.Length);
            result.MinuteByteParametrs = (FirstMinuteByteParams)BitConverter.ToInt16(buffer, 0);

            // byte
            result.CoolingCircuitTemperature = (byte)stream.ReadByte();

            // short
            buffer = new byte[2];
            stream.Read(buffer, 0, buffer.Length);
            result.LeftFuelVolume = BitConverter.ToInt16(buffer, 0);

            // short
            buffer = new byte[2];
            stream.Read(buffer, 0, buffer.Length);
            result.RightFuelVolume = BitConverter.ToInt16(buffer, 0);

            // short
            buffer = new byte[2];
            stream.Read(buffer, 0, buffer.Length);
            result.MiddleFuelVolume = BitConverter.ToInt16(buffer, 0);

            // short
            buffer = new byte[2];
            stream.Read(buffer, 0, buffer.Length);
            result.FuelMass = BitConverter.ToInt16(buffer, 0);

            // byte
            result.LeftTsDutTemperature = (byte)stream.ReadByte();

            // byte
            result.RightTsDutTemperature = (byte)stream.ReadByte();

            // int
            buffer = new byte[4];
            stream.Read(buffer, 0, buffer.Length);
            result.Latitude = BitConverter.ToInt32(buffer, 0);

            // int
            buffer = new byte[4];
            stream.Read(buffer, 0, buffer.Length);
            result.Longitude = BitConverter.ToInt32(buffer, 0);


            // byte
            result.FuelTemperature = (byte)stream.ReadByte();

            // byte
            result.FuelDensityCurrent = (byte)stream.ReadByte();

            // byte
            result.FuelDensityStandard = (byte)stream.ReadByte();

            // byte
            result.OilCircuitTemperature = (byte)stream.ReadByte();

            // byte
            result.EnvironmentTemperature = (byte)stream.ReadByte();

            // byte
            result.UPSTemperature = (byte)stream.ReadByte();

            // int
            buffer = new byte[4];
            stream.Read(buffer, 0, buffer.Length);
            result.TabularNumber = BitConverter.ToInt32(buffer, 0);

            // skip 2 bytes
            stream.ReadByte();
            stream.ReadByte();

            // short
            buffer = new byte[2];
            stream.Read(buffer, 0, buffer.Length);
            result.TKCoefficient = BitConverter.ToInt16(buffer, 0);

            // short
            buffer = new byte[2];
            stream.Read(buffer, 0, buffer.Length);
            result.EquipmentAmount = BitConverter.ToInt16(buffer, 0);

            // short
            buffer = new byte[2];
            stream.Read(buffer, 0, buffer.Length);
            result.BIVersion = BitConverter.ToInt16(buffer, 0);

            // short
            buffer = new byte[2];
            stream.Read(buffer, 0, buffer.Length);
            result.LeftDUTOffset = BitConverter.ToInt16(buffer, 0);

            // short
            buffer = new byte[2];
            stream.Read(buffer, 0, buffer.Length);
            result.RightDUTOffset = BitConverter.ToInt16(buffer, 0);

            // short
            buffer = new byte[2];
            stream.Read(buffer, 0, buffer.Length);
            result.CurrentCoefficient = BitConverter.ToInt16(buffer, 0);

            // short
            buffer = new byte[2];
            stream.Read(buffer, 0, buffer.Length);
            result.VoltageCoefficient = BitConverter.ToInt16(buffer, 0);

            // byte
            result.DieselSpeed = (byte)stream.ReadByte();

            // skip 2 bytes
            stream.ReadByte();
            stream.ReadByte();

            // byte
            result.ColdWaterCircuitTemperature = (byte)stream.ReadByte();

            // int
            buffer = new byte[4];
            stream.Read(buffer, 0, buffer.Length);
            result.MinuteRecordId = BitConverter.ToInt32(buffer, 0);

            // byte
            result.MRKStatusFlags = (byte)stream.ReadByte();

            // byte
            result.FuelDensityOnEquip = (byte)stream.ReadByte();

            for (int i = 0; i < result.SecondsBlock.Length; i++)
            {
                result.SecondsBlock[i] = ParseSecondBlock(stream);
            }

            // short
            buffer = new byte[2];
            stream.Read(buffer, 0, buffer.Length);
            result.CRC = BitConverter.ToInt16(buffer, 0);

            return result;
        }

        private SecondBlock ParseSecondBlock(Stream stream)
        {
            SecondBlock result = new SecondBlock();

            byte[] buffer;

            // short (enum)
            buffer = new byte[2];
            stream.Read(buffer, 0, buffer.Length);
            result.BitParametrsSecond = (FirstSecondByteParams)BitConverter.ToInt16(buffer, 0);

            // short
            buffer = new byte[2];
            stream.Read(buffer, 0, buffer.Length);
            result.TurnoversDisel = BitConverter.ToInt16(buffer, 0);

            // short
            buffer = new byte[2];
            stream.Read(buffer, 0, buffer.Length);
            result.TurnoversTurbochanrger = BitConverter.ToInt16(buffer, 0);

            // short
            buffer = new byte[2];
            stream.Read(buffer, 0, buffer.Length);
            result.GeneratorPower = BitConverter.ToInt16(buffer, 0);

            // short
            buffer = new byte[2];
            stream.Read(buffer, 0, buffer.Length);
            result.GeneratorCurrent = BitConverter.ToInt16(buffer, 0);

            // short
            buffer = new byte[2];
            stream.Read(buffer, 0, buffer.Length);
            result.VoltageGenerator = BitConverter.ToInt16(buffer, 0);

            // short
            buffer = new byte[2];
            stream.Read(buffer, 0, buffer.Length);
            result.Speed = BitConverter.ToInt16(buffer, 0);

            // byte
            result.BoostPressure = (byte)stream.ReadByte();

            // short
            buffer = new byte[2];
            stream.Read(buffer, 0, buffer.Length);
            result.AverageFuelVolume = BitConverter.ToInt16(buffer, 0);

            // byte
            result.FuelPressure = (byte)stream.ReadByte();

            // byte
            result.OilPressure = (byte)stream.ReadByte();

            // byte
            result.PositionControllerDriver = (byte)stream.ReadByte();

            // byte
            result.OilPressureWithFilter = (byte)stream.ReadByte();

            // skip byte
            stream.ReadByte();

            return result;
        }
    }
}