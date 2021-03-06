using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using EllaSciFair;
using EllaSciFair.Data;
using System.Net.Mail;
using EllaSciFair.Models.Dialog;
using System.Runtime.InteropServices;

namespace EllaSciFair.Pages
{
    public partial class Main : ComponentBase
    {

        TakeANumber? takeANumber;
        SignUp? CurrentPerson;
        List<SignUp>? SignupQueue;
        int progress = 0;
        bool isRecording;
        bool rotate = true;
        protected override void OnAfterRender(bool firstRender)
        {
            SignupQueue = repoSignUp?.GetOpenSignUps()?.ToList();
            takeANumber = repoTakeANum.Get();
            base.OnAfterRender(firstRender);
            StateHasChanged();
        }
        void OnSelectId(SignUp signUp)
        {
            if (takeANumber == null)
            {
                return;
            }
            CurrentPerson = signUp;
            takeANumber.CurrentNumber = signUp.Id;
            repoTakeANum.Update(takeANumber);
            isRecording = false;
            StateHasChanged();
        }
        async void OnRecordVideo()
        {
            isRecording = true;
            progress = 0;
            StateHasChanged();
            if (CurrentPerson == null)
            {
                return;
            }

            //Recording 
            var isLinux = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            if (isLinux)
            {
                await RecordVideo();
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    await Task.Delay(150);
                    progress += 10;
                    StateHasChanged();
                }
            }

            CurrentPerson.FileName = "done";
            repoSignUp.Update(CurrentPerson);
            SignupQueue = repoSignUp?.GetOpenSignUps()?.ToList();
            isRecording = false;
            StateHasChanged();
        }
        async Task RecordVideo()
        {

            progress = 1;
            StateHasChanged();

            await Task.Delay(100);
            var dir = new System.IO.DirectoryInfo("/home/pi/Documents/console2/temp");
            foreach (var file in dir.GetFiles("*.mp4", SearchOption.AllDirectories))
            {
                file.Delete();
            }

            if (!System.IO.Directory.Exists("/home/pi/Documents/console2/temp"))
            {
                System.IO.Directory.CreateDirectory("/home/pi/Documents/console2/temp");
            }
            if (!System.IO.Directory.Exists("/home/pi/Documents/console2/temp/1"))
            {
                System.IO.Directory.CreateDirectory("/home/pi/Documents/console2/temp/1");
            }
            if (!System.IO.Directory.Exists("/home/pi/Documents/console2/temp/2"))
            {
                System.IO.Directory.CreateDirectory("/home/pi/Documents/console2/temp/2");
            }
            if (!System.IO.Directory.Exists("/home/pi/Documents/console2/temp/3"))
            {
                System.IO.Directory.CreateDirectory("/home/pi/Documents/console2/temp/3");
            }

            var proc = new System.Diagnostics.Process();
            proc.StartInfo.WorkingDirectory = "/home/pi/Documents/console2/temp";
            proc.StartInfo.FileName = "libcamera-vid";
            proc.StartInfo.Arguments = "-t 10000 --codec mjpeg --segment 1 -o test%05d.jpg";
            proc.Start();
            proc.WaitForExit();

            progress = 2;
            StateHasChanged();
            await Task.Delay(100);

            progress += 10;
            StateHasChanged();
            await Task.Delay(100);

            var files = dir.GetFiles("*.jpg");
            var first30 = files.OrderBy(f => f.Name).Take(60);
            var slowfiles = files.OrderBy(f => f.Name).Skip(60).Take(190);
            var last30 = files.OrderBy(f => f.Name).Skip(250).Take(60);
            foreach (var file in first30)
            {
                file.MoveTo(System.IO.Path.Combine(dir.FullName, "1", file.Name));
            }
            foreach (var file in slowfiles)
            {
                file.MoveTo(System.IO.Path.Combine(dir.FullName, "2", file.Name));
            }
            foreach (var file in last30)
            {
                file.MoveTo(System.IO.Path.Combine(dir.FullName, "3", file.Name));
            }

            progress += 10;
            StateHasChanged();
            await Task.Delay(100);

            var proc1 = new System.Diagnostics.Process();
            proc1.StartInfo.WorkingDirectory = "/home/pi/Documents/console2/temp/1";
            proc1.StartInfo.FileName = "ffmpeg";
            proc1.StartInfo.Arguments = "-framerate 25 -i test%05d.jpg -c:v libx264 -r 25 output1.mp4";
            proc1.StartInfo.RedirectStandardError = true;
            proc1.Start();
            proc1.WaitForExit();
            if (proc1.ExitCode != 0)
            {
                Console.WriteLine(proc1.StandardError.ReadToEnd());
            }

            progress += 10;
            StateHasChanged();
            await Task.Delay(100);

            var proc2 = new System.Diagnostics.Process();
            proc2.StartInfo.WorkingDirectory = "/home/pi/Documents/console2/temp/2";
            proc2.StartInfo.FileName = "ffmpeg";
            proc2.StartInfo.Arguments = "-framerate 8 -pattern_type glob -i \"*.jpg\" -c:v libx264 -r 25 output2.mp4";
            proc2.StartInfo.RedirectStandardError = true;
            proc2.Start();
            proc2.WaitForExit();
            string error2 = string.Empty;
            if (proc2.ExitCode != 0)
            {
                error2 = proc2.StandardError.ReadToEnd();
                Console.WriteLine(error2);
            }


            progress += 10;
            StateHasChanged();
            await Task.Delay(100);

            var proc3 = new System.Diagnostics.Process();
            proc3.StartInfo.WorkingDirectory = "/home/pi/Documents/console2/temp/3";
            proc3.StartInfo.FileName = "ffmpeg";
            proc3.StartInfo.Arguments = "-framerate 25 -pattern_type glob -i \"*.jpg\" -c:v libx264 -r 25 output3.mp4";
            proc3.StartInfo.RedirectStandardError = true;
            proc3.Start();
            proc3.WaitForExit();
            string error3 = string.Empty;
            if (proc3.ExitCode != 0)
            {
                error3 = proc3.StandardError.ReadToEnd();
                Console.WriteLine(error3);
            }

            foreach (var file in dir.GetFiles("*.jpg", SearchOption.AllDirectories))
            {
                file.Delete();
            }

            foreach (var file in dir.GetFiles("*.mp4", SearchOption.AllDirectories))
            {
                file.MoveTo($"/home/pi/Documents/console2/temp/{file.Name}");
            }

            progress += 10;
            StateHasChanged();
            string concatFiles = "file '/home/pi/Documents/console2/temp/output1.mp4'";
            concatFiles += System.Environment.NewLine + "file '/home/pi/Documents/console2/temp/output2.mp4'";
            concatFiles += System.Environment.NewLine + "file '/home/pi/Documents/console2/temp/output3.mp4'";
            System.IO.File.WriteAllText("/home/pi/Documents/console2/temp/fileList.txt", concatFiles);

            var rotateText = "";
            if (rotate)
            {
                rotateText = " -metadata:s:v rotate=\"-180\" ";
            }

            var procJoin = new System.Diagnostics.Process();
            procJoin.StartInfo.WorkingDirectory = "/home/pi/Documents/console2/temp";
            procJoin.StartInfo.FileName = "ffmpeg";
            procJoin.StartInfo.Arguments = $"-safe 0 -f concat -i fileList.txt {rotateText} -c copy output.mp4";
            procJoin.Start();
            procJoin.WaitForExit();

            System.IO.File.Move("/home/pi/Documents/console2/temp/output.mp4", $"/home/pi/Videos/{CurrentPerson?.Id}.mp4");

            progress = 100;
            StateHasChanged();
            await Task.Delay(100);
        }

