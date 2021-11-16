using System;
using System.Linq;

namespace GBML_Model
{
    public class Formula9ChangeDim : IFormulaChangeDim
    {
        public void Formula(CostCenter costCenter, PossibleProducts prod)
        {
            double wc = PublicData.TablesTechData.Where(c => c.Des.Equals("Wc") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //175
            PublicData.PossibleProducts[prod.Index].ThiknessProdNew = PublicData.PossibleProducts[prod.Index].ThiknessProd;
            PublicData.PossibleProducts[prod.Index].WidthProdNew = Convert.ToInt32(wc);
            PublicData.PossibleProducts[prod.Index].LengthProdNew = PublicData.PossibleProducts[prod.Index].LengthProd;
        }
    }
}