$(() => {
    getAllStocks();    
    $("#mySite").click(() => window.location.href = "mySite.html");
    $("#myStocks").click(() => window.location.href = "myStocks.html");
    $("#landing").click(() => window.location.href = "index.html");
});

const getAllStocks = () => $.get("stock/getAllStocks", stockList => formatStocks(stockList));

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
        utskrift +=
            '<tr>' +
            '<td>' + stock.name + '</td>' +
            '<td>' + stock.diff + '</td>' +
            '<td>' + stock.diffPer + '</td>' +
            '<td>' + stock.price + '</td>' +
            '<td>' + stock.turnover + '</td>' +
            '<td> <input type="text" id="quantity' + stock.id + '"> </td>' +
            '<td> <button id="buy' + stock.id + '"> </td>' +
            '</tr>';
    }
    stockTable += '</table>';

    $("#stockTable").html(stockTable);
}