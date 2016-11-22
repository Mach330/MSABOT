using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Weather_Bot.Models;
using System.Collections.Generic;
using System.Web.Services.Description;

namespace Weather_Bot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                //get user to sign in if they haven't already
                //greet with personal sign in message
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));


                //add option to find a transaction

                //'pay' someone/an account

                //add option to make an appointment
                if (activity.Text == "hello" || activity.Text == "hi")
                {
                    Activity reply = activity.CreateReply($"Hello");
                    await connector.Conversations.ReplyToActivityAsync(reply);
                }
                else if (activity.Text.Contains("shit") || activity.Text.ToLower().Contains("fuck"))
                {
                    Activity reply = activity.CreateReply($"Please watch your language");
                    connector.Conversations.ReplyToActivity(reply);
                }
                else if (activity.Text.ToLower().Contains("conver"))
                {
                    Activity reply = activity.CreateReply($"make conversion calls and stuffs here");
                    connector.Conversations.ReplyToActivity(reply);

                    //use of cards to select currencies or amount

                    //want to link to my currency converter
                    //have to call API from within BOT
                }
                else if (activity.Text.ToLower().Contains("trans"))
                {
                    Activity reply = activity.CreateReply($"transfer money here");
                    connector.Conversations.ReplyToActivity(reply);

                        //get accounts of transfer (to/from)

                        //acount from (cards)

                        //acount to (cards)

                        //get amount

                        //ask for confirmation using a card to check the transfer details

                        //transfer successful or transfer failed

                }
                else if (activity.Text.ToLower().Contains("view"))
                {
                    //view accounts on cards
                    //one card per account
                    //shows amount, account number, 

                    //buttons for transaction history
                    //button to start transfer
                    //button to cancel

                    Activity replyToConversation = activity.CreateReply("Should go to conversation, with a thumbnail card");
                    replyToConversation.Recipient = activity.From;
                    replyToConversation.Type = "message";
                    replyToConversation.Attachments = new List<Attachment>();
                    System.Collections.Generic.List<CardImage> cardImages = new List<CardImage>();
                    cardImages.Add(new CardImage(url: "https://<ImageUrl1>"));
                    System.Collections.Generic.List<CardAction> cardButtons = new List<CardAction>();
                    CardAction plButton = new CardAction()
                    {
                        Value = "https://en.wikipedia.org/wiki/Pig_Latin",
                        Type = "openUrl",
                        Title = "WikiPedia Page"
                    };
                    cardButtons.Add(plButton);
                    ThumbnailCard plCard = new ThumbnailCard()
                    {
                        Title = "I'm a thumbnail card",
                        Subtitle = "Pig Latin Wikipedia Page",
                        Images = cardImages,
                        Buttons = cardButtons
                    };
                    Attachment plAttachment = plCard.ToAttachment();
                    replyToConversation.Attachments.Add(plAttachment);
                    var reply = await connector.Conversations.SendToConversationAsync(replyToConversation);
                }
                else
                {

                    WeatherObject.RootObject rootObject;

                    // Console.WriteLine(activity.Attachments[0].ContentUrl);

                    HttpClient client = new HttpClient();
                    string x = await client.GetStringAsync(new Uri("http://api.openweathermap.org/data/2.5/weather?q=" + activity.Text + "&units=metric&APPID=440e3d0ee33a977c5e2fff6bc12448ee"));

                    rootObject = JsonConvert.DeserializeObject<WeatherObject.RootObject>(x);

                    string cityName = rootObject.name;
                    string temp = rootObject.main.temp + "°C";
                    string pressure = rootObject.main.pressure + "hPa";
                    string humidity = rootObject.main.humidity + "%";
                    string wind = rootObject.wind.deg + "°";

                    // return our reply to the user
                    Activity reply = activity.CreateReply($"Current weather for {cityName} is {temp}, pressure {pressure}, humidity {humidity}, and wind speeds of {wind}");
                    await connector.Conversations.ReplyToActivityAsync(reply);
                }
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}