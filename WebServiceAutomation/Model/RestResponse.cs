using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceAutomation.Model
{
    public class RestResponse
    {
        private int statusCode;
        private string responseData;

        public RestResponse(int statusCode, string responseData)
        {
            this.statusCode = statusCode;
            this.responseData = responseData;
        }

        public int StatusCode
        {
            get
            {
                return this.statusCode;
            }
        }

        public string ResponseData
        {
            get
            {
                return this.responseData;
            }
        }

        public override string ToString()
        {
            return String.Format("StatusCode : {0} \nResponseData : {1}", statusCode, responseData);
        }
    }
}
