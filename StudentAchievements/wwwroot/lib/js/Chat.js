function GetMessages(id) {
    fetch('/Message/GetMessages/?id=' + id)
     .then((response) => {
      return response.text();
     })
     .then((result) => {
      document.getElementById('chat').innerHTML = result;
      document.getElementById("chatBox").scrollTop = document.getElementById("chatBox").scrollHeight;
     });
}

const hubConnection = new signalR.HubConnectionBuilder().withUrl("/chat", { accessTokenFactory: () => this.loginToken }).build();

hubConnection.on("Receive", function (from) {
    GetMessages(from);
});

let i = setInterval(function() {
    if (document.getElementById("test")){
        clearInterval(i);
        document.getElementById("test").addEventListener("click", function (e) {
            let message = document.getElementById("message").value;
            let from = document.getElementById("from").value;
            let to = document.getElementById("to").value;
            hubConnection.invoke("Send", message, from, to);

            GetMessages(to);
        });
    }
}, 1000);

hubConnection.start();