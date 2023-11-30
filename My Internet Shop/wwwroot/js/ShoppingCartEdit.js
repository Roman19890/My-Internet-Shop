Success = function (data) {

    console.log(data);

    if (data.hasOwnProperty('Error')) {
        alert(data.Error);
        return;
    }

    if (data.hasOwnProperty('remove')) {
        document.getElementById(data.CountId).remove();

        const cart = document.getElementById('productQuantity');
        const cont = document.getElementById('cont');
        const totalSum = document.getElementById('totalSum');

        let newSum = parseFloat(totalSum.innerText) - data.RemoveSum;
        totalSum.innerText = parseFloat(newSum).toFixed(2).replace('.', ',');

        let newCartQuantity = Number(cart.innerText) - data.RemoveQuantity;
        cart.innerText = newCartQuantity;

        if (newCartQuantity < 1) {

            cart.style.display = "none";
        }

        if (cont.firstChild.nextElementSibling.nodeName == "H1") {
            cont.innerHTML = `<div class="cart-empty">
                    <h1 class="text-center display-3 text-danger">Корзина пока пуста</h1>
                </div>`;
        }
        return;
    }

    const cart = document.getElementById('productQuantity');
    const productsSum = document.getElementById(data.CountId).children[4].children[0];
    const totalSum = document.getElementById('totalSum');
    console.log(productsSum.innerText);

    let newCartQuantity = Number(cart.innerText) + data.CountDifference;
    cart.innerText = newCartQuantity;      

    let newSum = parseFloat(productsSum.innerText) + data.Difference;
    productsSum.innerText = parseFloat(newSum).toFixed(2).replace('.', ',');

    let newTotalSum = parseFloat(totalSum.innerText) + data.Difference;
    totalSum.innerText = parseFloat(newTotalSum).toFixed(2).replace('.', ',');
};

Failure = function (response) {
    alert("Возникла непредвиденная ошибка");
    console.log(response);
};