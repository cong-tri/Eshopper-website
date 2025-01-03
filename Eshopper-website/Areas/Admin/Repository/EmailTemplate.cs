﻿using Eshopper_website.Models;

namespace Eshopper_website.Areas.Admin.Repository
{
    public static class EmailTemplates
    {
        public static string GetOrderConfirmationEmail(Order order)
        {
            var items = string.Join("<br/>", order.OrderDetails.Select(item =>
                $"- {item.Product.PRO_Name} (Quantity: {item.ORDE_Quantity}: {order.ORD_TotalPrice.ToString()}VND"));
            return $@"
            <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                <h2 style='color: #333;'>Xác nhận đơn hàng #{order.ORD_OrderCode}</h2>
                
                <p>Xin chào {order.Member.MEM_FirstName} {order.Member.MEM_LastName},</p>
                
                <p>Cảm ơn bạn đã đặt hàng tại ShoeStore. Đơn hàng của bạn đã được xác nhận.</p>
                
                <div style='background-color: #f8f9fa; padding: 15px; margin: 15px 0;'>
                    <h3 style='color: #333; margin-top: 0;'>Chi tiết đơn hàng:</h3>
                    <p>{items}</p>
                    <hr style='border-top: 1px solid #ddd;'/>
                    <p style='font-weight: bold;'>Tổng tiền: {order.ORD_TotalPrice.ToString("N0")}đ</p>
                </div>

                <div style='margin: 15px 0;'>
                    <h3 style='color: #333;'>Thông tin giao hàng:</h3>
                    <p>Người nhận: {order.Member.MEM_FirstName} {order.Member.MEM_LastName}</p>
                    <p>Số điện thoại: {order.Member.MEM_Phone}</p>
                    <p>Địa chỉ: </p>
                    <p>Phương thức thanh toán: cash</p>
                </div>

                <p>Chúng tôi sẽ thông báo cho bạn khi đơn hàng được gửi đi.</p>
                
                <p style='color: #666;'>Nếu bạn có bất kỳ câu hỏi nào, vui lòng liên hệ với chúng tôi.</p>
                
                <p style='color: #666;'>Trân trọng,<br/>EShopper Team</p>
            </div>";
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
