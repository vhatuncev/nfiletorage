using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FileStorage;
using System.Web.UI.HtmlControls;
using FileStorage.Enums.Behaviours;

namespace WebExample
{
    public partial class _Default : System.Web.UI.Page
    {
        public int maxAllowed = 8;

        public string fileStorageName
        {
            get 
            { 
                return FileStorageName_TextBox.Text; 
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            FileStorageFacade.Create(fileStorageName, CreateFileStorageBehaviour.IgnoreWhenExists);
            InfoLabel.Text = "";
            RedrawPicturesInFileStorage();
        }

        protected void Persist_Button_Click(object sender, EventArgs e)
        {
            Guid dataIdentifier = Guid.NewGuid();

            if (FileStorageFacade.FileCountBasedUponFileStorageIndexFile(fileStorageName) > maxAllowed)
            {
                foreach (var guid in FileStorageFacade.GetAllDataIdentifiersBasedUponFileStorageIndexFile(fileStorageName))
                {
                    FileStorageFacade.DeleteDataIdentifier(fileStorageName, guid, DeleteFileBehaviour.ThrowExceptionWhenNotExists);
                }

                InfoLabel.Text = InfoLabel.Text + "Storage was wiped to prevent generating too much traffic";
            }

            try
            {
                FileStorageFacade.StoreHttpRequest(fileStorageName, dataIdentifier, urlToPersist_TextBox.Text, null, AddFileBehaviour.ThrowExceptionWhenAlreadyExists, "NFileStorage");
                RedrawPicturesInFileStorage();
                InfoLabel.Text += string.Format("<br>File {0} was persisted.", dataIdentifier);
            } 
            catch (Exception exception)
            {
                InfoLabel.Text = InfoLabel.Text + string.Format("Are you sure the URL was valid?<br><small>Caught exception; {0}</small>", exception.Message);
            }
        }

        private void RedrawPicturesInFileStorage()
        {
            FileStorageContentRepeater.DataSource = FileStorageFacade.GetAllDataIdentifiersBasedUponFileStorageIndexFile(fileStorageName).Take(maxAllowed);
            FileStorageContentRepeater.DataBind();
        }

        protected void FileStorageContentRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            switch (e.Item.ItemType)
            {
                case ListItemType.Item:
                case ListItemType.AlternatingItem:
                    var currentdataIdentifier = (Guid)e.Item.DataItem;
                    var contentFromFileStorage_HtmlImage = e.Item.FindControl("contentFromFileStorage_HtmlImage") as HtmlImage;
                    contentFromFileStorage_HtmlImage.Src = string.Format("~/FileStorageImg/Render.aspx?dataIdentifier={0}&fileStorage={1}", currentdataIdentifier, HttpUtility.UrlEncode(FileStorageName_TextBox.Text));
                    var dataIdentifierLabel = e.Item.FindControl("dataIdentifier_Label") as Label;
                    dataIdentifierLabel.Text = currentdataIdentifier.ToString();
                    break;
            }
        }

        protected void example_LinkButton_Click(object sender, EventArgs e)
        {
            urlToPersist_TextBox.Text = "http://www.prijsvaneenhuis.nl/img/spandoek/NFileStorage_banner.jpg";
            InfoLabel.Text = InfoLabel.Text + string.Format("Url is now selected in the textbox but not yet persist, push the persist button to do so, or enter another URL of a picture");
        }
    }
}
