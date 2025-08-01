using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Configuration;
using System.Xml.Serialization;
using System.IO;
using System.Net.Cache;

using CMLabExtim;
using CMLabExtim.WODClasses;


namespace DLLabExtim
{
    public class WODGateway
    {

        public static DHL GetRealTimeDataFromBOBST1()
        {
            DHL returnVal = null;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
                        string.Format("http://{0}/DHLWEB?displaystyle=multivar&smallvarinfo=yes&output=xml_v2", ConfigurationManager.AppSettings["BOBST1_IPAddressAndPort"]));
            //request.Method = "POST";
            request.ContentType = "text/xml";
            request.Timeout = 2000;
            HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            request.CachePolicy = noCachePolicy;

            //#if !DEBUG

            using (var response = (HttpWebResponse)request.GetResponse())
            {

                Stream responseStream = response.GetResponseStream();
                var serializer = new XmlSerializer(typeof(DHL));

                try
                {
                    returnVal = (DHL)serializer.Deserialize(responseStream);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            //#endif

            return returnVal;

        }

        public static Query_OperationCount GetHistoricalDataFromBOBST1(int poID)
        {
            Query_OperationCount returnVal = null;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
                        string.Format("http://{0}/SQLWEB?query=SELECT+%0D%0A+++++++Operation.Ref+as+Order_Number%2C%0D%0A+++++++SUM(mc5b_Event.D_JOBUNITCOUNT)%0D%0AFROM+Job+%0D%0A++JOIN+Operation+ON+Job.JobId+%3D+Operation.JobId+%0D%0A++JOIN+Event+ON+Operation.OperationId+%3D+Event.OperationId++%0D%0A++JOIN+mc5b_Event+ON+Event.EventId+%3D+mc5b_Event.EventId+%0D%0AWHERE+mc5b_EVENT.D_JOBUNITCOUNT+%3E%3D0+AND+%0D%0Aoperation.Ref+%3D'{1}'%0D%0A++%0D%0AGROUP+BY+%0D%0A+++++++++Operation.Ref%0D%0A&dateformat=dd.MMM.yyyy%20hh:mm:ss&output=xml_v2",
                        ConfigurationManager.AppSettings["BOBST1_IPAddressAndPort"], poID));
            //request.Method = "POST";
            request.ContentType = "text/xml";
            request.Timeout = 2000;
            HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            request.CachePolicy = noCachePolicy;

            //#if !DEBUG
            using (var response = (HttpWebResponse)request.GetResponse())
            {

                Stream responseStream = response.GetResponseStream();
                var serializer = new XmlSerializer(typeof(Query_OperationCount));

                try
                {
                    returnVal = (Query_OperationCount)serializer.Deserialize(responseStream);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            //#endif
            return returnVal;

        }


        public static OdPBag GetCurOdP()
        {
            DHL returnVal = null;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
                        string.Format("http://{0}/DHLWEB?displaystyle=multivar&smallvarinfo=yes&output=xml_v2", ConfigurationManager.AppSettings["BOBST1_IPAddressAndPort"]));
            //request.Method = "POST";
            request.ContentType = "text/xml";
            request.Timeout = 2000;
            HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            request.CachePolicy = noCachePolicy;

            //#if !DEBUG

            using (var response = (HttpWebResponse)request.GetResponse())
            {

                Stream responseStream = response.GetResponseStream();
                var serializer = new XmlSerializer(typeof(DHL));

                try
                {
                    returnVal = (DHL)serializer.Deserialize(responseStream);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            //#endif
            string result = returnVal.Var.FirstOrDefault(d => d.Prefix == "Bobst_Machine" && d.Suffix == "OperationRef").Value;
            int curOdp = -1;
            Int32.TryParse(result, out curOdp);
            return new OdPBag { Id = curOdp };

        }

        public static OdPBag GetHistoricalDataFromBOBST(int poID, int quantity)
        {
            Query_OperationCount returnVal = null;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
                        string.Format("http://{0}/SQLWEB?query=SELECT+%0D%0A+++++++Operation.Ref+as+Order_Number%2C%0D%0A+++++++SUM(mc5b_Event.D_JOBUNITCOUNT)%0D%0AFROM+Job+%0D%0A++JOIN+Operation+ON+Job.JobId+%3D+Operation.JobId+%0D%0A++JOIN+Event+ON+Operation.OperationId+%3D+Event.OperationId++%0D%0A++JOIN+mc5b_Event+ON+Event.EventId+%3D+mc5b_Event.EventId+%0D%0AWHERE+mc5b_EVENT.D_JOBUNITCOUNT+%3E%3D0+AND+%0D%0Aoperation.Ref+%3D'{1}'%0D%0A++%0D%0AGROUP+BY+%0D%0A+++++++++Operation.Ref%0D%0A&dateformat=dd.MMM.yyyy%20hh:mm:ss&output=xml_v2",
                        ConfigurationManager.AppSettings["BOBST1_IPAddressAndPort"], poID));
            //request.Method = "POST";
            request.ContentType = "text/xml";
            request.Timeout = 2000;
            HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            request.CachePolicy = noCachePolicy;

            //#if !DEBUG
            using (var response = (HttpWebResponse)request.GetResponse())
            {

                Stream responseStream = response.GetResponseStream();
                var serializer = new XmlSerializer(typeof(Query_OperationCount));

                try
                {
                    returnVal = (Query_OperationCount)serializer.Deserialize(responseStream);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            //#endif

            Record result = returnVal.Record;
            int copieLav = 0;
            if (result != null)
            {
                Int32.TryParse(result.SUM, out copieLav);
            }
            return new OdPBag { Id = poID, CopieRichieste = quantity, CopieLavorate = copieLav };

        }



    }

}
