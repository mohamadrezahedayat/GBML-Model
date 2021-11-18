using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GBML_Model
{
    class PublicData
    {
        public static string ConnectionString,
            ConnectionServer,
            InputPath;

        public static List<PossibleProducts> PossibleProducts;
        public static List<CostCenter> CostCenter;
        public static List<TablesTechData> TablesTechData;
        public static List<TechnicalData> TechnicalData;
        public static List<FormulaNumber> NumberFormulas;
        public static FormulaParameters FormulaParameters;
        public static CreateFatherProd CreateFatherProd;
        public static ReturnFormulaElements ReturnFormulaElements;
        public static List<Ferroalloys> Ferroalloys;
        public static List<ExtractAssign> ExtractAssign;
        public static List<ActiveMachine> ActiveMachine;
        public static List<MachineData> MachineData;
        public static List<ResultMachineData> ResultmachineData;
        public static List<PercentCostCenter> PercentCostCenter;
        public static List<ResultQtyHour> ResultQtyHour;
        public static List<ResultQtyCharge> ResultQtyCharge;
        public static List<PcnCostCenter> PcnCostCenter;
        public static List<AlternativeMachine> AlternativeMachine;
        public static string ValYear;
        public static int NumPossibleProducts = 0, NumCostCenter = 0;
        public static int NumVersion, CodProcedure, ProcedureId, TypCost, CoopsStatusId;
        public static DateTime DateEnd;
        public static double TimeDuration;

        public static double[] ChargingRatio, Std;

        public static double Paintlosses,               //02314 Paint losses
            Accidental,                 //02315 accidental
            Samples,                //02318 samples
            HeadAndTail,                //02310 Coil process Scraps head & tail
            DryCoating,                 //05067 Dry coating products charge
            CoilProcess,                //02350 Coil Process (scraps - accidental - samples - head and tail)
            ConsumableTin,              //05065 Consumable Tin anodes
            TinLosses,              //02329 .Tin losses for drap-out 
            ZincIngot,              //05066 Zinc ingot feed
            ZincLosses,                 //02313 . Zinc losses for drap-out 
            Trimming,               //02320 Trimming
            CoilLoss,               //02990 coil process loss
            LossSample,                 //02310 Loss for sample
            BarCrops,               //02340 bar crops
            HeatingScale,               //02140 heating scale
            Cobbles,                //02330 Cobbles
            OxideScale,                 //02100 Oxide scale
            Slabcrops,              //02290 Slab crops
            SlabTrimmer,                //02300 Slab Trimmer
            SlabScrapped,               //02280 Slab scrapped
            SlabLosses,                 //02110 Slab cutting Losses
            LadleSkull,                 //02270 Ladle skull & liquidSteel splashed 
            TundishSkull,               //02260 Tundish skull 
            SpongeIron,                 //Sponge iron
            Scrap,              //Scrap
            NormalHeat,                 //Ferroalloy for Normal Heat 
            RecycledSteel,              //Ferroalloy for Recycled steel
            RecuperableMaterials;

                        //Recuperable Materials                                                    
        public static double Products = 1;

                        //PRODUCTS
        public static double TotStd,                //Total Standard Time
            TotCharg,               //Total Charge
            TotQtyStd,              //Total Standard Time * Qty Prod
            TotQtyCharg;

                        //Total Charge * Qty Prod
        public static int[] LkpGroupHfl;

        public static int NumAltMachine;
        public static double ObligateQty, FreeQty, FreeQty2, SumpcnCostCenter, FlgPcnCostCenter;
    }
}