<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebExample._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        
        <div>
            File container name<br />
            <asp:TextBox ID="FileStorageName_TextBox" runat="server" Text="webexample" Enabled="true"></asp:TextBox><br />
            <br />
            Enter a URL of a <b>JPG</b> file to persist<br />
            <asp:TextBox ID="urlToPersist_TextBox" runat="server" 
                Text="http://" 
                Width="500px"></asp:TextBox><asp:LinkButton ID="example_LinkButton" 
                runat="server" onclick="example_LinkButton_Click">(example)</asp:LinkButton>
            <br />
            <asp:Button ID="Persist_Button" runat="server" Text="Persist in a FileStorage" onclick="Persist_Button_Click" />
            <br />
        </div>
        <br />
        <asp:Label Font-Bold="true" Font-Size="Large" ForeColor="Red" ID="InfoLabel" runat="server" Text="Status: OK"></asp:Label>
        <br />
        <br />
        Current file storage contents (in random order):<br />
        <asp:Repeater ID="FileStorageContentRepeater" runat="server" OnItemDataBound="FileStorageContentRepeater_ItemDataBound">
                <ItemTemplate>
                    <table border="1"><tr><td>
                        <table>
                            <tr>
                                <td><asp:Label ID="dataIdentifier_Label" runat="server" Text=""></asp:Label></td>
                            </tr>
                            <tr>
                                <td><img runat="server" id="contentFromFileStorage_HtmlImage" /></td>
                            </tr>
                        </table>
                    </td></tr></table>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <table border="1"><tr><td>
                        <table>
                            <tr>
                                <td><asp:Label ID="dataIdentifier_Label" runat="server" Text=""></asp:Label></td>
                            </tr>
                            <tr>
                                <td><img runat="server" id="contentFromFileStorage_HtmlImage" /></td>
                            </tr>
                        </table>
                    </td></tr></table>
                </AlternatingItemTemplate>
        </asp:Repeater>

    </form>
</body>
</html>
