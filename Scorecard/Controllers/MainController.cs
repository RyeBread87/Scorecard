using scorecard.Models;
using System.Collections.Generic;

namespace scorecard.Controllers
{
    class MainController
    {
        public List<Customer> getCustList()
        {
            ExcelDAL db = new ExcelDAL("getCustomerList");
            db.openCustomerListConn();
            List<Customer> customerSet = db.getCustomerList();
            if (db.isConnectionOpen())
            {
                db.closeConn();
            }
            db = null;
            return customerSet;
        }

        public CustomerData getCustomerData(short customerID)
        {
            ExcelDAL db = new ExcelDAL("getCustomerData");
            db.openCustomerDataConn();
            CustomerData customerData = db.getCustomerData(customerID);
            if (db.isConnectionOpen())
            {
                db.closeConn();
            }
            db = null;
            return customerData;
        }
    }
}



