﻿import {noValidationIssues } from "./validation.js"

$(() => {
    $("#register").click(() => register());
});

const register = () => {
    const user = {      // Creates object that can get sent to controller 
        firstname: $("#inFirstName").val(),
        surname: $("#inSurname").val(),
        address: $("#inAddress").val(),
        postalCode: $("#inPostalCode").val(),
        postalOffice: $("#inPostalOffice").val(),
        username: $("#inUsername").val(),
        password: $("#inPassword").val()
    }
    if (noValidationIssues()) {     // If validation succeded, back to login
        $.post("/stock/RegisterUser", user, (ok) => {
            window.location.href = "login.html"
        });
      
    }
}