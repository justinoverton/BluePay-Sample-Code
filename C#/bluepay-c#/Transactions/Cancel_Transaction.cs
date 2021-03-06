/*
* BluePay C#.NET Sample code.
*
* This code sample runs a $3.00 Credit Card Sale transaction
* against a customer using test payment information.
* If using TEST mode, odd dollar amounts will return
* an approval and even dollar amounts will return a decline.
*/

using System;
using BluePayLibrary;
using BluePayLibrary.Interfaces;
using BluePayLibrary.Interfaces.BluePay20Post;
using BluePayLibrary.Interfaces.BluePay20Post.Fluent;

namespace Transactions
{
    class CancelTransaction
    {
        public static void Main()
        {
            var accountID = "Merchant's Account ID Here";
            var secretKey = "Merchant's Secret Key Here";
            var mode = Mode.TEST;
            
            var payment = BluePayMessage.Build(accountID, mode)
                //Override duplicate just for tests
                .WithFields(builder => builder.DuplicateOverride(true))
                .ForCustomer(
                    name1: "Bob",
                    name2: "Tester",
                    addr1: "1234 Test St.",
                    addr2: "Apt #500",
                    city: "Testville",
                    state: "IL",
                    zip: "54321",
                    country: "USA",
                    phone: "123-123-12345",
                    email: "test@bluepay.com"
                )
                .FromCreditCard(
                    ccNumber: "4111111111111111",
                    expiration: DateTime.Today.AddYears(1),
                    cvv2: "123"
                )
                .Sale(amount: 3.00m);
            
            var client = new BluePay20PostClient();

            var result = client.Process(payment.ToMessage(secretKey));

            if (!result.IsError && result.IsApproved)
            {
                // Creates a payment cancelation
                var paymentCancel = BluePayMessage.Build(accountID, mode)
                    // Finds the previous payment by ID and attempts to void it
                    .Void(result.TransId);
                
                // Makes the API Request with BluePay to cancel transaction
                var cancelResult = client.Process(paymentCancel.ToMessage(secretKey));

                // Reads the responses from BluePAy
                Console.WriteLine("Transaction Status: " + cancelResult.Status);
                Console.WriteLine("Transaction ID: " + cancelResult.TransId);
                Console.WriteLine("Transaction Message: " + cancelResult.Message);
                Console.WriteLine("AVS Response: " + cancelResult.Avs);
                Console.WriteLine("CVV2 Response: " + cancelResult.Cvv2);
                Console.WriteLine("Masked Payment Account: " + cancelResult.PaymentAccountMask);
                Console.WriteLine("Card Type: " + cancelResult.CardType);
                Console.WriteLine("Authorization Code: " + cancelResult.AuthCode);
            }
            else
            {
                Console.WriteLine("Error: " + result.Message);
            }
        }
    }
}