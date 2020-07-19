using scorecard.Controllers;
using scorecard.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using Scorecard.Controllers;

namespace scorecard
{
    public partial class scorecard_form : System.Windows.Forms.Form
    {
        bool outputRunning;
        MainController mainController;

        public scorecard_form()
        {
            mainController = new MainController();
            InitializeComponent();
            updateComboBox();
        }

        private void updateComboBox()
        {
            // build the combobox
            var dataSource = new List<Customer>();
            List<Customer> custSet = mainController.getCustList();
            foreach (Customer cust in custSet)
            {
                dataSource.Add(new Customer() { Name = cust.Name, ID = cust.ID });
            }

            // set up data binding
            this.customer_select_combobox.DataSource = dataSource;
            this.customer_select_combobox.DisplayMember = "Name";
            this.customer_select_combobox.ValueMember = "ID";

            // make it readonly
            this.customer_select_combobox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void button_Click(object sender, EventArgs e)
        {
            if (outputRunning)
            {
                Debug.WriteLine("ITS ALREADY RUNNING!!!");
                return;
            }

            outputRunning = true;
            short customerID = (short) this.customer_select_combobox.SelectedValue;
            string customerName = this.customer_select_combobox.Text;
            String fileName = "C:\\Users\\Public\\" + customerName + "_scorecard.xlsx";
            BackgroundWorker bw = new BackgroundWorker();

            // this allows our worker to report progress during work
            bw.WorkerReportsProgress = true;

            // this lets us cancel processing if we hit an error
            bw.WorkerSupportsCancellation = true;

            // what to do when progress changed - we'll update our output to indicate that we've done something
            bw.ProgressChanged += new ProgressChangedEventHandler(
                delegate (object o, ProgressChangedEventArgs args)
                {
                    string progressString = args.UserState.ToString();
                    if (args.ProgressPercentage == -1)
                    {
                        this.output.Text = this.output.Text + progressString + "\n";
                    }
                    else
                    {
                        this.output.Text = this.output.Text + "Did something: " + progressString + "\n";
                    }
                }
            );

            // what to do when worker completes its task (notify the user)
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                delegate (object o, RunWorkerCompletedEventArgs args)
                {
                    if (args.Cancelled == true)
                    {
                        this.output.Text = this.output.Text + "Cancelled!\n";
                    }
                    else
                    {
                        this.output.Text = this.output.Text + "Finished!\nFile located at: " + fileName + "\n";
                    }
                }
            );

            // what to do in the background thread
            bw.DoWork += new DoWorkEventHandler(
                delegate (object o, DoWorkEventArgs args)
                {
                    BackgroundWorker b = o as BackgroundWorker;
                    CustomerData customerData = GetCustomerData(customerID, b);
                    if (customerData == null)
                    {
                        args.Cancel = true;
                    }
                    else
                    {
                        ExcelInterop excelInterop = new ExcelInterop();
                        if (excelInterop.CreateWorkbook(fileName, customerData, b))
                        {
                            b.ReportProgress(30, "Scorecard saved!");
                        }
                        else
                        {
                            b.ReportProgress(-1, "Failed to Create Scorecard!");
                            b.CancelAsync();
                            args.Cancel = true;
                        }
                    }
                }
            );

            bw.RunWorkerAsync();
            outputRunning = false;
        }


        private CustomerData GetCustomerData(short customerID, BackgroundWorker b)
        {
            CustomerData customerData = mainController.getCustomerData(customerID);
            if (customerData != null)
            {
                b.ReportProgress(30, "Customer Data Retrieved!");
            }
            else
            {
                b.ReportProgress(-1, "Failed to Retrieve Customer Data!");
                b.CancelAsync();
            }
            return customerData;
        }
    }
}
