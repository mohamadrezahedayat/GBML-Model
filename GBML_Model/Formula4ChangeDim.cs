using System;
using System.Linq;

namespace GBML_Model
{
    public class Formula4ChangeDim : IFormulaChangeDim
    {
        public void Formula(CostCenter costCenter, PossibleProducts prod)
        {
            double k4 = PublicData.TablesTechData.Where(c => c.Des.Equals("K4") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //104
            double k6 = PublicData.TablesTechData.Where(c => c.Des.Equals("K6") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //106
            PublicData.PossibleProducts[prod.Index].ThiknessProdNew = PublicData.PossibleProducts[prod.Index].ThiknessProd;
            PublicData.PossibleProducts[prod.Index].WidthProdNew = Convert.ToInt32(PublicData.PossibleProducts[prod.Index].WidthProd + k4);
            PublicData.PossibleProducts[prod.Index].LengthProdNew = Convert.ToInt32(PublicData.PossibleProducts[prod.Index].LengthProd + k6);
        }
    }
}