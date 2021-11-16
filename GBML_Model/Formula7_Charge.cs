using System.Collections;
using System.Linq;

namespace GBML_Model
{
    public class Formula7Charge : ChargeFormula, IFormulaCharge
    {
        public Formula7Charge(ReturnFormulaElements returnFormulaElements) : base(returnFormulaElements)
        {

        }
        public void Formula(CostCenter costCenter, PossibleProducts prod, ref ArrayList expenseItems)
        {
            double tr = PublicData.TablesTechData.Where(c => c.Des.Equals("Tr") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//135
            double lt = PublicData.TablesTechData.Where(c => c.Des.Equals("Lt") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//131
            double la = PublicData.TablesTechData.Where(c => c.Des.Equals("La") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//132
            double ls = PublicData.TablesTechData.Where(c => c.Des.Equals("Ls") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//133
            double rio = PublicData.TablesTechData.Where(c => c.Des.Equals("Rio") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//120
            double k1 = PublicData.TablesTechData.Where(c => c.Des.Equals("K1") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//101
            double wg = PublicData.TablesTechData.Where(c => c.Des.Equals("Wg") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//119
            double lo = PublicData.TablesTechData.Where(c => c.Des.Equals("Lo") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//136


            if (wg != 0)
            {
                PublicData.CoilProcess = ((PublicData.FormulaParameters.Tk) *
                                          ((PublicData.FormulaParameters.Wd) + (tr)) * PublicData.FormulaParameters.Sigma * Power6 *
                                          ((lt) + (ls) + (la)) * (rio)) / (wg);
                expenseItems.Add(Ret.ExpenseItem(0, 1, costCenter.Id, prod, 0, PublicData.CoilProcess, 0, 0));
            }
            else
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "fc7", "Wg", "", prod.Index);

            /////////02320 Trimming
            if (PublicData.FormulaParameters.Wd != 0)
            {
                PublicData.Trimming = ((tr) / (PublicData.FormulaParameters.Wd)) * (k1);
                expenseItems.Add(Ret.ExpenseItem(0, 2, costCenter.Id, prod, 0, PublicData.Trimming, 0, 0));
            }
            else
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "fc7", "Wd", "act", prod.Index);

            ///02990 coil process loss
            PublicData.CoilLoss = lo;
            expenseItems.Add(Ret.ExpenseItem(0, 3, costCenter.Id, prod, 0, PublicData.CoilLoss, 0, 0));

            PublicData.ChargingRatio[prod.Index] =
                PublicData.Products
                + (PublicData.CoilProcess
                   + PublicData.Trimming
                   + PublicData.CoilLoss);

            PublicData.TotCharg = PublicData.TotCharg + PublicData.ChargingRatio[prod.Index];
            PublicData.TotQtyCharg = PublicData.TotQtyCharg + (PublicData.ChargingRatio[prod.Index] * PublicData.PossibleProducts[prod.Index].QtyProd);
        }
    }
}