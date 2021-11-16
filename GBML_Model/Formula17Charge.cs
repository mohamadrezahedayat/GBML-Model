using System.Collections;
using System.Linq;

namespace GBML_Model
{
    public class Formula17Charge : ChargeFormula, IFormulaCharge
    {
        public Formula17Charge(ReturnFormulaElements returnFormulaElements) : base(returnFormulaElements)
        {

        }
        public void Formula(CostCenter costCenter, PossibleProducts prod, ref ArrayList expenseItems)
        {
            double lt = PublicData.TablesTechData.Where(c => c.Des.Equals("Lt") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //131
            double la = PublicData.TablesTechData.Where(c => c.Des.Equals("La") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;    //132
            double ls = PublicData.TablesTechData.Where(c => c.Des.Equals("Ls") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //134
            double rio = PublicData.TablesTechData.Where(c => c.Des.Equals("Rio") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//120
            double wg = PublicData.TablesTechData.Where(c => c.Des.Equals("Wg") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;    //119
            double co = PublicData.TablesTechData.Where(c => c.Des.Equals("Co") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;    //182
            double kp = PublicData.TablesTechData.Where(c => c.Des.Equals("Kp") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;    //183
            double dc = PublicData.TablesTechData.Where(c => c.Des.Equals("Dc") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;    //186

            if (wg != 0)
            {
                /////////02310 Coil process Scraps head & tail
                PublicData.HeadAndTail =
                    ((((((lt)) * (rio)) * (PublicData.FormulaParameters.Wd))
                      * Power6) * ((((PublicData.FormulaParameters.Tk) - (((co) * Power3) / PublicData.FormulaParameters.Gw)
                                                                       - (((dc) * Power3) / PublicData.FormulaParameters.Cw)) * PublicData.FormulaParameters.Sigma)
                                   + ((co) * Power3))) / (wg);

                ////////02318 samples
                PublicData.Samples =
                    ((((((ls)) * (rio)) * (PublicData.FormulaParameters.Wd))
                      * Power6) * ((((PublicData.FormulaParameters.Tk) - (((co) * Power3) / PublicData.FormulaParameters.Gw)
                                                                       - (((dc) * Power3) / PublicData.FormulaParameters.Cw)) * PublicData.FormulaParameters.Sigma)
                                   + ((co) * Power3))) / (wg);

                ////////02315 accidental
                PublicData.Accidental =
                    ((((((la)) * (rio)) * (PublicData.FormulaParameters.Wd))
                      * Power6) * ((((PublicData.FormulaParameters.Tk) - (((co) * Power3) / PublicData.FormulaParameters.Gw)
                                                                       - (((dc) * Power3) / PublicData.FormulaParameters.Cw)) * PublicData.FormulaParameters.Sigma)
                                   + ((co) * Power3))) / (wg);

                expenseItems.Add(Ret.ExpenseItem(0, 1, costCenter.Id, prod, 0,
                    PublicData.HeadAndTail +
                    PublicData.Samples +
                    PublicData.Accidental,
                    0, 0));
            }
            else
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "fc17", "Wg", "", prod.Index);

            double denominator = ((((((PublicData.FormulaParameters.Tk) - (((co) * Power3) / PublicData.FormulaParameters.Gw)
                                                                        - (((dc) * Power3) / PublicData.FormulaParameters.Cw)) * PublicData.FormulaParameters.Sigma)
                                    + ((co) * Power3)) + ((dc) * Power3)));
            if (denominator != 0)
            {
                ////////Paint losses
                PublicData.Paintlosses = (kp - 1) * (((dc) * Power3) / denominator);
                expenseItems.Add(Ret.ExpenseItem(0, 2, costCenter.Id, prod, 0, PublicData.Paintlosses, 0, 0));

                //////05067 Dry coating products charge
                PublicData.DryCoating = (kp) * (((dc) * Power3) / denominator);
                expenseItems.Add(Ret.ExpenseItem(0, 3, costCenter.Id, prod, 0, PublicData.DryCoating, 0, 0));
            }
            else
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "fc17", "", "", prod.Index);

            PublicData.ChargingRatio[prod.Index] =
                PublicData.Products
                - (PublicData.DryCoating)
                + (PublicData.HeadAndTail
                   + PublicData.Samples
                   + PublicData.Accidental
                   + PublicData.Paintlosses);

            PublicData.TotCharg = PublicData.TotCharg + PublicData.ChargingRatio[prod.Index];
            PublicData.TotQtyCharg = PublicData.TotQtyCharg + (PublicData.ChargingRatio[prod.Index] * PublicData.PossibleProducts[prod.Index].QtyProd);
        }
    }
}