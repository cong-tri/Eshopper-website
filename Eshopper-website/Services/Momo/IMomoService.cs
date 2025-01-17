using Eshopper_website.Models;
using Eshopper_website.Models.Momo;

namespace Eshopper_website.Services.Momo
{
	public interface IMomoService
	{
		Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfo model);
		MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
	}
}
