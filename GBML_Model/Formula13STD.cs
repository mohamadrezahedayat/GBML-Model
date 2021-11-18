using System.Linq;

namespace GBML_Model
{
    public class Formula13Std : StandardTimeFormula, IFormulaStd
    {
        public void Formula(CostCenter costCenter, PossibleProducts prod)
        {
            double sm = PublicData.TablesTechData.FirstOrDefault(c => c.Des.Equals("Sm") && c.CcCod == costCenter.Code).NumCoef;//151
            double nw = PublicData.TablesTechData.FirstOrDefault(c => c.Des.Equals("Nw") && c.CcCod == costCenter.Code).NumCoef;//116
            double iw = PublicData.TablesTechData.Where(c => c.Des.Equals("Iw") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//117
            double co = PublicData.TablesTechData.Where(c => c.Des.Equals("Co") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//182
            double s = PublicData.TablesTechData.Where(c => c.Des.Equals("S") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //183

            double denomerator = ((sm) * (PublicData.FormulaParameters.Wd) * 60) *
                                 ((((PublicData.FormulaParameters.Tk) - (((co) * (s) * Power3) / PublicData.FormulaParameters.Tw)) *
                                   PublicData.FormulaParameters.Sigma) +
                                  ((co) * (s) * Power3));

            if (denomerator != 0)
                PublicData.Std[prod.Index] =
                    ((Power6 / (denomerator)) * (nw)) + (iw);
            else
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "ft13", "", "", prod.Index);

            PublicData.TotStd = PublicData.TotStd + PublicData.Std[prod.Index];
            PublicData.TotQtyStd = PublicData.TotQtyStd + (PublicData.Std[prod.Index] * PublicData.PossibleProducts[prod.Index].QtyProd);
        }
    }
}