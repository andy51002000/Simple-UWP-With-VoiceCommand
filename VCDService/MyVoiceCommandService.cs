using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.VoiceCommands;
using Windows.ApplicationModel.Resources.Core;
using Windows.ApplicationModel.AppService;
using Windows.Storage;

namespace VCDService
{
    public sealed class MyVoiceCommandService:IBackgroundTask
    {
        private BackgroundTaskDeferral serviceDefferral;
        VoiceCommandServiceConnection voiceCommandServiceConnection;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            this.serviceDefferral = taskInstance.GetDeferral();

            taskInstance.Canceled += OnTaskCanceled;

            var triggerDetails = taskInstance.TriggerDetails as AppServiceTriggerDetails;

            if(triggerDetails!=null && triggerDetails.Name == "MyVoiceCommandService")
            {
                voiceCommandServiceConnection =
                    VoiceCommandServiceConnection.FromAppServiceTriggerDetails(triggerDetails);

                voiceCommandServiceConnection.VoiceCommandCompleted += OnVoiceCommandCompleted;

                VoiceCommand voiceCommand = await voiceCommandServiceConnection.GetVoiceCommandAsync();

                switch(voiceCommand.CommandName)
                {
                    case "findMovie":

                        string moviceName =
                            voiceCommand.Properties["MovieName"].FirstOrDefault();

                        await SendCompletionMessage(moviceName);
                        break;

                    default:

                        break;
                }
            }
        }

        private async Task SendCompletionMessage(string moviceName)
        {
            //Message
            VoiceCommandUserMessage userMessage = new VoiceCommandUserMessage()
            {
                DisplayMessage = "Here you are",
                SpokenMessage = "Here you are"
            };

            //Tile
            VoiceCommandContentTile contentTile = new VoiceCommandContentTile()
            {
                ContentTileType = VoiceCommandContentTileType.TitleWith280x140Icon,
                Title = moviceName,
                Image = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///Images/IMDB_Logo.png"))
            };

            VoiceCommandResponse response =
                VoiceCommandResponse.CreateResponse(
                    userMessage, new List<VoiceCommandContentTile>() { contentTile }
                    );

            await voiceCommandServiceConnection.ReportSuccessAsync(response);
        }

        private void OnVoiceCommandCompleted(VoiceCommandServiceConnection sender, VoiceCommandCompletedEventArgs args)
        {
            if (this.serviceDefferral != null)
            {
                this.serviceDefferral.Complete();
            }
        }

        private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            System.Diagnostics.Debug.WriteLine("Task canceled");
            if(this.serviceDefferral!= null)
            {
                this.serviceDefferral.Complete();
            }
        }

 
    }
}
