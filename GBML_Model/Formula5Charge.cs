using System.Collections;
using System.Linq;

namespace GBML_Model
{
    public class Formula5Charge : ChargeFormula, IFormulaCharge
    {
        public Formula5Charge(ReturnFormulaElements returnFormulaElements) : base(returnFormulaElements)
        {

        }
        public void Formula(CostCenter costCenter, PossibleProducts prod, ref ArrayList expenseItems)
        {
            double tkb = PublicData.TablesTechData.Where(c => c.Des.Equals("Tkb") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//177
            double ls = PublicData.TablesTechData.Where(c => c.Des.Equals("Ls") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//134
            double la = PublicData.TablesTechData.Where(c => c.Des.Equals("La") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//132
            double lt = PublicData.TablesTechData.Where(c => c.Des.Equals("Lt") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//131
            double wg = PublicData.TablesTechData.Where(c => c.Des.Equals("Wg") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//119
            double hs = PublicData.TablesTechData.Where(c => c.Des.Equals("Hs") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//137
            double tks = PublicData.TablesTechData.Where(c => c.Des.Equals("Tks") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//171
            double wds = PublicData.TablesTechData.Where(c => c.Des.Equals("Wds") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//172
            double yi = PublicData.TablesTechData.Where(c => c.Des.Equals("Yi") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//166

            // 02310 Loss for sample
            if (wg != 0)
            {
                PublicData.LossSample = ((PublicData.FormulaParameters.Tk)
                                         * (PublicData.FormulaParameters.Wd) * (ls)
                                         * PublicData.FormulaParameters.Sigma * Power6)
                                        / (wg);
                expenseItems.Add(Ret.ExpenseItem(0, 1, costCenter.Id, prod, 0, PublicData.LossSample, 0, 0));

                // 02340 bar crops
                PublicData.BarCrops =
                    (((tkb) * (PublicData.FormulaParameters.Wd) * (lt)
                      * PublicData.FormulaParameters.Delta * Power6)
                     / (wg))
                    * (PublicData.FormulaParameters.X);
                expenseItems.Add(Ret.ExpenseItem(0, 2, costCenter.Id, prod, 0, PublicData.BarCrops, 0, 0));
            }
            else
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "fc5", "Wg", "", prod.Index);

            // 02140 heating scale
            if (tks != 0 && wds != 0 && yi != 0 && wg != 0)
            {
                PublicData.HeatingScale =
                    ((hs) / 100) * (((tks) * (wds) * (((wg) * Power9) / ((tks) * (wds)
                                                                               * ((yi) / 100) * PublicData.FormulaParameters.Delta)) * PublicData.FormulaParameters.Delta * Power91) / (wg));
                expenseItems.Add(Ret.ExpenseItem(0, 3, costCenter.Id, prod, 0, PublicData.HeatingScale, 0, 0));

                // 02330 Cobbles
                PublicData.Cobbles =
                    ((la) / 100) * (((tks) * (wds) * (((wg) * Power9) / ((tks) * (wds) * ((yi) / 100)
                                                                         * PublicData.FormulaParameters.Delta)) * PublicData.FormulaParameters.Delta * Power91) / (wg));
                expenseItems.Add(Ret.ExpenseItem(0, 4, costCenter.Id, prod, 0, PublicData.Cobbles, 0, 0));
            }
            else if (tks == 0)
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "fc5", "Tks", "act", prod.Index);
            else if (wds == 0)
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "fc5", "Wds", "act", prod.Index);
            else if (yi == 0)
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "fc5", "Yi", "0", prod.Index);
            else
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "fc5", "Wg", "", prod.Index);

            PublicData.ChargingRatio[prod.Index] =
                PublicData.Products
                + (PublicData.LossSample
                   + PublicData.BarCrops
                   + PublicData.HeatingScale
                   + PublicData.Cobbles);

            PublicData.TotCharg = PublicData.TotCharg + PublicData.ChargingRatio[prod.Index];
            PublicData.TotQtyCharg = PublicData.TotQtyCharg + (PublicData.ChargingRatio[prod.Index] * PublicData.PossibleProducts[prod.Index].QtyProd);
        }
    }
}