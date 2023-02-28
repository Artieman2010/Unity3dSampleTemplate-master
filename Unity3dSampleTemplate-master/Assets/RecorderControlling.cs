using OPS.AntiCheat.Prefs;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEditor;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;
using UnityEngine;

using System.IO;
using System.Threading;
using System;
using System.Net;
using Google;
using Google.Apis;
using Google.Apis.Auth;
using Google.Apis.Drive;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Drive.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;

public class RecorderControlling : MonoBehaviour
{
    RecorderController TestRecorderController;
    public bool whentheGameEnds = false;
    MovieRecorderSettings videoRecorder;
    public string filePathy;
    // Start is called before the first frame update
    void Start()
    {

        var controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
        TestRecorderController = new RecorderController(controllerSettings);
        TestRecorderController.PrepareRecording();
        videoRecorder = ScriptableObject.CreateInstance<MovieRecorderSettings>();
        videoRecorder.name = "My Video Recorder" + UnityEngine.Random.Range(0, 1010101010000000);
        videoRecorder.Enabled = true;
        videoRecorder.VideoBitRateMode = VideoBitrateMode.Medium;

        videoRecorder.ImageInputSettings = new GameViewInputSettings
        {
            OutputWidth = 640,
            OutputHeight = 480
        };

        videoRecorder.AudioInputSettings.PreserveAudio = true;
        //videoRecorder.OutputFile; // Change this to change the output file name (no extension)
        filePathy = Application.dataPath + (ProtectedPlayerPrefs.GetFloat("SavedRecorderIndex") + 1) + "videofilelol";
        videoRecorder.OutputFile = filePathy;

        controllerSettings.AddRecorderSettings(videoRecorder);
        controllerSettings.SetRecordModeToFrameInterval(0, 59); // 2s @ 30 FPS
        controllerSettings.FrameRate = 30;

        RecorderOptions.VerboseMode = false;
        TestRecorderController.StartRecording();

    }

    // Update is called once per frame
    void Update()
    {
        if (whentheGameEnds)
        {


            TestRecorderController.StopRecording();
            UploadBasic.DriveUploadBasic(filePathy);
            //DriveUploadBasic(filePathy);

            var controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
            TestRecorderController = new RecorderController(controllerSettings);
            TestRecorderController.PrepareRecording();
            videoRecorder = ScriptableObject.CreateInstance<MovieRecorderSettings>();

        }
    }
   
 
}
public class UploadBasic
{
    /// <summary>
    /// Upload new file.
    /// </summary>
    /// <param name="filePath">Image path to upload.</param>
    /// <returns>Inserted file metadata if successful, null otherwise.</returns>
    public static string DriveUploadBasic(string filePath)
    {
        try
        {
            /* Load pre-authorized user credentials from the environment.
             TODO(developer) - See https://developers.google.com/identity for
             guides on implementing OAuth2 for your application. */
            GoogleCredential credential = GoogleCredential.GetApplicationDefault()
                .CreateScoped(DriveService.Scope.Drive);

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "Drive API Snippets"
            });

            // Upload file photo.jpg on drive.
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = "CheckFile.mp4"
            };
            FilesResource.CreateMediaUpload request;
            // Create a new file on drive.
            using (var stream = new FileStream(filePath,
                       FileMode.Open))
            {
                // Create a new file, with metadata and stream.
                request = service.Files.Create(
                    fileMetadata, stream, "video/mp4");
                request.Fields = "id";
                request.Upload();
            }

            var file = request.ResponseBody;
            // Prints the uploaded file id.
            Console.WriteLine("File ID: " + file.Id);
            return file.Id;
        }
        catch (Exception e)
        {
            // TODO(developer) - handle error appropriately
            if (e is AggregateException)
            {
                Debug.LogError("Credential Not found");
            }
            else if (e is FileNotFoundException)
            {
                Debug.LogError("File not found");
            }
            else
            {
                throw;
            }
        }
        return null;
    }
}


