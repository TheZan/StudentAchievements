function GetMessages(id) {
    fetch('/Message/GetMessages/?id=' + id)
     .then((response) => {
      return response.text();
     })
     .then((result) => {
      document.getElementById('chat').innerHTML = result;
      document.getElementById("chatBox").scrollTop = document.getElementById("chatBox").scrollHeight;

      let i = setInterval(function() {
        if (document.getElementById("sendButton")){
            clearInterval(i);
            document.getElementById("sendButton").addEventListener("click", function (e) {
                let message = document.getElementById("message").value;
                let from = document.getElementById("from").value;
                let to = document.getElementById("to").value;
                if(message != ""){
                hubConnection.invoke("Send", message, from, to);
                ResetForm();
                GetMessages(to);
                }
                else{
                    alert("Введите текст сообщения!");
                }
            });
        }
    }, 1000);
     });
}

const hubConnection = new signalR.HubConnectionBuilder().withUrl("/chat", { accessTokenFactory: () => this.loginToken }).build();

hubConnection.on("Receive", function (from) {
    GetMessages(from);
});

function ResetForm() {
    document.getElementById("message").value = "";
}

hubConnection.start();