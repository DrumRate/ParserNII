namespace ParserNII.DataStructures
{
    public enum FirstSecondByteParams : short
    {
        CallWithPowerSensor = 0,
        CallWithCrossBlock = 2,
        CallWithDutLeft = 4,
        CallWithDutRight = 8,
        CallWithRKD,PM = 16,
        CallWithModuleRadioChannel = 32,
        DataGPSValid = 64,
        WaterInFuelTank = 128,
        ControlCardInserted = 256,
        OpenedCapBI = 512,
        Thrust = 2048

    }
}