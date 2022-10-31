﻿$(() => {
    $("#landing").click(() => window.location.href = "index.html");
    getUserStocks();
});

const getUserStocks = () => $.get("stock/GetUserStocks", stockList => formatStocks(stockList));

const sellStock = (stockId) => {
    let stockID = stockId;
    let Quantity = $("#quantity" + stockId).val();
    const url = "stock/sellStock?StockId=" + stockID + "?Quantity=" + Quantity;
    $.post(url, (ok) => {
        if (ok) {
            $("#sale").html("Your chosen stocks have been sold");
        } else $("#sale").html("Oops, something went wrong! Please try again later");
    });
}

const formatStocks = stockList => {
    let stockTable =
        '<table><tr>' +
        '<th>Name</th>' +
        '<th>+/-</th>' +
        '<th>+/-%</th>' +
        '<th>Price</th>' +
        '<th>Turnover</th>' +
        '<th>Quantity</th>' +
        '</tr>';

    for (let stock of stockList) {
        stockTable +=
            '<tr>' +
            '<td>' + stock.name + '</td>' +
            '<td>' + stock.diff + '</td>' +
            '<td>' + stock.diffPer + '</td>' +
            '<td>' + stock.price + '</td>' +
            '<td>' + stock.turnover + '</td>' +
            '<td>' + order.quantity + '</td>' +
            '<td> <input type="text" id="quantity' + stock.id + '"> </td>' +
            '<td> <button onclick="sellStock('+stock.id+')">Sell</td>' +
                '</tr>';
    }
    stockTable += '</table>';

    $("#userStocks").html(stockTable);
}