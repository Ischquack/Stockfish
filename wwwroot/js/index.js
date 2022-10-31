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

/* This method generates a list containing all stocks stored in the database
 * as in login.js. But it also lets the user buy stocks by generating buttons
 * and inputfields so that he can specify how many of each stock he wants to buy.
 * This is done by generating IDs based on the current Stock ID in the for-
 * loop.
*/
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

// Input validation for the quantity*id* input fields.
// Checks if the user passed in an integer larger than 0.
const validateQuantity = (id) => {
    const quantity = parseInt($("#quantity" + id).val());
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