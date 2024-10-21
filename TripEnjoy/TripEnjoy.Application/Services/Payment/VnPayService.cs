using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Data;
using TripEnjoy.Application.Helper;
using TripEnjoy.Application.Interface.Payment;

namespace TripEnjoy.Application.Services.Payment
{
    public class VnPayService : IVnPayServices
    {
        private readonly IConfiguration configuration;

        public VnPayService(IConfiguration configuration)
        {
           this.configuration = configuration;
        }
        public string CreatePaymentUrl(HttpContext context, VnPaymentRequestDTO vnPaymentRequest)
        {
            var tick = DateTime.Now.Ticks.ToString();
            var vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", configuration["VnPay:Version"]);
            vnpay.AddRequestData("vnp_Command", configuration["VnPay:Command"]);
            vnpay.AddRequestData("vnp_TmnCode", configuration["VnPay:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", (vnPaymentRequest.Amount * 100).ToString("0"));     
            vnpay.AddRequestData("vnp_CreateDate", vnPaymentRequest.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", configuration["VnPay:Currency"]);
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", configuration["VnPay:Locale"]);
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán cho đơn hàng: " + vnPaymentRequest.BookingId);
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", configuration["VnPay:ReturnUrl"]);
            vnpay.AddRequestData("vnp_TxnRef", tick);

            var paymentUrl = vnpay.CreateRequestUrl(configuration["VnPay:Url"], configuration["VnPay:HashSecret"]);
            return paymentUrl;
        }

        public VnPaymentResponseDTO PaymentExcute(IQueryCollection collections)
        {
          var vnpay = new VnPayLibrary();
            foreach (var (key,value) in collections)
            {
               if(!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }
            var oderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
            var vnp_TransactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            string vnp_SecureHash = collections.FirstOrDefault(x => x.Key == "vnp_SecureHash").Value;
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");
            bool checkSingature = vnpay.ValidateSignature(vnp_SecureHash, configuration["VnPay:HashSecret"]);
            if (!checkSingature)
            {
                return new VnPaymentResponseDTO
                {
                    Success = false,
                };
            }
            return new VnPaymentResponseDTO
            {
                Success = true,
                PaymentMethod = "VnPay",
                OrderDescription = vnp_OrderInfo,
                OrderId = oderId.ToString(),
                PaymentId = vnp_TransactionId.ToString(),
                TransactionId = vnp_TransactionId.ToString(),
                Token = vnp_SecureHash,
                VnPayResponseCode = vnp_ResponseCode
            };
        }
    }
}
