using scorecard.Models;
using scorecard.Shared;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace scorecard.Controllers
{
    class ExcelDAL
    {
        System.IO.StreamWriter errorFile;
        String errorFileLabel;
        String errorFileName;
        DateTime date;
        Conversions conv;
        OleDbConnection conn;

        // constructor, sets up error log file
        public ExcelDAL(string fileLabel)
        {
            conv = new Conversions();
            try
            {
                date = DateTime.Now;
                errorFileLabel = fileLabel;
                errorFileName = "C:\\Users\\Public\\errors_" + fileLabel + "_" + date.ToString("yyyyMMddHHmmSS+") + ".txt";
                errorFile = new System.IO.StreamWriter(errorFileName);
                errorFile.Close();
            }
            catch (Exception e)
            {
                Debug.WriteLine("file open error: " + e.Message);
            }
        }

        public bool isConnectionOpen()
        {
            return (!((conn == null) || (conn.State == System.Data.ConnectionState.Closed)));
        }

        // opens connection to customer list for selection from drop-down
        public bool openCustomerListConn()
        {
            openFile(errorFileLabel);
            try
            {
                string connectionString = @"Provider = Microsoft.ACE.OLEDB.12.0;"
                    + "Data Source = C:\\Users\\Public\\customer_list.xlsx;"
                    + "Extended Properties=Excel 8.0;";
                conn = new OleDbConnection(connectionString);
                conn.Open();
            }
            catch (Exception e)
            {
                try
                {
                    using (var file = errorFile)
                    {
                        file.WriteLine("You failed! " + errorFileLabel + " error: " + e.Message);
                        return false;
                    }
                }
                catch (Exception e2)
                {
                    Debug.WriteLine("errorFile issue: " + e2.Message);
                    return false;
                }
            }
            return true;
        }

        // opens connection to customer data
        public bool openCustomerDataConn()
        {
            
            openFile(errorFileLabel);
            try
            {
                String connectionString = "provider=Microsoft.ACE.OLEDB.12.0;"
                    + "Data Source = C:\\Users\\Public\\customers.xlsx;"
                    + "Extended Properties=Excel 8.0;";
                conn = new OleDbConnection(connectionString);
                conn.Open();
            }
            catch (Exception e)
            {
                try
                {
                    using (var file = errorFile)
                    {
                        file.WriteLine("You failed! " + errorFileLabel + " error: " + e.Message);
                        return false;
                    }
                }
                catch (Exception e2)
                {
                    Debug.WriteLine("errorFile issue: " + e2.Message);
                    return false;
                }
            }

            return true;
        }

        // opens a file for writing
        private void openFile(string filename)
        {
            try
            {
                errorFile = new System.IO.StreamWriter(errorFileName);
            }
            catch (Exception e)
            {
                Debug.WriteLine("file open error: " + e.Message);
            }
        }

        // gets the list of customers for selection in the drop-down
        internal List<Customer> getCustomerList()
        {
            List<Customer> customerList = new List<Customer>();

            try
            {
                OleDbCommand objCmd = new OleDbCommand("Select CustomerID, CustomerName From [Sheet1$]", conn);
                OleDbDataReader reader = objCmd.ExecuteReader();

                while (reader.Read())
                {
                    Customer c = new Customer();
                    c.ID = conv.convertToShort(reader[0].ToString());
                    c.Name = reader[1].ToString();
                    customerList.Add(c);
                }

                reader.Close();
                objCmd.Dispose();
            }
            catch (Exception e)
            {
                try
                {
                    using (var file = errorFile)
                    {
                        file.WriteLine("You failed! " + errorFileLabel + " error: " + e.Message);
                        return null;
                    }
                }
                catch (Exception e2)
                {
                    Debug.WriteLine("errorFile issue: " + e2.Message);
                    return null;
                }
            }

            return customerList;
        }

        // gets data for a selected customer
        internal CustomerData getCustomerData(int customerID)
        {
            CustomerData customerData = new CustomerData();

            try
            {
                StringBuilder stbQuery = new StringBuilder();
                stbQuery.Append("SELECT * FROM [" + "Sheet1$] WHERE [Customer ID] = @CustomerID");
                OleDbCommand odbCommand = new OleDbCommand(stbQuery.ToString(), conn);
                odbCommand.Parameters.AddWithValue("@CustomerID", customerID);
                OleDbDataReader odbReader = odbCommand.ExecuteReader();

                while (odbReader.Read())
                {
                    if (odbReader[0].ToString().Equals("")) { break; }
                    customerData.CustomerID = odbReader[0].ToString();
                    customerData.CustomerName = odbReader[1].ToString();
                    customerData.CustomerSince = odbReader[2].ToString();
                    customerData.OrganizationType = odbReader[3].ToString();
                    customerData.CEO = odbReader[4].ToString();
                    customerData.Representative = odbReader[5].ToString();
                    customerData.TopProduct1 = odbReader[6].ToString();
                    customerData.TopProduct2 = odbReader[7].ToString();
                    customerData.TopProduct3 = odbReader[8].ToString();
                    customerData.FeedbackScore = odbReader[9].ToString();
                    customerData.SurveyResponseText = odbReader[10].ToString();
                    customerData.TopRequest = odbReader[11].ToString();
                    customerData.OrganizationSize = odbReader[12].ToString();
                    customerData.IndustryRanking = odbReader[13].ToString();
                    customerData.NetworkingPercentile = odbReader[14].ToString();
                    customerData.DaysSinceLastIncident = odbReader[15].ToString();
                    customerData.OverallLT1month = odbReader[16].ToString();
                    customerData.Overall1To2months = odbReader[17].ToString();
                    customerData.Overall3To4months = odbReader[18].ToString();
                    customerData.Overall5To6months = odbReader[19].ToString();
                    customerData.Overall7To8months = odbReader[20].ToString();
                    customerData.Overall9To12months = odbReader[21].ToString();
                    customerData.FinancialLT1month = odbReader[22].ToString();
                    customerData.Financial1To2months = odbReader[23].ToString();
                    customerData.Financial3To4months = odbReader[24].ToString();
                    customerData.Financial5To6months = odbReader[25].ToString();
                    customerData.Financial7To8months = odbReader[26].ToString();
                    customerData.Financial9To12months = odbReader[27].ToString();
                    customerData.PerformanceLT1month = odbReader[28].ToString();
                    customerData.Performance1To2months = odbReader[29].ToString();
                    customerData.Performance3To4months = odbReader[30].ToString();
                    customerData.Performance5To6months = odbReader[31].ToString();
                    customerData.Performance7To8months = odbReader[32].ToString();
                    customerData.Performance9To12months = odbReader[33].ToString();
                    customerData.PerceptionLT1month = odbReader[34].ToString();
                    customerData.Perception1To2months = odbReader[35].ToString();
                    customerData.Perception3To4months = odbReader[36].ToString();
                    customerData.Perception5To6months = odbReader[37].ToString();
                    customerData.Perception7To8months = odbReader[38].ToString();
                    customerData.Perception9To12months = odbReader[39].ToString();
                    customerData.StaffingLT1month = odbReader[40].ToString();
                    customerData.Staffing1To2months = odbReader[41].ToString();
                    customerData.Staffing3To4months = odbReader[42].ToString();
                    customerData.Staffing5To6months = odbReader[43].ToString();
                    customerData.Staffing7To8months = odbReader[44].ToString();
                    customerData.Staffing9To12months = odbReader[45].ToString();
                }
            }
            catch (Exception e)
            {
                try
                {
                    using (var file = errorFile)
                    {
                        file.WriteLine("You failed! " + errorFileLabel + " error: " + e.Message);
                        return null;
                    }
                }
                catch (Exception e2)
                {
                    Debug.WriteLine("errorFile issue: " + e2.Message);
                    return null;
                }
            }

            return customerData;
        }

        // close connection
        public bool closeConn()
        {
            conn.Close();
            if (errorFile != null)
            {
                errorFile.Close();
            }
            FileInfo fileInfo = new FileInfo(errorFileName);
            if (fileInfo.Length == 0)
            {
                File.Delete(errorFileName);
            }
            return true;
        }
    }
}
