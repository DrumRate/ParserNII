using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace ParserNII.DataStructures
{
    public class DatFileParser : Parser
    {
        public override List<DataFile> Parse(Stream stream)
        {
            List<DataFile> results = new List<DataFile>();
            byte[] streamBuffer = new byte[stream.Length];
            stream.Read(streamBuffer, 0, (int)stream.Length);
            List<byte[]> dataChunks = Split(streamBuffer);

            for (int j = 0; j < dataChunks.Count; j++)
            {
                var dataChunk = dataChunks[j];
                int position = 0;
                var result = new DataFile();
                // 3 bytes
                byte[] buffer = new byte[3];
                stream.Read(buffer, 0, buffer.Length);
                result.Data.Add("Три байта 0xEE", new DataElement { OriginalValue = (dataChunk[position++], dataChunk[position++], dataChunk[position++]) });

                // byte
                var labelType = dataChunk[position++];
                result.Data.Add("Тип метки", new DataElement { OriginalValue = labelType, DisplayValue = labelType.ToString() });

                // int
                buffer = new byte[] { dataChunk[position++], dataChunk[position++], dataChunk[position++], dataChunk[position++] };
                // stream.Read(buffer, 0, buffer.Length);
                var unixTime = BitConverter.ToUInt32(buffer, 0);
                result.Data.Add("Время в “UNIX” формате", new DataElement { OriginalValue = unixTime, DisplayValue = DateTimeOffset.FromUnixTimeSeconds(unixTime).AddHours(3).ToString("dd.MM.yyyy HH:mm:ss") });

                // byte
                var LocomotiveType = dataChunk[position++];
                result.Data.Add("Тип локомотива", new DataElement { OriginalValue = LocomotiveType, DisplayValue = LocomotiveType.ToString() });

                // short
                buffer = new byte[] { dataChunk[position++], dataChunk[position++] };
                // stream.Read(buffer, 0, buffer.Length);
                var LocomotiveNumber = BitConverter.ToUInt16(buffer, 0);
                result.Data.Add("№ тепловоза", new DataElement { OriginalValue = LocomotiveNumber, DisplayValue = LocomotiveNumber.ToString() });


                // byte
                var LocomotiveSection = dataChunk[position++];
                result.Data.Add("Секция локомотива", new DataElement { OriginalValue = LocomotiveSection, DisplayValue = LocomotiveSection.ToString() });


                // short (enum)
                buffer = new byte[] { dataChunk[position++], dataChunk[position++] };
                //stream.Read(buffer, 0, buffer.Length);
                var MinuteByteParametrs = (FirstMinuteByteParams)BitConverter.ToInt16(buffer, 0);


                // byte
                var CoolingCircuitTemperature = (sbyte)dataChunk[position++];
                result.Data.Add("Температура контура охлаждения", new DataElement { OriginalValue = CoolingCircuitTemperature, DisplayValue = CoolingCircuitTemperature.ToString(), ChartValue = CoolingCircuitTemperature, Display = true });

                // short
                buffer = new byte[] { dataChunk[position++], dataChunk[position++] };
                //stream.Read(buffer, 0, buffer.Length);
                var LeftFuelVolume = BitConverter.ToUInt16(buffer, 0);
                result.Data.Add("Объем топлива левый", new DataElement { OriginalValue = LeftFuelVolume, DisplayValue = LeftFuelVolume.ToString(), ChartValue = LeftFuelVolume, Display = true });

                // short
                buffer = new byte[] { dataChunk[position++], dataChunk[position++] };
                //stream.Read(buffer, 0, buffer.Length);
                var RightFuelVolume = BitConverter.ToUInt16(buffer, 0);
                result.Data.Add("Объем топлива правый", new DataElement { OriginalValue = RightFuelVolume, DisplayValue = RightFuelVolume.ToString(), ChartValue = RightFuelVolume, Display = true });

                // short
                buffer = new byte[] { dataChunk[position++], dataChunk[position++] };
                // stream.Read(buffer, 0, buffer.Length);
                var MiddleFuelVolume = BitConverter.ToUInt16(buffer, 0);
                result.Data.Add("Объем топлива средний", new DataElement { OriginalValue = MiddleFuelVolume, DisplayValue = MiddleFuelVolume.ToString(), ChartValue = MiddleFuelVolume, Display = true });

                // short
                buffer = new byte[] { dataChunk[position++], dataChunk[position++] };
                //stream.Read(buffer, 0, buffer.Length);
                var FuelMass = BitConverter.ToUInt16(buffer, 0);
                result.Data.Add("Масса  топлива", new DataElement { OriginalValue = FuelMass, DisplayValue = FuelMass.ToString(), ChartValue = FuelMass, Display = true });

                // byte
                var LeftTsDutTemperature = (sbyte)dataChunk[position++];
                result.Data.Add("Температура ТС ДУТ левая", new DataElement { OriginalValue = LeftTsDutTemperature, DisplayValue = LeftTsDutTemperature.ToString(), ChartValue = LeftTsDutTemperature, Display = true });

                // byte
                var RightTsDutTemperature = (sbyte)dataChunk[position++];
                result.Data.Add("Температура ТС ДУТ правая", new DataElement { OriginalValue = RightTsDutTemperature, DisplayValue = RightTsDutTemperature.ToString(), ChartValue = RightTsDutTemperature, Display = true });

                // int
                buffer = new byte[] { dataChunk[position++], dataChunk[position++], dataChunk[position++], dataChunk[position++] };
                //stream.Read(buffer, 0, buffer.Length);
                var Latitude = BitConverter.ToInt32(buffer, 0);
                result.Data.Add("Широта", new DataElement { OriginalValue = Latitude, DisplayValue = ParseCoordinate(Latitude), ChartValue = Latitude, Display = true });

                // int
                buffer = new byte[] { dataChunk[position++], dataChunk[position++], dataChunk[position++], dataChunk[position++] };
                //stream.Read(buffer, 0, buffer.Length);
                var Longitude = BitConverter.ToInt32(buffer, 0);
                result.Data.Add("Долгота", new DataElement { OriginalValue = Longitude, DisplayValue = ParseCoordinate(Longitude), ChartValue = Longitude, Display = true });


                // byte
                var FuelTemperature = (sbyte)dataChunk[position++];
                result.Data.Add("Температура топлива", new DataElement { OriginalValue = FuelTemperature, DisplayValue = FuelTemperature.ToString(), ChartValue = FuelTemperature, Display = true });

                // byte
                var FuelDensityCurrent = (short)((sbyte)dataChunk[position++] + 850);
                result.Data.Add("Плотность топлива текущая", new DataElement { OriginalValue = FuelDensityCurrent, DisplayValue = FuelDensityCurrent.ToString(), ChartValue = FuelDensityCurrent, Display = true });

                // byte
                var FuelDensityStandard = (short)((sbyte)dataChunk[position++] + 850);
                result.Data.Add("Плотность топлива при 20°С", new DataElement { OriginalValue = FuelDensityStandard, DisplayValue = FuelDensityStandard.ToString(), ChartValue = FuelDensityStandard, Display = true });

                // byte
                var OilCircuitTemperature = (sbyte)dataChunk[position++];
                result.Data.Add("Температура контура масла", new DataElement { OriginalValue = OilCircuitTemperature, DisplayValue = OilCircuitTemperature.ToString(), ChartValue = OilCircuitTemperature, Display = true });

                // byte
                var EnvironmentTemperature = (sbyte)dataChunk[position++];
                result.Data.Add("Температура окружающего воздуха", new DataElement { OriginalValue = EnvironmentTemperature, DisplayValue = EnvironmentTemperature.ToString(), ChartValue = EnvironmentTemperature, Display = true });

                // byte
                var UPSVoltage = (double)dataChunk[position++] / 10;
                result.Data.Add("Напряжение ИБП", new DataElement { OriginalValue = UPSVoltage, DisplayValue = UPSVoltage.ToString(), ChartValue = UPSVoltage, Display = true });

                // int
                buffer = new byte[] { dataChunk[position++], dataChunk[position++], dataChunk[position++], dataChunk[position++] };
                //stream.Read(buffer, 0, buffer.Length);
                var TabularNumber = BitConverter.ToInt32(buffer, 0);
                result.Data.Add("Табельный номер", new DataElement { OriginalValue = TabularNumber, DisplayValue = TabularNumber.ToString(), ChartValue = TabularNumber });

                // skip 2 bytes
                //stream.ReadByte();
                //stream.ReadByte();
                position++;
                position++;

                // short
                buffer = new byte[] { dataChunk[position++], dataChunk[position++] };
                //stream.Read(buffer, 0, buffer.Length);
                var TKCoefficient = BitConverter.ToInt16(buffer, 0);
                result.Data.Add("Коэффициент по ТК", new DataElement { OriginalValue = TKCoefficient, DisplayValue = TKCoefficient.ToString(), ChartValue = TKCoefficient, Display = true });

                // short
                buffer = new byte[] { dataChunk[position++], dataChunk[position++] };
                //stream.Read(buffer, 0, buffer.Length);
                var EquipmentAmount = BitConverter.ToInt16(buffer, 0);
                result.Data.Add("Объем экипировки", new DataElement { OriginalValue = EquipmentAmount, DisplayValue = EquipmentAmount.ToString(), ChartValue = EquipmentAmount, Display = true });

                // short
                buffer = new byte[] { dataChunk[position++], dataChunk[position++] };
                //stream.Read(buffer, 0, buffer.Length);
                var BIVersion = BitConverter.ToUInt16(buffer, 0);
                result.Data.Add("Версия БИ", new DataElement { OriginalValue = BIVersion, DisplayValue = BIVersion.ToString(), ChartValue = BIVersion });

                // short
                buffer = new byte[] { dataChunk[position++], dataChunk[position++] };
                //stream.Read(buffer, 0, buffer.Length);
                var LeftDUTOffset = (double)BitConverter.ToInt16(buffer, 0) / 10;
                result.Data.Add("Смещение ДУТ левого", new DataElement { OriginalValue = LeftDUTOffset, DisplayValue = LeftDUTOffset.ToString(), ChartValue = LeftDUTOffset, Display = true });

                // short
                buffer = new byte[] { dataChunk[position++], dataChunk[position++] };
                //stream.Read(buffer, 0, buffer.Length);
                var RightDUTOffset = (double)BitConverter.ToInt16(buffer, 0) / 10;
                result.Data.Add("Смещение ДУТ правого", new DataElement { OriginalValue = RightDUTOffset, DisplayValue = RightDUTOffset.ToString(), ChartValue = RightDUTOffset, Display = true });

                // short
                buffer = new byte[] { dataChunk[position++], dataChunk[position++] };
                //stream.Read(buffer, 0, buffer.Length);
                var CurrentCoefficient = BitConverter.ToInt16(buffer, 0);
                result.Data.Add("Коэффициент по току", new DataElement { OriginalValue = CurrentCoefficient, DisplayValue = CurrentCoefficient.ToString(), ChartValue = CurrentCoefficient, Display = true });

                // short
                buffer = new byte[] { dataChunk[position++], dataChunk[position++] };
                //stream.Read(buffer, 0, buffer.Length);
                var VoltageCoefficient = BitConverter.ToInt16(buffer, 0);
                result.Data.Add("Коэффициент по напряжению", new DataElement { OriginalValue = VoltageCoefficient, DisplayValue = VoltageCoefficient.ToString(), ChartValue = VoltageCoefficient, Display = true });

                // byte
                var DieselSpeed = (byte)dataChunk[position++];
                result.Data.Add("Коэффициент по оборотам дизеля", new DataElement { OriginalValue = DieselSpeed, DisplayValue = DieselSpeed.ToString(), ChartValue = DieselSpeed, Display = true });

                // skip 2 bytes
                //stream.ReadByte();
                //stream.ReadByte();
                position++;
                position++;

                // byte
                var ColdWaterCircuitTemperature = (sbyte)dataChunk[position++];
                result.Data.Add("Температура воды холодного контура", new DataElement { OriginalValue = ColdWaterCircuitTemperature, DisplayValue = ColdWaterCircuitTemperature.ToString(), ChartValue = ColdWaterCircuitTemperature, Display = true });

                // int
                buffer = new byte[] { dataChunk[position++], dataChunk[position++], dataChunk[position++], dataChunk[position++] };
                //stream.Read(buffer, 0, buffer.Length);
                var MinuteRecordId = BitConverter.ToInt32(buffer, 0);
                result.Data.Add("ID минутной записи", new DataElement { OriginalValue = MinuteRecordId, DisplayValue = MinuteRecordId.ToString(), ChartValue = MinuteRecordId });

                // byte
                var MRKStatusFlags = (byte)dataChunk[position++];
                result.Data.Add("Флаги состояния МРК", new DataElement { OriginalValue = MRKStatusFlags, DisplayValue = MRKStatusFlags.ToString(), ChartValue = MRKStatusFlags });

                // byte
                var FuelDensityOnEquip = (short)((sbyte)dataChunk[position++] + 850);
                result.Data.Add("Плотность топлива при экипировке", new DataElement { OriginalValue = FuelDensityOnEquip, DisplayValue = FuelDensityOnEquip.ToString(), ChartValue = FuelDensityOnEquip, Display = true });


                // skip byte
                //stream.ReadByte();
                position++;

                // short
                buffer = new byte[] { dataChunk[dataChunk.Length - 2], dataChunk[dataChunk.Length - 1] };
                stream.Read(buffer, 0, buffer.Length);
                var CRC = BitConverter.ToInt16(buffer, 0);
                result.Data.Add("CRC", new DataElement { OriginalValue = CRC, DisplayValue = CRC.ToString(), ChartValue = CRC });

                var secondsResult = new DataFile[20];
                for (int i = 0; i < 20; i++)
                {
                    secondsResult[i] = result.Clone();
                    uint timeWithSeconds = unixTime + (uint) (i * 3);
                    secondsResult[i].Data["Время в “UNIX” формате"] = new DataElement { OriginalValue = timeWithSeconds, DisplayValue = DateTimeOffset.FromUnixTimeSeconds(timeWithSeconds).AddHours(3).ToString("dd.MM.yyyy HH:mm:ss") };

                    // short (enum)
                    buffer = new byte[] { dataChunk[position++], dataChunk[position++] };
                    //stream.Read(buffer, 0, buffer.Length);
                    var secondsByteParametrs = (FirstSecondByteParams)BitConverter.ToInt16(buffer, 0);

                    // short
                    buffer = new byte[] { dataChunk[position++], dataChunk[position++] };
                    //stream.Read(buffer, 0, buffer.Length);
                    var dieselTurneover = BitConverter.ToInt16(buffer, 0);
                    secondsResult[i].Data.Add("Обороты дизеля", new DataElement { OriginalValue = dieselTurneover, DisplayValue = dieselTurneover.ToString(), ChartValue = dieselTurneover, Display = true });

                    // short
                    buffer = new byte[] { dataChunk[position++], dataChunk[position++] };
                    //stream.Read(buffer, 0, buffer.Length);
                    var turbochanrgerTurnovers = BitConverter.ToInt16(buffer, 0);
                    secondsResult[i].Data.Add("Обороты турбокомпрессора", new DataElement { OriginalValue = turbochanrgerTurnovers, DisplayValue = turbochanrgerTurnovers.ToString(), ChartValue = turbochanrgerTurnovers, Display = true });

                    // short
                    buffer = new byte[2] { dataChunk[position++], dataChunk[position++] };
                    //stream.Read(buffer, 0, buffer.Length);
                    var generatorPower = BitConverter.ToInt16(buffer, 0);
                    secondsResult[i].Data.Add("Мощность генератора", new DataElement { OriginalValue = generatorPower, DisplayValue = generatorPower.ToString(), ChartValue = generatorPower, Display = true });

                    // short
                    buffer = new byte[2] { dataChunk[position++], dataChunk[position++] };
                    // stream.Read(buffer, 0, buffer.Length);
                    var generatorCurrent = BitConverter.ToInt16(buffer, 0);
                    secondsResult[i].Data.Add("Ток генератора", new DataElement { OriginalValue = generatorCurrent, DisplayValue = generatorCurrent.ToString(), ChartValue = generatorCurrent, Display = true });

                    // short
                    buffer = new byte[2] { dataChunk[position++], dataChunk[position++] };
                    //stream.Read(buffer, 0, buffer.Length);
                    var generatorVoltage = BitConverter.ToInt16(buffer, 0);
                    secondsResult[i].Data.Add("Напряжение генератора", new DataElement { OriginalValue = generatorVoltage, DisplayValue = generatorVoltage.ToString(), ChartValue = generatorVoltage, Display = true });

                    // short
                    buffer = new byte[2] { dataChunk[position++], dataChunk[position++] };
                    //stream.Read(buffer, 0, buffer.Length);
                    var speed = BitConverter.ToInt16(buffer, 0);
                    secondsResult[i].Data.Add("Скорость", new DataElement { OriginalValue = speed, DisplayValue = speed.ToString(), ChartValue = speed, Display = true });

                    // byte
                    var boostPressure = dataChunk[position++];
                    secondsResult[i].Data.Add("Давление наддува", new DataElement { OriginalValue = boostPressure, DisplayValue = boostPressure.ToString(), ChartValue = boostPressure, Display = true });

                    // short
                    buffer = new byte[2] { dataChunk[position++], dataChunk[position++] };
                    //stream.Read(buffer, 0, buffer.Length);
                    var averageFuelVolume = BitConverter.ToInt16(buffer, 0);
                    secondsResult[i].Data.Add("Объем топлива средний 2", new DataElement { OriginalValue = averageFuelVolume, DisplayValue = averageFuelVolume.ToString(), ChartValue = averageFuelVolume, Display = true });

                    // byte
                    var fuelPressure = dataChunk[position++];
                    secondsResult[i].Data.Add("Давление топлива", new DataElement { OriginalValue = fuelPressure, DisplayValue = fuelPressure.ToString(), ChartValue = fuelPressure, Display = true });

                    // byte
                    var oilPressure = dataChunk[position++];
                    secondsResult[i].Data.Add("Давление масла", new DataElement { OriginalValue = oilPressure, DisplayValue = oilPressure.ToString(), ChartValue = oilPressure, Display = true });

                    // byte
                    var positionControllerDriver = dataChunk[position++];
                    secondsResult[i].Data.Add("Позиция контроллера машиниста", new DataElement { OriginalValue = positionControllerDriver, DisplayValue = positionControllerDriver.ToString(), ChartValue = positionControllerDriver, Display = true });

                    // byte
                    var oilPressureWithFilter = dataChunk[position++];
                    secondsResult[i].Data.Add("Давление масла с фильтром", new DataElement { OriginalValue = oilPressureWithFilter, DisplayValue = oilPressureWithFilter.ToString(), ChartValue = oilPressureWithFilter, Display = true });

                    // skip byte
                    position++;
                    
                }

                results.AddRange(secondsResult);
            }
            return results;
        }

        private SecondBlock ParseSecondBlock(byte[] dataChunk, int position)
        {
            SecondBlock result = new SecondBlock();

            byte[] buffer;

            // short (enum)
            buffer = new byte[] { dataChunk[position++], dataChunk[position++] };
            //stream.Read(buffer, 0, buffer.Length);
            result.BitParametrsSecond = (FirstSecondByteParams)BitConverter.ToInt16(buffer, 0);

            // short
            buffer = new byte[] { dataChunk[position++], dataChunk[position++] };
            //stream.Read(buffer, 0, buffer.Length);
            result.TurnoversDisel = BitConverter.ToInt16(buffer, 0);

            // short
            buffer = new byte[] { dataChunk[position++], dataChunk[position++] };
            //stream.Read(buffer, 0, buffer.Length);
            result.TurnoversTurbochanrger = BitConverter.ToInt16(buffer, 0);

            // short
            buffer = new byte[2] { dataChunk[position++], dataChunk[position++] };
            //stream.Read(buffer, 0, buffer.Length);
            result.GeneratorPower = BitConverter.ToInt16(buffer, 0);

            // short
            buffer = new byte[2] { dataChunk[position++], dataChunk[position++] };
            // stream.Read(buffer, 0, buffer.Length);
            result.GeneratorCurrent = BitConverter.ToInt16(buffer, 0);

            // short
            buffer = new byte[2] { dataChunk[position++], dataChunk[position++] };
            //stream.Read(buffer, 0, buffer.Length);
            result.VoltageGenerator = BitConverter.ToInt16(buffer, 0);

            // short
            buffer = new byte[2] { dataChunk[position++], dataChunk[position++] };
            //stream.Read(buffer, 0, buffer.Length);
            result.Speed = BitConverter.ToInt16(buffer, 0);

            // byte
            result.BoostPressure = dataChunk[position++];

            // short
            buffer = new byte[2] { dataChunk[position++], dataChunk[position++] };
            //stream.Read(buffer, 0, buffer.Length);
            result.AverageFuelVolume = BitConverter.ToInt16(buffer, 0);

            // byte
            result.FuelPressure = dataChunk[position++];

            // byte
            result.OilPressure = dataChunk[position++];

            // byte
            result.PositionControllerDriver = dataChunk[position++];

            // byte
            result.OilPressureWithFilter = dataChunk[position++];

            // skip byte
            position++;

            return result;
        }



        private string ParseCoordinate(int coordinate)
        {
            double coordDouble = coordinate * 0.000001000;
            return coordDouble.ToString();
        }

        private List<byte[]> Split(byte[] dataStream)
        {
            var result = new List<byte[]>();
            var dataBlock = new List<byte>();
            int i = 0;
            while (!(dataStream[i] == 0xEE && dataStream[i + 1] == 0xEE && dataStream[i + 2] == 0xEE) && i < dataStream.Length - 3)
            {
                i++;
            }

            for (; i < dataStream.Length - 3; i++)
            {
                if (dataStream[i] == 0xEE && dataStream[i + 1] == 0xEE && dataStream[i + 2] == 0xEE)
                {
                    if (dataBlock.Count == 512)
                    {
                        result.Add(dataBlock.ToArray());
                    }
                    else
                    {
                        dataBlock = new List<byte>();
                    }
                    dataBlock.Add(dataStream[i]);
                    dataBlock.Add(dataStream[i + 1]);
                    dataBlock.Add(dataStream[i + 2]);
                    i += 2;
                    continue;
                }

                dataBlock.Add(dataStream[i]);
            }

            return result;
        }
    }
}