using System.Linq;

namespace GBML_Model
{
    public class Formula1Std : StandardTimeFormula, IFormulaStd
    {
        public void Formula(CostCenter costCenter, PossibleProducts prod)
        {
            double nw = PublicData.TablesTechData.Where(c => c.Des.Equals("Nw") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//116
            double iw = PublicData.TablesTechData.Where(c => c.Des.Equals("Iw") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//117
            double t1 = PublicData.TablesTechData.Where(c => c.Des.Equals("T1") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//152
            double t2 = PublicData.TablesTechData.Where(c => c.Des.Equals("T2") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//153
            double t3 = PublicData.TablesTechData.Where(c => c.Des.Equals("T3") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//154
            double t4 = PublicData.TablesTechData.Where(c => c.Des.Equals("T4") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//155
            double cc = PublicData.TablesTechData.Where(c => c.Des.Equals("Cc") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//118
            double wg = PublicData.TablesTechData.Where(c => c.Des.Equals("Wg") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//119

            if (wg != 0)
                PublicData.Std[prod.Index] =
                    (((((t1) + (t2) + (t3) + (t4)) / ((wg) * 60)) * (nw)) + (iw)) * (cc);
            else
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "ft1", "Wg", "", prod.Index);

            PublicData.TotStd = PublicData.TotStd + PublicData.Std[prod.Index];
            PublicData.TotQtyStd = PublicData.TotQtyStd + (PublicData.Std[prod.Index] * PublicData.PossibleProducts[prod.Index].QtyProd);
        }
    }
}