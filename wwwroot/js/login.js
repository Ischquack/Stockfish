$(() => {
    getAllStocks();     // Retrieves all stocks when loading file 
    $("#register").click(() => window.location.href = "register.html");
    $("#login").click(() => login());
});

const getAllStocks = () => $.get("stock/getAllStocks",
    stockList => formatStocks(stockList));

const login = () => {
    let username = $("#inUsername").val();
    let password = $("#inPassword").val();
    // url string access endpoint in controller and sends username and password:
    const url = "stock/login?username=" + username + "&password=" + password;

    $.post(url, ok => {
        if (ok == 1) {          // If admin
            window.location.href = "admin.html";
        } else if (ok == 0) {   // If user
            window.location.href = "index.html";
        } else {
            $("#wrongLogin").html("The username and password does not match");
        } 
    });
}

// This function generates a table based on the incoming Stock List from the
// controller:
const formatStocks = stockList => {
    let stockTable =
        '<table class="table"><tr>' +
        '<th>Name</th>' +
        '<th>+/-</th>' +
        '<th>+/-%</th>' +
        '<th>Price</th>' +
        '<th>Turnover</th>' +
        '</tr>';

    for (let stock of stockList) {
        stockTable +=
            '<tr>' +
            '<td>' + stock.name + '</td>' +
            '<td>' + stock.diff + '</td>' +
            '<td>' + (stock.diff / (stock.price - stock.diff) * 100).toFixed(1) + '</td>' +
            '<td>' + stock.price + '</td>' +
            '<td>' + stock.turnover + '</td>' +

            '</tr>';
    }
    stockTable += '</table>';

    $("#stockTable").html(stockTable);  // Prints to html
}