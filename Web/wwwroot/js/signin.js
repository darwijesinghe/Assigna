// imports
import { Alert, Error, Process, AlertType } from "./helper-js.js";

// variables
const sigForm  = document.getElementById("signin");
const userName = document.getElementById("user-name");
const password = document.getElementById("password");

// form submit event
sigForm.addEventListener("submit", (e) => {
    try {
        e.preventDefault();
        if (!Validate()) {
            return false;
        }
        Process.show();
        fetch("/account/signin", {
            method: "POST",
            headers: {
                "X-Requested-With": "XMLHttpRequest",
            },
            body: new FormData(sigForm),
        })
            .then((response) => {
                Process.hide();
                return response.json();
            })
            .then((data) => {
                if (data.success) {
                    sigForm.reset();
                    var directlink = data.returnurl;
                    return window.location.replace(directlink);
                } else {
                    Alert.show(AlertType.error, data.message, "alert-message");
                }
            })
            .catch((error) => {
                Process.hide();
                console.log(error);
            });
    } catch (error) {
        Process.hide();
        console.log(error);
    }
});

// validations
function Validate() {
    try {
        if (userName.value.trim() === "") {
            Error.seterror(userName, "Username can not be empty");
            return false;
        } else {
            Error.diserror(userName);
        }
        if (password.value.trim() === "") {
            Error.seterror(password, "Password can not be empty");
            return false;
        } else {
            Error.diserror(password);
        }
        return true;
    } catch (error) {
        console.log(error);
    }
}
