using System;
using System.Collections;
using System.Linq;

namespace GBML_Model
{
    public class Formula14Charge : ChargeFormula, IFormulaCharge
    {
        public Formula14Charge(ReturnFormulaElements returnFormulaElements) : base(returnFormulaElements)
        {

        }
        public void Formula(CostCenter costCenter, PossibleProducts prod, ref ArrayList expenseItems)
        {
            double tr = PublicData.TablesTechData.Where(c => c.Des.Equals("Tr") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//135
            double lt = PublicData.TablesTechData.Where(c => c.Des.Equals("Lt") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //131
            double la = PublicData.TablesTechData.Where(c => c.Des.Equals("La") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //132
            double ls = PublicData.TablesTechData.Where(c => c.Des.Equals("Ls") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //134
            double rio = PublicData.TablesTechData.Where(c => c.Des.Equals("Rio") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//120
            double k1 = PublicData.TablesTechData.Where(c => c.Des.Equals("K1") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //101
            double wg = PublicData.TablesTechData.Where(c => c.Des.Equals("Wg") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //119
            double co = PublicData.TablesTechData.Where(c => c.Des.Equals("Co") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //182
            double s = PublicData.TablesTechData.Where(c => c.Des.Equals("S") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;    //183
            if (wg != 0)
            {
                PublicData.HeadAndTail = ((((lt) + (ls) +
                                            (la)) * (rio) *
                                           (PublicData.FormulaParameters.Wd) * Math.Pow(10, -6)) *
                                          (((((PublicData.FormulaParameters.Tk) -
                                              (((co) * (s) *
                                                Math.Pow(10, -3)) / PublicData.FormulaParameters.Tw)) * PublicData.FormulaParameters.Sigma) +
                                            ((co) * (s) *
                                             Math.Pow(10, -3))))) / (wg);
                expenseItems.Add(Ret.ExpenseItem(0, 1, costCenter.Id, prod, 0,
                    PublicData.HeadAndTail,
                    0, 0));
            }
            else
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "fc14", "Wg", "", prod.Index);
            PublicData.ChargingRatio[prod.Index] =
                PublicData.Products
                + (PublicData.HeadAndTail);
            PublicData.TotCharg = PublicData.TotCharg + PublicData.ChargingRatio[prod.Index];
            PublicData.TotQtyCharg = PublicData.TotQtyCharg + (PublicData.ChargingRatio[prod.Index]
                                                                 * PublicData.PossibleProducts[prod.Index].QtyProd);
        }
    }
}