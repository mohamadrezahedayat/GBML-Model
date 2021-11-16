using System;
using System.Collections;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace GBML_Model
{
    class WriteFile
    {
        private string _path;

        public void WriteDataToFiles(int codCostCenter, ArrayList expenseItems)
        {
            _path = PublicData.InputPath;
            WriteTimeDuration(codCostCenter);
            //WritePossibleProducts(codCostCenter);
            WriteCostCenters(codCostCenter);
            WriteExpencesItem(codCostCenter, expenseItems);
            //WritePossibleProductsToExcel(codCostCenter);
            //WriteCostCentersToExcel(codCostCenter);
        }
        private void WriteTimeDuration(int codCostCenter)
        {
            StreamWriter writePossibleProducts = new StreamWriter(_path + "\\" + codCostCenter + "_TimeDuration.txt");
            writePossibleProducts.WriteLine(PublicData.TimeDuration);
            writePossibleProducts.Close();
        }
        private void WritePossibleProducts(int codCostCenter)
        {
            StreamWriter writePossibleProducts = new StreamWriter(_path + "\\" + codCostCenter + "_PossibleProducts.txt");
            writePossibleProducts.WriteLine("id, Cod_Prod, codCC, STD, ChargingRatio, qtyProd");
            writePossibleProducts.WriteLine("");
            for (int i = 0; i < PublicData.NumPossibleProducts; i++)
            {
                if (PublicData.ChargingRatio[PublicData.PossibleProducts[i].Index] > 0 || PublicData.Std[PublicData.PossibleProducts[i].Index] > 0)
                {
                    writePossibleProducts.WriteLine((PublicData.PossibleProducts[i].Id)
                                                    + "," + (PublicData.PossibleProducts[i].CodProd)
                                                    + "," + (codCostCenter)
                                                    + "," + (PublicData.Std[PublicData.PossibleProducts[i].Index])
                                                    + "," + (PublicData.ChargingRatio[PublicData.PossibleProducts[i].Index])
                                                    + "," + (PublicData.PossibleProducts[i].QtyProd));
                }
            }
            writePossibleProducts.Close();

            writePossibleProducts = new StreamWriter(_path + "\\" + codCostCenter + "_TonPossibleProducts.txt");

            for (int i = 0; i < PublicData.NumPossibleProducts; i++)
            {
                if ((PublicData.ChargingRatio[PublicData.PossibleProducts[i].Index] * PublicData.PossibleProducts[i].QtyProd) > 0 || (PublicData.Std[PublicData.PossibleProducts[i].Index] * PublicData.PossibleProducts[i].QtyProd) > 0)
                {
                    writePossibleProducts.WriteLine((PublicData.PossibleProducts[i].Id)
                                                    + "," + (PublicData.PossibleProducts[i].CodProd)
                                                    + "," + (codCostCenter)
                                                    + "," + (PublicData.Std[PublicData.PossibleProducts[i].Index]
                                                             * PublicData.PossibleProducts[i].QtyProd)
                                                    + "," + (PublicData.ChargingRatio[PublicData.PossibleProducts[i].Index]
                                                             * PublicData.PossibleProducts[i].QtyProd));
                }
            }
            writePossibleProducts.Close();
        }
        private void WriteCostCenters(int codCostCenter)
        {
            StreamWriter writeCostCenters = new StreamWriter(_path + "\\" + codCostCenter + "_TotCostCenters.txt");
            writeCostCenters.WriteLine("cod, Tot_Std, Tot_Charg, Tot_QtyStd, Tot_QtyCharg");
            writeCostCenters.WriteLine("");
            writeCostCenters.WriteLine((codCostCenter)
                                       + "," + (PublicData.TotStd)
                                       + "," + (PublicData.TotCharg)
                                       + "," + (PublicData.TotQtyStd)
                                       + "," + (PublicData.TotQtyCharg));
            writeCostCenters.Close();
        }
        private void WritePossibleProductsToExcel(int codCostCenter)
        {
            Excel.Application excelApp = new Excel.Application();
            if (excelApp != null)
            {
                Excel.Workbook excelWorkbook = excelApp.Workbooks.Add();
                Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelWorkbook.Sheets.Add();
                excelWorksheet.Cells[1, 1] = "ID";
                excelWorksheet.Cells[1, 2] = "CodProd";
                excelWorksheet.Cells[1, 3] = "StdHour";
                excelWorksheet.Cells[1, 4] = "StdBasic";
                for (int i = 0; i < PublicData.NumPossibleProducts; i++)
                {
                    string cod = PublicData.PossibleProducts[i].CodProd;
                    excelWorksheet.Cells[i + 2, 1] = PublicData.PossibleProducts[i].Id;
                    excelWorksheet.Cells[i + 2, 2] = cod.ToString();
                    excelWorksheet.Cells[i + 2, 3] = PublicData.Std[PublicData.PossibleProducts[i].Index];
                    excelWorksheet.Cells[i + 2, 4] = PublicData.ChargingRatio[PublicData.PossibleProducts[i].Index];
                }

                excelApp.ActiveWorkbook.SaveAs((_path + "\\" + codCostCenter + "_PossibleProducts"), Excel.XlFileFormat.xlWorkbookNormal);

                excelWorkbook.Close();
                excelApp.Quit();

                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelWorksheet);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelWorkbook);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelApp);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
        private void WriteCostCentersToExcel(int codCostCenter)
        {
            Excel.Application excelApp = new Excel.Application();
            if (excelApp != null)
            {
                Excel.Workbook excelWorkbook = excelApp.Workbooks.Add();
                Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelWorkbook.Sheets.Add();

                excelWorksheet.Cells[1, 1] = "CodCc";
                excelWorksheet.Cells[1, 2] = "StdHour";
                excelWorksheet.Cells[1, 3] = "StdBasic";
                excelWorksheet.Cells[1, 4] = "TotStdHour";
                excelWorksheet.Cells[1, 5] = "TotStdBasic";

                excelWorksheet.Cells[2, 1] = codCostCenter;
                excelWorksheet.Cells[2, 2] = PublicData.TotStd;
                excelWorksheet.Cells[2, 3] = PublicData.TotCharg;
                excelWorksheet.Cells[2, 4] = PublicData.TotQtyStd;
                excelWorksheet.Cells[2, 5] = PublicData.TotQtyCharg;


                excelApp.ActiveWorkbook.SaveAs((_path + "\\" + codCostCenter + "_CostCenter"), Excel.XlFileFormat.xlWorkbookNormal);

                excelWorkbook.Close();
                excelApp.Quit();

                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelWorksheet);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelWorkbook);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelApp);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
        private void WriteExpencesItem(int codCostCenter, ArrayList expenseItems)
        {
            StreamWriter writeExpencesItem = new StreamWriter(_path + "\\" + codCostCenter + "_ExpencesItem.txt");
            writeExpencesItem.WriteLine("codCostCenter, ProdIndex, ExpitExpenseId, machineDataId, numCoef, Cod_Int_Qly, cod_data");
            writeExpencesItem.WriteLine("");
            foreach (ArrayList item in expenseItems)
            {
                for (int i = 0; i < item.Count; i++)
                {
                    writeExpencesItem.Write(item[i] + ",");
                }
                writeExpencesItem.WriteLine("");
            }
            writeExpencesItem.Close();
        }
    }
}