ConfirmSuccess = function (data) {

    let id = data.id;
    let status = document.getElementById(id);
    status.innerText = data.status;
    status.classList.remove("text-warning", "text-danger");
    status.classList.add("text-success");
    document.getElementById("cancelButton").removeAttribute("disabled", "");
    document.getElementById("confirmButton").setAttribute("disabled", "");
};

CancelSuccess = function (data) {

    let id = data.id;
    let status = document.getElementById(id);
    status.innerText = data.status;
    status.classList.remove("text-success", "text-warning");
    status.classList.add("text-danger");
    document.getElementById("cancelButton").setAttribute("disabled", "");
    document.getElementById("confirmButton").removeAttribute("disabled", "");
};

Failure = function (response) {
    alert("Возникла непредвиденная ошибка");
    console.log(response);
};