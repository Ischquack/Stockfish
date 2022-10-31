//import { validateQuantity } from "./validation.js"

$(() => {
    getAllStocks();    
    $("#sellStocks").click(() => window.location.href = "sellStocks.html");
    $("#landing").click(() => window.location.href = "index.html");
    $("#logOut").click(() => logOut());
});

const getAllStocks = () => $.get("stock/getAllStocks", stockList => formatStocks(stockList));

const buyStock = (stockId) => {
    let Quantity = $("#quantity" + stockId).val();
    if (validateQuantity(stockId)){
        const url = "stock/exchangeStock?StockId=" + stockId + "&Quantity=" + Quantity;
        $.post(url, ok => {
            $("#buyFeedback" + stockId).html("Stocks succesfully purchased!");
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
        '<th>Quantity</th>' +
        '</tr>';

    for (let stock of stockList) {
        stockTable +=
            '<tr>' +
            '<td>' + stock.name + '</td>' +
            '<td>' + stock.diff + '</td>' +
            '<td>' + (stock.diff / (stock.price - stock.diff) * 100).toFixed(1) + '</td>' +
            '<td>' + stock.price + '</td>' +
            '<td>' + stock.turnover + '</td>' +
            '<td> <input type="text" id="quantity' + stock.id + '"> </td>' +
            '<td><em id="buyFeedback'+stock.id+'"></em></td>' +
            '<td> <button onclick="buyStock('+stock.id+')">Buy</td>' +
            '</tr>';
    }
    stockTable += '</table>';

    $("#stockTable").html(stockTable);
}

const validateQuantity = (id) => {
    const quantity = parseInt($("#quantity" + id).val());
    console.log(Number.isInteger(quantity));
    console.log(quantity);
    if (!Number.isInteger(quantity) || quantity < 0) {
        $("#buyFeedback"+id).html("Invalid quantity");
        return false;
    }
    else {
        console.log(quantity);
        $("#quantity" + id).val("");
        return true;
    } 
}