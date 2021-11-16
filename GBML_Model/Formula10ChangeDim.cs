namespace GBML_Model
{
    public class Formula10ChangeDim : IFormulaChangeDim
    {
        public void Formula(CostCenter costCenter, PossibleProducts prod)
        {
            PublicData.PossibleProducts[prod.Index].ThiknessProdNew = PublicData.PossibleProducts[prod.Index].ThiknessProd;
            PublicData.PossibleProducts[prod.Index].WidthProdNew = PublicData.PossibleProducts[prod.Index].WidthProd;
            PublicData.PossibleProducts[prod.Index].LengthProdNew = PublicData.PossibleProducts[prod.Index].LengthProd;
        }
    }
}