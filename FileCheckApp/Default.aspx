<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FileCheckApp._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <div class="row">
         <section class="col-md-12">
             <h2>Directory Reader</h2>
             <asp:TextBox ID="DirectoryPathTextBox" runat="server" placeholder="Enter relative directory path"></asp:TextBox>
             <asp:Button ID="CheckDirectoryButton" runat="server" Text="Read Directory" OnClick="CheckDirectoryButton_Click" />
             <asp:Literal ID="DirectoryContents" runat="server"></asp:Literal>
         </section>
     </div>
    </main>

</asp:Content>
