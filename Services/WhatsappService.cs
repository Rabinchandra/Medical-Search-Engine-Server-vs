using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

public class WhatsAppService
{
    public void SendWhatsAppMessage(string recipientPhoneNumber, string patientName, string doctorName, string meetingUrl, string appointmentDate, string appointmentTime)
    {
        try
        {
            const string accountSid = "AC066520972e7969b13dfbc198a19a58d6";
            const string authToken = "f64edcaf1df4197e332553f5a6e23d54";

            TwilioClient.Init(accountSid, authToken);

            var messageOptions = new CreateMessageOptions(
                new PhoneNumber("whatsapp:" + recipientPhoneNumber))
            {
                From = new PhoneNumber("whatsapp:+14155238886"),
                Body = $"Dear {patientName}, We are pleased to confirm your upcoming appointment at Guardian Angel Hospital. Please find the details below:\n*Date*: {appointmentDate}\n*Time*: {appointmentTime}\n\t\n*Doctor*: {doctorName}\n*Location*: Guardian Angel Hospital\n*Virtual Meeting URL*: https://localhost:4200/{meetingUrl}"
            };

            var message = MessageResource.Create(messageOptions);

            Console.WriteLine($"Message sent: SID {message.Sid}");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
}