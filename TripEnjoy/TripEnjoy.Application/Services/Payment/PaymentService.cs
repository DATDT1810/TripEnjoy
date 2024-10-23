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
using TripEnjoy.Application.Interface.TransactionHistories;
using TripEnjoy.Application.Interface.Wallet;
using TripEnjoy.Application.Services.Email;

namespace TripEnjoy.Application.Services.Payment
{
	public class PaymentService : IPaymentService
	{
		private readonly IPaymentRepository paymentRepository;
		private readonly IAccountRepository _accountRepository;
		private readonly IEmailService emailService;
		private readonly IWalletRepository walletRepository;
		private readonly ITransactionHistoryRepository transactionHistory;
        public PaymentService(IPaymentRepository paymentRepository, IAccountRepository accountRepository, IEmailService emailService, IWalletRepository walletRepository, ITransactionHistoryRepository transactionHistory)
		{
			this.paymentRepository = paymentRepository;
			this._accountRepository = accountRepository;
			this.emailService = emailService;
            this.walletRepository = walletRepository;
            this.transactionHistory = transactionHistory;
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


			// setting for partner
			var accountPartner = await _accountRepository.GetAccountByRoomID(booking.RoomId);
			if(accountPartner == null)
			{
                throw new Exception("Partner not found");
            }

            // + money for partner
            var wallet = await walletRepository.GetWalletByAccountId(accountPartner.AccountId);
			wallet.WalletBalance += booking.BookingTotalPrice;
            await walletRepository.UpdateWallet(wallet);

			
			// create transaction history for partner
			var transaction = new TransactionDTO()
			{
				WalletId = wallet.WalletId,
                Amount = booking.BookingTotalPrice,
                IsCredit = true,
                TransactionDate = DateTime.Now,
                Description = "Payment For Booking"
				
            };
            await transactionHistory.CreateTransaction(transaction);

            // send mail to partner
            MailRequest mailRequest2 = new MailRequest();
            mailRequest2.ToEmail = accountPartner.AccountEmail;
			mailRequest2.Subject = "Notification";
			string content2 = "You have new Booking";
			mailRequest2.Body = emailService.GetNotificationHtmlContent(content2);
			await emailService.SendEmailAsync(mailRequest2);
		}

	}
}
