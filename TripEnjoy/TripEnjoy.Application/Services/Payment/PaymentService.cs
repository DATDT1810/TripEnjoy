using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Data;
using TripEnjoy.Application.Interface;
using TripEnjoy.Application.Interface.EmailService;
using TripEnjoy.Application.Interface.Payment;
using TripEnjoy.Application.Services.Email;

namespace TripEnjoy.Application.Services.Payment
{
	public class PaymentService : IPaymentService
	{
		private readonly IPaymentRepository paymentRepository;
		private readonly IAccountRepository _accountRepository;
		private readonly IEmailService emailService;
		public PaymentService(IPaymentRepository paymentRepository, IAccountRepository accountRepository, IEmailService emailService)
		{
			this.paymentRepository = paymentRepository;
			this._accountRepository = accountRepository;
			this.emailService = emailService;
		}
		public async Task CreatePayment(Domain.Models.Booking booking)
		{
			// new payment history
			await paymentRepository.CreatePayment(booking);


			// mail settting for user
			MailRequest mailRequest = new MailRequest();
			var accountUser = await _accountRepository.GetAccountById(booking.AccountId);
			mailRequest.ToEmail = accountUser.AccountEmail;
			mailRequest.Subject = "Notification";
			string content = "You have payment successfull";
			mailRequest.Body = emailService.GetNotificationHtmlContent(content); 
			await emailService.SendEmailAsync(mailRequest);


            //// setting for partner
            //MailRequest mailRequest2 = new MailRequest();
            //var accountPartner = await _accountRepository.GetAccountById(booking.AccountId);
            //mailRequest2.ToEmail = accountPartner.AccountEmail;
            //mailRequest2.Subject = "Notification";
            //string content2 = "You have payment successfull";
            //mailRequest2.Body = emailService.GetNotificationHtmlContent(content2);
            //await emailService.SendEmailAsync(mailRequest2);
        }

	}
}
