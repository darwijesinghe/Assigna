// imports
import { Alert, Error, Process, AlertType, Verify } from "./helper-js.js";

// variables
const sigForm   = document.getElementById("signup");
const firstName = document.getElementById("first-name");
const userName  = document.getElementById("user-name");
const email     = document.getElementById("email");
const password  = document.getElementById("password");

// form submit event
sigForm.addEventListener("submit", (e) => {
    try {
        e.preventDefault();
        if (!Validate()) {
            return false;
        }
        Process.show();
        fetch("/account/signup", {
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
        console.log(error);
    }
});

// validations
function Validate() {
    try {
        if (firstName.value.trim() === "") {
            Error.seterror(firstName, "First name can not be empty");
            return false;
        } else {
            firstName.value = firstName.value.trim(); // -> trim value to ignore white space
            Error.diserror(firstName);
        }
        if (userName.value.trim() === "") {
            Error.seterror(userName, "Username can not be empty");
            return false;
        } else {
            Error.diserror(userName);
        }
        if (userName.value.indexOf(" ") >= 0) {
            Error.seterror(userName, "Username can only have letters and numbers");
            return false;
        } else {
            Error.diserror(userName);
        }
        if (email.value.trim() === "") {
            Error.seterror(email, "Email can not be empty");
            return false;
        } else {
            if (!Verify("EMAIL", email.value.trim())) {
                Error.seterror(email, "This email is not valid email");
                return false;
            } else {
                Error.diserror(email);
            }
        }
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

        return true;
    } catch (error) {
        console.log(error);
    }
}
