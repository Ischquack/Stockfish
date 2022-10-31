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
                $("#sellFeedback" + stockId).html("Your stocks have been exchanged succesfully!");
                getUserStocks();
            } else $("#sellFeedback" + stockId).html("Oops, something went wrong! Please try again later");
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
            '<td><em id="sellFeedback'+stock.id+'"></em><td>' +
            '<td> <button onclick="sellStock('+stock.id+')">Sell</td>' +
            '</tr>';
    }
    stockTable += '</table>';

    $("#userStocks").html(stockTable);
}

const validateQuantity = (id) => {
    const quantity = parseInt($("#quantity" + id).val());  
    const oldQuantity = parseInt($("#oldQuantity"+id).text());
    console.log(oldQuantity);
    if (!Number.isInteger(quantity) || quantity < 0) {
        $("#sellFeedback"+id).html("Invalid quantity");
        return false;
    }
    else if (oldQuantity < quantity) {
        $("#sellFeedback"+id).html("You can't sell more stocks than you own!");
        return false;
    }
    else {
        $("#quantity" + id).val("");
        return true;
    } 
}