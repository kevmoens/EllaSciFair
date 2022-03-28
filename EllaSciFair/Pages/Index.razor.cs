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
using EllaSciFair.Models.Dialog;
using EllaSciFair.Models.Index;
using HashidsNet;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System;

namespace EllaSciFair.Pages
{
    public partial class Index : ComponentBase
    {

        IndexFormModel formModel = new IndexFormModel();
        public SignUp? mySignup;
        public TakeANumber? takeANumber;
        public Timer? timer;
        public string? errMsg;

        private string? myNumber;
        [Parameter]
        public string? MyNumber
        {
            get { return myNumber; }
            set
            {
                myNumber = value;
                var rawIds = hashIds.Decode(value);
                if (rawIds.Length == 0)
                {
                    int newValue = 0;
                    if (int.TryParse(value, out newValue))
                    {
                        myNumber = newValue.ToString();
                    }
                    return;
                }
                myNumber = rawIds[0].ToString();
            }
        }
        protected override void OnInitialized()
        {

            errMsg = "";
            int myNumber = 0;
            if (int.TryParse(MyNumber, out myNumber))
            {
                mySignup = repoSignUp.GetSignUp(myNumber);
            }
            takeANumber = repoTakeANum.Get();
            base.OnInitialized();
            StateHasChanged();
        }

        async void OnRegister()
        {
            if (string.IsNullOrEmpty(formModel.Email))
            {
                await MsgBox.Show("Email is required.", "Error", DialogButtons.OK);
                return;
            }
            if (string.IsNullOrEmpty(formModel.Name))
            {
                await MsgBox.Show("Name is required.", "Error", DialogButtons.OK);
                return;
            }
            errMsg = "";
            var mySignUp = repoSignUp.GetEmailSignup(formModel.Email);
            if (mySignup != null)
            {
                //Found Match
                StateHasChanged();
                return;
            }

            mySignup = new SignUp();
            mySignup.Email = formModel.Email;
            mySignup.Name = formModel.Name;
            mySignup.IsPublic = formModel.IsPublic ?? false;
            var id = repoSignUp.Add(mySignup);
            mySignup.Id = id;
            StateHasChanged();
        }
        void OnNewSignup()
        {
            mySignup = null;
            formModel = new IndexFormModel();
            errMsg = "";
            StateHasChanged();
        }
        void OnRefresh()
        {
            if (mySignup?.Id == null)
            {
                return;
            }
            navMan.NavigateTo($"/{hashIds.Encode(mySignup.Id)}", true);
        }


        private async Task DownloadFileFromStream()
        {
            try
            {
                using (FileStream fsSource = new FileStream($"/home/pi/Videos/{mySignup?.Id}.mp4",
                    FileMode.Open, FileAccess.Read))
                {
                    var fileName = $"{mySignup?.Id}.mp4";

                    using var streamRef = new DotNetStreamReference(stream: fsSource);

                    await JS.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
                    errMsg = "Download Complete";
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                StateHasChanged();
            }
        }
    }
}