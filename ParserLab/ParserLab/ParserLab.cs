using System;
using System.IO;
using ParserLab.Types;

namespace ParserLab
{
    /*
    * TODO: ALWAYS CHANGE PATH InitEnvironment()
    */
    public class ParserLab
    {
        public static void Main()
        {
            Operate();
            //OperateAsHex();
            Console.ReadKey();
        }

        private static void Operate()
        {
            do
            {
                int singleValuesOperationPointer = 0;
                int secondOperationPointer = 0;

                //  Init
                InitEnvironment();
                _fs.Position += 4;

                //  OPs
                for (int i = 0; i < 59; i++)
                {
                    switch (i)
                    {
                        case 0:
                            _time.Operate();
                            break;
                        case 4:
                            _minuteBitRecord1.Operate();
                            break;
                        case 31:
                            _minuteBitRecord2.Operate();
                            break;
                        case 58:
                            _crcRecord.Operate();
                            break;
                        default:
                            if (i < 38)
                            {
                                _singleValues[singleValuesOperationPointer].Operate();
                                singleValuesOperationPointer++;
                            }
                            else
                            {
                                _secondRecords[secondOperationPointer].Operate();
                                secondOperationPointer++;
                            }

                            break;
                    }
                }
            } while (!(_fs.Position >= _fs.Length));
        }

        private static void OperateAsHex()
        {
            do
            {
                int singleValuesOperationPointer = 0;
                int secondOperationPointer = 0;

                //  Init
                InitEnvironment();
                Console.WriteLine("\n");
                byte[] recordStart = new byte[4];
                Console.WriteLine($"\t next pos: {_fs.Position}");
                _fs.Read(recordStart, 0, 4);
                Console.WriteLine($"+\tRecordStart:\tHEX readed: {BitConverter.ToString(recordStart)}");
                Console.WriteLine($"\t next pos: {_fs.Position}");

                //  OPs
                for (int i = 0; i < 59; i++)
                {
                    switch (i)
                    {
                        case 0:
                            _time.OperateAsHex();
                            break;
                        case 4:
                            _minuteBitRecord1.OperateAsHex();
                            break;
                        case 31:
                            _minuteBitRecord2.OperateAsHex();
                            break;
                        case 58:
                            _crcRecord.OperateAsHex();
                            break;
                        default:
                            if (i < 38)
                            {
                                _singleValues[singleValuesOperationPointer].OperateAsHex();
                                singleValuesOperationPointer++;
                            }
                            else
                            {
                                _secondRecords[secondOperationPointer].OperateAsHex();
                                secondOperationPointer++;
                            }

                            break;
                    }
                }
            } while (!(_fs.Position >= _fs.Length));
        }

        private static void InitEnvironment()
        {
            _fs = MyFileStream.GetFileStreamInstance("C:\\Users\\nick\\Desktop\\ParserLab-master\\198_смена.dat");
            _time = new Time(0, 4);
            _singleValues = new SingleValue[35];
            _minuteBitRecord1 = new MinuteBitRecord1(4, 2);
            _minuteBitRecord2 = new MinuteBitRecord2(31, 1);
            _secondRecords = new SecondRecord[20];
            _crcRecord = new CrcRecord(58, 2);
            int initPointer = 0;
            for (int i = 1; i < 38; i++)
            {
                if (i == 4 || i == 31) continue;
                _singleValues[initPointer] = new SingleValue(i, SingleValueByteMap[initPointer]);
                initPointer++;
            }

            initPointer = 0;
            for (int i = 1; i < 21; i++)
            {
                _secondRecords[initPointer] = new SecondRecord(i, 22, _time);
                initPointer++;
            }
        }

        private static readonly int[] SingleValueByteMap =
            {1, 2, 1, 1, 2, 2, 2, 2, 1, 1, 4, 4, 1, 1, 1, 1, 1, 1, 4, 1, 1, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 4, 1, 1, 1};

        private static FileStream _fs;
        private static Time _time;
        private static SingleValue[] _singleValues;
        private static MinuteBitRecord1 _minuteBitRecord1;
        private static MinuteBitRecord2 _minuteBitRecord2;
        private static SecondRecord[] _secondRecords;
        private static CrcRecord _crcRecord;
    }
}