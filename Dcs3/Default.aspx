<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Dcs3._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
    <asp:Button ID="Button1" runat="server" Text="GetUsernameAndPassword" OnClick="Button1_Click" />
     &ensp;
        <p ><asp:Label ID="Label1" runat="server" Text=""></asp:Label> </p>

    <asp:Button ID="Button2" runat="server" Text="SuppliersGain" OnClick="Button2_Click" />
      
     <p ><asp:Label ID="Label2" runat="server" Text=""></asp:Label> </p>
     <p ><asp:Label ID="Label3" runat="server" Text=""></asp:Label> </p>
     <p ><asp:Label ID="Label4" runat="server" Text=""></asp:Label> </p>
         </div>

</asp:Content>