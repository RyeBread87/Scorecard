using System;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using scorecard.Models;

namespace Scorecard.Controllers
{
    class ExcelInterop
    {
        private Application application;
        private Workbook workbook;
        private Worksheet worksheet;
        private Worksheet mainWorksheet;

        public bool CreateWorkbook(string filePath, CustomerData customerData, BackgroundWorker b)
        {
            try
            {
                // Start Excel and create a workbook and worksheet.
                application = new Application();
                application.ScreenUpdating = false;
                application.DisplayAlerts = false;
                application.WorkbookBeforeClose += (Workbook wb, ref bool cancel) => Dispose();

                workbook = application.Workbooks.Open("C:\\Users\\Public\\scorecard_template.xlsx", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                worksheet = workbook.Sheets["Data"];
                mainWorksheet = workbook.Sheets["Scorecard"];
                mainWorksheet.Cells[2, 4] = customerData.CustomerName;

                // we're generating a scorecard for one customer, so won't have a drill-down with multiple options
                mainWorksheet.Cells[2, 4].Validation.Delete();

                PopulateScorecard(customerData);
                showWorkbook();
                save(filePath);

                Dispose();
            }
            catch (Exception e)
            {
                Debug.Print("Exception: " + e.Message);
                Dispose();
                return false;
            }
            return true;
        }

        // sets all the data on the Data tab
        public void PopulateScorecard(CustomerData customerData)
        {
            if (customerData == null)
                return;

            worksheet.Cells[2, 1] = customerData.CustomerID;
            worksheet.Cells[2, 2] = customerData.CustomerName;
            worksheet.Cells[2, 3] = customerData.CustomerSince;
            worksheet.Cells[2, 4] = customerData.OrganizationType;
            worksheet.Cells[2, 5] = customerData.CEO;
            worksheet.Cells[2, 6] = customerData.Representative;
            worksheet.Cells[2, 7] = customerData.TopProduct1;
            worksheet.Cells[2, 8] = customerData.TopProduct2;
            worksheet.Cells[2, 9] = customerData.TopProduct3;
            worksheet.Cells[2, 10] = customerData.FeedbackScore;
            worksheet.Cells[2, 11] = customerData.SurveyResponseText;
            worksheet.Cells[2, 12] = customerData.TopRequest;
            worksheet.Cells[2, 13] = customerData.OrganizationSize;
            worksheet.Cells[2, 14] = customerData.IndustryRanking;
            worksheet.Cells[2, 15] = customerData.NetworkingPercentile;
            worksheet.Cells[2, 16] = customerData.DaysSinceLastIncident;
            worksheet.Cells[2, 17] = customerData.OverallLT1month;
            worksheet.Cells[2, 18] = customerData.Overall1To2months;
            worksheet.Cells[2, 19] = customerData.Overall3To4months;
            worksheet.Cells[2, 20] = customerData.Overall5To6months;
            worksheet.Cells[2, 21] = customerData.Overall7To8months;
            worksheet.Cells[2, 22] = customerData.Overall9To12months;
            worksheet.Cells[2, 23] = customerData.FinancialLT1month;
            worksheet.Cells[2, 24] = customerData.Financial1To2months;
            worksheet.Cells[2, 25] = customerData.Financial3To4months;
            worksheet.Cells[2, 26] = customerData.Financial5To6months;
            worksheet.Cells[2, 27] = customerData.Financial7To8months;
            worksheet.Cells[2, 28] = customerData.Financial9To12months;
            worksheet.Cells[2, 29] = customerData.PerformanceLT1month;
            worksheet.Cells[2, 30] = customerData.Performance1To2months;
            worksheet.Cells[2, 31] = customerData.Performance3To4months;
            worksheet.Cells[2, 32] = customerData.Performance5To6months;
            worksheet.Cells[2, 33] = customerData.Performance7To8months;
            worksheet.Cells[2, 34] = customerData.Performance9To12months;
            worksheet.Cells[2, 35] = customerData.PerceptionLT1month;
            worksheet.Cells[2, 36] = customerData.Perception1To2months;
            worksheet.Cells[2, 37] = customerData.Perception3To4months;
            worksheet.Cells[2, 38] = customerData.Perception5To6months;
            worksheet.Cells[2, 39] = customerData.Perception7To8months;
            worksheet.Cells[2, 40] = customerData.Perception9To12months;
            worksheet.Cells[2, 41] = customerData.StaffingLT1month;
            worksheet.Cells[2, 42] = customerData.Staffing1To2months;
            worksheet.Cells[2, 43] = customerData.Staffing3To4months;
            worksheet.Cells[2, 44] = customerData.Staffing5To6months;
            worksheet.Cells[2, 45] = customerData.Staffing7To8months;
            worksheet.Cells[2, 46] = customerData.Staffing9To12months;
        }

        public void Dispose()
        {
            ReleaseComObject(worksheet);
            worksheet = null;

            ReleaseComObject(workbook);
            workbook = null;

            if (application != null)
            {
                application.Quit();
            }
            ReleaseComObject(application);
            application = null;
        }

        public void save(string filePath)
        {
            application.DisplayAlerts = false;
            string folderPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            try
            {
                workbook.SaveAs(filePath);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Excel save error: " + e.Message);
            }
        }

        public static void ReleaseComObject(object obj)
        {
            if (obj != null && Marshal.IsComObject(obj))
            {
                Marshal.ReleaseComObject(obj);
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void showWorkbook()
        {
            application.ScreenUpdating = true;
            application.Visible = true;
            workbook.Activate();
        }
    }
}
