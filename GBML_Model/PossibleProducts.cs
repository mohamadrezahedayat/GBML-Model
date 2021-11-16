namespace GBML_Model
{
    public class PossibleProducts
    {
        public int Index,
            Id,
            CostCenterId,
            MachinDataId,
            CodShape,
            CodType,
            CodEdge,
            CodSurface,
            CodRoughness,
            CodProtection,
            CodTolerance,
            CodInternalQuality,
            CodAttributeQuality,
            CodCmmercialQuality,
            CodPrticularRule,
            CodNextUse,
            CodNextUser,
            WidthProd,
            LengthProd,
            WidthProdNew,
            LengthProdNew,
            CodExit;
        public string CodProd, CodProdFather;
        public double ThiknessProd,
            ThiknessProdNew,
            QtyProd;
        public bool FlagFoundFather;
        public int CodShapeMid,
            CodTypeMid,
            CodEdgeMid,
            CodSurfaceMid,
            CodRoughnessMid,
            CodProtectionMid,
            CodToleranceMid,
            CodInternalQualityMid,
            CodAttributeQualityMid,
            CodCmmercialQualityMid,
            CodPrticularRuleMid,
            CodNextUseMid,
            CodNextUserMid,
            WidthProdMid,
            LengthProdMid;
        public double ThiknessProdMid;
        public string CodProdMid,
            CodEnterPointMid,
            CodEnterPoint;
    }
}