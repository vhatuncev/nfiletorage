using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FileStorage;

namespace WebExample.FileStorageImg
{
    public partial class Render : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string dataIdentifierString = Request.QueryString["dataIdentifier"];
            string fileStorageName = Request.QueryString["fileStorage"];

            if (!string.IsNullOrEmpty(dataIdentifierString) && !string.IsNullOrEmpty(fileStorageName))
            {
                var dataIdentifier = new Guid(this.Request.QueryString["dataIdentifier"] as string);

                Response.ContentType = "image/jpeg";

                Response.Buffer = true;
                Response.Clear();
                byte[] bytes = FileStorageFacade.GetFileByteData(fileStorageName, dataIdentifier);
                Response.OutputStream.Write(bytes, 0, bytes.Length);
                Response.OutputStream.Flush();
                Response.End();
            }
        }
    }
}
