using System.Linq;

namespace GBML_Model
{
    public class Formula6Std : StandardTimeFormula, IFormulaStd
    {
        public void Formula(CostCenter costCenter, PossibleProducts prod)
        {
            double sm = PublicData.TablesTechData.Where(c => c.Des.Equals("Sm") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //151
            double wg = PublicData.TablesTechData.Where(c => c.Des.Equals("Wg") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //119
            double nw = PublicData.TablesTechData.Where(c => c.Des.Equals("Nw") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //116
            double iw = PublicData.TablesTechData.Where(c => c.Des.Equals("Iw") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //117
            double fet = PublicData.TablesTechData.Where(c => c.Des.Equals("Fet") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//159
            double cc = PublicData.TablesTechData.Where(c => c.Des.Equals("Cc") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //118

            if (PublicData.FormulaParameters.Tk * PublicData.FormulaParameters.Wd * sm != 0 && wg != 0)
                PublicData.Std[prod.Index] = (((((Power6 / ((PublicData.FormulaParameters.Tk) * (PublicData.FormulaParameters.Wd)
                    * PublicData.FormulaParameters.Sigma * (sm) * 60)) + (((fet) / ((wg) * 60))
                                                                          * (PublicData.FormulaParameters.X)))) * (nw)) + (iw)) * (cc);
            else if (PublicData.FormulaParameters.Tk == 0)
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "ft6", "Tk", "act", prod.Index);
            else if (PublicData.FormulaParameters.Wd == 0)
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "ft6", "Wd", "act", prod.Index);
            else if (wg == 0)
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "ft6", "Wg", "", prod.Index);
            else
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "ft6", "Sm", "", prod.Index);

            PublicData.TotStd = PublicData.TotStd + PublicData.Std[prod.Index];
            PublicData.TotQtyStd = PublicData.TotQtyStd + (PublicData.Std[prod.Index] * PublicData.PossibleProducts[prod.Index].QtyProd);
        }
    }
}