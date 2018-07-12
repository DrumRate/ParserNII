using System;
using System.Collections.Generic;
using System.IO;

namespace ParserNII.DataStructures
{
    public class DatFileParser : Parser
    {
        public override List<DataFile> Parse(Stream stream)
        {
            List<DataFile> results = new List<DataFile>();
            byte[] buffer;

            while (stream.Position < stream.Length)
            {
                var result = new DataFile();
                // 3 bytes
                buffer = new byte[3];
                stream.Read(buffer, 0, buffer.Length);
                result.Data.Add("Три байта 0xEE", new DataElement{ OriginalValue = (buffer[0], buffer[1], buffer[2]) });

                // byte
                var labelType = (byte)stream.ReadByte();
                result.Data.Add("Тип метки", new DataElement { OriginalValue = labelType, DisplayValue = labelType.ToString() });

                // int
                buffer = new byte[4];
                stream.Read(buffer, 0, buffer.Length);
                var unixTime = BitConverter.ToUInt32(buffer, 0);
                result.Data.Add("Время в “UNIX” формате", new DataElement { OriginalValue = unixTime, DisplayValue = DateTimeOffset.FromUnixTimeSeconds(unixTime).AddHours(3).ToString("dd.MM.yyyy HH:mm:ss") });

                // byte
                var LocomotiveType = (byte)stream.ReadByte();
                result.Data.Add("Тип локомотива", new DataElement { OriginalValue = LocomotiveType, DisplayValue = LocomotiveType.ToString() });

                // short
                buffer = new byte[2];
                stream.Read(buffer, 0, buffer.Length);
                var LocomotiveNumber = BitConverter.ToUInt16(buffer, 0);
                result.Data.Add("№ тепловоза", new DataElement { OriginalValue = LocomotiveNumber, DisplayValue = LocomotiveNumber.ToString() });


                // byte
                var LocomotiveSection = (byte)stream.ReadByte();
                result.Data.Add("Секция локомотива", new DataElement { OriginalValue = LocomotiveSection, DisplayValue = LocomotiveSection.ToString() });


                // short (enum)
                buffer = new byte[2];
                stream.Read(buffer, 0, buffer.Length);
                var MinuteByteParametrs = (FirstMinuteByteParams)BitConverter.ToInt16(buffer, 0);


                // byte
                var CoolingCircuitTemperature = (sbyte)stream.ReadByte();
                result.Data.Add("Температура контура охлаждения", new DataElement { OriginalValue = CoolingCircuitTemperature, DisplayValue = CoolingCircuitTemperature.ToString(), ChartValue = CoolingCircuitTemperature, Display = true });

                // short
                buffer = new byte[2];
                stream.Read(buffer, 0, buffer.Length);
                var LeftFuelVolume = BitConverter.ToUInt16(buffer, 0);
                result.Data.Add("Объем топлива левый", new DataElement { OriginalValue = LeftFuelVolume, DisplayValue = LeftFuelVolume.ToString(), ChartValue = LeftFuelVolume, Display = true });

                // short
                buffer = new byte[2];
                stream.Read(buffer, 0, buffer.Length);
                var RightFuelVolume = BitConverter.ToUInt16(buffer, 0);
                result.Data.Add("Объем топлива правый", new DataElement { OriginalValue = RightFuelVolume, DisplayValue = RightFuelVolume.ToString(), ChartValue = LeftFuelVolume, Display = true });

                // short
                buffer = new byte[2];
                stream.Read(buffer, 0, buffer.Length);
                var MiddleFuelVolume = BitConverter.ToUInt16(buffer, 0);
                result.Data.Add("Объем топлива средний", new DataElement { OriginalValue = MiddleFuelVolume, DisplayValue = MiddleFuelVolume.ToString(), ChartValue = MiddleFuelVolume, Display = true });

                // short
                buffer = new byte[2];
                stream.Read(buffer, 0, buffer.Length);
                var FuelMass = BitConverter.ToUInt16(buffer, 0);
                result.Data.Add("Масса  топлива", new DataElement { OriginalValue = FuelMass, DisplayValue = FuelMass.ToString(), ChartValue = FuelMass, Display = true });

                // byte
                var LeftTsDutTemperature = (sbyte)stream.ReadByte();
                result.Data.Add("Температура ТС ДУТ левая", new DataElement { OriginalValue = LeftTsDutTemperature, DisplayValue = LeftTsDutTemperature.ToString(), ChartValue = LeftTsDutTemperature, Display = true });

                // byte
                var RightTsDutTemperature = (sbyte)stream.ReadByte();
                result.Data.Add("Температура ТС ДУТ правая", new DataElement { OriginalValue = RightTsDutTemperature, DisplayValue = RightTsDutTemperature.ToString(), ChartValue = RightTsDutTemperature, Display = true });

                // int
                buffer = new byte[4];
                stream.Read(buffer, 0, buffer.Length);
                var Latitude = BitConverter.ToInt32(buffer, 0);
                result.Data.Add("Широта", new DataElement { OriginalValue = Latitude, DisplayValue = ParseCoordinate(Latitude), ChartValue = Latitude, Display = true });

                // int
                buffer = new byte[4];
                stream.Read(buffer, 0, buffer.Length);
                var Longitude = BitConverter.ToInt32(buffer, 0);
                result.Data.Add("Долгота", new DataElement { OriginalValue = Longitude, DisplayValue = ParseCoordinate(Longitude), ChartValue = Longitude, Display = true });


                // byte
                var FuelTemperature = (sbyte)stream.ReadByte();
                result.Data.Add("Температура топлива", new DataElement { OriginalValue = FuelTemperature, DisplayValue = FuelTemperature.ToString(), ChartValue = FuelTemperature, Display = true });

                // byte
                var FuelDensityCurrent = (short)((sbyte)stream.ReadByte() + 850);
                result.Data.Add("Плотность топлива текущая", new DataElement { OriginalValue = FuelDensityCurrent, DisplayValue = FuelDensityCurrent.ToString(), ChartValue = FuelDensityCurrent, Display = true });

                // byte
                var FuelDensityStandard = (short)((sbyte)stream.ReadByte() + 850);
                result.Data.Add("Плотность топлива при 20°С", new DataElement { OriginalValue = FuelDensityStandard, DisplayValue = FuelDensityStandard.ToString(), ChartValue = FuelDensityStandard, Display = true });

                // byte
                var OilCircuitTemperature = (sbyte)stream.ReadByte();
                result.Data.Add("Температура контура масла", new DataElement { OriginalValue = OilCircuitTemperature, DisplayValue = OilCircuitTemperature.ToString(), ChartValue = OilCircuitTemperature, Display = true });

                // byte
                var EnvironmentTemperature = (sbyte)stream.ReadByte();
                result.Data.Add("Температура окружающего воздуха", new DataElement { OriginalValue = EnvironmentTemperature, DisplayValue = EnvironmentTemperature.ToString(), ChartValue = EnvironmentTemperature, Display = true });

                // byte
                var UPSVoltage = (double)stream.ReadByte() / 10;
                result.Data.Add("Напряжение ИБП", new DataElement { OriginalValue = UPSVoltage, DisplayValue = UPSVoltage.ToString(), ChartValue = UPSVoltage, Display = true });

                // int
                buffer = new byte[4];
                stream.Read(buffer, 0, buffer.Length);
                var TabularNumber = BitConverter.ToInt32(buffer, 0);
                result.Data.Add("Табельный номер", new DataElement { OriginalValue = TabularNumber, DisplayValue = TabularNumber.ToString(), ChartValue = TabularNumber });

                // skip 2 bytes
                stream.ReadByte();
                stream.ReadByte();

                // short
                buffer = new byte[2];
                stream.Read(buffer, 0, buffer.Length);
                var TKCoefficient = BitConverter.ToInt16(buffer, 0);
                result.Data.Add("Коэффициент по ТК", new DataElement { OriginalValue = TKCoefficient, DisplayValue = TKCoefficient.ToString(), ChartValue = TKCoefficient, Display = true });

                // short
                buffer = new byte[2];
                stream.Read(buffer, 0, buffer.Length);
                var EquipmentAmount = BitConverter.ToInt16(buffer, 0);
                result.Data.Add("Объем экипировки", new DataElement { OriginalValue = EquipmentAmount, DisplayValue = EquipmentAmount.ToString(), ChartValue = EquipmentAmount, Display = true });

                // short
                buffer = new byte[2];
                stream.Read(buffer, 0, buffer.Length);
                var BIVersion = BitConverter.ToUInt16(buffer, 0);
                result.Data.Add("Версия БИ", new DataElement { OriginalValue = BIVersion, DisplayValue = BIVersion.ToString(), ChartValue = BIVersion });

                // short
                buffer = new byte[2];
                stream.Read(buffer, 0, buffer.Length);
                var LeftDUTOffset = (double)BitConverter.ToInt16(buffer, 0) /10;
                result.Data.Add("Смещение ДУТ левого", new DataElement { OriginalValue = LeftDUTOffset, DisplayValue = LeftDUTOffset.ToString(), ChartValue = LeftDUTOffset, Display = true });

                // short
                buffer = new byte[2];
                stream.Read(buffer, 0, buffer.Length);
                var RightDUTOffset = (double)BitConverter.ToInt16(buffer, 0) / 10;
                result.Data.Add("Смещение ДУТ правого", new DataElement { OriginalValue = RightDUTOffset, DisplayValue = RightDUTOffset.ToString(), ChartValue = RightDUTOffset, Display = true });

                // short
                buffer = new byte[2];
                stream.Read(buffer, 0, buffer.Length);
                var CurrentCoefficient = BitConverter.ToInt16(buffer, 0);
                result.Data.Add("Коэффициент по току", new DataElement { OriginalValue = CurrentCoefficient, DisplayValue = CurrentCoefficient.ToString(), ChartValue = CurrentCoefficient, Display = true });

                // short
                buffer = new byte[2];
                stream.Read(buffer, 0, buffer.Length);
                var VoltageCoefficient = BitConverter.ToInt16(buffer, 0);
                result.Data.Add("Коэффициент по напряжению", new DataElement { OriginalValue = VoltageCoefficient, DisplayValue = VoltageCoefficient.ToString(), ChartValue = VoltageCoefficient, Display = true });

                // byte
                var DieselSpeed = (byte)stream.ReadByte();
                result.Data.Add("Коэффициент по оборотам дизеля", new DataElement { OriginalValue = DieselSpeed, DisplayValue = DieselSpeed.ToString(), ChartValue = DieselSpeed, Display = true });

                // skip 2 bytes
                stream.ReadByte();
                stream.ReadByte();

                // byte
                var ColdWaterCircuitTemperature = (sbyte)stream.ReadByte();
                result.Data.Add("Температура воды холодного контура", new DataElement { OriginalValue = ColdWaterCircuitTemperature, DisplayValue = ColdWaterCircuitTemperature.ToString(), ChartValue = ColdWaterCircuitTemperature, Display = true });

                // int
                buffer = new byte[4];
                stream.Read(buffer, 0, buffer.Length);
                var MinuteRecordId = BitConverter.ToInt32(buffer, 0);
                result.Data.Add("ID минутной записи", new DataElement { OriginalValue = MinuteRecordId, DisplayValue = MinuteRecordId.ToString(), ChartValue = MinuteRecordId });

                // byte
                var MRKStatusFlags = (byte)stream.ReadByte();
                result.Data.Add("Флаги состояния МРК", new DataElement { OriginalValue = MRKStatusFlags, DisplayValue = MRKStatusFlags.ToString(), ChartValue = MRKStatusFlags });

                // byte
                var FuelDensityOnEquip = (short)((sbyte)stream.ReadByte() + 850);
                result.Data.Add("Плотность топлива при экипировке", new DataElement { OriginalValue = FuelDensityOnEquip, DisplayValue = FuelDensityOnEquip.ToString(), ChartValue = FuelDensityOnEquip, Display = true });


                // skip byte
                stream.ReadByte();

                for (int i = 0; i < result.SecondsBlock.Length; i++)
                {
                    result.SecondsBlock[i] = ParseSecondBlock(stream);
                }

                // short
                buffer = new byte[2];
                stream.Read(buffer, 0, buffer.Length);
                var CRC = BitConverter.ToInt16(buffer, 0);
                result.Data.Add("CRC", new DataElement { OriginalValue = CRC, DisplayValue = CRC.ToString(), ChartValue = CRC });

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

        

        private string ParseCoordinate(int coordinate)
        {
            string coordinateStr = coordinate.ToString();
            return $"{coordinateStr.Substring(0, 2)}.{coordinateStr.Substring(3,coordinateStr.Length - 3)}";
        }
    }
}