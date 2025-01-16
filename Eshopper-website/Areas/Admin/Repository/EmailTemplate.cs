using Eshopper_website.Models;

namespace Eshopper_website.Areas.Admin.Repository
{
    public static class EmailTemplates
    {
        public static string GetOrderConfirmationEmail(account order)
        {
            var index = 1;
            //var orderDetail = "";

            if (order != null)
            {
                //foreach(var item in order.OrderDetails as List<OrderDetail> ?? [])
                //{
                //    orderDetail += $@"
                //    <tr>
                //        <td>{index}</td>                        
                //        <td>
                //            <img src='~/images/product-details/{item?.Product?.PRO_Image}' alt='{item?.Product?.PRO_Name}' width='100px'/>
                //        </td>
                //        <td>{item?.Product?.PRO_Name}</td>
                //        <td>{item?.ORDE_Quantity}</td>                        
                //        <td>{item?.Product?.PRO_Price.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-US"))}</td>
                //        <td>{(item.ORDE_Quantity * item.ORDE_Price).ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en - US"))}</td>
                //    </tr>
                //    ";
                //    index++;
                //}

               return $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                        <h2 style='color: #333;'>Order Confirmed #{order.ORD_OrderCode}</h2>
                        <p>Welcome {order?.Member?.MEM_FirstName} {order?.Member?.MEM_LastName} to EShopper.</p>
                        <p>Thank you for buying at EShopper. Your order had been confirmed.</p>
                
                        <div style='background-color: #f8f9fa; padding: 15px; margin: 15px 0;'>
                            <h3 style='color: #333; margin-top: 0;'>Order Details:</h3>
                            <div>
                                <table style='border: 1px solid; width: 100%'>
                                    <thead>
                                        <tr>
                                            <th style='border: 1px solid;'>No.</th>
                                            <th style='border: 1px solid;'>Name</th>
                                            <th style='border: 1px solid;'>Quantity</th>
                                            <th style='border: 1px solid;'>Price</th>
                                            <th style='border: 1px solid;'>Total</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {string.Join("", order.OrderDetails!.Select((item, idx) => $@"
                                            <tr>
                                                <td style='border: 1px solid;'>{idx + 1}</td> 
                                                <td style='border: 1px solid;'>{item?.Product?.PRO_Name}</td>
                                                <td style='border: 1px solid;'>{item?.ORDE_Quantity}</td>                        
                                                <td style='border: 1px solid;'>{item?.Product?.PRO_Price.ToString("C0", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"))}</td>
                                                <td style='border: 1px solid;'>{(item.ORDE_Quantity * item.ORDE_Price).ToString("C0", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"))}</td>
                                            </tr>
                                            "))}
                                    </tbody>
                                    <tfoot>
                                        <tr>
                                            <td colspan='4' style='border: 1px solid;'>Shipping Cost</td>
                                            <td style='border: 1px solid;'>{order.ORD_ShippingCost.ToString("C0", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"))}</td>
                                        </tr>
                                        <tr>
                                            <td colspan='4' style='border: 1px solid;'>Total price</td>
                                            <td style='border: 1px solid;'>{order.ORD_TotalPrice.ToString("C0", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"))}</td>
                                        </tr>
                                    </tfoot>
                                </table>
                            </div>
                        </div>
                        <div style='margin: 15px 0;'>
                            <h3 style='color: #333;'>Information Delivery:</h3>
                            <p>Receiver: {order?.Member?.MEM_FirstName} {order?.Member.MEM_LastName}</p>
                            <p>Phone number: {order?.Member.MEM_Phone}</p>
                            <p>Address: </p>
                            <p>Payment method: Cash</p>
                        </div>
                        <p>We will contact to you when the order have been delivered.</p>
                        <p style='color: #666;'>If you have any answer, please contact to us.</p>
                        <p style='color: #666;'>Best Regard,<br/>EShopper Team</p>
                    </div>";
            }
            else
            {
                return "";
            }
        }

        public static string GetResetPasswordEmail(string fullName, string newPassword)
        {
            return $@"
            <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                <h2 style='color: #333;'>Đặt lại mật khẩu</h2>
                
                <p>Xin chào {fullName},</p>
                
                <p>Chúng tôi nhận được yêu cầu đặt lại mật khẩu của bạn.</p>
                
                <div style='background-color: #f8f9fa; padding: 15px; margin: 15px 0;'>
                    <p>Mật khẩu mới của bạn là: <strong>{newPassword}</strong></p>
                </div>

                <p>Vui lòng đăng nhập và đổi mật khẩu mới ngay sau khi nhận được email này.</p>
                
                <p style='color: #666;'>Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng liên hệ với chúng tôi ngay.</p>
                
                <p style='color: #666;'>Trân trọng,<br/>ShoeStore Team</p>
            </div>";
        }
    }
}
