let ul = document.getElementById("some");


const init = function (ticketId) {
    fetch(`/Comment/GetComments/${ticketId}`)
        .then(response => response.json())
        .then(data => {
            data.foreach()
            ul.appendChild(CreateLi(`Name: ${data.CreatedOn}`));
        });
};

const CreateLi = function (txt) {
    let li = document.createElement("li");
    li.appendChild(document.createTextNode(txt));

    return li;
}

window.onload = Init();