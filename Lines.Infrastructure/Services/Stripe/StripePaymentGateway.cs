using Lines.Application.Features.PaymentMethods.GetAllCreditCardsByUserId.DTOs;
using Lines.Application.Interfaces.Stripe;
using Lines.Domain.Enums;
using Lines.Presentation.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;




namespace Lines.Infrastructure.Services.Stripe
{
    public class StripePaymentGateway : IPaymentGateway
    {
        private readonly StripeOptions _options;
        private readonly ILogger<StripePaymentGateway> _logger;

        public StripePaymentGateway(
            IOptions<StripeOptions> options,
            ILogger<StripePaymentGateway> logger)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // Stripe SDK global configuration
            StripeConfiguration.ApiKey = _options.SecretKey;
        }

        // =========================================================
        // 1) Create PaymentIntent (frontend ياخد client_secret)
        // =========================================================
        public async Task<PaymentIntentResult> CreatePaymentIntentAsync(
            decimal amount,
            string currency,
            string description)
        {
            var service = new PaymentIntentService();

            try
            {
                if (amount <= 0)
                    throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

                if (string.IsNullOrWhiteSpace(currency))
                    throw new ArgumentException("Currency is required.", nameof(currency));

                var createOptions = new PaymentIntentCreateOptions
                {
                    Amount = ConvertAmountToStripe(amount, currency),
                    Currency = currency.ToLowerInvariant(),
                    Description = description,

                    // نسيب Stripe يختار أفضل طريقة دفع متاحة (Cards / ApplePay / GooglePay)
                    AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                    {
                        Enabled = true
                    },

                    Metadata = new Dictionary<string, string>
                    {
                        { "system", "Lines" },
                        { "created_by", "StripePaymentGateway" }
                        // ممكن تزود TripId/PassengerId هنا لما نربطه بالتريب
                    }
                };

                var intent = await service.CreateAsync(createOptions);

                return new PaymentIntentResult
                {
                    Success = true,
                    PaymentIntentId = intent.Id,
                    ClientSecret = intent.ClientSecret,
                    Status = intent.Status,
                    Amount = amount,
                    Currency = currency
                };
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex,
                    "Stripe error while creating PaymentIntent. Code: {Code}, Message: {Message}",
                    ex.StripeError?.Code, ex.StripeError?.Message);

                return new PaymentIntentResult
                {
                    Success = false,
                    ErrorCode = ex.StripeError?.Code,
                    ErrorMessage = ex.StripeError?.Message ?? ex.Message
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unexpected error while creating PaymentIntent.");

                return new PaymentIntentResult
                {
                    Success = false,
                    ErrorMessage = "Unexpected error while creating payment."
                };
            }
        }

