$(() => {
    $("#landing").click(() => window.location.href = "index.html");
    getUserStocks();
});

const getUserStocks = () => $.get("stock/GetUserStocks", stockList => formatStocks(stockList));

const sellStock = (stockId) => {
    let stockID = stockId;
    let Quantity = $("#quantity" + stockId).val();
    console.log(Quantity + " " + stockId);
    if (validateQuantity(stockId)) {
        Quantity = 0 - Quantity;
        const url = "stock/BuyStock?StockId=" + stockID + "&Quantity=" + Quantity;

        $.post(url, (ok) => {
            if (ok) {
                $("#sale").html("Your chosen stocks have been sold");
                getUserStocks();
            } else $("#sale").html("Oops, something went wrong! Please try again later");
        });
    }
    
}

const formatStocks = stockList => {
    let stockTable =
        '<table class="table"><tr>' +
        '<th>Name</th>' +
        '<th>+/-</th>' +
        '<th>+/-%</th>' +
        '<th>Price</th>' +
        '<th>Turnover</th>' +
        '<th>Your Quantity</th>' +
        '<th>Quantity</th>' +
        '</tr>';

    for (let stock of stockList) {
        stockTable +=
            '<tr>' +
            '<td>' + stock.name + '</td>' +
            '<td>' + stock.diff + '</td>' +
            '<td>' + (stock.diff/(stock.price-stock.diff)*100).toFixed(1) + '</td>' +
            '<td>' + stock.price + '</td>' +
            '<td>' + stock.turnover + '</td>' +
            '<td><b id=oldQuantity'+stock.id+'>' + stock.quantity + '<b></td>' +
            '<td> <input type="text" id="quantity' + stock.id + '"> </td>' +
            '<td><em id="sellFeedback"></em><td>' +
            '<td> <button onclick="sellStock('+stock.id+')">Sell</td>' +
            '</tr>';
    }
    stockTable += '</table>';
    //id = "oldQuantity'+stock.id+'

    $("#userStocks").html(stockTable);
}

const validateQuantity = (id) => {
    const quantity = parseInt($("#quantity" + id).val());
    const oldQuantity = parseInt($("#oldQuantity"+id).val());
    console.log(oldQuantity);
    if (!Number.isInteger(quantity) || quantity < 0) {
        $("#sellFeedback").html("Invalid quantity");
        return false;
    }
    else if (oldQuantity < quantity) {
        $("#sellFeedback").html("You can't sell more stocks than you own!");
        return false;
    }
    else {
        $("#sellFeedback").html("Your stocks have been exchanged succesfully!");
        return true;
    } 
}