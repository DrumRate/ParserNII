using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ParserNII.DataStructures
{
    public class DatFileParser
    {
        public List<DataFile> Parse(Stream stream)
        {
            List<DataFile> results = new List<DataFile>();
            byte[] buffer;

            while (stream.Position < stream.Length)
            {
                var result = new DataFile();
                // 3 bytes
                buffer = new byte[3];
                stream.Read(buffer, 0, buffer.Length);
                result.ZeroxEE = (buffer[0], buffer[1], buffer[2]);

                // byte
                result.LabelType = (byte)stream.ReadByte();

                // int
                buffer = new byte[4];
                stream.Read(buffer, 0, buffer.Length);
                result.UnixTime = BitConverter.ToUInt32(buffer, 0);

                // byte
                result.LocomotiveType = (byte)stream.ReadByte();

                // short
                buffer = new byte[2];
                stream.Read(buffer, 0, buffer.Length);
                result.LocomotiveNumber = BitConverter.ToUInt16(buffer, 0);

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
                result.LeftFuelVolume = BitConverter.ToUInt16(buffer, 0);

                // short
                buffer = new byte[2];
                stream.Read(buffer, 0, buffer.Length);
                result.RightFuelVolume = BitConverter.ToUInt16(buffer, 0);

                // short
                buffer = new byte[2];
                stream.Read(buffer, 0, buffer.Length);
                result.MiddleFuelVolume = BitConverter.ToUInt16(buffer, 0);

                // short
                buffer = new byte[2];
                stream.Read(buffer, 0, buffer.Length);
                result.FuelMass = BitConverter.ToUInt16(buffer, 0);

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
                result.BIVersion = BitConverter.ToUInt16(buffer, 0);

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

                // skip byte
                stream.ReadByte();

                for (int i = 0; i < result.SecondsBlock.Length; i++)
                {
                    result.SecondsBlock[i] = ParseSecondBlock(stream);
                }

                // short
                buffer = new byte[2];
                stream.Read(buffer, 0, buffer.Length);
                result.CRC = BitConverter.ToInt16(buffer, 0);

                results.Add(result);
            }
            return results;
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

        public DataArrays ToArray(List<DataFile> data)
        {
            var result = new DataArrays
            {
                UnixTime = data.Select(d => d.UnixTime).ToArray(),
                LocomotiveSection = data.Select(d => d.LocomotiveSection).ToArray(),
                LocomotiveType = data.Select(d => d.LocomotiveType).ToArray(),
                LabelType = data.Select(d => d.LabelType).ToArray(),
                CoolingCircuitTemperature = data.Select(d => d.CoolingCircuitTemperature).ToArray(),
                FuelMass = data.Select(d => d.FuelMass).ToArray(),
                MiddleFuelVolume = data.Select(d => d.MiddleFuelVolume).ToArray(),
                RightTsDutTemperature = data.Select(d => d.RightTsDutTemperature).ToArray(),
                LeftDUTOffset = data.Select(d => d.LeftDUTOffset).ToArray(),
                Longitude = data.Select(d => d.Longitude).ToArray(),
                Latitude = data.Select(d => d.Latitude).ToArray(),
                MinuteByteParametrs = data.Select(d => d.MinuteByteParametrs).ToArray(),
                ColdWaterCircuitTemperature = data.Select(d => d.ColdWaterCircuitTemperature).ToArray(),
                RightFuelVolume = data.Select(d => d.RightFuelVolume).ToArray(),
                LocomotiveNumber = data.Select(d => d.LocomotiveNumber).ToArray(),
                LeftTsDutTemperature = data.Select(d => d.LeftTsDutTemperature).ToArray(),
                FuelTemperature = data.Select(d => d.FuelTemperature).ToArray(),
                BIVersion = data.Select(d => d.BIVersion).ToArray(),
                CRC = data.Select(d => d.CRC).ToArray(),
                CurrentCoefficient = data.Select(d => d.CurrentCoefficient).ToArray(),
                DieselSpeed = data.Select(d => d.DieselSpeed).ToArray(),
                EnvironmentTemperature = data.Select(d => d.EnvironmentTemperature).ToArray(),
                EquipmentAmount = data.Select(d => d.EquipmentAmount).ToArray(),
                FuelDensityCurrent = data.Select(d => d.FuelDensityCurrent).ToArray(),
                FuelDensityOnEquip = data.Select(d => d.FuelDensityOnEquip).ToArray(),
                FuelDensityStandard = data.Select(d => d.FuelDensityStandard).ToArray(),
                LeftFuelVolume = data.Select(d => d.LeftFuelVolume).ToArray(),
                MRKStatusFlags = data.Select(d => d.MRKStatusFlags).ToArray(),
                MinuteRecordId = data.Select(d => d.MinuteRecordId).ToArray(),
                OilCircuitTemperature = data.Select(d => d.OilCircuitTemperature).ToArray(),
                RightDUTOffset = data.Select(d => d.RightDUTOffset).ToArray(),
                TKCoefficient = data.Select(d => d.TKCoefficient).ToArray(),
                TabularNumber = data.Select(d => d.TabularNumber).ToArray(),
                UPSTemperature = data.Select(d => d.UPSTemperature).ToArray(),
                VoltageCoefficient = data.Select(d => d.VoltageCoefficient).ToArray(),
                ZeroxEE = data.Select(d => d.ZeroxEE).ToArray()
            };

            return result;
        } 
    }
}