using System.Collections;
using System.Linq;

namespace GBML_Model
{
    public class RunModel
    {
        IFormulaCharge _formulaCharge;
        IFormulaStd _formulaStd;
        IFormulaChangeDim _formulaChangeDim;
        CostCenter _codCostCenter;
        public RunModel()
        {

            PublicData.ReturnFormulaElements = new ReturnFormulaElements();
        }
        private void ChargeFormulaSelector(PossibleProducts prod, int formuleNumber, ref ArrayList expenseItems)
        {
            switch (formuleNumber)
            {
                case 1:
                    _formulaCharge = new Formula1Charge(PublicData.ReturnFormulaElements);
                    _formulaCharge.Formula(_codCostCenter, prod, ref expenseItems);
                    break;
                case 2:
                    _formulaCharge = new Formula2Charge(PublicData.ReturnFormulaElements);
                    _formulaCharge.Formula(_codCostCenter, prod, ref expenseItems);
                    break;
                case 3:
                    _formulaCharge = new Formula3Charge(PublicData.ReturnFormulaElements);
                    _formulaCharge.Formula(_codCostCenter, prod, ref expenseItems);
                    break;
                case 4:
                    _formulaCharge = new Formula4Charge(PublicData.ReturnFormulaElements);
                    _formulaCharge.Formula(_codCostCenter, prod, ref expenseItems);
                    break;
                case 5:
                    _formulaCharge = new Formula5Charge(PublicData.ReturnFormulaElements);
                    _formulaCharge.Formula(_codCostCenter, prod, ref expenseItems);
                    break;
                case 6:
                    _formulaCharge = new Formula6Charge(PublicData.ReturnFormulaElements);
                    _formulaCharge.Formula(_codCostCenter, prod, ref expenseItems);
                    break;
                case 7:
                    _formulaCharge = new Formula7Charge(PublicData.ReturnFormulaElements);
                    _formulaCharge.Formula(_codCostCenter, prod, ref expenseItems);
                    break;
                case 8:
                    _formulaCharge = new Formula8Charge(PublicData.ReturnFormulaElements);
                    _formulaCharge.Formula(_codCostCenter, prod, ref expenseItems);
                    break;
                case 9:
                    _formulaCharge = new Formula9Charge(PublicData.ReturnFormulaElements);
                    _formulaCharge.Formula(_codCostCenter, prod, ref expenseItems);
                    break;
                case 10:
                    _formulaCharge = new Formula10Charge(PublicData.ReturnFormulaElements);
                    _formulaCharge.Formula(_codCostCenter, prod, ref expenseItems);
                    break;
                case 11:
                    _formulaCharge = new Formula11Charge(PublicData.ReturnFormulaElements);
                    _formulaCharge.Formula(_codCostCenter, prod, ref expenseItems);
                    break;
                case 13:
                    _formulaCharge = new Formula13Charge(PublicData.ReturnFormulaElements);
                    _formulaCharge.Formula(_codCostCenter, prod, ref expenseItems);
                    break;
                case 14:
                    _formulaCharge = new Formula14Charge(PublicData.ReturnFormulaElements);
                    _formulaCharge.Formula(_codCostCenter, prod, ref expenseItems);
                    break;
                case 16:
                    _formulaCharge = new Formula16Charge(PublicData.ReturnFormulaElements);
                    _formulaCharge.Formula(_codCostCenter, prod, ref expenseItems);
                    break;
                case 17:
                    _formulaCharge = new Formula17Charge(PublicData.ReturnFormulaElements);
                    _formulaCharge.Formula(_codCostCenter, prod, ref expenseItems);
                    break;
                default:
                    break;
            }
        }
        private void StdFormulaSelector(PossibleProducts prod, int formuleNumber)
        {
            switch (formuleNumber)
            {
                case 1:
                    _formulaStd = new Formula1Std();
                    _formulaStd.Formula(_codCostCenter, prod);
                    break;
                case 2:
                    _formulaStd = new Formula2Std();
                    _formulaStd.Formula(_codCostCenter, prod);
                    break;
                case 3:
                    _formulaStd = new Formula3Std();
                    _formulaStd.Formula(_codCostCenter, prod);
                    break;
                case 4:
                    _formulaStd = new Formula4Std();
                    _formulaStd.Formula(_codCostCenter, prod);
                    break;
                case 5:
                    _formulaStd = new Formula5Std();
                    _formulaStd.Formula(_codCostCenter, prod);
                    break;
                case 6:
                    _formulaStd = new Formula6Std();
                    _formulaStd.Formula(_codCostCenter, prod);
                    break;
                case 7:
                    _formulaStd = new Formula7Std();
                    _formulaStd.Formula(_codCostCenter, prod);
                    break;
                case 8:
                    _formulaStd = new Formula8Std();
                    _formulaStd.Formula(_codCostCenter, prod);
                    break;
                case 13:
                    _formulaStd = new Formula13Std();
                    _formulaStd.Formula(_codCostCenter, prod);
                    break;
                case 14:
                    _formulaStd = new Formula14Std();
                    _formulaStd.Formula(_codCostCenter, prod);
                    break;
                case 16:
                    _formulaStd = new Formula16Std();
                    _formulaStd.Formula(_codCostCenter, prod);
                    break;
                case 17:
                    _formulaStd = new Formula17Std();
                    _formulaStd.Formula(_codCostCenter, prod);
                    break;
                default:
                    break;
            }
        }
        private void ChangeDimFormulaSelector(PossibleProducts prod, int formuleNumber)
        {
            switch (formuleNumber)
            {
                case 1:
                    _formulaChangeDim = new Formula1ChangeDim();
                    _formulaChangeDim.Formula(_codCostCenter, prod);
                    break;
                case 2:
                    _formulaChangeDim = new Formula2ChangeDim();
                    _formulaChangeDim.Formula(_codCostCenter, prod);
                    break;
                case 3:
                    _formulaChangeDim = new Formula3ChangeDim();
                    _formulaChangeDim.Formula(_codCostCenter, prod);
                    break;
                case 4:
                    _formulaChangeDim = new Formula4ChangeDim();
                    _formulaChangeDim.Formula(_codCostCenter, prod);
                    break;
                case 5:
                    _formulaChangeDim = new Formula5ChangeDim();
                    _formulaChangeDim.Formula(_codCostCenter, prod);
                    break;
                case 6:
                    _formulaChangeDim = new Formula6ChangeDim();
                    _formulaChangeDim.Formula(_codCostCenter, prod);
                    break;
                case 7:
                    _formulaChangeDim = new Formula7ChangeDim();
                    _formulaChangeDim.Formula(_codCostCenter, prod);
                    break;
                case 8:
                    _formulaChangeDim = new Formula8ChangeDim();
                    _formulaChangeDim.Formula(_codCostCenter, prod);
                    break;
                case 9:
                    _formulaChangeDim = new Formula9ChangeDim();
                    _formulaChangeDim.Formula(_codCostCenter, prod);
                    break;
                case 10:
                    _formulaChangeDim = new Formula10ChangeDim();
                    _formulaChangeDim.Formula(_codCostCenter, prod);
                    break;
                default:
                    break;
            }
        }
        public void RunModelMain(CostCenter costCenter, ref ArrayList expenseItems)
        {
            this._codCostCenter = costCenter;
            FormulaNumber listCod = new FormulaNumber();
            listCod = PublicData.NumberFormulas.Where(c => c.CcCod == costCenter.Code).FirstOrDefault();
            foreach (PossibleProducts prod in PublicData.PossibleProducts)
            {
                if (costCenter.Id != prod.CostCenterId) return;
                if (costCenter.FlagVirtual == 1)
                {
                    PublicData.ChargingRatio[prod.Index] = 1;
                    PublicData.Std[prod.Index] = 0;
                    continue;
                }
                PublicData.ReturnFormulaElements.ReturnElements(prod);
                if (listCod.NumDirMat != -1)
                    ChargeFormulaSelector(prod, listCod.NumDirMat, ref expenseItems);
                if (listCod.NumStdProd != -1)
                    StdFormulaSelector(prod, listCod.NumStdProd);
                if (listCod.NumDimChange != -1)
                    ChangeDimFormulaSelector(prod, listCod.NumDimChange);
                PublicData.CreateFatherProd = new CreateFatherProd(prod, "CALCULATE");
            }
        }
    }
}