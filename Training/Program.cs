using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.ProjectOxford.Face.Contract;
using Newtonsoft.Json;
using Common = Microsoft.ProjectOxford.Common;
using FaceAPI = Microsoft.ProjectOxford.Face;
using VisionAPI = Microsoft.ProjectOxford.Vision;


namespace Training
{
    class Program
    {
        static void Main(string[] args)
        {
            await Train();
        }
        public static async Task Train()
        {
           
            FaceAPI.FaceServiceClient _faceClient = new FaceAPI.FaceServiceClient("952a5cbdd78845948079fe7e2c61807d","https://australiaeast.api.cognitive.microsoft.com/face/v1.0");
             
            string personGroupId = "myfriendsServtechAPJ";
            await _faceClient.CreatePersonGroupAsync(personGroupId, "My Friends");

            // Define Jose
            CreatePersonResult Josefriend = await _faceClient.CreatePersonAsync(
                // Id of the PersonGroup that the person belonged to
                personGroupId,
                // Name of the person
                "Jose"
            );

            // Define Jose
            CreatePersonResult Aaronfriend = await _faceClient.CreatePersonAsync(
                // Id of the PersonGroup that the person belonged to
                personGroupId,
                // Name of the person
                "Aaron"
            );

            // Directory contains image files of Anna
            const string JoseImageDir = @"C:\Users\joseo\source\repos\FutureofWork-ServTechAPJ2018\FutureofWork-ServTech2018APJ\LiveCameraSample\Pictures\Jose\";

            foreach (string imagePath in Directory.GetFiles(JoseImageDir, "*.jpg"))
            {
                using (Stream s = File.OpenRead(imagePath))
                {
                    // Detect faces in the image and add to Anna
                    await _faceClient.AddPersonFaceAsync(
                        personGroupId, Josefriend.PersonId, s);
                }
            }
            // Do the same for Bill and Clare

            const string AaronImageDir = @"C:\Users\joseo\source\repos\FutureofWork-ServTechAPJ2018\FutureofWork-ServTech2018APJ\LiveCameraSample\Pictures\Aaron\";

            foreach (string imagePath in Directory.GetFiles(AaronImageDir, "*.jpg"))
            {
                using (Stream s = File.OpenRead(imagePath))
                {
                    // Detect faces in the image and add to Anna
                    await _faceClient.AddPersonFaceAsync(
                        personGroupId, Aaronfriend.PersonId, s);
                }
            }
            // Do the same for Bill and Clare


            await _faceClient.TrainPersonGroupAsync(personGroupId);

            FaceAPI.Contract.TrainingStatus trainingStatus = null;
            while (true)
            {
                trainingStatus = await _faceClient.GetPersonGroupTrainingStatusAsync(personGroupId);

                if (trainingStatus.Status != Status.Running)
                {
                    break;
                }

                await Task.Delay(1000);
            }

            Console.Write("Training successfully completed");
        }
        
    }
}
