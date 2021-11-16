using System.Collections;
using System.Linq;

namespace GBML_Model
{
    public class Formula6Charge : ChargeFormula, IFormulaCharge
    {
        public Formula6Charge(ReturnFormulaElements returnFormulaElements) : base(returnFormulaElements)
        {

        }
        public void Formula(CostCenter costCenter, PossibleProducts prod, ref ArrayList expenseItems)
        {
            double tr = PublicData.TablesTechData.Where(c => c.Des.Equals("Tr") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//135
            double lt = PublicData.TablesTechData.Where(c => c.Des.Equals("Lt") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//131
            double la = PublicData.TablesTechData.Where(c => c.Des.Equals("La") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//132
            double ls = PublicData.TablesTechData.Where(c => c.Des.Equals("Ls") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//134
            double ld = PublicData.TablesTechData.Where(c => c.Des.Equals("Ld") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//133
            double k1 = PublicData.TablesTechData.Where(c => c.Des.Equals("K1") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//101
            double k2 = PublicData.TablesTechData.Where(c => c.Des.Equals("K2") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//102
            double k3 = PublicData.TablesTechData.Where(c => c.Des.Equals("K3") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//103
            double k4 = PublicData.TablesTechData.Where(c => c.Des.Equals("K4") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//104
            double k5 = PublicData.TablesTechData.Where(c => c.Des.Equals("K5") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//105
            double wg = PublicData.TablesTechData.Where(c => c.Des.Equals("Wg") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//119


            /////////02350 Coil process Scraps head & tail
            if (wg != 0)
            {
                PublicData.CoilProcess = (((((lt) * (PublicData.FormulaParameters.Tk)
                                                  * ((PublicData.FormulaParameters.Wd) + (tr)) * PublicData.FormulaParameters.Sigma * Power6)) * (k1))
                                          + (((la) * (k2)) + ((ld) * (k3)) + ((ls) * (k4))) * (PublicData.FormulaParameters.Tk)
                                          * (PublicData.FormulaParameters.Wd) * PublicData.FormulaParameters.Sigma * Power6) / (wg);
                expenseItems.Add(Ret.ExpenseItem(0, 1, costCenter.Id, prod, 0, PublicData.CoilProcess, 0, 0));
            }
            else
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "fc6", "Wg", "", prod.Index);

            /////////02320 Trimming
            if (PublicData.FormulaParameters.Wd != 0)
            {
                PublicData.Trimming = ((tr) / (PublicData.FormulaParameters.Wd)) * (k5);
                expenseItems.Add(Ret.ExpenseItem(0, 2, costCenter.Id, prod, 0, PublicData.Trimming, 0, 0));
            }
            else
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "fc6", "Wd", "act", prod.Index);

            PublicData.ChargingRatio[prod.Index] =
                PublicData.Products
                + (PublicData.CoilProcess
                   + PublicData.Trimming);

            PublicData.TotCharg = PublicData.TotCharg + PublicData.ChargingRatio[prod.Index];
            PublicData.TotQtyCharg = PublicData.TotQtyCharg + (PublicData.ChargingRatio[prod.Index] * PublicData.PossibleProducts[prod.Index].QtyProd);
        }
    }
}