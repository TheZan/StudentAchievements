var pageNumber = 0;

function GetOldMessages(id) {
    if(pageNumber > -1){
    pageNumber++;
    fetch('/Message/GetMessages/?id=' + id + '&pageNumber=' + pageNumber)
     .then((response) => {
      return response.text();
     })
     .then((result) => {
        if(!!result){
            $('#chatBox').prepend(result);
            $('#loading').hide();
            $('#chatBox').scrollTop(30);
        }
        else{
            pageNumber = -1;
        }
     });
    }
}

function GetMessages(id) {
    pageNumber = 0;
    fetch('/Message/GetMessages/?id=' + id)
     .then((response) => {
      return response.text();
     })
     .then((result) => {
      var chatName = 'companion-' + id;
      document.getElementById(chatName).innerHTML = "";

      document.getElementById('chat').innerHTML = result;
      document.getElementById("chatBox").scrollTop = document.getElementById("chatBox").scrollHeight;
      
      let i = setInterval(function() {
        if (document.getElementById("sendButton")){
            clearInterval(i);
            $('#loading').hide();

            $('#chatBox').scroll(function(){
                if ($('#chatBox').scrollTop() == 0){
                     $('#loading').show();
                    
                    var id = document.getElementById("chatCompanionId").value;
                    GetOldMessages(id);
                }
            });
            
            document.getElementById("sendButton").addEventListener("click", function (e) {
                SendMessage();
            });

            document.getElementById("message").addEventListener('keydown', function(event) {
                if (event.code == 'Enter') {
                    SendMessage();
                }
              });
        }
    }, 1000);
     });
}

function SendMessage() {
    let message = document.getElementById("message").value;
    let from = document.getElementById("from").value;
    let to = document.getElementById("to").value;
    if (message != "") {
        hubConnection.invoke("Send", message, from, to);
        var sendDate = new Date().toLocaleString();
        GetMeMessage(message, sendDate);
        Reset();
        document.getElementById("chatBox").scrollTop = document.getElementById("chatBox").scrollHeight;
    }
    else {
        alert("Введите текст сообщения!");
    }
}

const hubConnection = new signalR.HubConnectionBuilder().withUrl("/chat", { accessTokenFactory: () => this.loginToken }).build();

hubConnection.on("Receive", function (message, from, fromId, sendDate, senderPhoto) {
    if(document.getElementById(fromId)){
        GetCompanionMessage(message, from, sendDate, senderPhoto);
        document.getElementById("chatBox").scrollTop = document.getElementById("chatBox").scrollHeight;
    }
    else
    {
        var chatName = 'companion-' + fromId;
        var unreadMessageCount = document.getElementById(chatName).textContent;
        if(document.getElementById(chatName).textContent != "")
        {
            document.getElementById(chatName).innerHTML = Number(unreadMessageCount) + 1;
        }
        else
        {
            document.getElementById(chatName).style.visibility='visible';
            document.getElementById(chatName).innerHTML = 1;
        }
        
        ShowNotify(from, senderPhoto);
    }
});

var photo;

hubConnection.on("Connected", function (receiverPhoto) {
    photo = receiverPhoto;
});

hubConnection.start();

function GetMeMessage(message, sendDate) {
    $('#chatBox').append(`<div class="chat-message-right pb-4">
    <div>
        <img src="data:image/jpeg;base64,${photo}"
            asp-append-version="true" class="rounded-circle mr-1" width="40" height="40">
        <div class="text-muted small text-nowrap mt-2">${sendDate}</div>
    </div>
    <div class="flex-shrink-1 bg-light rounded py-2 px-3 mr-3">    
        <div class="font-weight-bold mb-1">Вы</div>
        <div>${message}</div>
    </div>
    </div>`);
}

function GetCompanionMessage(message, from, sendDate, senderPhoto) {
    $('#chatBox').append(`<div class="chat-message-left pb-4">
    <div>
        <img src="data:image/jpeg;base64,${senderPhoto}"
            asp-append-version="true" class="rounded-circle mr-1" width="40"
            height="40">
        <div class="text-muted small text-nowrap mt-2">${sendDate}</div>
    </div>
    <div class="flex-shrink-1 bg-light rounded py-2 px-3 mr-3 ml-2">
        <div class="font-weight-bold mb-2">${from}</div>
        <div>${message}</div>
    </div>
</div>`);
}

function Reset() {
    if(document.getElementById("message")){
        document.getElementById("message").value = "";
    }
}

function ShowNotify(from, senderPhoto) {
    var audio = new Audio();
    audio.src = '/sounds/notification.mp3';
    audio.autoplay = true;

    bs4Toast.show(from, 'Новое сообщение!', {
        icon : {
          type : 'image',
          src : `data:image/jpeg;base64,${senderPhoto}`
        }
      });
  }