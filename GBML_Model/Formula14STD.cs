using System.Linq;

namespace GBML_Model
{
    public class Formula14Std : StandardTimeFormula, IFormulaStd
    {
        public void Formula(CostCenter costCenter, PossibleProducts prod)
        {
            double sm = PublicData.TablesTechData.Where(c => c.Des.Equals("Sm") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //151
            double ft = PublicData.TablesTechData.Where(c => c.Des.Equals("Ft") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //152
            double et = PublicData.TablesTechData.Where(c => c.Des.Equals("Et") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //153
            double nt = PublicData.TablesTechData.Where(c => c.Des.Equals("Nt") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //154
            double rio = PublicData.TablesTechData.Where(c => c.Des.Equals("Rio") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//120
            double wt = PublicData.TablesTechData.Where(c => c.Des.Equals("Wt") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //155
            double st = PublicData.TablesTechData.Where(c => c.Des.Equals("St") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //156
            double wg = PublicData.TablesTechData.Where(c => c.Des.Equals("Wg") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //119
            double nw = PublicData.TablesTechData.Where(c => c.Des.Equals("Nw") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //116
            double iw = PublicData.TablesTechData.Where(c => c.Des.Equals("Iw") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //117
            double co = PublicData.TablesTechData.Where(c => c.Des.Equals("Co") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //182
            double s = PublicData.TablesTechData.Where(c => c.Des.Equals("S") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;    //183

            double denomerator = ((sm) * (PublicData.FormulaParameters.Wd) * 60)
                                 * ((((PublicData.FormulaParameters.Tk) - (((co) * (s) * Power3) / PublicData.FormulaParameters.Tw))
                                     * PublicData.FormulaParameters.Sigma) + ((co) * (s) * Power3));

            if (denomerator != 0 && wg != 0)
                PublicData.Std[prod.Index] = (((Power6 / (denomerator))
                                               + ((((((((ft) + (wt) + (nt)) * (rio)) + (et)) + (st))) / ((wg) * 60))
                                                  * (PublicData.FormulaParameters.X))) * (nw)) + (iw);
            else
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "ft14", "", "", prod.Index);

            PublicData.TotStd = PublicData.TotStd + PublicData.Std[prod.Index];
            PublicData.TotQtyStd = PublicData.TotQtyStd + (PublicData.Std[prod.Index] * PublicData.PossibleProducts[prod.Index].QtyProd);
        }
    }
}