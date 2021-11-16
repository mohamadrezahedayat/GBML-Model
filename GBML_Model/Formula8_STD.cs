using System.Linq;

namespace GBML_Model
{
    public class Formula8Std : StandardTimeFormula, IFormulaStd
    {
        public void Formula(CostCenter costCenter, PossibleProducts prod)
        {
            double wcf = PublicData.TablesTechData.Where(c => c.Des.Equals("Wcf") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef; //119
            double ct = PublicData.TablesTechData.Where(c => c.Des.Equals("Ct") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//158
            double nw = PublicData.TablesTechData.Where(c => c.Des.Equals("Nw") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//116
            double iw = PublicData.TablesTechData.Where(c => c.Des.Equals("Iw") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//117
            double cc = PublicData.TablesTechData.Where(c => c.Des.Equals("Cc") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//118

            if (wcf != 0)
                PublicData.Std[prod.Index] = ((((ct) / (wcf)) * (nw)) + (iw)) * cc;
            else
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "ft8", "Wcf", "", prod.Index);

            PublicData.TotStd = PublicData.TotStd + PublicData.Std[prod.Index];
            PublicData.TotQtyStd = PublicData.TotQtyStd + (PublicData.Std[prod.Index] * PublicData.PossibleProducts[prod.Index].QtyProd);
        }
    }
}