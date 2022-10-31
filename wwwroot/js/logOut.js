const logOut = () => {
    $.get("stock/logOut", () => window.location.href = "login.html");
}