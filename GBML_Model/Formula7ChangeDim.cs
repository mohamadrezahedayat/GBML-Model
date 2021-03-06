using System;
using System.Linq;

namespace GBML_Model
{
    public class Formula7ChangeDim : IFormulaChangeDim
    {
        public void Formula(CostCenter costCenter, PossibleProducts prod)
        {
            double k1 = PublicData.TablesTechData.Where(c => c.Des.Equals("K1") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //101
            double tr = PublicData.TablesTechData.Where(c => c.Des.Equals("Tr") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //135
            PublicData.PossibleProducts[prod.Index].ThiknessProdNew = PublicData.PossibleProducts[prod.Index].ThiknessProd;
            PublicData.PossibleProducts[prod.Index].WidthProdNew = Convert.ToInt32(Math.Round(PublicData.PossibleProducts[prod.Index].WidthProd + (tr * k1)));
            PublicData.PossibleProducts[prod.Index].LengthProdNew = PublicData.PossibleProducts[prod.Index].LengthProd;
        }
    }
}