        // =========================================================
        // 2) تأكيد عملية الدفع / جلب حالة الـ PaymentIntent
        // (ممكن نستخدمه بعد Webhook أو بعد ما الموبايل يكمّل الـ flow)
        // =========================================================
        public async Task<PaymentConfirmationResult> ConfirmPaymentAsync(string paymentIntentId)
        {
            var service = new PaymentIntentService();

            try
            {
                if (string.IsNullOrWhiteSpace(paymentIntentId))
                    throw new ArgumentException("paymentIntentId is required.", nameof(paymentIntentId));

                // لو محتاجين نعمل Confirm من السيرفر:
                // var intent = await service.ConfirmAsync(paymentIntentId);
                var intent = await service.GetAsync(paymentIntentId);

                bool requiresAction =
                    intent.Status == "requires_action" ||
                    intent.Status == "requires_source_action";

                bool succeeded = intent.Status == "succeeded";

                var result = new PaymentConfirmationResult
                {
                    Success = succeeded,
                    RequiresAction = requiresAction,
                    PaymentIntentId = intent.Id,
                    Status = intent.Status
                };

                if (!succeeded && !requiresAction)
                {
                    result.ErrorMessage =
                        $"PaymentIntent finished with status '{intent.Status}'.";
                }

                return result;
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex,
                    "Stripe error while confirming PaymentIntent {PaymentIntentId}. Code: {Code}, Message: {Message}",
                    paymentIntentId, ex.StripeError?.Code, ex.StripeError?.Message);

                return new PaymentConfirmationResult
                {
                    Success = false,
                    PaymentIntentId = paymentIntentId,
                    ErrorCode = ex.StripeError?.Code,
                    ErrorMessage = ex.StripeError?.Message ?? ex.Message
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unexpected error while confirming PaymentIntent {PaymentIntentId}.",
                    paymentIntentId);

                return new PaymentConfirmationResult
                {
                    Success = false,
                    PaymentIntentId = paymentIntentId,
                    ErrorMessage = "Unexpected error while confirming payment."
                };
            }
        }

        // =========================================================
        // 3) Refund
        // =========================================================
        public async Task<RefundResult> RefundAsync(string chargeId, decimal? amount = null)
        {
            var refundService = new RefundService();

            try
            {
                if (string.IsNullOrWhiteSpace(chargeId))
                    throw new ArgumentException("chargeId is required.", nameof(chargeId));

                var createOptions = new RefundCreateOptions
                {
                    Charge = chargeId
                };

                // لو عايزين نعمل Partial Refund
                if (amount.HasValue)
                {
                    // هنا استخدم نفس العملة اللي اتدفعت بيها (QAR غالباً في المشروع عندك)
                    createOptions.Amount = ConvertAmountToStripe(amount.Value, "QAR");
                }

                var refund = await refundService.CreateAsync(createOptions);

                bool success = refund.Status == "succeeded" || refund.Status == "pending";

                return new RefundResult
                {
                    Success = success,
                    RefundId = refund.Id,
                    Status = refund.Status,
                    ChargeId = chargeId
                };
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex,
                    "Stripe error while refunding charge {ChargeId}. Code: {Code}, Message: {Message}",
                    chargeId, ex.StripeError?.Code, ex.StripeError?.Message);

                return new RefundResult
                {
                    Success = false,
                    ChargeId = chargeId,
                    ErrorCode = ex.StripeError?.Code,
                    ErrorMessage = ex.StripeError?.Message ?? ex.Message
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unexpected error while refunding charge {ChargeId}.",
                    chargeId);

                return new RefundResult
                {
                    Success = false,
                    ChargeId = chargeId,
                    ErrorMessage = "Unexpected error while creating refund."
                };
            }
        }

        // =========================================================
        // Create customer for our user on stripe
        // =========================================================

        public async Task<string> CreateCustomerAsync(string name, string email)  
        {
            var options = new CustomerCreateOptions
            {
                Email = email,
                Name = name
            };

            var service = new CustomerService();
            Customer customer = await service.CreateAsync(options);

            return customer.Id;
        }


        // =========================================================
        // Attach payment method to customer on stripe
        // =========================================================

        public async Task AttachPaymentMethodToCustomerAsync(string paymentMethodId, string customerId)
        {
            var service = new PaymentMethodService();

            // Attach the PaymentMethod to the Customer
            var paymentMethod = await service.AttachAsync(
                paymentMethodId,
                new PaymentMethodAttachOptions
                {
                    Customer = customerId
                }
            );
        }

        // =========================================================
        // Detach payment method from customer on stripe
        // =========================================================
        public async Task DetachPaymentMethodFromCustomerAsync(string paymentMethodId)
        {
            var service = new PaymentMethodService();
            await service.DetachAsync(paymentMethodId);
        }


        // =========================================================
        // Create Payment Method intent >> return client secret to front end to use it on creating a payment method
        // =========================================================

        public async Task<string> CreatePaymentMethodIntent(string customerId, PaymentMethodType type)  
        {
            var options = new SetupIntentCreateOptions
            {
                Customer = customerId,
                PaymentMethodTypes = new List<string> { type.ToString() }
            };

            var service = new SetupIntentService();
            var intent = await service.CreateAsync(options);
            return intent.ClientSecret;
        }


        // =========================================================
        // Get customer default payment method id
        // =========================================================
        public string GetCustomerDefaultPaymentMethodIdByCustomerId(string customerId)
        {
            var service = new CustomerService();
            var customer = service.Get(customerId);

            return customer.InvoiceSettings.DefaultPaymentMethodId;
        }


        // =========================================================
        // Get credit cards by customer id
        // =========================================================

        public List<PaymentGatewayCreditCardDto>? GetCreditCardsByCustomerId(string customerId)
        {
            var service = new PaymentMethodService();
            var options = new PaymentMethodListOptions
            {
                Customer = customerId,
                Type = "card"
            };
            var paymentMethods = service.List(options);

            var result = paymentMethods.Data.Select(pm => new PaymentGatewayCreditCardDto  
            {
                Id = pm.Id,
                Brand = pm.Card.Brand,
                Last4 = pm.Card.Last4,
                ExpMonth = pm.Card.ExpMonth,
                ExpYear = pm.Card.ExpYear,
                IsDefault = false // will be changed in the orchestrator based on this function GetCustomerDefaultPaymentMethodId
            }).ToList();


            return result;
        }


        // =========================================================
        // Set payment method as default
        // =========================================================

        public async Task SetDefaultPaymentMethodAsync(string customerId, string paymentMethodId, CancellationToken cancellationToken = default)
        {
            var customerService = new CustomerService();

            var options = new CustomerUpdateOptions
            {
                InvoiceSettings = new CustomerInvoiceSettingsOptions
                {
                    DefaultPaymentMethod = paymentMethodId
                }
            };

            await customerService.UpdateAsync(customerId, options, cancellationToken: cancellationToken);
        }



        //public async Task<string> CreateExpressConnectedAccountAsync(Guid driverId, CancellationToken ct)
        //{
        //    var service = new AccountService();
        //    var account = await service.CreateAsync(new AccountCreateOptions
        //    {
        //        Type = "express",
        //        Country = "QA",                 // أو EG حسب شغلك
        //        Capabilities = new AccountCapabilitiesOptions
        //        {
        //            Transfers = new AccountCapabilitiesTransfersOptions { Requested = true }
        //        },
        //        Metadata = new Dictionary<string, string>
        //        {
        //            ["system"] = "Lines",
        //            ["driverId"] = driverId.ToString()
        //        }
        //    }, cancellationToken: ct);

        //    return account.Id;
        //}

        public async Task<string> CreateExpressConnectedAccountAsync(
          Guid driverId,
          CancellationToken ct)
        {
            var service = new AccountService();

            var account = await service.CreateAsync(
                new AccountCreateOptions
                {
                    Type = "express",
                    Country = "US", // أو EG / CH حسب السوق
                    BusinessType = "individual",

                    Capabilities = new AccountCapabilitiesOptions
                    {
                        Transfers = new AccountCapabilitiesTransfersOptions
                        {
                            Requested = true
                        }
                    },

                    Metadata = new Dictionary<string, string>
                    {
                        ["system"] = "Lines",
                        ["driverId"] = driverId.ToString()
                    }
                },
                cancellationToken: ct
            );

            return account.Id;
        }


        async Task<string> IPaymentGateway.CreateAccountOnboardingLinkAsync(string accountId, string refreshUrl, string returnUrl, CancellationToken ct)
        {
            var service = new AccountLinkService();
            var link = await service.CreateAsync(new AccountLinkCreateOptions
            {
                Account = accountId,
                RefreshUrl = refreshUrl,
                ReturnUrl = returnUrl,
                Type = "account_onboarding"
            }, cancellationToken: ct);

            return link.Url;
        }



        // =========================================================
        // Helper: تحويل الـ decimal لـ long بالـ format المطلوب من Stripe
        // =========================================================
        private static long ConvertAmountToStripe(decimal amount, string currency)
        {
            var zeroDecimalCurrencies = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "JPY", "KRW"    // إللي مافيهاش كسور
            };

            if (zeroDecimalCurrencies.Contains(currency))
            {
                // Stripe بيتوقع المبلغ بوحدة العملة نفسها (بدون * 100)
                return (long)decimal.Truncate(amount);
            }

            // معظم العملات (QAR, USD, EUR, ...) محتاجة * 100
            return (long)decimal.Round(amount * 100m, 0, MidpointRounding.AwayFromZero);
        }
    }




}
