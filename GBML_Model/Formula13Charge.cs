using System;
using System.Collections;
using System.Linq;

namespace GBML_Model
{
    public class Formula13Charge : ChargeFormula, IFormulaCharge
    {
        public Formula13Charge(ReturnFormulaElements returnFormulaElements) : base(returnFormulaElements)
        {

        }
        public void Formula(CostCenter costCenter, PossibleProducts prod, ref ArrayList expenseItems)
        {
            double tr = PublicData.TablesTechData.FirstOrDefault(c => c.Des.Equals("Tr") && c.CcCod == costCenter.Code).NumCoef;//135
            double lt = PublicData.TablesTechData.FirstOrDefault(c => c.Des.Equals("Lt") && c.CcCod == costCenter.Code).NumCoef;//131
            double la = PublicData.TablesTechData.FirstOrDefault(c => c.Des.Equals("La") && c.CcCod == costCenter.Code).NumCoef;//132
            double ls = PublicData.TablesTechData.FirstOrDefault(c => c.Des.Equals("Ls") && c.CcCod == costCenter.Code).NumCoef;//134
            double rio = PublicData.TablesTechData.FirstOrDefault(c => c.Des.Equals("Rio") && c.CcCod == costCenter.Code).NumCoef;//120
            double k1 = PublicData.TablesTechData.FirstOrDefault(c => c.Des.Equals("K1") && c.CcCod == costCenter.Code).NumCoef;//101
            double wg = PublicData.TablesTechData.FirstOrDefault(c => c.Des.Equals("Wg") && c.CcCod == costCenter.Code).NumCoef;//119
            double co = PublicData.TablesTechData.FirstOrDefault(c => c.Des.Equals("Co") && c.CcCod == costCenter.Code).NumCoef;//182
            double s = PublicData.TablesTechData.FirstOrDefault(c => c.Des.Equals("S") && c.CcCod == costCenter.Code).NumCoef;//183
            double kp = PublicData.TablesTechData.FirstOrDefault(c => c.Des.Equals("Kp") && c.CcCod == costCenter.Code).NumCoef;//186
            /////////02310 Coil process Scraps head & tail
            if (wg != 0)
            {
                PublicData.HeadAndTail = ((((((lt) + (ls) +
                                              (la)) * (rio)) *
                                            ((PublicData.FormulaParameters.Wd) + (tr))) *
                                           ((PublicData.FormulaParameters.Tk) -
                                            (((co) * (s) *
                                              Math.Pow(10, -3)) / PublicData.FormulaParameters.Tw)) * PublicData.FormulaParameters.Sigma) * Math.Pow(10, -6)) /
                                         (wg);
                expenseItems.Add(Ret.ExpenseItem(0, 1, costCenter.Id, prod, 0,
                    (PublicData.HeadAndTail),
                    0, 0));
            }
            else
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "fc13", "Wg", "", prod.Index);
            PublicData.Trimming = ((tr) / (PublicData.FormulaParameters.Wd)) *
                                  (1 - (((co) * (s) *
                                         Math.Pow(10, -3)) /
                                        ((((PublicData.FormulaParameters.Tk) -
                                           (((co) * (s) *
                                             Math.Pow(10, -3)) / PublicData.FormulaParameters.Tw)) * PublicData.FormulaParameters.Sigma) +
                                         ((co) * (s) *
                                          Math.Pow(10, -3)))));
            PublicData.TinLosses = ((kp) - 1) *
                                   (((co) * (s) *
                                     Math.Pow(10, -3)) / ((((PublicData.FormulaParameters.Tk) -
                                                            (((co) *
                                                              (s) * Math.Pow(10, -3)) / PublicData.FormulaParameters.Tw)) * PublicData.FormulaParameters.Sigma) +
                                                          ((co) *
                                                           (s) * Math.Pow(10, -3))));
            PublicData.ConsumableTin = ((kp)) *
                                       (((co) * (s) *
                                         Math.Pow(10, -3)) / ((((PublicData.FormulaParameters.Tk) -
                                                                (((co) *
                                                                  (s) * Math.Pow(10, -3)) / 7.31)) * 7.85) +
                                                              ((co) *
                                                               (s) * Math.Pow(10, -3))));
            PublicData.ChargingRatio[prod.Index] =
                PublicData.Products
                - (PublicData.ConsumableTin)
                + (PublicData.HeadAndTail
                   + PublicData.Trimming
                   + PublicData.TinLosses);
            PublicData.TotCharg = PublicData.TotCharg + PublicData.ChargingRatio[prod.Index];
            PublicData.TotQtyCharg = PublicData.TotQtyCharg + (PublicData.ChargingRatio[prod.Index] * PublicData.PossibleProducts[prod.Index].QtyProd);
        }
    }
}

