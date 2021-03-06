' *
' * Bluepay VB.NET Sample code.
' *
' * This code sample runs a $0.00 AUTH transaction
' * against a customer using test payment information.
' * This stores the customer's payment information securely in
' * BluePay to be used for further transactions.
' * Note: THIS DOES NOT ENSURE THAT THE CREDIT CARD OR ACH
' * ACCOUNT IS VALID.
' *


Imports System
Imports vbnet.BPVB

Namespace Transactions

    Public Class StorePaymentInformation

        Public Shared Sub run()

            Dim accountID As String = "Merchant's Account ID Here"
            Dim secretKey As String = "Merchant's Secret Key Here"
            Dim mode As String = "TEST"

            Dim payment As BluePay = New BluePay(
                accountID,
                secretKey,
                mode
            )

            payment.setCustomerInformation(
                firstName:="Bob",
                lastName:="Tester",
                address1:="123 Test St.",
                address2:="Apt #500",
                city:="Testville",
                state:="IL",
                zipCode:="54321", 
                country:="USA",
                phone:="123-123-12345",
                email:="test@bluepay.com"
            )

            payment.setCCInformation(
                ccNumber:="4111111111111111", 
                ccExpiration:="1215", 
                cvv2:="123" 
            )

            payment.auth("0.00")
            
            payment.process()

            If payment.isSuccessfulTransaction() Then
                Console.Write("Transaction Status: " + payment.getStatus() + Environment.NewLine)
                Console.Write("Transaction Message: " + payment.getMessage() + Environment.NewLine)
                Console.Write("Transaction ID: " + payment.getTransID() + Environment.NewLine)
                Console.Write("AVS Result: " + payment.getAVS() + Environment.NewLine)
                Console.Write("CVV2 Result: " + payment.getCVV2() + Environment.NewLine)
                Console.Write("Masked Payment Account: " + payment.getMaskedPaymentAccount() + Environment.NewLine)
                Console.Write("Card Type: " + payment.getCardType() + Environment.NewLine)
                Console.Write("Authorization Code: " + payment.getAuthCode() + Environment.NewLine)
            Else
                Console.Write("Transaction Error: " + payment.getMessage() + Environment.NewLine)
            End If
        End Sub
    End Class
End Namespace