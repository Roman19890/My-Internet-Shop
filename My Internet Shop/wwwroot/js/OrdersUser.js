ResumeSuccess = function (data) {
    console.log(data);

    if (data.hasOwnProperty("message")) {
        alert(data.message);
        return;
    }

    let id = data.id;
    let status = document.getElementById(id);
    status.innerText = data.status;
    status.classList.remove("text-danger");
    status.classList.add("text-warning");
    document.getElementById("button").innerHTML =
        `<form method="post" action="CancelOrder" data-ajax="true" data-ajax-method="post" data-ajax-success="CancelSuccess" data-ajax-failure="Failure">
                    <input type="hidden" name="OrderId" value="${id}" />
                    <button type="submit" class=" btn btn-lg btn-danger user-orders-button">Отменить</button>
                </form>`;
};

CancelSuccess = function (data) {
    console.log(data);
    let id = data.id;
    let status = document.getElementById(id);
    status.innerText = data.status;
    status.classList.remove("text-success", "text-warning");
    status.classList.add("text-danger");
    document.getElementById("button").innerHTML =
        `<form id="resumeButton" method="post" action="ResumeOrder" data-ajax="true" data-ajax-method="post" data-ajax-success="ResumeSuccess" data-ajax-failure="Failure">
                    <input type="hidden" name="OrderId" value="${id}" />
                    <button type="submit" class=" btn btn-lg btn-success user-orders-button">Возобновить</button>
                </form>`;
};

Failure = function (response) {
    alert("Возникла непредвиденная ошибка");
    console.log(response);
};