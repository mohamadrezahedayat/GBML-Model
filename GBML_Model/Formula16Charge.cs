using System.Collections;
using System.Linq;

namespace GBML_Model
{
    public class Formula16Charge : ChargeFormula, IFormulaCharge
    {
        public Formula16Charge(ReturnFormulaElements returnFormulaElements) : base(returnFormulaElements)
        {

        }
        public void Formula(CostCenter costCenter, PossibleProducts prod, ref ArrayList expenseItems)
        {
            double lt = PublicData.TablesTechData.Where(c => c.Des.Equals("Lt") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //131
            double la = PublicData.TablesTechData.Where(c => c.Des.Equals("La") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //132
            double ls = PublicData.TablesTechData.Where(c => c.Des.Equals("Ls") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //134
            double rio = PublicData.TablesTechData.Where(c => c.Des.Equals("Rio") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//120
            double k1 = PublicData.TablesTechData.Where(c => c.Des.Equals("K1") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //101
            double wg = PublicData.TablesTechData.Where(c => c.Des.Equals("Wg") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //119
            double co = PublicData.TablesTechData.Where(c => c.Des.Equals("Co") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //182
            double s = PublicData.TablesTechData.Where(c => c.Des.Equals("S") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;    //183
            double kp = PublicData.TablesTechData.Where(c => c.Des.Equals("Kp") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //184

            /////////02310 Coil process Scraps head & tail
            if (wg != 0)
            {
                PublicData.HeadAndTail = ((((((lt)) * (rio))
                                            * (PublicData.FormulaParameters.Wd)) * ((PublicData.FormulaParameters.Tk) - (((co) * 2 * Power3)
                    / PublicData.FormulaParameters.Gw)) * PublicData.FormulaParameters.Sigma) * Power6) / (wg);

                ////////02318 samples
                PublicData.Samples = ((((((ls)) * (rio))
                                        * (PublicData.FormulaParameters.Wd)) * ((PublicData.FormulaParameters.Tk) - (((co) * 2 * Power3)
                    / PublicData.FormulaParameters.Gw)) * PublicData.FormulaParameters.Sigma) * Power6) / (wg);

                ////////02315 accidental
                PublicData.Accidental = ((((((la)) * (rio))
                                           * (PublicData.FormulaParameters.Wd)) * ((PublicData.FormulaParameters.Tk) - (((co) * 2 * Power3)
                    / PublicData.FormulaParameters.Gw)) * PublicData.FormulaParameters.Sigma) * Power6) / (wg);

                expenseItems.Add(Ret.ExpenseItem(0, 1, costCenter.Id, prod, 0,
                    PublicData.HeadAndTail +
                    PublicData.Samples +
                    PublicData.Accidental,
                    0, 0));
            }
            else
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "fc16", "Wg", "", prod.Index);

            ////////Zinc losses
            double denominator = (((PublicData.FormulaParameters.Tk) - (((co) * 2 * Power3) / PublicData.FormulaParameters.Gw))
                                  * PublicData.FormulaParameters.Sigma) + ((co) * 2 * Power3);
            if (denominator != 0)
            {
                PublicData.ZincLosses =
                    (kp - 1) * (((co) * 2 * Power3) /
                                (denominator));
                expenseItems.Add(Ret.ExpenseItem(0, 2, costCenter.Id, prod, 0, PublicData.ZincLosses, 0, 0));

                //////05066 Zinc ingot feed 
                PublicData.ZincIngot =
                    (kp) * (((co) * 2 * Power3) /
                            (denominator));
                expenseItems.Add(Ret.ExpenseItem(0, 3, costCenter.Id, prod, 0, PublicData.ZincIngot, 0, 0));
            }
            else
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "fc16", "", "", prod.Index);

            PublicData.ChargingRatio[prod.Index] =
                PublicData.Products
                - (PublicData.ZincIngot)
                + (PublicData.HeadAndTail
                   + PublicData.Samples
                   + PublicData.Accidental
                   + PublicData.ZincLosses);

            PublicData.TotCharg = PublicData.TotCharg + PublicData.ChargingRatio[prod.Index];
            PublicData.TotQtyCharg = PublicData.TotQtyCharg + (PublicData.ChargingRatio[prod.Index] * PublicData.PossibleProducts[prod.Index].QtyProd);
        }
    }
}