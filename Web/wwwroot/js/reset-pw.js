// imports
import { Alert, Error, Process, AlertType, Verify } from "./helper-js.js";

// variables
const resForm     = document.getElementById("reset");
const password    = document.getElementById("password");
const conPassword = document.getElementById("con-pass");

// form submit event
resForm.addEventListener("submit", (e) => {
    try {
        e.preventDefault();
        if (!Validate()) {
            return false;
        }
        Process.show();
        fetch("/account/reset-password", {
            method: "POST",
            headers: {
                "X-Requested-With": "XMLHttpRequest",
            },
            body: new FormData(resForm),
        })
            .then((response) => {
                Process.hide();
                return response.json();
            })
            .then((data) => {
                if (data.success) {
                    resForm.reset();
                    Alert.show(AlertType.error, data.message, "alert-message");
                } else {
                    Alert.show(AlertType.error, data.message, "alert-message");
                }
            })
            .catch((error) => {
                Process.hide();
                console.log(error);
            });
    } catch (error) {
        console.log(error);
    }
});

// validations
function Validate() {
    try {
        if (password.value.trim() === "") {
            Error.seterror(password, "Password can not be empty");
            return false;
        } else {
            if (!Verify("PASSWORD", password.value.trim())) {
                Error.seterror(password, "This password is not strong");
                return false;
            } else {
                Error.diserror(password);
            }
        }
        if (conPassword.value.trim() === "") {
            Error.seterror(conPassword, "Confirm password can not be empty");
            return false;
        } else {
            Error.diserror(conPassword);
        }
        if (password.value.trim() != conPassword.value.trim()) {
            Error.seterror(conPassword, "Confirm password does not match");
            return false;
        } else {
            Error.diserror(conPassword);
        }

        return true;
    } catch (error) {
        console.log(error);
    }
}
