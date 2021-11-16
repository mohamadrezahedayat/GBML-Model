using System;
using System.Linq;

namespace GBML_Model
{
    public class Formula5ChangeDim : IFormulaChangeDim
    {
        public void Formula(CostCenter costCenter, PossibleProducts prod)
        {
            double tks = PublicData.TablesTechData.Where(c => c.Des.Equals("Tks") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //171
            double wds = PublicData.TablesTechData.Where(c => c.Des.Equals("Wds") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //172
            double wg = PublicData.TablesTechData.Where(c => c.Des.Equals("Wg") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //119
            double yi = PublicData.TablesTechData.Where(c => c.Des.Equals("Yi") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//166
            double power9 = Math.Pow(10, 9);
            PublicData.PossibleProducts[prod.Index].ThiknessProdNew = tks;
            PublicData.PossibleProducts[prod.Index].WidthProdNew = Convert.ToInt32(wds);
            PublicData.PossibleProducts[prod.Index].LengthProdNew = Convert.ToInt32(Math.Round((wg * power9) / ((tks * wds * PublicData.FormulaParameters.Delta) * (yi / 100))));

        }
    }
}