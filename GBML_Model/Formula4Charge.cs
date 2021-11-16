using System.Collections;
using System.Linq;

namespace GBML_Model
{
    public class Formula4Charge : ChargeFormula, IFormulaCharge
    {
        public Formula4Charge(ReturnFormulaElements returnFormulaElements) : base(returnFormulaElements)
        {

        }
        public void Formula(CostCenter costCenter, PossibleProducts prod, ref ArrayList expenseItems)
        {
            double k1 = PublicData.TablesTechData.FirstOrDefault(c => c.Des.Equals("K1") && c.CcCod == costCenter.Code).NumCoef;//101
            double k2 = PublicData.TablesTechData.FirstOrDefault(c => c.Des.Equals("K2") && c.CcCod == costCenter.Code).NumCoef;//102
            double p1 = PublicData.TablesTechData.FirstOrDefault(c => c.Des.Equals("P1") && c.CcCod == costCenter.Code).NumCoef;//160
            double wg = PublicData.TablesTechData.FirstOrDefault(c => c.Des.Equals("Wg") && c.CcCod == costCenter.Code).NumCoef;//119
            double hs = PublicData.TablesTechData.FirstOrDefault(c => c.Des.Equals("Hs") && c.CcCod == costCenter.Code).NumCoef;//137
            double pm = PublicData.TablesTechData.FirstOrDefault(c => c.Des.Equals("Pm") && c.CcCod == costCenter.Code).NumCoef;//167
            double sc = PublicData.TablesTechData.FirstOrDefault(c => c.Des.Equals("Sc") && c.CcCod == costCenter.Code).NumCoef;//131
            double k3 = PublicData.TablesTechData.FirstOrDefault(c => c.Des.Equals("K3") && c.CcCod == costCenter.Code).NumCoef;//103
            double tr = PublicData.TablesTechData.FirstOrDefault(c => c.Des.Equals("Tr") && c.CcCod == costCenter.Code).NumCoef;//135
            double k5 = PublicData.TablesTechData.FirstOrDefault(c => c.Des.Equals("K5") && c.CcCod == costCenter.Code).NumCoef;//105
            double la = PublicData.TablesTechData.FirstOrDefault(c => c.Des.Equals("La") && c.CcCod == costCenter.Code).NumCoef;//132
            double k6 = PublicData.TablesTechData.FirstOrDefault(c => c.Des.Equals("K6") && c.CcCod == costCenter.Code).NumCoef;//106
            double k4 = PublicData.TablesTechData.FirstOrDefault(c => c.Des.Equals("K4") && c.CcCod == costCenter.Code).NumCoef;//104
            double ls1 = PublicData.TablesTechData.FirstOrDefault(c => c.Des.Equals("Ls1") && c.CcCod == costCenter.Code).NumCoef;//139
            double ls2 = PublicData.TablesTechData.FirstOrDefault(c => c.Des.Equals("Ls2") && c.CcCod == costCenter.Code).NumCoef;//140
            double ts = PublicData.TablesTechData.FirstOrDefault(c => c.Des.Equals("Ts") && c.CcCod == costCenter.Code).NumCoef;//141

            //02100 Oxide scale
            var denominator1 = (wg) - ((hs) * (p1) * (PublicData.FormulaParameters.Wd) * (PublicData.FormulaParameters.Lg) * Power6);
            var denominator2 = (wg) - ((pm) * (2 * ((PublicData.FormulaParameters.Tk)
                                                    + (PublicData.FormulaParameters.Wd))) * (PublicData.FormulaParameters.Lg) * PublicData.FormulaParameters.Delta * Power91);

            if (denominator1 != 0 && denominator2 != 0)
            {
                PublicData.OxideScale =
                    ((((hs) * (p1) * (PublicData.FormulaParameters.Wd) * (PublicData.FormulaParameters.Lg) * Power6)
                      / (denominator1)) * (k1))
                    + ((((pm) * (2 * ((PublicData.FormulaParameters.Tk) + (PublicData.FormulaParameters.Wd)))
                              * (PublicData.FormulaParameters.Lg) * PublicData.FormulaParameters.Delta * Power91) / (denominator2)) * (k2));
                expenseItems.Add(Ret.ExpenseItem(0, 1, costCenter.Id, prod, 0, PublicData.OxideScale, 0, 0));
            }
            else
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "fc4", "", "", prod.Index);

            //02290 Slab crops
            PublicData.Slabcrops = (sc) * (k3);
            expenseItems.Add(Ret.ExpenseItem(0, 2, costCenter.Id, prod, 0, PublicData.Slabcrops, 0, 0));

            //02300 Slab Trimmer
            PublicData.SlabTrimmer = (tr) * (k5);
            expenseItems.Add(Ret.ExpenseItem(0, 3, costCenter.Id, prod, 0, PublicData.SlabTrimmer, 0, 0));

            //02280 Slab scrapped
            PublicData.SlabScrapped = (la) * (k6);
            expenseItems.Add(Ret.ExpenseItem(0, 4, costCenter.Id, prod, 0, PublicData.SlabScrapped, 0, 0));

            //02110 Slab cutting Losses
            PublicData.SlabLosses = (((ls1) * (k3)) + ((ls2) * (k4)) + ((ts) * (k5)));
            expenseItems.Add(Ret.ExpenseItem(0, 5, costCenter.Id, prod, 0, PublicData.SlabLosses, 0, 0));


            PublicData.ChargingRatio[prod.Index] =
                PublicData.Products
                + (PublicData.OxideScale
                   + PublicData.Slabcrops
                   + PublicData.SlabTrimmer
                   + PublicData.SlabScrapped
                   + PublicData.SlabLosses);

            PublicData.TotCharg += PublicData.ChargingRatio[prod.Index];
            PublicData.TotQtyCharg += (PublicData.ChargingRatio[prod.Index] * PublicData.PossibleProducts[prod.Index].QtyProd);
        }
    }
}