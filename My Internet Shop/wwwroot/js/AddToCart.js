OnSuccess = function (response) {

    if (response.hasOwnProperty('message')) {
        alert(response.message);
        return;
    }

    alert("Добавлено");
    let sup = document.getElementById("productQuantity");
    sup.innerText = response;

    if (response > 0 && sup.style.display == '')
        sup.style.display = 'inline';
};

OnFailure = function (response) {
    alert("При добавлении возникла ошибка");
    console.log(response);
};