using System;

namespace GBML_Model
{
    class CreateFatherProd
    {
        public CreateFatherProd(PossibleProducts possibleProduct, string type)
        {
            if (type == "TRANSPORT")
            {
                var codSha = possibleProduct.CodShapeMid.ToString("000");
                var codTyp = possibleProduct.CodTypeMid.ToString("0");
                var codEdge = possibleProduct.CodEdgeMid.ToString("0");
                string codSur = possibleProduct.CodSurfaceMid.ToString("00");
                string codFla = possibleProduct.CodRoughnessMid.ToString("00");
                string codPtc = possibleProduct.CodProtectionMid.ToString("0");
                string codTol = possibleProduct.CodToleranceMid.ToString("00");
                string codIntQly = possibleProduct.CodInternalQualityMid.ToString("0000");
                string codAttrQly = possibleProduct.CodAttributeQualityMid.ToString("0000");
                string codCmmQly = possibleProduct.CodCmmercialQualityMid.ToString("000");
                string codPvc = Convert.ToInt32(possibleProduct.CodEnterPointMid).ToString("0000");
                string codPrtcRule = possibleProduct.CodPrticularRuleMid.ToString("00");
                string codNextUse = possibleProduct.CodNextUseMid.ToString("000");
                string codNextUser = possibleProduct.CodNextUserMid.ToString("000");
                string tksProd = (possibleProduct.ThiknessProdMid != 0) ? possibleProduct.ThiknessProdMid.ToString("000.000") : "999.999";
                string widProd = (possibleProduct.WidthProdMid != 0) ? possibleProduct.WidthProdMid.ToString("0000") : "9999";
                string lthProd = (possibleProduct.LengthProdMid != 0) ? possibleProduct.LengthProdMid.ToString("000000") : "999999";
                possibleProduct.CodProdMid =
                    codSha +
                    codTyp +
                    codEdge +
                    codSur +
                    codFla +
                    codPtc +
                    codTol +
                    codIntQly +
                    codAttrQly +
                    codCmmQly +
                    codPvc +
                    codPrtcRule +
                    codNextUse +
                    codNextUser +
                    tksProd +
                    widProd +
                    lthProd;
            }
            else if (type == "CALCULATE")
            {
                string tksProd = (possibleProduct.ThiknessProdNew != 0) ? possibleProduct.ThiknessProdNew.ToString("000.000") : "999.999";
                string widProd = (possibleProduct.WidthProdNew != 0) ? possibleProduct.WidthProdNew.ToString("0000") : "9999";
                string lthProd = (possibleProduct.LengthProdNew != 0) ? possibleProduct.LengthProdNew.ToString("000000") : "999999";
                possibleProduct.CodProdFather = "99999999999999999999999999999999999" +
                                                tksProd +
                                                widProd +
                                                lthProd;
            }
        }
    }
}