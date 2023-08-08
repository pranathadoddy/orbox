using Framework.RepositoryContract;
using Framework.ServiceContract.Response;
using Orderbox.RepositoryContract.Common;
using Orderbox.RepositoryContract.Transaction;
using Orderbox.ServiceContract.Payment;
using Orderbox.ServiceContract.Payment.Request;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xendit.net;
using Xendit.net.Enum;
using Xendit.net.Exception;
using Xendit.net.Model.Invoice;
using Xendit.net.Network;
using Xendit.net.Struct;

namespace Orderbox.Service.Payment
{
    public class XenditHandler : IXenditHandler
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;

        public XenditHandler(
            IOrderRepository orderRepository, 
            IPaymentRepository paymentRepository
        )
        {
            this._orderRepository = orderRepository;
            this._paymentRepository = paymentRepository;
        }

        public async Task<GenericResponse<string>> CreatePurchaseAsync(CreatePurchaseRequest request)
        {
            var response = new GenericResponse<string>();

            var paymentResponse = await this._paymentRepository.PagedSearchAsync(new PagedSearchParameter { 
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"tenantId={request.TenantId} and paymentOptionCode=\"PMGW\" and providerName=\"XENDIT\""
            });
            var paymentDto = paymentResponse.Result.First();
            var orderDto = await this._orderRepository.TenantReadAsync(request.TenantId, request.OrderId);

            HttpClient httpClient = new HttpClient();
            NetworkClient networkClient = new NetworkClient(httpClient);
            XenditConfiguration.RequestClient = networkClient;
            XenditConfiguration.ApiKey = paymentDto.ApiKey;

            var currency = Currency.IDR;

            try
            {
                decimal totalCharge = 0;
                var items = new List<ItemInvoice>();
                foreach (var detail in orderDto.OrderItems)
                {
                    var item = new ItemInvoice
                    {
                        Name = detail.ProductName,
                        Quantity = detail.Quantity,
                        Price = (long)detail.ExtTotalPrice
                    };
                    items.Add(item);
                    totalCharge += detail.ExtTotalPrice;
                }

                InvoiceParameter parameter = new InvoiceParameter
                {
                    ExternalId = request.OrderId.ToString(),
                    Amount = (long)totalCharge,
                    Items = items.ToArray(),
                    Currency = currency,
                    PaymentMethods = new InvoicePaymentChannelType[] {
                        InvoicePaymentChannelType.Alfamart,
                        InvoicePaymentChannelType.Bca,
                        InvoicePaymentChannelType.Bni,
                        InvoicePaymentChannelType.BniSyariah,
                        InvoicePaymentChannelType.Bri,
                        InvoicePaymentChannelType.CreditCard,
                        InvoicePaymentChannelType.Dana,
                        InvoicePaymentChannelType.Indomaret,
                        InvoicePaymentChannelType.Linkaja,
                        InvoicePaymentChannelType.Mandiri,
                        InvoicePaymentChannelType.Ovo,
                        InvoicePaymentChannelType.Qris,
                        InvoicePaymentChannelType.Shopeepay
                    },
                    ShouldSendEmail = false
                };

                if (paymentDto.ActiveDurationInMinutes > 0)
                {
                    parameter.InvoiceDuration = paymentDto.ActiveDurationInMinutes * 60;
                }

                InvoiceResponse invoice = await Xendit.net.Model.Invoice.Invoice.Create(parameter);

                response.Data = invoice.InvoiceUrl;

                return response;
            }
            catch (XenditException e)
            {
                response.AddErrorMessage(e.Message);
                return response;
            }
        }
    }
}
