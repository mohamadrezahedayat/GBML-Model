using System.Linq;

namespace GBML_Model
{
    public class Formula2Std : StandardTimeFormula, IFormulaStd
    {
        public void Formula(CostCenter costCenter, PossibleProducts prod)
        {
            double nw = PublicData.TablesTechData.Where(c => c.Des.Equals("Nw") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //116
            double iw = PublicData.TablesTechData.Where(c => c.Des.Equals("Iw") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //117
            double n1 = PublicData.TablesTechData.Where(c => c.Des.Equals("N1") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //110
            double cc = PublicData.TablesTechData.Where(c => c.Des.Equals("Cc") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //118
            double nsq = PublicData.TablesTechData.Where(c => c.Des.Equals("Nsq") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//111
            double yi = PublicData.TablesTechData.Where(c => c.Des.Equals("Yi") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //166
            double sm = PublicData.TablesTechData.Where(c => c.Des.Equals("Sm") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //151
            double k1 = PublicData.TablesTechData.Where(c => c.Des.Equals("K1") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //101
            double pm = PublicData.TablesTechData.Where(c => c.Des.Equals("Pm") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //167
            double wg = PublicData.TablesTechData.Where(c => c.Des.Equals("Wg") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //119

            double denomerator1 = (PublicData.FormulaParameters.Tk)
                                  * (PublicData.FormulaParameters.Wd) * PublicData.FormulaParameters.Delta * ((yi) / 100)
                                  * (n1) * (sm) * (k1) * 60;
            double denomerator2 = (wg) * ((yi) / 100) * (nsq) * 60;

            if (denomerator1 != 0 && denomerator2 != 0)
                PublicData.Std[prod.Index] = ((((Power6 / (denomerator1)) +
                                                (((pm) / (denomerator2)) * (PublicData.FormulaParameters.X)))
                                               * (nw)) + (iw)) * (cc);
            else
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "ft2", "", "", prod.Index);

            PublicData.TotStd = PublicData.TotStd + PublicData.Std[prod.Index];
            PublicData.TotQtyStd = PublicData.TotQtyStd + (PublicData.Std[prod.Index] * PublicData.PossibleProducts[prod.Index].QtyProd);
        }
    }
}