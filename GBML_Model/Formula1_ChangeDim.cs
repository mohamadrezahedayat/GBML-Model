using System.Linq;

namespace GBML_Model
{
    public class Formula1ChangeDim : IFormulaChangeDim
    {
        public void Formula(CostCenter costCenter, PossibleProducts prod)
        {
            double tc = PublicData.TablesTechData.Where(c => c.Des.Equals("Tc") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //173
            PublicData.PossibleProducts[prod.Index].ThiknessProdNew = tc;
            PublicData.PossibleProducts[prod.Index].WidthProdNew = PublicData.PossibleProducts[prod.Index].WidthProd;
            PublicData.PossibleProducts[prod.Index].LengthProdNew = PublicData.PossibleProducts[prod.Index].LengthProd;
        }
    }
}