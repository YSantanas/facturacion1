<%@ Control Language="VB" AutoEventWireup="false" CodeFile="cuadro_trabajando.ascx.vb" Inherits="cuadro_trabajando" %>
<!-- Cuadro Trabajando para que espere el usuario------------------------------------------------------------------------------------------------->

<!-- Static Modal -->
<div class="modal modal-static fade" id="processing-modal" role="dialog" data-backdrop="static" aria-hidden="true">
    <div class="modal-dialog modal-sm">
        <div class="modal-content">
            <div class="modal-body">

                 <table style="width:100%;">
                    <tr>
                        <td align="center">
                            <div class="spinner-border text-primary" style="width: 3rem; height: 3rem;" role="status"></div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height:25px;"></td>
                    </tr>
                    <tr>
                        <td align="center">
                            <h5><asp:Label ID="lbl_mensaje_trabajando" runat="server" CssClass ="text-primary"></asp:Label></h5>
                        </td>
                    </tr>
                </table>

            </div>
        </div>
    </div>
</div>