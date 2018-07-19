using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ParserNII.DataStructures
{
    public class DatFileParser : Parser
    {
        public override List<DataFile> Parse(byte[] fileBytes)
        {
            Dictionary<string, ConfigElement> datFileParams = Config.Instance.datFileParams.ToDictionary(d => d.name);

            List<DataFile> results = new List<DataFile>();
            List<byte[]> dataChunks = Split(fileBytes);

            for (int j = 0; j < dataChunks.Count; j++)
            {
                var dataChunk = dataChunks[j];
                int position = 0;
                var result = new DataFile();
                // 3 bytes
                byte[] buffer = new byte[3] {dataChunk[position++], dataChunk[position++], dataChunk[position++]};
                result.Data.Add("Три байта 0xEE", new DataElement { OriginalValue = (buffer[0], buffer[1], buffer[2]), DataParams = datFileParams["Три байта 0xEE"] });

                // byte
                var labelType = dataChunk[position++];
                result.Data.Add("Тип метки", new DataElement { OriginalValue = labelType, DisplayValue = labelType.ToString() });

                // int
                buffer = new[] { dataChunk[position++], dataChunk[position++], dataChunk[position++], dataChunk[position++] };
                var unixTime = BitConverter.ToUInt32(buffer, 0);
                result.Data.Add("Время в “UNIX” формате", new DataElement { OriginalValue = unixTime, DisplayValue = DateTimeOffset.FromUnixTimeSeconds(unixTime).AddHours(3).ToString("dd.MM.yyyy HH:mm:ss") });

                // byte
                var LocomotiveType = dataChunk[position++];
                result.Data.Add("Тип локомотива", new DataElement { OriginalValue = LocomotiveType, DisplayValue = LocomotiveType.ToString() });

                // short
                buffer = new[] { dataChunk[position++], dataChunk[position++] };
                var LocomotiveNumber = BitConverter.ToUInt16(buffer, 0);
                result.Data.Add("№ тепловоза", new DataElement { OriginalValue = LocomotiveNumber, DisplayValue = LocomotiveNumber.ToString() });


                // byte
                var LocomotiveSection = dataChunk[position++];
                result.Data.Add("Секция локомотива", new DataElement { OriginalValue = LocomotiveSection, DisplayValue = LocomotiveSection.ToString() });


                // short (enum)
                buffer = new[] { dataChunk[position++], dataChunk[position++] };
                var MinuteByteParametrs = (FirstMinuteByteParams)BitConverter.ToInt16(buffer, 0);


                // byte
                var CoolingCircuitTemperature = (sbyte)dataChunk[position++];
                result.Data.Add("Температура контура охлаждения", new DataElement { OriginalValue = CoolingCircuitTemperature, DisplayValue = CoolingCircuitTemperature.ToString(), ChartValue = CoolingCircuitTemperature, Display = true });

                // short
                buffer = new[] { dataChunk[position++], dataChunk[position++] };
                var LeftFuelVolume = BitConverter.ToUInt16(buffer, 0);
                result.Data.Add("Объем топлива левый", new DataElement { OriginalValue = LeftFuelVolume, DisplayValue = LeftFuelVolume.ToString(), ChartValue = LeftFuelVolume, Display = true });

                // short
                buffer = new[] { dataChunk[position++], dataChunk[position++] };
                var RightFuelVolume = BitConverter.ToUInt16(buffer, 0);
                result.Data.Add("Объем топлива правый", new DataElement { OriginalValue = RightFuelVolume, DisplayValue = RightFuelVolume.ToString(), ChartValue = RightFuelVolume, Display = true });

                // short
                buffer = new[] { dataChunk[position++], dataChunk[position++] };
                var MiddleFuelVolume = BitConverter.ToUInt16(buffer, 0);
                result.Data.Add("Объем топлива средний", new DataElement { OriginalValue = MiddleFuelVolume, DisplayValue = MiddleFuelVolume.ToString(), ChartValue = MiddleFuelVolume, Display = true });

                // short
                buffer = new[] { dataChunk[position++], dataChunk[position++] };
                var FuelMass = BitConverter.ToUInt16(buffer, 0);
                result.Data.Add("Масса  топлива", new DataElement { OriginalValue = FuelMass, DisplayValue = FuelMass.ToString(), ChartValue = FuelMass, Display = true });

                // byte
                var LeftTsDutTemperature = (sbyte)dataChunk[position++];
                result.Data.Add("Температура ТС ДУТ левая", new DataElement { OriginalValue = LeftTsDutTemperature, DisplayValue = LeftTsDutTemperature.ToString(), ChartValue = LeftTsDutTemperature, Display = true });

                // byte
                var RightTsDutTemperature = (sbyte)dataChunk[position++];
                result.Data.Add("Температура ТС ДУТ правая", new DataElement { OriginalValue = RightTsDutTemperature, DisplayValue = RightTsDutTemperature.ToString(), ChartValue = RightTsDutTemperature, Display = true });

                // int
                buffer = new[] { dataChunk[position++], dataChunk[position++], dataChunk[position++], dataChunk[position++] };
                var Latitude = BitConverter.ToInt32(buffer, 0);
                result.Data.Add("Широта", new DataElement { OriginalValue = Latitude, DisplayValue = ParseCoordinate(Latitude), ChartValue = Latitude, Display = true });

                // int
                buffer = new[] { dataChunk[position++], dataChunk[position++], dataChunk[position++], dataChunk[position++] };
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
                buffer = new[] { dataChunk[position++], dataChunk[position++], dataChunk[position++], dataChunk[position++] };
                var tabularNumber = BitConverter.ToInt32(buffer, 0);
                result.Data.Add("Табельный номер", new DataElement { OriginalValue = tabularNumber, DisplayValue = tabularNumber.ToString(), ChartValue = tabularNumber });

                // skip 2 bytes
                position+=2;

                // short
                buffer = new[] { dataChunk[position++], dataChunk[position++] };
                var TKCoefficient = BitConverter.ToInt16(buffer, 0);
                result.Data.Add("Коэффициент по ТК", new DataElement { OriginalValue = TKCoefficient, DisplayValue = TKCoefficient.ToString(), ChartValue = TKCoefficient, Display = true });

                // short
                buffer = new[] { dataChunk[position++], dataChunk[position++] };
                var equipmentAmount = BitConverter.ToInt16(buffer, 0);
                result.Data.Add("Объем экипировки", new DataElement { OriginalValue = equipmentAmount, DisplayValue = equipmentAmount.ToString(), ChartValue = equipmentAmount, Display = true });

                // short
                buffer = new[] { dataChunk[position++], dataChunk[position++] };
                var BIVersion = BitConverter.ToUInt16(buffer, 0);
                result.Data.Add("Версия БИ", new DataElement { OriginalValue = BIVersion, DisplayValue = BIVersion.ToString(), ChartValue = BIVersion });

                // short
                buffer = new[] { dataChunk[position++], dataChunk[position++] };
                var leftDUTOffset = (double)BitConverter.ToInt16(buffer, 0) / 10;
                result.Data.Add("Смещение ДУТ левого", new DataElement { OriginalValue = leftDUTOffset, DisplayValue = leftDUTOffset.ToString(), ChartValue = leftDUTOffset, Display = true });

                // short
                buffer = new[] { dataChunk[position++], dataChunk[position++] };
                var rightDutOffset = (double)BitConverter.ToInt16(buffer, 0) / 10;
                result.Data.Add("Смещение ДУТ правого", new DataElement { OriginalValue = rightDutOffset, DisplayValue = rightDutOffset.ToString(), ChartValue = rightDutOffset, Display = true });

                // short
                buffer = new[] { dataChunk[position++], dataChunk[position++] };
                var currentCoefficient = BitConverter.ToInt16(buffer, 0);
                result.Data.Add("Коэффициент по току", new DataElement { OriginalValue = currentCoefficient, DisplayValue = currentCoefficient.ToString(), ChartValue = currentCoefficient, Display = true });

                // short
                buffer = new[] { dataChunk[position++], dataChunk[position++] };
                var voltageCoefficient = BitConverter.ToInt16(buffer, 0);
                result.Data.Add("Коэффициент по напряжению", new DataElement { OriginalValue = voltageCoefficient, DisplayValue = voltageCoefficient.ToString(), ChartValue = voltageCoefficient, Display = true });

                // byte
                var dieselSpeed = (byte)dataChunk[position++];
                result.Data.Add("Коэффициент по оборотам дизеля", new DataElement { OriginalValue = dieselSpeed, DisplayValue = dieselSpeed.ToString(), ChartValue = dieselSpeed, Display = true });

                // skip 2 bytes
                position+=2;

                // byte
                var coldWaterCircuitTemperature = (sbyte)dataChunk[position++];
                result.Data.Add("Температура воды холодного контура", new DataElement { OriginalValue = coldWaterCircuitTemperature, DisplayValue = coldWaterCircuitTemperature.ToString(), ChartValue = coldWaterCircuitTemperature, Display = true });

                // int
                buffer = new[] { dataChunk[position++], dataChunk[position++], dataChunk[position++], dataChunk[position++] };
                var minuteRecordId = BitConverter.ToInt32(buffer, 0);
                result.Data.Add("ID минутной записи", new DataElement { OriginalValue = minuteRecordId, DisplayValue = minuteRecordId.ToString(), ChartValue = minuteRecordId });

                // byte
                var MRKStatusFlags = (byte)dataChunk[position++];
                result.Data.Add("Флаги состояния МРК", new DataElement { OriginalValue = MRKStatusFlags, DisplayValue = MRKStatusFlags.ToString(), ChartValue = MRKStatusFlags });

                // byte
                var fuelDensityOnEquip = (short)((sbyte)dataChunk[position++] + 850);
                result.Data.Add("Плотность топлива при экипировке", new DataElement { OriginalValue = fuelDensityOnEquip, DisplayValue = fuelDensityOnEquip.ToString(), ChartValue = fuelDensityOnEquip, Display = true });


                // skip byte
                position++;

                // short
                buffer = new[] { dataChunk[dataChunk.Length - 2], dataChunk[dataChunk.Length - 1] };
                var CRC = BitConverter.ToInt16(buffer, 0);
                result.Data.Add("CRC", new DataElement { OriginalValue = CRC, DisplayValue = CRC.ToString(), ChartValue = CRC });

                var secondsResult = new DataFile[20];
                for (int i = 0; i < 20; i++)
                {
                    secondsResult[i] = result.Clone();
                    uint timeWithSeconds = unixTime + (uint) (i * 3);
                    secondsResult[i].Data["Время в “UNIX” формате"] = new DataElement { OriginalValue = timeWithSeconds, DisplayValue = DateTimeOffset.FromUnixTimeSeconds(timeWithSeconds).AddHours(3).ToString("dd.MM.yyyy HH:mm:ss") };

                    // short (enum)
                    buffer = new[] { dataChunk[position++], dataChunk[position++] };
                    var secondsByteParametrs = (FirstSecondByteParams)BitConverter.ToInt16(buffer, 0);

                    // short
                    buffer = new[] { dataChunk[position++], dataChunk[position++] };
                    var dieselTurneover = BitConverter.ToInt16(buffer, 0);
                    secondsResult[i].Data.Add("Обороты дизеля", new DataElement { OriginalValue = dieselTurneover, DisplayValue = dieselTurneover.ToString(), ChartValue = dieselTurneover, Display = true });

                    // short
                    buffer = new[] { dataChunk[position++], dataChunk[position++] };
                    var turbochanrgerTurnovers = BitConverter.ToInt16(buffer, 0);
                    secondsResult[i].Data.Add("Обороты турбокомпрессора", new DataElement { OriginalValue = turbochanrgerTurnovers, DisplayValue = turbochanrgerTurnovers.ToString(), ChartValue = turbochanrgerTurnovers, Display = true });

                    // short
                    buffer = new[] { dataChunk[position++], dataChunk[position++] };
                    var generatorPower = BitConverter.ToInt16(buffer, 0);
                    secondsResult[i].Data.Add("Мощность генератора", new DataElement { OriginalValue = generatorPower, DisplayValue = generatorPower.ToString(), ChartValue = generatorPower, Display = true });

                    // short
                    buffer = new[] { dataChunk[position++], dataChunk[position++] };
                    var generatorCurrent = BitConverter.ToInt16(buffer, 0);
                    secondsResult[i].Data.Add("Ток генератора", new DataElement { OriginalValue = generatorCurrent, DisplayValue = generatorCurrent.ToString(), ChartValue = generatorCurrent, Display = true });

                    // short
                    buffer = new[] { dataChunk[position++], dataChunk[position++] };
                    var generatorVoltage = BitConverter.ToInt16(buffer, 0);
                    secondsResult[i].Data.Add("Напряжение генератора", new DataElement { OriginalValue = generatorVoltage, DisplayValue = generatorVoltage.ToString(), ChartValue = generatorVoltage, Display = true });

                    // short
                    buffer = new[] { dataChunk[position++], dataChunk[position++] };
                    var speed = BitConverter.ToInt16(buffer, 0);
                    secondsResult[i].Data.Add("Скорость", new DataElement { OriginalValue = speed, DisplayValue = speed.ToString(), ChartValue = speed, Display = true });

                    // byte
                    var boostPressure = dataChunk[position++];
                    secondsResult[i].Data.Add("Давление наддува", new DataElement { OriginalValue = boostPressure, DisplayValue = boostPressure.ToString(), ChartValue = boostPressure, Display = true });

                    // short
                    buffer = new[] { dataChunk[position++], dataChunk[position++] };
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

                if (j > 0 && (uint)secondsResult[0].Data["Время в “UNIX” формате"].OriginalValue - (uint)results.Last().Data["Время в “UNIX” формате"].OriginalValue > 63)
                {
                    var emptyResult = secondsResult[0].Clone();
                    var dataKeys = emptyResult.Data.Keys.ToArray();
                    for (int k = 0; k < dataKeys.Length; k++)
                    {
                        if (emptyResult.Data[dataKeys[k]].Display)
                        {
                            emptyResult.Data[dataKeys[k]] = new DataElement() { OriginalValue = null, DisplayValue = "Разрыв", ChartValue = double.NaN, Display = true };
                        }
                    }

                    results.Add(emptyResult);
                }

                results.AddRange(secondsResult);
            }
            return results;
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