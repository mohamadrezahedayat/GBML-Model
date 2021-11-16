namespace GBML_Model
{
    public class Formula3ChangeDim : IFormulaChangeDim
    {
        public void Formula(CostCenter costCenter, PossibleProducts prod)
        {
            PublicData.PossibleProducts[prod.Index].ThiknessProdNew = 0;
            PublicData.PossibleProducts[prod.Index].WidthProdNew = 0;
            PublicData.PossibleProducts[prod.Index].LengthProdNew = 0;
        }
    }
}