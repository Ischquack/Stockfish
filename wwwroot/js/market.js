$(() => {
    getAllStocks();
});

const getAllStocks = () => {
    $.get("stock/getAllStocks", stocks => formatStocks(stocks));
}


const formatStocks = stocks => {
    let out =
        '<table><tr>' +
        '<th>Company</th>' +
        '<th>Value</th>' +
        '<th>Price</th>' +
        '<th>Quantum</th>' +
        '</tr>';

    for (let stock of stocks) {
        out +=
            '<tr>' +
            '<td>' + stock.name + '</td>' +
            '<td>' + stock.value + '</td>' +
            '<td>' + stock.price + '</td>' +
            '<td>' + '' + '</td>' +
            '</tr>';
    }
    out += "</table>";
    $("#stockList").html(out);
}