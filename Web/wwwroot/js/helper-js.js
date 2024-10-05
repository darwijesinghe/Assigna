// alert types
export const AlertType = {
    error  : "al_error",
    success: "al_success",
};

// alert message
export const Alert = {
    show(type, message, id) {
        try {
            this.cleantimeout = null;
            const element = document.createElement("div");
            element.className = `alert ${type}`;
            element.id = "alert";
            element.role = "alert";

            clearTimeout(this.cleantimeout);

            if (document.getElementById("alert")) {
                document.getElementById("al_message").textContent = message;
            } else {
                element.innerHTML = `<div class="al_content">
                                    <p id="al_message">${message}</p>
                                </div>`;

                document.getElementById(id).appendChild(element);
            }

            // hides alert
            this.cleantimeout = setTimeout(() => {
                element.remove();
            }, 8000);
        } catch (error) {
            console.log(error);
        }
    },
};

// initialize methods
document.addEventListener("DOMContentLoaded", () => {
    try {
        // fetch loading
        Process.init();
    } catch (error) {
        console.log(error);
    }
});

// error handling
export const Error = {
    seterror(target, message) {
        try {
            const pelement         = target.parentElement;
            const errorelement     = pelement.querySelector("span");
            errorelement.innerText = message;
            target.focus();
        } catch (e) {
            console.log(e);
        }
    },

    diserror(target) {
        try {
            const pelement         = target.parentElement;
            const errorelement     = pelement.querySelector("span");
            errorelement.innerText = "";
        } catch (e) {
            console.log(e);
        }
    },
};

// input format validations
export function Verify(type, field) {
    try {

        switch (type) {
            case "PASSWORD":
                var password = /^(?=.*[0-9])(?=.{4,})/;
                if (field.match(password)) {
                    return true;
                }
                break;
            case "PHONE":
                var phoneno = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
                if (field.match(phoneno)) {
                    return true;
                }
                break;
            case "EMAIL":
                if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(field)) {
                    return true;
                }
                break;
        }

        return false;
    } catch (error) {
        console.log(error);
    }
}

// api fetch loading
export const Process = {
    init() {

        // initializing
        try {
            this.element = document.createElement("div");
            this.element.className = "loading hide-loader";
            this.element.innerHTML = `<div class="loading-spinner">
                                    <svg class="spinner" viewBox="0 0 50 50" style="height: 50px; width: 50px;">
                                        <circle class="path" cx="25" cy="25" r="20"></circle>
                                    </svg>
                                </div>`;
            document.body.appendChild(this.element);
        } catch (error) {
            console.log(error);
        }
    },

    show() {

        try {
            // show fetch process
            this.element.className = "loading show-loader";
        } catch (error) {
            console.log(error);
        }
    },

    hide() {

        try {
            // hides fetch process
            this.element.className = "loading hide-loader";
        } catch (error) {
            console.log(error);
        }
    },
};

// confirm dialog
export const Confirm = {
    show(title, message, callback) {
        try {
            const html = `<div class="confirm" id="confirm">
            <div class="confirm_window">
                <div class="con_title">
                    <p class="con_topic">${title}</p>
                    <div class="con_close">
                        <span class="material-symbols-outlined icon" id="con-close">
                            close
                        </span>
                    </div>
                </div>
                <div class="con_ask">
                    <p class="con_question">${message}</p>
                </div>
                <div class="con_actions">
                    <button class="button con_yes" id="con-yes">Yes</button>
                    <button class="button con_no" id="con-no">No</button>
                </div>
            </div>
        </div>
      `;

            const template     = document.createElement("template");
            template.innerHTML = html;

            // events
            const backDrop = template.content.getElementById("confirm");
            const btnClose = template.content.getElementById("con-close");
            const btnYes   = template.content.getElementById("con-yes");
            const btnNo    = template.content.getElementById("con-no");

            backDrop.addEventListener("click", (e) => {
                if (e.target === backDrop) {
                    this.hide(backDrop);
                }
            });

            btnNo.addEventListener("click", (e) => {
                this.hide(backDrop);
            });

            btnClose.addEventListener("click", (e) => {
                this.hide(backDrop);
            });

            btnYes.addEventListener("click", (e) => {
                this.hide(backDrop);
                callback();
            });

            document.body.appendChild(template.content);
        } catch (error) {
            console.log(error);
        }
    },
    // hides dialog
    hide(element) {

        try {
            document.body.removeChild(element);
        } catch (error) {
            console.log(error);
        }
    },
};

// sets task count
export function TaskCount(tasks) {
    try {
        // count result
        const count = tasks;

        // elements
        const allTask = document.getElementById("all-task");
        const penTask = document.getElementById("pen-task");
        const comTask = document.getElementById("com-task");
        const higTask = document.getElementById("hig-task");
        const medTask = document.getElementById("med-task");
        const lowTask = document.getElementById("low-task");

        // sets count values
        allTask.textContent = count.allTask;
        penTask.textContent = count.pendings;
        comTask.textContent = count.complete;
        higTask.textContent = count.high;
        medTask.textContent = count.medium;
        lowTask.textContent = count.low;
    } catch (error) {
        console.log(error);
    }
}