        public async void OnEmailAllCompleted()
        {

            var completedSignups = repoSignUp?.GetCompletedSignUps()?.ToList();
            if (completedSignups == null)
            {
                var tcsNone = new TaskCompletionSource<string>();
                PubSub.Hub.Default.Publish<MsgBoxShowEvent>(
                    new MsgBoxShowEvent()
                    {
                        Buttons = DialogButtons.OK
                        ,
                        Message = "There are no emails to send."
                        ,
                        Title = "Email All"
                        ,
                        CompletionSource = tcsNone
                    });
                await tcsNone.Task;
                return;
            }
            isRecording = true;
            progress = 0;
            int idx = 0;
            StateHasChanged();
            foreach (var signup in completedSignups)
            {

                idx++;
                progress = (int)((idx * 100) / (completedSignups.Count * 100));
                StateHasChanged();
                if (signup.Email == null)
                {
                    continue;
                }
                EmailVideo(signup.Id, signup.Email);
            }
            isRecording = false;
            StateHasChanged();

            var tcs = new TaskCompletionSource<string>();
            PubSub.Hub.Default.Publish<MsgBoxShowEvent>(
                new MsgBoxShowEvent()
                {
                    Buttons = DialogButtons.OK
                    ,
                    Message = $"{completedSignups.Count} emails sent."
                    ,
                    Title = "Email All"
                    ,
                    CompletionSource = tcs
                });
            await tcs.Task;
        }
        //Implement Gmail Relay
        //https://www.youtube.com/watch?v=ql5Dex4m40w
        public void EmailVideo(int Id, string emailAddress)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("our.simple.humor@gmail.com", "fpjjhtwyhfxuqzzz");
            client.EnableSsl = true;
            MailMessage message = new MailMessage();
            message.From = new MailAddress("our.simple.humor@gmail.com");
            message.To.Add(emailAddress);
            message.Subject = "Your 360 Photo Booth Video";
            message.Body = $"Visit our YouTube Channel Our.Simple.Humor {Environment.NewLine} https://www.youtube.com/channel/UCOW9gIJoAhWP45f1xag9-PA";
            message.Attachments.Add(new Attachment($"/home/pi/Videos/{Id}.mp4"));
            client.Send(message);
        }
    }
}