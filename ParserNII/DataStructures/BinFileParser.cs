using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ParserNII.DataStructures
{
    public class BinFileParser : Parser
    {
        private Dictionary<int, string> uidsNames = new Dictionary<int, string>
            {
                { 25, "Табельный номер"},
                { 29, "Скорость по GPS"},
                { 21, "Широта"},
                { 22, "Долгота"},
                { 23, "Обороты дизеля"},
                { 34, "Коэффициен по оборотам"},
                { 15, "Давление в топливной системе"},
                { 16, "Давление в масляной системе"},
                { 102, "Давление в масляной системе 2"},
                { 30, "Давление наддувочного воздуха"},
                { 24, "Обороты турбокомпрессора"},
                { 9, "Температура воды на выходе дизеля"},
                { 50, "Температура масла на выходе масла"},
                { 10, "Сила тока главного генератора"},
                { 32, "эффициент по току"},
                { 31, "Напряжение главного генератора"},
                {  33, "Коэффициент по напряжению"},
                { 28, "Мощность главного генератора" },
                { 27, "Позиция контроллера машиниста" },
                { 67, "ДУТ поплавковый"},
                { 11, "Объем топлива левый" },
                { 12, "Объем топлива правый" },
                { 61, "Объем топлива секундный" },
                { 13, "Объем топлива" },
                { 42, "Плотность топлива"},
                { 3102, "Температура топлива левая" },
                { 3101, "Температура топлива правая" },
                { 2, "Температура топлива" },
                { 14, "Масса топлива" },
                { 75, "Объем экипированного топлива"},
                { 122, "Признак экипировки" },
                { 3104, "Температура окружающего воздуха"},
                { 151, "Давление в тормозной магистрали"},
                { 158, "ЭПК"},
                { 3103, "Код позиции"},
                { 159, "Код АЛСН"},
                { 156, "Напряжение АКБ"},
                { 155, "Ток зар./разр. АКБ"},
                { 53, "Контрольный режим"},
                { 41, "Версия БИ"},
                { 58, "Связь с РМ РКД" },
                { 56, "Связь с ДУТ левый" },
                { 57, "Связь с ДУТ правый" },
                { 54, "Связь с ДМ" },
                { 3105, "Связь с БДП" },
                { 181, "Данные GPS валидны" },
                { 117, "Подключение к ЕСМБС" },
                { 3106, "Связь с МЭК" },
                { 3192, "Работа САЗДТ" }
            };

        public override List<DataFile> Parse(Stream stream)
        {
            List<BinFile> result = new List<BinFile>();
            double timeNowEpoch = Convert.ToInt64(DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds);
            using (BinaryReader reader = new BinaryReader(stream))
            {
                while (stream.Position < stream.Length)
                {
                    long time = reader.ReadInt64();
                    int uid = reader.ReadInt32();
                    double value = reader.ReadDouble();


                    if (time > (timeNowEpoch * 1000))
                        continue;

                    if ((uid == 2 || uid == 6 || uid == 9 || uid == 19
                        || uid == 20 || uid == 50 || uid == 101
                        || uid == 3101 || uid == 3102 || uid == 3104)
                        && (value > 127))
                    {
                        value -= 256;
                    }

                    result.Add(new BinFile
                    {
                        Date = time,
                        Uid = uid,
                        Value = value
                    });
                }
            }

            return ToDataFile(result);
        }

        public Dictionary<string, List<BinFile>> ToDictionary(List<BinFile> data)
        {
        //    var temp = data.Select(d => d.Uid).Distinct().ToArray();

            return data.GroupBy(r => r.Uid).ToDictionary(r => uidsNames[r.Key], r => r.ToList());
        }

        private List<DataFile> ToDataFile(List<BinFile> data)
        {
            var result = new List<DataFile>();
            var dates = data.Select(d => d.Date).Distinct().OrderBy(d => d).ToList();

            for (int i = 0; i < dates.Count; i++)
            {
                var elementsOfDate = data.Where(d => d.Date == dates[i]).ToList();
                var resultElement = new DataFile();
                resultElement.Data.Add("Время в “UNIX” формате", new DataElement { OriginalValue = dates[i], DisplayValue = DateTimeOffset.FromUnixTimeMilliseconds(dates[i]).AddHours(3).ToString("dd.MM.yyyy HH:mm:ss") });

                for (int j = 0; j < elementsOfDate.Count; j++)
                {
                    var dataElement = new DataElement
                    {
                        ChartValue = elementsOfDate[j].Value,
                        DisplayValue = elementsOfDate[j].Value.ToString(),
                        Display = true
                    };
                    resultElement.Data.Add(uidsNames[elementsOfDate[j].Uid], dataElement);
                }

                if (i > 0 && result.Last().Data.Count > resultElement.Data.Count)
                {
                    var keys = result.Last().Data.Keys;
                    foreach (var key in keys)
                    {
                        if (!resultElement.Data.ContainsKey(key))
                        {
                            resultElement.Data.Add(key, result.Last().Data[key]);
                        }
                    }
                }

                result.Add(resultElement);

            }

            return result;
        }
    }
}