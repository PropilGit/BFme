using BFme.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BFme.Services
{
    public class FtpController : IFileController
    {

        private Access access;

        public bool Configure(Access access)
        {
            try
            {
                if (this.access != null) return true;
                if (access == null) return false;

                this.access = access;
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public bool Upload(string name, byte[] bytes)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + access.Server + "/files/" + name);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                request.Credentials = new NetworkCredential(access.Login, access.Password);
                //request.EnableSsl = true;

                Stream ftpStream = request.GetRequestStream();
                ftpStream.Write(bytes, 0, bytes.Length);

                ftpStream.Close();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public byte[] Download(string name)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + access.Server + "/files/" + name);
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                request.Credentials = new NetworkCredential(access.Login, access.Password);
                //request.EnableSsl = true;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                MemoryStream mStream = new MemoryStream();
                response.GetResponseStream().CopyTo(mStream);

                byte[] bytes = mStream.ToArray();

                mStream.Close();
                response.Close();

                return bytes;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
