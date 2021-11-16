namespace GBML_Model
{

    public enum NumVersionTest
    {
        CurrentData = 1,
        LastSuccessRunData = 3
    }

    public enum FlagIdentifierName
    {
        CalculateModel = 1,
        AssignmentModel = 2
    }

    public enum FlagNumModule
    {
        RunActiveMachine = 1,
        RunAllMachines = 2,
        RunSpecialMachine = 3
    }

    public enum FlagCostCenter
    {
        RunAllCostCenters = 0,
        RunSpecialCostCenter = 1
    }

    public enum ActiveMachineTest
    {
        Crm0101 = 111,
        Crm0102 = 121,
        Crm0103 = 131,
        Crm0104 = 43,
        Crm0105 = 44,
        Hfl0201 = 45,
        Pic0301 = 46,
        Hsm0401 = 47,
        Ccm0501 = 49,
        Ccm0502 = 50
    }

    public enum CostCenterCodeTest
    {
        Eaf = 2210,
        Dh = 2220,
        Lf = 2240,
        Ds = 2250,
        Ccm = 2410,
        Cooling = 2510,
        Heat = 3210,
        Hsm = 3310,
        Lg1Hfl = 3420,
        Lg2Hfl = 3430,
        HgHfl = 3440,
        SkpHfl = 3450,
        Pic1 = 4220,
        Pic2 = 4225,
        Tan = 4310,
        Srm = 4315,
        Ecl = 4405,
        Skp1 = 4510,
        Skp2 = 4515,
        Temp = 4520,
        Corr1 = 4610,
        Corr2 = 4615,
        Corr3 = 4616,
        Corr4 = 4618,
        Lg = 4630,
        Hg = 4640,
        Tin = 4710,
        Tsh = 4720,
        Cgl = 4810,
        Ccl = 4820
    }
}
